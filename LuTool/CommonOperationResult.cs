namespace LuTool
{
    /// <summary>
    /// 通用操作结果
    /// </summary>
    public class CommonOperationResult
    {
        /// <summary>
        /// 操作结果
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 操作返回消息
        /// </summary>
        public string Message { get; set; }

        public CommonOperationResult()
        {
            Result = true;
            Message = string.Empty;
        }

        public CommonOperationResult(string msg)
        {
            Result = false;
            Message = msg;
        }

        public CommonOperationResult(bool rslt, string msg = "")
        {
            Result = rslt;
            Message = msg;
        }

        public virtual CommonOperationResult Fail(string msg)
        {
            Result = false;
            Message = msg;
            return this;
        }

        public virtual CommonOperationResult Fill(bool rslt, string msg = "")
        {
            Result = false;
            Message = msg;
            return this;
        }
    }
}