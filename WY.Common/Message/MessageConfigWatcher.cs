using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Web;

namespace KZ.Common
{
    /// <summary>
    /// Message�����ļ��۲���,����ļ��б仯�����¼��ػ�������
    /// </summary>
    public sealed class MessageConfigWatcher : ConfigWatcher
    {
        /// <summary>
        /// ��ʼ�۲�
        /// </summary>
        public static void StartWatching()
        {
            new MessageConfigWatcher(new FileInfo(HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath + "/Message.config")));
        }

        private MessageConfigWatcher(FileInfo fileInfo)
            : base(fileInfo)
        {
        }

        /// <summary>
        /// �ļ��仯�������Ĵ����¼�
        /// </summary>
        /// <param name="state"></param>
        protected override void OnWatchedFileChange(object state)
        {
            Message.Reload();
        }
    }
}
