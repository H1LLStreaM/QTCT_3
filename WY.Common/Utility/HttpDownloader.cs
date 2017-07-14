using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace WY.Common.Utility
{
    public class HttpDownloader : IDisposable
    {
        public const int MAX_RETRY_TIMES = 5;
        public const int TIMEOUT = 50000;

        private string _url;
        /// <summary>
        /// 服务器地址（不包括文件名）
        /// </summary>
        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                //if (!_url.EndsWith("/"))
                //{
                //    _url += "/";
                //}
            }
        }

        private string _filename;
        /// <summary>
        /// 文件名
        /// </summary>
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        private string _destFullPath;
        /// <summary>
        /// 目标文件全路径
        /// </summary>
        public string DestFullPath
        {
            get { return _destFullPath; }
            set { _destFullPath = value; }
        }

        private int _contentLenght;

        public int ContentLenght
        {
            get { return _contentLenght; }
            set { _contentLenght = value; }
        }

        private static bool _downloading;

        //public bool Downloading
        //{
        //    get { return _downloading; }
        //    set { _downloading = value; }
        //}

        private string _errormsg;

        public string Errormsg
        {
            get { return _errormsg; }
            set { _errormsg = value; }
        }

        protected WebClient client;
        protected int _retrytimes = 0;

        //开始下载
        public delegate void ProgressHandler(object sender, DownloadingEventArgs e);
        public event ProgressHandler BeforeDownload;

        //下载进度
        public event ProgressHandler Progress;

        //下载完成
        public event EventHandler DownloadComplete;

        //下载失败
        public event EventHandler DownloadFail;

        public delegate void ResponseStringCompleteHandler(object sender, string response);
        public event ResponseStringCompleteHandler ResponseStringComplete;

        public HttpDownloader(string url, string filename, string destfullpath)
        {
            this.Url = url;
            this.Filename = filename;
            this.DestFullPath = destfullpath;
        }

        #region GetResponseString

        public string GetResponseString()
        {
            _downloading = true;

            try
            {
                client = new WebClient();
                using (Stream stream = client.OpenRead(new Uri(this._url + this._filename)))
                {
                    this._contentLenght = Convert.ToInt32(this.client.ResponseHeaders["Content-Length"]);
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                this.Errormsg = ex.Message;
                return string.Empty;
            }
            finally
            {
                _downloading = false;
            }
        }

        public bool StartGetResponseString()
        {
            while (_downloading)
            {
                System.Threading.Thread.Sleep(100);
            }

            _downloading = true;

            try
            {
                _retrytimes = 0;

                //debug
                //Log.Info("debug.log", "start download : " + this._filename);

                //开始下载
                client = new WebClient();
                client.OpenReadCompleted += new OpenReadCompletedEventHandler(GetResponseString_OpenReadCompleted);
                client.OpenReadAsync(new Uri(this._url + System.Web.HttpUtility.UrlEncode(this._filename)));

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                this.Errormsg = ex.Message;
                return false;
            }
        }

        private void GetResponseString_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                //debug
                //Log.Info("debug.log", "open read completed : " + this._filename);

                //if (this.client.ResponseHeaders == null || this.client.ResponseHeaders["Content-Length"] == null)
                //{
                //    this.Retry();
                //    return;
                //}

                this._contentLenght = Convert.ToInt32(this.client.ResponseHeaders["Content-Length"]);


                Stream stream = e.Result;
                StreamReader sr = new StreamReader(stream);

                string response = sr.ReadToEnd();

                stream.Close();
                sr.Close();

                _downloading = false;

                if (ResponseStringComplete != null)
                {
                    this.ResponseStringComplete(this, response);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                //throw ex;
                if (DownloadFail != null)
                {
                    this.DownloadFail(this, new EventArgs());
                }
                _downloading = false;
            }
        }
        #endregion

        public bool Download()
        {
            _downloading = true;

            try
            {
                client = new WebClient();
                using (Stream stream = client.OpenRead(new Uri(this._url + this._filename)))
                {
                    this._contentLenght = Convert.ToInt32(this.client.ResponseHeaders["Content-Length"]);
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        byte[] mbyte = new byte[this._contentLenght];
                        int allbyte = (int)mbyte.Length;
                        int startbyte = 0;

                        while (this._contentLenght > 0)
                        {
                            try
                            {
                                int m = stream.Read(mbyte, startbyte, allbyte);
                                if (m == 0) { break; }
                                startbyte += m;
                                allbyte -= m;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                return false;
                            }
                        }

                        using (FileStream fs = new FileStream(this._destFullPath, FileMode.Create))
                        {
                            fs.Write(mbyte, 0, mbyte.Length);
                            fs.Flush();
                        }
                    }
                }
                //stream.Close();
                //sr.Close();
                //fs.Close();

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                this.Errormsg = ex.Message;
                return false;
            }
            finally
            {
                _downloading = false;
            }
        }

        public bool StartDownload()
        {
            while (_downloading)
            {
                System.Threading.Thread.Sleep(100);
            }

            _downloading = true;

            try
            {
                _retrytimes = 0;

                //debug
                //Log.Info("debug.log", "start download : " + this._filename);

                //开始下载
                client = new WebClient();
                client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
                client.OpenReadAsync(new Uri(this._url + System.Web.HttpUtility.UrlEncode(this._filename)));

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                this.Errormsg = ex.Message;
                return false;
            }
        }

        private void Retry()
        {
            if (_retrytimes < MAX_RETRY_TIMES)
            {
                _retrytimes++;
                client.OpenReadAsync(new Uri(this._url + this._filename));
            }
            else
            {
                if (DownloadFail != null)
                {
                    //Log.Error("Retry times:" + _retrytimes);
                    this.DownloadFail(this, new EventArgs());
                }
                _downloading = false;
            }
        }

        public void CancelDownload()
        {
            try
            {
                if (client != null)
                {
                    client.CancelAsync();
                }
                _downloading = false;
            }
            catch { }
        }

        private void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                //debug
                //Log.Info("debug.log", "open read completed : " + this._filename);

                if (this.client.ResponseHeaders == null || this.client.ResponseHeaders["Content-Length"] == null)
                {
                    this.Retry();
                    return;
                }

                this._contentLenght = Convert.ToInt32(this.client.ResponseHeaders["Content-Length"]);

                if (BeforeDownload != null)
                {
                    this.BeforeDownload(this, new DownloadingEventArgs(this._contentLenght, 0, this._filename, this._destFullPath));
                }

                Stream stream = e.Result;
                StreamReader sr = new StreamReader(stream);
                byte[] mbyte = new byte[this._contentLenght];
                int allbyte = (int)mbyte.Length;
                int startbyte = 0;

                while (this._contentLenght > 0)  //################   循环读取文件,并显示进度.....
                {
                    try
                    {
                        int m = stream.Read(mbyte, startbyte, allbyte);
                        if (m <= 0) { break; }
                        startbyte += m;
                        allbyte -= m;

                        if (Progress != null)
                        {
                            //显示进度
                            this.Progress(this, new DownloadingEventArgs((int)this._contentLenght, startbyte, this._filename, this._destFullPath));
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        if (DownloadFail != null)
                        {
                            this.DownloadFail(this, new EventArgs());
                        }
                        _downloading = false;
                        return;
                    }
                }

                FileStream fs = new FileStream(this._destFullPath, FileMode.Create);
                fs.Write(mbyte, 0, mbyte.Length);
                fs.Flush();

                stream.Close();
                sr.Close();
                fs.Close();

                _downloading = false;

                if (DownloadComplete != null)
                {
                    this.DownloadComplete(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                //throw ex;
                if (DownloadFail != null)
                {
                    this.DownloadFail(this, new EventArgs());
                }
                _downloading = false;
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }

            System.GC.SuppressFinalize(this);
        }

        #endregion
    }

    public class DownloadingEventArgs : EventArgs
    {
        private int _downloadedLength;
        /// <summary>
        /// 已下载的字节数
        /// </summary>
        public int DownloadedLength
        {
            get { return _downloadedLength; }
            set { _downloadedLength = value; }
        }

        private int _contentLenght;
        /// <summary>
        /// 总字节数
        /// </summary>
        public int ContentLenght
        {
            get { return _contentLenght; }
            set { _contentLenght = value; }
        }

        private string _filename;
        /// <summary>
        /// 文件名（不包含路径）
        /// </summary>
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        private string _destFullPath;
        /// <summary>
        /// 目标文件全路径
        /// </summary>
        public string DestFullPath
        {
            get { return _destFullPath; }
            set { _destFullPath = value; }
        }

        public DownloadingEventArgs(int contentLenght, int downloadedLength, string filename, string destfullpath)
        {
            this._contentLenght = contentLenght;
            this._downloadedLength = downloadedLength;
            this._filename = filename;
            this._destFullPath = destfullpath;
        }
    }
}
