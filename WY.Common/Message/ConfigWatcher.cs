using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace KZ.Common
{
    /// <summary>
    /// 文件观察器的基类
    /// </summary>
    public abstract class ConfigWatcher
    {
        /// <summary>
        /// 需要观察的文件信息
        /// </summary>
        protected FileInfo configFile;

        /// <summary>
        /// Timer实例
        /// </summary>
        protected Timer timer;

        /// <summary>
        /// 执行方法的时间间隔
        /// </summary>
        protected const int TimeoutMillis = 500;

        /// <summary>
        /// 构造函数
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
        /// 文件改变的代理事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void ConfigureAndWatchHandler_OnChanged(object source, FileSystemEventArgs e)
        {
            timer.Change(TimeoutMillis, Timeout.Infinite);
        }

        /// <summary>
        /// 文件重名名的代理事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void ConfigureAndWatchHandler_OnRenamed(object source, RenamedEventArgs e)
        {
            timer.Change(TimeoutMillis, Timeout.Infinite);
        }

        /// <summary>
        /// 文件变化而触发的代理事件
        /// </summary>
        /// <param name="state"></param>
        protected abstract void OnWatchedFileChange(object state);
    }
}
