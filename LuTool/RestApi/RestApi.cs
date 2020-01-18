namespace LuTool
{
    /// <summary>
    /// 基础RestApi返回实体
    /// </summary>
    public class RestApi
    {
        /// <summary>
        /// 是否处理成功
        /// </summary>
        public bool Result
        {
            get
            {
                return Code == 0;
            }
        }

        /// <summary>
        /// 返回状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 默认构造返回
        /// </summary>
        public RestApi()
        {
            this.Code = (int)RestApiCodeEnum.OK;
            this.Msg = string.Empty;
        }

        /// <summary>
        /// 错误数据返回
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="msg">返回消息</param>
        public RestApi(RestApiCodeEnum code, string msg)
        {
            this.Code = (int)code;
            this.Msg = msg;
        }

        /// <summary>
        /// 错误数据返回
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="msg">返回消息</param>
        public RestApi(int code, string msg)
        {
            this.Code = code;
            this.Msg = msg;
        }

        /// <summary>
        /// 错误数据返回
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="msg">返回消息</param>
        public RestApi(bool success, string msg)
        {
            this.Code = success ? 0 : -1;
            this.Msg = msg;
        }
    }

    /// <summary>
    /// 标准数据对象返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RestApi<T> : RestApi
    {
        /// <summary>
        /// 返回实体数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 构造返回数据
        /// </summary>
        public RestApi()
        {
            this.Data = default(T);
            this.Code = (int)RestApiCodeEnum.OK;
            this.Msg = string.Empty;
        }

        /// <summary>
        /// 构造返回数据
        /// </summary>
        /// <param name="t">数据</param>
        public RestApi(T t)
        {
            this.Data = t;
            this.Code = (int)RestApiCodeEnum.OK;
            this.Msg = string.Empty;
        }

        /// <summary>
        /// 构造返回数据
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="msg">返回消息</param>
        /// <param name="t">数据</param>
        public RestApi(RestApiCodeEnum code, string msg, T t)
        {
            this.Code = (int)code;
            this.Msg = msg;
            this.Data = t;
        }

        /// <summary>
        /// 错误数据返回
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="msg">返回消息</param>
        public RestApi(int code, string msg, T t)
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = t;
        }

        /// <summary>
        /// 错误数据返回
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="msg">返回消息</param>
        public RestApi(bool success, string msg, T t)
        {
            this.Code = success ? 0 : -1;
            this.Msg = msg;
            this.Data = t;
        }
    }
}