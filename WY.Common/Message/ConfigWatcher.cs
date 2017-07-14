using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace KZ.Common
{
    /// <summary>
    /// �ļ��۲����Ļ���
    /// </summary>
    public abstract class ConfigWatcher
    {
        /// <summary>
        /// ��Ҫ�۲���ļ���Ϣ
        /// </summary>
        protected FileInfo configFile;

        /// <summary>
        /// Timerʵ��
        /// </summary>
        protected Timer timer;

        /// <summary>
        /// ִ�з�����ʱ����
        /// </summary>
        protected const int TimeoutMillis = 500;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="fileInfo"></param>
        protected ConfigWatcher(FileInfo fileInfo)
        {
            configFile = fileInfo;

            FileSystemWatcher watcher = new FileSystemWatcher();

            watcher.Path = configFile.DirectoryName;
            watcher.Filter = configFile.Name;

            watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName;

            watcher.Changed += new FileSystemEventHandler(ConfigureAndWatchHandler_OnChanged);
            watcher.Created += new FileSystemEventHandler(ConfigureAndWatchHandler_OnChanged);
            watcher.Deleted += new FileSystemEventHandler(ConfigureAndWatchHandler_OnChanged);
            watcher.Renamed += new RenamedEventHandler(ConfigureAndWatchHandler_OnRenamed);

            watcher.EnableRaisingEvents = true;

            timer = new Timer(new TimerCallback(OnWatchedFileChange), null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// �ļ��ı�Ĵ����¼�
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void ConfigureAndWatchHandler_OnChanged(object source, FileSystemEventArgs e)
        {
            timer.Change(TimeoutMillis, Timeout.Infinite);
        }

        /// <summary>
        /// �ļ��������Ĵ����¼�
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void ConfigureAndWatchHandler_OnRenamed(object source, RenamedEventArgs e)
        {
            timer.Change(TimeoutMillis, Timeout.Infinite);
        }

        /// <summary>
        /// �ļ��仯�������Ĵ����¼�
        /// </summary>
        /// <param name="state"></param>
        protected abstract void OnWatchedFileChange(object state);
    }
}
