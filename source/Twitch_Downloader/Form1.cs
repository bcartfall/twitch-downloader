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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.IO;
using NReco.VideoConverter;
using System.Runtime.InteropServices;

namespace Twitch_Downloader
{
    public partial class mainForm : Form
    {

        public delegate void DownloadComplete(String html, int error);
        delegate void SetTextCallback(string text);
        delegate void SetBoolCallback(bool value);
        private FFMpegConverter _ffmpeg;

        public mainForm()
        {
            InitializeComponent();
        }

        /**
         * Set enabled status of Download and View buttons in form.
         */
        public void setFormUIEnabled(bool enabled)
        {
            if (downloadButton.InvokeRequired)
            {
                SetBoolCallback d = new SetBoolCallback(setFormUIEnabled);
                downloadButton.Invoke(d, new object[] { enabled });
            }
            else
            {
                downloadButton.Enabled = viewButton.Enabled = vodURL.Enabled = enabled;
            }
        }

        /**
         * Clear text console
         */
        public void clear()
        {
            status.Text = "";
        }

        /**
         * Add line to text console
         */
        public void addLine(String line)
        {
            if (status.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(addLine);
                status.Invoke(d, new object[] { line });
            }
            else
            {
                status.Text += line + Environment.NewLine;

                // scroll to bottom
                status.SelectionStart = status.Text.Length;
                status.ScrollToCaret();

            }
        }

        /**
         * Download list of media files from vod URL
         */
        public void getFilesList(DownloadComplete Complete)
        {

            String url = vodURL.Text;
            DownloadThread Download = new DownloadThread(url, 1);
            Download.download((String html, int error) =>
            {

                Boolean found = false;
                String videoId = "";

                if (error == 1)
                {
                    Complete(html, 1);
                }
                else
                {

                    Array result = Regex.Split(html, @"\r?\r|\n");

                    foreach (String line in result)
                    {
                        if (line.Contains("og:video"))
                        {
                            int p = line.IndexOf("videoId=");
                            found = true;

                            if (p == -1)
                            {
                                continue;
                            }

                            p += "videoId=".Length - 1;

                            while (p++ < line.Length - 1)
                            {
                                if (line[p] == '&')
                                {
                                    break;
                                }
                                videoId += line[p];
                            }

                            break;
                        }
                    }

                    if (!found || videoId == "")
                    {
                        Complete("Supplied URL does not appear to have a Twitch VOD.", 1);
                    }

                    // get json from video id
                    String jsonUrl = "https://api.twitch.tv/api/videos/" + videoId + "?as3=t";

                    new DownloadThread(jsonUrl, 1).download((String json, int jsonError) =>
                    {

                        String videos = "";

                        // {
                        // "chunks": {
                        //       "live": {
                        //             "url": ...,
                        //             "length": 0...,
                        //       }
                        // }

                        try
                        {
                            Dictionary<String, dynamic> root = JsonConvert.DeserializeObject<Dictionary<String, dynamic>>(json);

                            int currentOffset = 0;
                            int startOffset = Convert.ToInt32(root["start_offset"]);
                            int endOffset = Convert.ToInt32(root["end_offset"]);

                            List<dynamic> live = JsonConvert.DeserializeObject<List<dynamic>>(Convert.ToString(root["chunks"].live));

                            List<int> starts = new List<int>();
                            List<int> ends = new List<int>();
                            List<String> urls = new List<String>();

                            int length = 0;
                            foreach (dynamic d in live)
                            {
                                Dictionary<String, dynamic> video = JsonConvert.DeserializeObject<Dictionary<String, dynamic>>(Convert.ToString(d));
                                int vlength = Convert.ToInt32(video["length"]);
                                String vurl = Convert.ToString(video["url"]);

                                starts.Add(currentOffset);

                                currentOffset += vlength;

                                ends.Add(vlength);

                                urls.Add(vurl);

                                length++;

                            }

                            // add only videos in range
                            bool started = false;
                            for (int i = 0; i < length;i++ )
                            {
                                if (started)
                                {
                                    videos += urls[i] + Environment.NewLine;

                                    // find end
                                    if (ends[i] >= endOffset)
                                    {
                                        // no more in sequence
                                        break;
                                    }

                                }
                                else
                                {
                                    // find start
                                    if (starts[i] >= startOffset)
                                    {
                                        started = true;
                                        videos += urls[i] + Environment.NewLine;
                                    }
                                }
                            }

                            // return videos
                            Complete(videos.Trim(), 0);

                        }
                        catch (Exception ex)
                        {
                            Complete("Error getting json from Video Id #" + videoId + ".", 1);
                        }


                    });

                }

            });
            
        }

