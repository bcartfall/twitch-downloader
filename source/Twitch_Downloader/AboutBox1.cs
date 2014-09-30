using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Twitch_Downloader
{
    partial class AboutBox1 : Form
    {
        public AboutBox1()
        {
            InitializeComponent();

            labelVersion.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
        }


    }
}
