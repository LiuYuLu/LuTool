using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Etcdserverpb;
using Google.Protobuf;
using Grpc.Core;

namespace LuTool.Etcd
{
    public partial class EtcdClient : IDisposable
    {
        #region GetVal

        /// <summary>
        /// Get the value for a specified key
        /// </summary>
        /// <returns>The value for the specified key</returns>
        /// <param name="key">Key for which value need to be fetched</param>
        public string GetVal(string key, Metadata headers = null)
        {
            var request = new RangeRequest
            {
                Key = ByteString.CopyFromUtf8(key)
            };
            var rangeResponse = Get(request, headers);
            return rangeResponse.Count != 0 ? rangeResponse.Kvs[0].Value.ToStringUtf8().Trim() : string.Empty;
        }

        /// <summary>
        /// Get the value for a specified key in async
        /// </summary>
        /// <returns>The value for the specified key</returns>
        /// <param name="key">Key for which value need to be fetched</param>
        public async Task<string> GetValAsync(string key, Metadata headers = null)
        {
            var request = new RangeRequest
            {
                Key = ByteString.CopyFromUtf8(key)
            };
            var rangeResponse = await GetAsync(request, headers);
            return rangeResponse.Count != 0 ? rangeResponse.Kvs[0].Value.ToStringUtf8().Trim() : string.Empty;
        }

