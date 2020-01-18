using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using LuTool.Etcd.Multiplexer;

namespace LuTool.Etcd
{
    /// <summary>
    /// Etcd连接客户端
    /// </summary>
    public partial class EtcdClient : IDisposable
    {
        private readonly Balancer _balancer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串：逗号隔开</param>
        /// <param name="port">端口</param>
        /// <param name="username">基本身份验证的etcd服务器的用户名</param>
        /// <param name="password">基本身份验证的etcd服务器的密码</param>
        /// <param name="caCert">连接到etcd的CA证书内容。</param>
        /// <param name="clientCert">连接到etcd的客户端证书内容。</param>
        /// <param name="clientKey">连接到etcd的客户端密钥内容。</param>
        /// <param name="publicRootCa">是否使用公共信任的根进行连接。</param>
        /// <exception cref="Exception"></exception>
        public EtcdClient(string connectionString, int port = 2379, string username = "", string password = "",
            string caCert = "", string clientCert = "", string clientKey = "", bool publicRootCa = false)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("连接到Etcd服务端的连接不能为空.");
            }

            string[] hosts;

            if (connectionString.ToLowerInvariant().StartsWith("discovery-srv://"))
            {
                // Expecting it to be discovery-srv://{domain}/{name}
                // Examples:
                // discovery-srv://my-domain.local/ would expect entries for either _etcd-client-ssl._tcp.my-domain.local or _etcd-client._tcp.my-domain.local
                // discovery-srv://my-domain.local/project1 would expect entries for either _etcd-client-ssl-project1._tcp.my-domain.local or _etcd-client-project1._tcp.my-domain.local
//                Uri discoverySrv = new Uri(connectionString);
//                var client = new LookupClient {UseCache = true};
//                // SSL first ...
//                var serviceName = "/".Equals(discoverySrv.AbsolutePath)
//                    ? ""
//                    : $"-{discoverySrv.AbsolutePath.Substring(1, discoverySrv.AbsolutePath.Length - 1)}";
//                var result = client.Query($"_etcd-client-ssl{serviceName}._tcp.{discoverySrv.Host}", QueryType.SRV);
//                var scheme = "https";
//                if (result.HasError)
//                {
//                    scheme = "http";
//                    // No SSL ...
//                    result = client.Query($"_etcd-client{serviceName}._tcp.{discoverySrv.Host}", QueryType.SRV);
//                    if (result.HasError)
//                    {
//                        throw new InvalidOperationException(result.ErrorMessage);
//                    }
//                }
//
//                var results = result.Answers.OfType<SrvRecord>().OrderBy(a => a.Priority)
//                    .ThenByDescending(a => a.Weight).ToList();
//                hosts = new string[results.Count];
//                for (int index = 0; index < results.Count; index++)
//                {
//                    var srvRecord = results[index];
//                    var additionalRecord =
//                        result.Additionals.FirstOrDefault(p => p.DomainName.Equals(srvRecord.Target));
//                    var host = srvRecord.Target.Value;
//
//                    if (additionalRecord is ARecord aRecord)
//                    {
//                        host = aRecord.Address.ToString();
//                    }
//                    else if (additionalRecord is CNameRecord cname)
//                    {
//                        host = cname.CanonicalName;
//                    }
//
//                    if (host.EndsWith("."))
//                        host = host.Substring(0, host.Length - 1);
//                    hosts[index] = $"{scheme}://{host}:{srvRecord.Port}";
//                }
                hosts = new string[] { };
            }
            else
            {
                hosts = connectionString.Split(',');
            }

            List<Uri> nodes = new List<Uri>();
            // 处理连接字符串没有加端口号
            for (int i = 0; i < hosts.Length; i++)
            {
                string host = hosts[i];
                if (host.Split(':').Length < 3)
                {
                    host += $":{Convert.ToString(port)}";
                }

                nodes.Add(new Uri(host));
            }

            _balancer = new Balancer(nodes, username, password, caCert, clientCert, clientKey, publicRootCa);
        }

        #region IDisposable Support

        private bool _disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposed = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 执行，失败时不断重试！
        /// </summary>
        private async Task InvokeWithRetryAsync(Func<Task> work, Func<Task> retry)
        {
            try
            {
                await work();
            }
            catch (RpcException ex)
            {
                // todo 修改日志记录方式
                // _logOnError(_logger, DateTimeOffset.Now, ErrorFormatter.Instance.Format(ex), null);
                await retry();
            }
            catch (Exception ex)
            {
                // todo 修改日志记录方式
                // _logOnError(_logger, DateTimeOffset.Now, ErrorFormatter.Instance.Format(ex), null);
                throw;
            }
        }

        /// <summary>
        /// 执行，失败时不断重试！
        /// </summary>
        private async Task<T> InvokeWithRetryAsync<T>(Func<Task<T>> work, Func<Task<T>> retry)
        {
            try
            {
                return await work();
            }
            catch (RpcException ex)
            {
                // todo 修改日志记录方式
                // _logOnError(_logger, DateTimeOffset.Now, ErrorFormatter.Instance.Format(ex), null);
                return await retry();
            }
            catch (Exception ex)
            {
                // todo 修改日志记录方式
                // _logOnError(_logger, DateTimeOffset.Now, ErrorFormatter.Instance.Format(ex), null);
                throw;
            }
        }

        #endregion
    }
}