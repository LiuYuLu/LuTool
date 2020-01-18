using System.ComponentModel;

namespace LuTool
{
    /// <summary>
    /// 向讨论组或单人发送
    /// </summary>
    public enum WechatWorkUserTypeEnum
    {
        /// <summary> 单人 </summary>
        [Description("单人")]
        Single = 0,

        /// <summary> 讨论组 </summary>
        [Description("讨论组")]
        Discuss = 1
    }
}