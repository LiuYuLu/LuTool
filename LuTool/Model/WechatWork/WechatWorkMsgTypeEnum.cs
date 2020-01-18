using System.ComponentModel;

namespace LuTool
{
    /// <summary>
    /// 消息类型，0：text|1：image|3:textcard|4:news
    /// </summary>
    public enum WechatWorkMsgTypeEnum
    {
        /// <summary> 文本消息 </summary>
        [Description("文本消息")]
        Text = 0,

        /// <summary> 图片消息 </summary>
        [Description("图片消息")]
        Image = 1,

        /// <summary> 卡片消息 </summary>
        [Description("卡片消息")]
        TextCard = 3,

        /// <summary> 新闻消息 </summary>
        [Description("新闻消息")]
        News = 4
    }
}