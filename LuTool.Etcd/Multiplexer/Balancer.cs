using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Etcdserverpb;
using Grpc.Core;

namespace LuTool.Etcd.Multiplexer
{
    internal class Balancer
    {
        /// <summary>
        /// 健康状态端节点
        /// </summary>
        private HashSet<Connection> _HealthyCluster;

        /// <summary>
        /// 非健康状态的节点，无法使用的
        /// </summary>
        private HashSet<Connection> _UnHealthyCluster;

        /// <summary>
        /// 基本身份验证的etcd服务器的用户名
        /// </summary>
        private readonly string _username;

        /// <summary>
        /// 基本身份验证的etcd服务器的密码
        /// </summary>
        private readonly string _password;

        /// <summary>
        /// 连接到etcd的CA证书内容。
        /// </summary>
        private readonly string _caCert;

        /// <summary>
        /// 连接到etcd的客户端证书内容。
        /// </summary>
        private readonly string _clientCert;

        /// <summary>
        /// 连接到etcd的客户端密钥内容。
        /// </summary>
        private readonly string _clientKey;

        /// <summary>
        /// 是否启用基本身份验证
        /// </summary>
        private readonly bool _basicAuth;

        /// <summary>
        /// 是否启用ssl
        /// </summary>
        private readonly bool _ssl;

        /// <summary>
        /// 是否启用了ssl 认证
        /// </summary>
        private readonly bool _clientSSL;

        /// <summary>
        /// 是否使用公共信任的根进行连接。
        /// </summary>
        private readonly bool _publicRootCa;

        /// <summary>
        /// etcd节点数
        /// </summary>
        internal readonly int _numNodes;

        /// <summary>
        /// 使用的节点索引
        /// </summary>
        private int _lastNodeIndex;

        internal Balancer(List<Uri> nodes, string username = "", string password = "", string caCert = "",
            string clientCert = "", string clientKey = "", bool publicRootCa = false)
        {
            _numNodes = nodes.Count;
            _caCert = caCert;
            _clientCert = clientCert;
            _clientKey = clientKey;
            _username = username;
            _password = password;
            _publicRootCa = publicRootCa;
            _lastNodeIndex = -1;

            _basicAuth = (!string.IsNullOrWhiteSpace(_username) && !(string.IsNullOrWhiteSpace(_password)));
            _ssl = !_publicRootCa && !string.IsNullOrWhiteSpace(_caCert);
            _clientSSL = _ssl && (!string.IsNullOrWhiteSpace(_clientCert) && !(string.IsNullOrWhiteSpace(_clientKey)));

            _HealthyCluster = new HashSet<Connection>();
            _UnHealthyCluster = new HashSet<Connection>();

            foreach (Uri node in nodes)
            {
                // 声明每个服务器节点的连接channel
                Channel channel = null;
                if (_publicRootCa)
                {
                    channel = new Channel(node.Host, node.Port, new SslCredentials());
                }
                else if (_clientSSL)
                {
                    channel = new Channel(
                        node.Host,
                        node.Port,
                        new SslCredentials(
                            _caCert,
                            new KeyCertificatePair(_clientCert, _clientKey)
                        )
                    );
                }
                else if (_ssl)
                {
                    channel = new Channel(node.Host, node.Port, new SslCredentials(_caCert));
                }
                else
                {
                    channel = new Channel(node.Host, node.Port, ChannelCredentials.Insecure);
                }

                // 声明每个节点应该存在的 clinet客户端
                Connection connection = new Connection
                {
                    kvClient = new KV.KVClient(channel),
                    watchClient = new Watch.WatchClient(channel),
                    leaseClient = new Lease.LeaseClient(channel),
                    lockClient = new V3Lockpb.Lock.LockClient(channel),
                    clusterClient = new Cluster.ClusterClient(channel),
                    maintenanceClient = new Maintenance.MaintenanceClient(channel),
                    authClient = new Auth.AuthClient(channel)
                };

                _HealthyCluster.Add(connection);
            }
        }

        internal Connection GetConnection()
        {
            return _HealthyCluster.ElementAt(GetNextNodeIndex());
        }

        internal Connection GetConnection(int index)
        {
            return _HealthyCluster.ElementAt(index);
        }

        internal void MarkUnHealthy(Connection connection)
        {
            _HealthyCluster.Remove(connection);
            _UnHealthyCluster.Add(connection);
        }

        internal void MarkHealthy(Connection connection)
        {
            _UnHealthyCluster.Remove(connection);
            _HealthyCluster.Add(connection);
        }

        // 根据上次使用的节点，获取当前应该使用的节点
        internal int GetNextNodeIndex()
        {
            int initial, computed;
            do
            {
                initial = _lastNodeIndex;
                computed = initial + 1;
                computed = computed >= _numNodes ? computed = 0 : computed;
            } while (Interlocked.CompareExchange(ref _lastNodeIndex, computed, initial) != initial);
            //CompareExchange: 将_lastNodeIndex和initial的值比较，相等则 _lastNodeIndex = desiredVal，否则不操作，
            //当_lastNodeIndex和initial 不相等的时候，说明_lastNodeIndex被修改了，退出循环。
            return computed;
        }
    }
}