using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AudioHelperLib;
using File = TagLib.File;

namespace Sorter
{
    public partial class Client : Form
    {
        private readonly SynchronizationContext _synchronizer;

        public Client()
        {
            _synchronizer = SynchronizationContext.Current;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox2.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Sorter.DoWork(() => Sorter.Sum());
        }

  
        private void button5_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;

            var list = TagHelper.RecursiveSearch(new List<File>(), folderBrowserDialog1.SelectedPath);
            listBox2.Items.AddRange(list.Where((f) => f.Tag.Performers.Any((p) => p.Contains("D:"))).ToArray());
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox4.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;

            var counter = new CounterMp3();
            counter.CurrentDirectoryChanged += (i) => _synchronizer.Post((c) => textBox4.Text = c.ToString(), i);
            counter.Finished += (i) => _synchronizer.Post((s) => MessageBox.Show(s.ToString()), string.Format("I found {0} files", i));

            var thread = new Thread(o => counter.Sum(folderBrowserDialog1.SelectedPath, true));
            thread.Start();
        }

        private void menuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            base.Capture = false;
            var m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }

        private void maxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case FormWindowState.Normal:
                    maxToolStripMenuItem.Text = "Min";
                    WindowState = FormWindowState.Maximized;
                    return;
                case FormWindowState.Maximized:
                    WindowState = FormWindowState.Normal;
                    maxToolStripMenuItem.Text = "Max";
                    return;
                case FormWindowState.Minimized:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