        /**
         * OnLoad Main Form
         * Make sure download folder exists, clear all existing files in download folder. Load default settings.
         */
        private void mainForm_Load(object sender, EventArgs e)
        {
            String downloadPath = Directory.GetCurrentDirectory() + "\\downloads\\";
            if (!Directory.Exists(downloadPath))
            {
                // create download directory
                addLine("Creating directory " + downloadPath + ".");
                Directory.CreateDirectory(downloadPath);
            }
            else
            {
                // delete all files in download directory
                DirectoryInfo directory = new DirectoryInfo(downloadPath);
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
            }
            addLine("Saving files to " + downloadPath + ".");

            if (File.Exists("list.txt"))
            {
                File.Delete("list.txt");
            }
           
            // Test concat
            //concatVideoFiles(Directory.GetCurrentDirectory() + "\\concat\\");
            //concatVideoFiles(Directory.GetCurrentDirectory() + "\\downloads\\");

            // set checked on video container setting
            checkVideoContainer();

        }

        /**
         * Error check vodURL form control
         */
        String vodURLErrorCheck()
        {

            if (vodURL.Text.Trim() == "")
            {
                return "Vod URL field is empty.";
            }
            if (vodURL.Text.Substring(0, 7) != "http://" && vodURL.Text.Substring(0, 8) != "https://")
            {
                return "Vod URL expecting http:// or https://.";
            }

            return "";
        }

        /**
         * Start download of vods
         */
        private void downloadButton_Click(object sender, EventArgs e)
        {
            // error check
            String result = vodURLErrorCheck();
            if (result != "")
            {
                MessageBox.Show(result);
                return;
            }

            clear();

            addLine("Fetching files list.");
            setFormUIEnabled(false);

            // disable sleep mode
            NativeMethods.SetThreadExecutionState(NativeMethods.ES_CONTINUOUS | NativeMethods.ES_SYSTEM_REQUIRED);

            // get list of files to download
            getFilesList((String filesList, int error) =>
            {

                addLine("");
                addLine(filesList);
                addLine("");

                // test download
                /*
                filesList =
                    "" 
                    + "http://www.mediacollege.com/video-gallery/testclips/20051210-w50s.flv" + Environment.NewLine
                    + "http://www.mediacollege.com/video-gallery/testclips/20051210-w50s.flv" + Environment.NewLine
                    + "http://www.mediacollege.com/video-gallery/testclips/20051210-w50s.flv" + Environment.NewLine
                    + "http://www.mediacollege.com/video-gallery/testclips/20051210-w50s.flv" + Environment.NewLine
                    + "http://www.mediacollege.com/video-gallery/testclips/20051210-w50s.flv" + Environment.NewLine
                    + "http://www.mediacollege.com/video-gallery/testclips/20051210-w50s.flv" + Environment.NewLine
                    + "";
                 */

                if (error == 0)
                {
                    // no error
                    Array list = Regex.Split(filesList.Trim(), Environment.NewLine);
                    DownloadQueue queue = new DownloadQueue(
                        list, 
                        new List<ProgressBar> {progressBar1, progressBar2, progressBar3, progressBar4}, 
                        new List<Label> {rate1, rate2, rate3, rate4},  
                        4, // max threads
                        onQueueProgress
                    ); 
                    queue.setStatusCallback(addLine);
                    queue.download(() =>
                    {
                        onQueueProgress(100);
                        addLine("Complete.");
                        addLine("");

                        concatVideoFiles();

                    });

                }

            });

        }

        /**
         * Clear taskbar progress
         */
        public void clearQueueProgress()
        {
            var prog = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
            prog.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.NoProgress);
        }

