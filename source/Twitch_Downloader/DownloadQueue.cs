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
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;

namespace Twitch_Downloader
{
    /**
     * Queue a list of files to download
     */
    class DownloadQueue
    {
        private object checkThreadsLock = new object();

        private int _maxThreads;
        private Array _list;
        private int _filesCompleted = 0;

        private List<ProgressBar> _progressBars;
        private List<Label> _labels;
        private List<DownloadThread> _threads;

        public delegate void SetStringCallback(String line);
        delegate void SetProgressBarCallback(ProgressBar progressBar, int min, int max, int value);
        delegate void SetLabelTextCallback(Label label, String text);
        public delegate void CompleteCallback();
        public delegate void SetProgressCallback(float percent);

        private SetStringCallback _addStatusLine;
        private SetProgressCallback _taskbarProgress;

        /**
         * Start downloading a list of files in the background.
         * @param Array list List of files (string)
         * @param List<ProgressBar> progressBars List of form progress bars.
         * @param List<Label> labels List of form labels.
         * @param int maxThreads Maximum number of concurrent downloads.
         * @param SetProgressCallback callback Call this method when the progress of the entire queue needs updating (used for Taskbar progress in most cases).
         */
        public DownloadQueue(Array list, List<ProgressBar> progressBars, List<Label> labels, int maxThreads, SetProgressCallback callback)
        {
            _list = list;
            _maxThreads = maxThreads;
            _progressBars = progressBars;
            _labels = labels;
            _taskbarProgress = callback;

            DownloadThread.maxConcurrentDownloads = maxThreads;
        }

        /**
         * Start queue thread
         */
        public void download(CompleteCallback complete)
        {

            // start thread
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(_run);
            bg.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => {
                complete();
            };
            bg.RunWorkerAsync();

        }

        /**
         * Check for an open slot for a concurrent download.
         */
        private int _findOpenThread()
        {
            int ret = -1;
            lock (checkThreadsLock)
            {
                for (int i = 0; i < _maxThreads; i++)
                {
                    if (_threads[i] == null)
                    {
                        ret = i;
                        break;
                    }
                }
            }
            return ret;
        }

        /**
         * Handle download in Background Worker thread.
         */
        private void _run(object sender, DoWorkEventArgs e)
        {

            _threads = new List<DownloadThread>();
            for (int i = 0; i < _maxThreads; i++) {
                _threads.Add(null);
            }

            _taskbarProgress(0);


            // run download
            int fileNumber = 0;
            foreach (String file in _list)
            {
                fileNumber++;

                int threadId = 0;
                while ((threadId = _findOpenThread()) == -1) {
                    // update download progress
                    _updateProgress();

                    // waiting for an open thread
                    Thread.Sleep(500);
                }

                // reset progress bar
                _addStatusLine(fileNumber + "/" + _list.Length + " - Starting " + file + " in thread #" + (threadId + 1));
                _setProgressBar(_progressBars[threadId], 0, 10000, 0);
                _setLabelText(_labels[threadId], "Starting...");

                // start thread
                DownloadThread download = new DownloadThread(file, 0);

                lock (checkThreadsLock)
                {
                    _threads[threadId] = download;
                }

                download.download((String savedFile, int savedError) =>
                {
                    if (savedError == 1)
                    {
                        // show error
                        _addStatusLine("DOWNLOAD FAILED - Failed to download " + file + ".");
                    }
                    else {
                        lock (checkThreadsLock)
                        {
                            // free up thread
                            _threads[threadId] = null;
                        }

                    }

                    _filesCompleted++;

                    // change status
                    _setProgressBar(_progressBars[threadId], 0, 100, 0);
                    _setLabelText(_labels[threadId], "");

                });

            }

            // wait for last thread
            bool threadFound = true;
            while (threadFound)
            {
                _updateProgress();

                threadFound = false;

                lock (checkThreadsLock)
                {
                    for (int i = 0; i < _maxThreads; i++)
                    {
                        if (_threads[i] != null)
                        {
                            threadFound = true;
                            break;
                        }
                    }
                }

                Thread.Sleep(500);
            }

            // done all downloads
            for (int i = 0; i < _maxThreads; i++)
            {
                _setProgressBar(_progressBars[i], 0, 10000, 0);
                _setLabelText(_labels[i], "");
            }

            // close thread

        }

        /**
         * Update Taskbar progress
         */
        private void _updateProgress()
        {

            float filesLength = (float)_list.Length;
            float perFile = 1 / filesLength;
            float percentage = (float)(_filesCompleted) / filesLength;

            // add percentage of currently download files
            lock (checkThreadsLock)
            {

                for (int i = 0; i < _maxThreads; i++)
                {
                    DownloadThread download = _threads[i];
                    if (download == null)
                    {
                        continue;
                    }

                    // form progress
                    _setProgressBar(_progressBars[i], 0, 10000, (int)(download.getProgressPercent() * 10000));
                    _setLabelText(_labels[i], "Downloading: " + (int)download.getProgressRate() + " KB/s");

                    // taskbar progress
                    percentage += (perFile * download.getProgressPercent());
                }

            }

            percentage = Math.Max(0, percentage);

            // call method
            _taskbarProgress(percentage);

            //Console.WriteLine(percentage);

        }

        /**
         * Update label for this download.
         */
        private void _setLabelText(Label label, String text)
        {
            if (label.InvokeRequired)
            {
                SetLabelTextCallback d = new SetLabelTextCallback(_setLabelText);
                label.Invoke(d, new object[] { label, text });
            }
            else
            {
                label.Text = text;
            }
        }

        /**
         * Update progressbar for this download.
         */
        private void _setProgressBar(ProgressBar progressBar, int min, int max, int value)
        {
            if (progressBar.InvokeRequired)
            {
                SetProgressBarCallback d = new SetProgressBarCallback(_setProgressBar);
                progressBar.Invoke(d, new object[] { progressBar, min, max, value });
            }
            else
            {
                progressBar.Minimum = min;
                progressBar.Maximum = max;
                progressBar.Value = value;
            }
        }

        public void setStatusCallback(SetStringCallback callback)
        {
            _addStatusLine = callback;
        }


    }
}
