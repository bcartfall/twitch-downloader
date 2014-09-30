/**
 * Twitch Downloader
 * (c) 2014 Bryan Wiebe <bcartfall at yahoo dot com>
 * License: GPLv2
 * 
 * This file is part of Twitch Downloader.
 *
 *   Twitch Downloader is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   Twitch Downloader is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with Twitch Downloader.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.ComponentModel;
using System.IO;

namespace Twitch_Downloader
{
    class DownloadThread
    {
        private object progressLock = new object();
        private float _percent = 0;
        private float _rate = 0;
        private long _start = 0;

        private static int _ID = 0;
        public int id;
        private int _error = 0;

        private String _url;
        private int _type;

        private String _result = "";

        public delegate void SetCompleteCallback(String html, int error);
        private SetCompleteCallback _completeFunction;

        public static int maxConcurrentDownloads = 4;


        /**
         * Create a new download thread
         * param @url
         * param @type int 0 = File, 1 = String
         */
        public DownloadThread(String url, int type = 0)
        {
            _url = url;
            if (type == 0)
            {
                id = ++_ID;
            }
            _type = type;
        }

        /**
         * Download file in another thread.
         */
        public void download(SetCompleteCallback CompleteFunction)
        {
            Console.WriteLine("Starting new download: " + _url);
            _completeFunction = CompleteFunction;

            // start thread
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(_run);
            bg.RunWorkerCompleted += _completed;
            bg.RunWorkerAsync();

        }

        /**
         * Called when thread is completed.
         */
        private void _completed(object sender, RunWorkerCompletedEventArgs e)
        {
            // callback complete function
            _completeFunction(_result, _error);

        }

        /**
         * Run download in Background Worker thread.
         */
        private void _run(object sender, DoWorkEventArgs e)
        {
            _start = DateTime.UtcNow.Ticks;

            Uri uri = new Uri(_url);

            // change concurrent limit to 4
            ServicePoint sp = ServicePointManager.FindServicePoint(uri);
            sp.ConnectionLimit = maxConcurrentDownloads;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.UseDefaultCredentials = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2049.0 Safari/537.36"; // chrome 37.0.2049.0

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (WebException ex)
            {
                // Handle exception
                _error = 1;
                _result = "WebException: " + ex.Message;
            }

            Int64 totalSize = 0;

            if (response == null)
            {
                _error = 1;
                _result = "404 file not found.";
            }
            else
            {
                 totalSize = response.ContentLength;
                if (totalSize == -1)
                {
                    // sometimes it's -1 sometimes it's not
                    // is this cache?

                    //_error = 1;
                    //_result = "File return error or not found.";
                }

            }


            if (_error == 0)
            {
                Int64 bytesReceived = 0;

                WebClient client = new WebClient();
                Stream remote = client.OpenRead(uri);

                Stream local = null;

                if (_type == 0)
                {
                    String stringId = Convert.ToString(id);
                    String filepath = "downloads/" + stringId.PadLeft(8, '0') + ".flv";
                    _result = filepath;
                    local = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None);
                }
                else
                {
                    local = new MemoryStream();
                }

                // loop the stream and get the file into the byte buffer
                byte[] buffer = new byte[65536];
                int currentBytes = 0;
                while ((currentBytes = remote.Read(buffer, 0, buffer.Length)) > 0)
                {
                    bytesReceived += currentBytes;

                    // write to local stream
                    local.Write(buffer, 0, currentBytes);

                    // update progress
                    float percent = (float)bytesReceived / (float)totalSize;
                    _onProgress(percent, bytesReceived);

                    //Console.WriteLine("percent = " + percent + ", currentBytes=" + currentBytes + ", " + "bytesReceived=" + bytesReceived + ", totalSize=" + totalSize);
                }

                if (_type == 1)
                {
                    // save memory stream as string
                    StreamReader r = new StreamReader(local);
                    local.Position = 0;
                    String s = r.ReadToEnd();

                    _result = s;

                }

                remote.Close();
                local.Close();

            }

            // close thread

        }

        /**
         * Update progress for this download.
         */
        private void _onProgress(float percent, long bytesReceived)
        {
            // mutex
            lock (progressLock) {
                _percent = percent;

                long ticks = DateTime.UtcNow.Ticks;
                long elapsed = ((ticks - _start) / (TimeSpan.TicksPerMillisecond * 1000));

                if (elapsed > 0) {
                    _rate = (bytesReceived / 1024) / elapsed;
                }
                else
                {
                    _rate = 0;
                }
                
            }
            
        }

        /**
         * Get progress of this download.
         */
        public float getProgressPercent()
        {
            float p;
            lock (progressLock) {
                p = _percent;
            }
            return p;
        }

        /**
         * Get rate of this download.
         */
        public float getProgressRate()
        {
            lock (progressLock)
            {
                return _rate;
            }
        }



    }

}