        /**
         * Update taskbar progress
         */
        public void onQueueProgress(float percent)
        {
            var prog = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
            prog.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.Normal);
            prog.SetProgressValue((int)(percent * 100000), 100000);
        }

        /**
         * Merge downloaded video files using ffmpeg into one big file container
         */
        public void concatVideoFiles(String downloadPath = "")
        {

            if (downloadPath == "")
            {
                downloadPath = Directory.GetCurrentDirectory() + "\\downloads\\";
            }

            addLine("Concatenating movie files.");

            // delete all files in download directory
            DirectoryInfo directory = new DirectoryInfo(downloadPath);

            FileInfo[] files = directory.GetFiles();
            String[] filenames = new String[files.Length]; // up to 128 files

            int i = 0;
            String listTxt = "";
            foreach (FileInfo file in directory.GetFiles())
            {
                Console.WriteLine("Adding " + file.FullName + " to list.txt");
                filenames[i++] = file.FullName;
                listTxt += "file '" + file.FullName + "'" + Environment.NewLine;
            }

            // backup concat.mp4
            String concatFolder = Directory.GetCurrentDirectory() + "\\concat\\";
            if (!Directory.Exists(concatFolder))
            {
                Directory.CreateDirectory(concatFolder);
            }

            String backupFile = concatFolder + "video.backup." + Properties.Settings.Default.VideoContainer;
            String videoFile = concatFolder + "video." + Properties.Settings.Default.VideoContainer;

            if (File.Exists(backupFile))
            {
                // remove backup
                File.Delete(backupFile);
            }
            if (File.Exists(videoFile))
            {
                // backup video file
                File.Move(videoFile, backupFile);
            }

            File.WriteAllText("list.txt", listTxt);

            _ffmpeg = new FFMpegConverter();

            _ffmpeg.ConvertProgress += ffmpeg_ConvertProgress;

            rate1.Text = "Concatenating.";
            progressBar1.Value = 100;

            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(_concatThread);
            bg.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => {
                progressBar1.Value = 0;
                rate1.Text = "";

                // check if succesfull
                if (File.Exists(videoFile))
                {
                    // open up folder
                    Process.Start("explorer.exe", concatFolder);

                    addLine("Concatenating completed.");
                    addLine("Video file: " + videoFile);

                }

                completeDownload();
            };
            bg.RunWorkerAsync();

        }

        /**
         * Concatenation happens in background worker thread.
         */
        private void _concatThread(object sender, DoWorkEventArgs e)
        {
            try
            {
                String videoFile = Directory.GetCurrentDirectory() + "\\concat\\video." + Properties.Settings.Default.VideoContainer;
                _ffmpeg.Invoke("-f concat -i \"list.txt\" -c copy \"" + videoFile + "\"");
            }
            catch (FFMpegException ex)
            {
                addLine(ex.Message);
            }

            clearQueueProgress();
            setFormUIEnabled(true);
            
        }

        /**
         * Update ffmpeg convert progress. This methods is not supported in concatenation it seems.
         */
        void ffmpeg_ConvertProgress(object sender, ConvertProgressEventArgs e)
        {
            Console.WriteLine(e.Processed);
        }

        /**
         * Fetch list of available vod videos to test if Vod URL is valid
         */
        private void viewButton_Click(object sender, EventArgs e)
        {

            // error check
            String result = vodURLErrorCheck();
            if (result != "")
            {
                MessageBox.Show(result);
                return;
            }

            clear();
            addLine("Fetching files list.");
            setFormUIEnabled(false);

            getFilesList((String html, int error) =>
            {

                addLine("");
                addLine(html);

                setFormUIEnabled(true);

            });

        }

        /**
         * Download and concat complete.
         * Tell computer that we can go on standby again 
         */
        void completeDownload()
        {
            // enable sleep
            NativeMethods.SetThreadExecutionState(NativeMethods.ES_CONTINUOUS);

        }

        /**
         * Standby native methods
         */
        internal static class NativeMethods
        {
            // Import SetThreadExecutionState Win32 API and necessary flags
            [DllImport("kernel32.dll")]
            public static extern uint SetThreadExecutionState(uint esFlags);
            public const uint ES_CONTINUOUS = 0x80000000;
            public const uint ES_SYSTEM_REQUIRED = 0x00000001;
        }

        /**
         * Enable stand by
         */
        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // enable sleep
            NativeMethods.SetThreadExecutionState(NativeMethods.ES_CONTINUOUS);

        }

        /**
         * File -> Exit -> Exit program
         */
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /*
         * Help -> About
         */
        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox1 box = new AboutBox1();
            box.ShowDialog();
            
        }

        /**
         * Settings -> Video Container -> Change
         */
        void changeVideoContainer(String format)
        {
            if (Properties.Settings.Default.VideoContainer != format)
            {
                // change settings
                Properties.Settings.Default.VideoContainer = format;
                Properties.Settings.Default.Save();
            }

            // update checked
            checkVideoContainer();

        }

        /**
         * Make settings drop down as checked.
         * Settings -> Video Container -> MP4|MKV
         */
        void checkVideoContainer()
        {
            mKVToolStripMenuItem.Checked = false;
            mP4ToolStripMenuItem.Checked = false;

            if (Properties.Settings.Default.VideoContainer == "mkv")
            {
                mKVToolStripMenuItem.Checked = true;
            }
            else
            {
                mP4ToolStripMenuItem.Checked = true;
            }

        }

        /**
         * Update video container setting.
         */
        private void mKVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeVideoContainer("mkv");
        }

        /**
         * Update video container setting.
         */
        private void mP4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeVideoContainer("mp4");
        }

    }
}
