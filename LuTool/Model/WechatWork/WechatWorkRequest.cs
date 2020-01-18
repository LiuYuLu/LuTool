using System.Collections.Generic;

namespace LuTool
{
    /// <summary>
    /// 企业微信请求接口，接口地址申请请联系@崔亚丽
    /// </summary>
    public class WechatWorkRequest
    {
        /// <summary> 向讨论组或单人发送，0：单人 |1：讨论组 </summary>
        public int UserType { get; }

        /// <summary> 消息类型，0：text|1：image|3:textcard|4:news </summary>
        public int MsgType { get; }

        /// <summary> 讨论组的唯一标识，32位guid或接口返回的ChatID </summary>
        public string ChatId { get; set; }

        /// <summary> 讨论组名称 </summary>
        public string Name { get; set; }

        /// <summary> 文本消息|卡片消息内容|图文消息内容| </summary>
        public string Content { get; set; }

        /// <summary> 接受消息的用户(用户的工号) </summary>
        public List<string> UserList { get; set; }

        /// <summary> 卡片消息的标题|图文消息的标题 </summary>
        public string Title { get; set; }

        /// <summary> 图片的url|卡片消息的跳转地址|图文消息的跳转地址 </summary>
        public string Url { get; set; }

        /// <summary> 讨论组群主 </summary>
        public string Owner { get; set; }

        /// <summary> 附件地址，图片地址。文档地址，视频地址，音频地址 </summary>
        public string FileUrl { get; set; }
        
        /// <summary> 部门ID </summary>
        public string DepartId { get; set; }
        
        /// <summary> 任务卡片事件Key 标识后续具体审核接口 </summary>
        public string EventKey { get; set; }
        
        /// <summary> 任务卡片的taskid </summary>
        public string TaskId { get; set; }

        /// <summary> 发送消息的应用ID </summary>
        public int AgentId { get; set; } = 0;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="userType">向讨论组或单人发送，0：单人 |1：讨论组</param>
        /// <param name="msgType">消息类型，0：text|1：image|3:textcard|4:news</param>
        public WechatWorkRequest(int userType, int msgType)
        {
            this.UserType = userType;
            this.MsgType = msgType;
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="userType">向讨论组或单人发送，0：单人 |1：讨论组</param>
        /// <param name="msgType">消息类型，0：text|1：image|3:textcard|4:news</param>
        public WechatWorkRequest(WechatWorkUserTypeEnum userType, WechatWorkMsgTypeEnum msgType)
        {
            this.UserType = (int)userType;
            this.MsgType = (int)msgType;
        }
    }
}