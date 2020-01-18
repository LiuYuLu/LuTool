using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Etcdserverpb;
using Google.Protobuf;
using Grpc.Core;

namespace LuTool.Etcd
{
    public partial class EtcdClient : IDisposable
    {
        public void Watch(string key, Action<WatchResponse> method, CancellationToken cancellationToken,
            Metadata headers = null, Action<Exception> exceptionHandler = null)
        {
            var request = new WatchRequest()
            {
                CreateRequest = new WatchCreateRequest()
                {
                    Key = ByteString.CopyFromUtf8(key)
                }
            };
            Task.Factory.StartNew(() => WatchAsync(request, method, headers, exceptionHandler), cancellationToken,
                TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        #region Watch 私有方法

        private async Task WatchAsync(WatchRequest request, Action<WatchResponse> method, Metadata headers = null,
            Action<Exception> exceptionHandler = null)
        {
            await InvokeWithRetryAsync(() => WatchCoreAsync(request, method, headers, exceptionHandler),
                async () => { await WatchAsync(request, method, headers, exceptionHandler); });
        }

        private async Task WatchCoreAsync(WatchRequest request, Action<WatchResponse> method, Metadata headers = null,
            Action<Exception> exceptionHandler = null)
        {
            var success = false;
            var retryCount = 0;
            while (!success)
            {
                try
                {
                    using (AsyncDuplexStreamingCall<WatchRequest, WatchResponse> watcher =
                        _balancer.GetConnection().watchClient.Watch(headers))
                    {
                        await watcher.RequestStream.WriteAsync(request);
                        await watcher.RequestStream.CompleteAsync();
                        try
                        {
                            while (await watcher.ResponseStream.MoveNext())
                            {
                                WatchResponse update = watcher.ResponseStream.Current;
                                method(update);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (exceptionHandler != null)
                            {
                                exceptionHandler(ex);
                            }
                            else
                            {
                                throw ex;
                            }
                        }

                        ;
                    }

                    success = true;
                }
                catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
                {
                    retryCount++;
                    if (retryCount >= _balancer._numNodes)
                    {
                        throw ex;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Watch指定的范围，并将响应传递给提供的方法。
        /// </summary>
        /// <param name="path">watch的范围</param>
        /// <param name="method">处理watch响应的方法</param>
        public void WatchRange(string path, Action<WatchResponse> method, CancellationToken cancellationToken,
            Metadata headers = null, Action<Exception> exceptionHandler = null)
        {
            WatchRequest request = new WatchRequest()
            {
                CreateRequest = new WatchCreateRequest()
                {
                    Key = ByteString.CopyFromUtf8(path),
                    RangeEnd = ByteString.CopyFromUtf8(EtcdUtil.GetRangeEnd(path))
                }
            };
            Task.Factory.StartNew(() => WatchAsync(request, method, headers, exceptionHandler), cancellationToken,
                TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        /// <summary>
        /// Watche指定的范围，并将响应传递给提供的方法。
        /// </summary>
        /// <param name="paths">watch的范围集合</param>
        /// <param name="method">处理watch响应的方法</param>
        public void WatchRange(string[] paths, Action<WatchEvent[]> method, CancellationToken cancellationToken,
            Metadata headers = null,
            Action<Exception> exceptionHandler = null)
        {
            List<WatchRequest> requests = new List<WatchRequest>();

            foreach (string path in paths)
            {
                WatchRequest request = new WatchRequest()
                {
                    CreateRequest = new WatchCreateRequest()
                    {
                        Key = ByteString.CopyFromUtf8(path),
                        RangeEnd = ByteString.CopyFromUtf8(EtcdUtil.GetRangeEnd(path))
                    }
                };
                requests.Add(request);
            }

            Watch(requests.ToArray(), method, headers, exceptionHandler);
        }

        /// <summary>
        /// Watch指定的范围，并将响应传递给提供的方法。
        /// </summary>
        /// <param name="paths">watch的范围集合</param>
        /// <param name="methods">处理watch响应的方法集合</param>
        /// <param name="cancellationToken">中止线程token</param>
        /// <param name="exceptionHandler">异常处理委托方法</param>
        public void WatchRange(string[] paths, Action<WatchEvent[]>[] methods, CancellationToken cancellationToken,
            Metadata headers = null,
            Action<Exception> exceptionHandler = null)
        {
            List<WatchRequest> requests = new List<WatchRequest>();
            foreach (string path in paths)
            {
                WatchRequest request = new WatchRequest()
                {
                    CreateRequest = new WatchCreateRequest()
                    {
                        Key = ByteString.CopyFromUtf8(path),
                        RangeEnd = ByteString.CopyFromUtf8(EtcdUtil.GetRangeEnd(path))
                    }
                };
                requests.Add(request);
            }

            Watch(requests.ToArray(), methods, headers, exceptionHandler);
        }

        public async void Watch(WatchRequest[] requests, Action<WatchEvent[]> method, Metadata headers = null,
            Action<Exception> exceptionHandler = null)
        {
            var success = false;
            var retryCount = 0;
            while (!success)
            {
                try
                {
                    using (AsyncDuplexStreamingCall<WatchRequest, WatchResponse> watcher =
                        _balancer.GetConnection().watchClient.Watch(headers))
                    {
                        Task watcherTask = Task.Run(async () =>
                        {
                            try
                            {
                                while (await watcher.ResponseStream.MoveNext())
                                {
                                    WatchResponse update = watcher.ResponseStream.Current;
                                    method(update.Events.Select(i =>
                                        {
                                            return new WatchEvent
                                            {
                                                Key = i.Kv.Key.ToStringUtf8(),
                                                Value = i.Kv.Value.ToStringUtf8(),
                                                Type = i.Type
                                            };
                                        }).ToArray()
                                    );
                                }
                            }
                            catch (Exception ex)
                            {
                                if (exceptionHandler != null)
                                {
                                    exceptionHandler(ex);
                                }
                                else
                                {
                                    throw ex;
                                }
                            }
                        });

                        foreach (WatchRequest request in requests)
                        {
                            await watcher.RequestStream.WriteAsync(request);
                        }

                        await watcher.RequestStream.CompleteAsync();
                        await watcherTask;
                    }

                    success = true;
                }
                catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
                {
                    retryCount++;
                    if (retryCount >= _balancer._numNodes)
                    {
                        throw ex;
                    }
                }
            }
        }

        public async void Watch(WatchRequest[] requests, Action<WatchEvent[]>[] methods, Metadata headers = null,
            Action<Exception> exceptionHandler = null)
        {
            bool success = false;
            int retryCount = 0;
            while (!success)
            {
                try
                {
                    using (AsyncDuplexStreamingCall<WatchRequest, WatchResponse> watcher =
                        _balancer.GetConnection().watchClient.Watch(headers))
                    {
                        Task watcherTask = Task.Run(async () =>
                        {
                            try
                            {
                                while (await watcher.ResponseStream.MoveNext())
                                {
                                    WatchResponse update = watcher.ResponseStream.Current;
                                    foreach (Action<WatchEvent[]> method in methods)
                                    {
                                        method(update.Events.Select(i =>
                                            {
                                                return new WatchEvent
                                                {
                                                    Key = i.Kv.Key.ToStringUtf8(),
                                                    Value = i.Kv.Value.ToStringUtf8(),
                                                    Type = i.Type
                                                };
                                            }).ToArray()
                                        );
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                if (exceptionHandler != null)
                                {
                                    exceptionHandler(ex);
                                }
                                else
                                {
                                    throw ex;
                                }
                            }
                        });

                        foreach (WatchRequest request in requests)
                        {
                            await watcher.RequestStream.WriteAsync(request);
                        }

                        await watcher.RequestStream.CompleteAsync();
                        await watcherTask;
                    }

                    success = true;
                }
                catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
                {
                    retryCount++;
                    if (retryCount >= _balancer._numNodes)
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}