        private RangeResponse Get(RangeRequest request, Metadata headers = null)
        {
            RangeResponse rangeResponse = new RangeResponse();
            bool success = false;
            int retryCount = 0;
            while (!success)
            {
                try
                {
                    rangeResponse = _balancer.GetConnection().kvClient.Range(request, headers);
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

            return rangeResponse;
        }

        private async Task<RangeResponse> GetAsync(RangeRequest request, Metadata headers = null)
        {
            RangeResponse rangeResponse = new RangeResponse();
            bool success = false;
            int retryCount = 0;
            while (!success)
            {
                try
                {
                    rangeResponse = await _balancer.GetConnection().kvClient.RangeAsync(request, headers);
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

            return rangeResponse;
        }

        #endregion


        #region GetRange

        /// <summary>
        /// Gets the range of keys with the specified prefix
        /// </summary>
        /// <returns>RangeResponse containing range of key-values</returns>
        /// <param name="prefixKey">Prefix key</param>
        public RangeResponse GetRange(string prefixKey, Metadata headers = null)
        {
            string rangeEnd = EtcdUtil.GetRangeEnd(prefixKey);
            var request = new RangeRequest
            {
                Key = ByteString.CopyFromUtf8(prefixKey),
                RangeEnd = ByteString.CopyFromUtf8(rangeEnd)
            };
            var rangeResponse = Get(request, headers);
            return rangeResponse;
        }

        /// <summary>
        /// Gets the range of keys with the specified prefix in async
        /// </summary>
        /// <returns>RangeResponse containing range of key-values</returns>
        /// <param name="prefixKey">Prefix key</param>
        public async Task<RangeResponse> GetRangeAsync(string prefixKey, Metadata headers = null)
        {
            string rangeEnd = EtcdUtil.GetRangeEnd(prefixKey);
            var request = new RangeRequest
            {
                Key = ByteString.CopyFromUtf8(prefixKey),
                RangeEnd = ByteString.CopyFromUtf8(rangeEnd)
            };
            var rangeResponse = await GetAsync(request, headers);
            return rangeResponse;
        }

        #endregion

        /// <summary>
        /// Gets the range of keys with the specified prefix
        /// </summary>
        /// <returns>Dictionary containing range of key-values</returns>
        /// <param name="prefixKey">Prefix key</param>
        public IDictionary<string, string> GetRangeVal(string prefixKey, Metadata headers = null)
        {
            string rangeEnd = EtcdUtil.GetRangeEnd(prefixKey);
            var request = new RangeRequest
            {
                Key = ByteString.CopyFromUtf8(prefixKey),
                RangeEnd = ByteString.CopyFromUtf8(rangeEnd)
            };
            var rangeResponse = Get(request, headers);
            return EtcdUtil.RangeRespondToDictionary(rangeResponse);
        }

        /// <summary>
        /// Gets the range of keys with the specified prefix in async
        /// </summary>
        /// <returns>Dictionary containing range of key-values</returns>
        /// <param name="prefixKey">Prefix key</param>
        public async Task<IDictionary<string, string>> GetRangeValAsync(string prefixKey, Metadata headers = null)
        {
            string rangeEnd = EtcdUtil.GetRangeEnd(prefixKey);
            var request = new RangeRequest
            {
                Key = ByteString.CopyFromUtf8(prefixKey),
                RangeEnd = ByteString.CopyFromUtf8(rangeEnd)
            };
            var rangeResponse = await GetAsync(request, headers);
            return EtcdUtil.RangeRespondToDictionary(rangeResponse);
        }

        
        
        /// <summary>
        /// 设置键/值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <returns></returns>
        public PutResponse Put(string key, string val, Metadata headers = null)
        {
            return Put(new PutRequest
            {
                Key = ByteString.CopyFromUtf8(key),
                Value = ByteString.CopyFromUtf8(val)
            }, headers);
        }

        /// <summary>
        /// 使用异步方式设置键/值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PutResponse> PutAsync(PutRequest request, Metadata headers = null)
        {
            PutResponse response = new PutResponse();
            bool success = false;
            int retryCount = 0;
            while (!success)
            {
                try
                {
                    response = await _balancer.GetConnection().kvClient.PutAsync(request, headers);
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

            return response;
        }


        /// <summary>
        /// Sets the key value in etcd in async
        /// </summary>
        /// <param name="key">Key for which value need to be set</param>
        /// <param name="val">Value corresponding the key</param>
        /// <returns></returns>
        public async Task<PutResponse> PutAsync(string key, string val, Metadata headers = null)
        {
            return await PutAsync(new PutRequest
            {
                Key = ByteString.CopyFromUtf8(key),
                Value = ByteString.CopyFromUtf8(val)
            }, headers);
        }

        private PutResponse Put(PutRequest request, Metadata headers = null)
        {
            PutResponse response = new PutResponse();
            bool success = false;
            int retryCount = 0;
            while (!success)
            {
                try
                {
                    response = _balancer.GetConnection().kvClient.Put(request, headers);
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
            return response;
        }
        
        
        /// <summary>
        /// Delete the specified key in etcd
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DeleteRangeResponse Delete(DeleteRangeRequest request, Metadata headers = null)
        {
            DeleteRangeResponse response = new DeleteRangeResponse();
            bool success = false;
            int retryCount = 0;
            while (!success)
            {
                try
                {
                    response = _balancer.GetConnection().kvClient.DeleteRange(request, headers);
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

            return response;
        }

        /// <summary>
        /// Delete the specified key in etcd
        /// </summary>
        /// <param name="key">Key which needs to be deleted</param>
        public DeleteRangeResponse Delete(string key, Metadata headers = null)
        {
            return Delete(new DeleteRangeRequest
            {
                Key = ByteString.CopyFromUtf8(key)
            }, headers);
        }


        /// <summary>
        /// Delete the specified key in etcd in async
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DeleteRangeResponse> DeleteAsync(DeleteRangeRequest request, Metadata headers = null)
        {
            DeleteRangeResponse response = new DeleteRangeResponse();
            bool success = false;
            int retryCount = 0;
            while (!success)
            {
                try
                {
                    response = await _balancer.GetConnection().kvClient.DeleteRangeAsync(request, headers);
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

            return response;
        }

        /// <summary>
        /// Delete the specified key in etcd in async
        /// </summary>
        /// <param name="key">Key which needs to be deleted</param>
        public async Task<DeleteRangeResponse> DeleteAsync(string key, Metadata headers = null)
        {
            return await DeleteAsync(new DeleteRangeRequest
            {
                Key = ByteString.CopyFromUtf8(key)
            }, headers);
        }

        /// <summary>
        /// Deletes all keys with the specified prefix
        /// </summary>
        /// <param name="prefixKey">Commin prefix of all keys that need to be deleted</param>
        public DeleteRangeResponse DeleteRange(string prefixKey, Metadata headers = null)
        {
            string rangeEnd = EtcdUtil.GetRangeEnd(prefixKey);
            return Delete(new DeleteRangeRequest
            {
                Key = ByteString.CopyFromUtf8(prefixKey),
                RangeEnd = ByteString.CopyFromUtf8(rangeEnd)
            }, headers);
        }

        /// <summary>
        /// Deletes all keys with the specified prefix in async
        /// </summary>
        /// <param name="prefixKey">Commin prefix of all keys that need to be deleted</param>
        public async Task<DeleteRangeResponse> DeleteRangeAsync(string prefixKey, Metadata headers = null)
        {
            string rangeEnd = EtcdUtil.GetRangeEnd(prefixKey);
            return await DeleteAsync(new DeleteRangeRequest
            {
                Key = ByteString.CopyFromUtf8(prefixKey),
                RangeEnd = ByteString.CopyFromUtf8(rangeEnd)
            }, headers);
        }
    }
}