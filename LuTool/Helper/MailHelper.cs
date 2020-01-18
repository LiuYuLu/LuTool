using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace LuTool
{
    public class MailHelper
    {

        /// <summary>
        /// 向用户发送邮件
        /// </summary>
        /// <param name="receive">接收邮件的用户</param>
        /// <param name="sendUser">发送者显求的邮箱地址,可为空</param>
        /// <param name="displayName">收件人显示发件人的联系人名，可为中文</param>
        /// <param name="sendUserName">发送者的邮箱登陆名，可以与发送者地址一样</param>
        /// <param name="userPassword">发送者的登陆密码</param>
        /// <param name="mailTitle">发送标题</param>
        /// <param name="mailContent">发送的内容</param>
        /// <param name="cc">抄送人</param>
        public static void SendMail(List<string> receive, string displayName, string sendUser, string sendUserName, string userPassword, string mailTitle, string mailContent, List<string> cc =null)
        {
            MailAddress fromMail = new MailAddress(sendUser, displayName);//发送者邮箱       
            MailMessage mail = new MailMessage()
            {
                From = fromMail,
                Subject = mailTitle,
                IsBodyHtml = true,//是否支持HTML
                Body = mailContent
            };

            receive.ForEach(m => mail.To.Add(m));
            if (cc.IsValuable()) cc.ForEach(m => mail.CC.Add(m));

            SmtpClient client = new SmtpClient
            {
                Host = "smtp.exmail.qq.com",//设置发送者邮箱对应的smtpserver
                UseDefaultCredentials = false,
                //Port = 465,
                Credentials = new NetworkCredential(sendUserName, userPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            try
            {
                client.Send(mail);
            }
            catch (SmtpException ex)
            {
                throw new Exception("发送邮件信息异常", ex);
            }
        }
    }
}
