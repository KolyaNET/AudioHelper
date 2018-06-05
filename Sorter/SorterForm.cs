using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Sorter
{
    public partial class SorterForm : Form
    {
        private readonly SynchronizationContext FormSynchronizationContext;

        public SorterForm()
        {
            InitializeComponent();
            FormSynchronizationContext = SynchronizationContext.Current;
            Sorter.Log += o => listBox1.Items.Add(o);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
                textBox2.Text = folderBrowserDialog2.SelectedPath;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Thread(Sorter.Start).Start(new StartParameters(FormSynchronizationContext, textBox1.Text, textBox2.Text));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox3.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var list = new List<TagLib.File>();
            MessageBox.Show(TagHelper.SetPerformersFromFilename(textBox3.Text, list).ToString());
            var l = new List<object>();
            l.AddRange(list.Select(o => o.Name));
            listBox2.Items.AddRange(l.ToArray());
        }
        

    }
}
