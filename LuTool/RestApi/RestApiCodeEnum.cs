using System.ComponentModel;

namespace LuTool
{
    /// <summary>
    /// RestApi返回码
    /// </summary>
    public enum RestApiCodeEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        OK = 0,

        /// <summary>
        /// 服务器异常
        /// </summary>
        [Description("服务器异常")]
        ServerException = 1,

        /// <summary>
        /// 参数错误
        /// </summary>
        [Description("参数错误")]
        ParamsError = 2,
    }
}