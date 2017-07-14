using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;

namespace WY.Common.Utility
{
    /// <summary>
    /// MailSender 的摘要说明
    /// </summary>
    public class MailSender
    {
        private const string MAIL_XML = "MailServer.xml";
        private static MailServer smtpserver = null;

        public MailSender()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 发送Html邮件
        /// </summary>
        /// <param name="mailto"></param>
        /// <param name="mailsubject"></param>
        /// <param name="mailbody"></param>
        public static void SendHtmlMail(string mailto, string mailsubject, string mailbody)
        {
            //mailto = "xuxx@we-win.com.cn";

            if (smtpserver == null)
            {
                LoadMailServer();
            }

            //try
            //{
            //    MailMessage mailobj = new MailMessage();
            //    mailobj.BodyFormat = MailFormat.Html;
            //    mailobj.Priority = MailPriority.Normal;
            //    mailobj.Subject = mailsubject;
            //    mailobj.Body = mailbody;
            //    mailobj.To = mailto;
            //    mailobj.From = smtpserver.Mailfrom;

            //    mailobj.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", 1);
            //    //set the user to be certified
            //    mailobj.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusername", smtpserver.Username);
            //    //the password of the account
            //    mailobj.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtppassword", smtpserver.Password);

            //    SmtpMail.SmtpServer = smtpserver.Smtp;
            //    SmtpMail.Send(mailobj);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            try
            {
                MailMessage mailmsg = new MailMessage();
                mailmsg.From = new MailAddress(smtpserver.Mailfrom, "hrschina");
                //mailmsg.To = new MailAddressCollection();
                mailmsg.To.Add(mailto);
                mailmsg.Subject = mailsubject;
                //mailmsg.SubjectEncoding = System.Text.Encoding.UTF8;
                mailmsg.Body = mailbody;
                //mailmsg.Priority = MailPriority.High;
                mailmsg.IsBodyHtml = true;
                //mailmsg.BodyEncoding = System.Text.Encoding.UTF8;

                SmtpClient smtp = new SmtpClient(smtpserver.Smtp);
                smtp.Credentials = new NetworkCredential(smtpserver.Username, smtpserver.Password);
                //smtp.Port = smtpserver.Port;
                //smtp.EnableSsl = true;
                smtp.Send(mailmsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

            //try
            //{
            //    CDO.Message oMsg = new CDO.Message();
            //    oMsg.To = mailto;
            //    oMsg.Subject = mailsubject;
            //    oMsg.HTMLBody = mailbody;
            //    oMsg.CC = "";

            //    CDO.IConfiguration iConfg;
            //    ADODB.Fields oFields;
            //    iConfg = oMsg.Configuration;
            //    oFields = iConfg.Fields;

            //    //oFields["http://schemas.microsoft.com/cdo/configuration/sendusing"].Value = 2;
            //    oFields["http://schemas.microsoft.com/cdo/configuration/sendemailaddress"].Value = smtpserver.Mailfrom;
            //    oFields["http://schemas.microsoft.com/cdo/configuration/smtpaccountname"].Value = smtpserver.Mailfrom;
            //    oFields["http://schemas.microsoft.com/cdo/configuration/sendusername"].Value = smtpserver.Username;
            //    oFields["http://schemas.microsoft.com/cdo/configuration/sendpassword"].Value = smtpserver.Password;
            //    oFields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"].Value = 1;
            //    oFields["http://schemas.microsoft.com/cdo/configuration/smtpserver"].Value = smtpserver.Smtp;
            //    oFields.Update();

            //    oMsg.Send();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    throw ex;
            //}
        }

        /// <summary>
        /// 获取邮件服务器设置
        /// </summary>
        private static void LoadMailServer()
        {
            using (FileStream fs = new FileStream(Application.StartupPath + "\\" + MAIL_XML, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MailServer));
                smtpserver = (MailServer)serializer.Deserialize(fs);
            }
        }
    }

    public class MailServer
    {
        private string _smtp;
        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        public string Smtp
        {
            get { return _smtp; }
            set { _smtp = value; }
        }

        private int _port;
        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        private string _mailfrom;
        /// <summary>
        /// 发件人
        /// </summary>
        public string Mailfrom
        {
            get { return _mailfrom; }
            set { _mailfrom = value; }
        }

        private string _displayname;
        /// <summary>
        /// 发件人显示名
        /// </summary>
        public string Displayname
        {
            get { return _displayname; }
            set { _displayname = value; }
        }

        private string _username;
        /// <summary>
        /// 发件人（身份验证）
        /// </summary>
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _password;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }
}
