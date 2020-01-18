using Etcdserverpb;
using V3Lockpb;

namespace LuTool.Etcd.Multiplexer
{
    internal class Connection
    {
        internal KV.KVClient kvClient;

        /// <summary> Watch客户端 </summary>
        internal Watch.WatchClient watchClient;

        internal Lease.LeaseClient leaseClient;

        internal Lock.LockClient lockClient;

        internal Cluster.ClusterClient clusterClient;

        internal Maintenance.MaintenanceClient maintenanceClient;

        internal Auth.AuthClient authClient;
    }
}