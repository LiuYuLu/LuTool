using static Mvccpb.Event.Types;

namespace LuTool.Etcd
{
    /// <summary>
    /// WatchEvent class is used for retrieval of minimal
    /// data from watch events on etcd.
    /// </summary>
    public class WatchEvent
    {
        /// <summary>
        /// etcd Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// etcd value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// etcd watch event type (PUT,DELETE etc.)
        /// </summary>
        public EventType Type { get; set; }
    }
}