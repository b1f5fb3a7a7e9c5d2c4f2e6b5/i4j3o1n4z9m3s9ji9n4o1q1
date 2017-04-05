using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WinFormsLCGDecoder.Properties;
using WinFormsMappingDecoder.Properties;

namespace WinFormsLCGDecoder
{
    public partial class Home : Form
    {
        private readonly HomeData _data = new HomeData();

        public Home()
        {
            InitializeComponent();

            var m = Math.Pow(10, 6);          // Значения M - 10^6

            //var x = (double)519253;
            //var xn = (double)910043;
            //var xnn = (double)747743;

            //var num = (m * 1 + (xnn - xn)) / (xn - x);
            //textBox.Text = num.ToString(CultureInfo.InvariantCulture);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown ||
                MessageBox.Show(string.Format(Resources.Home_OnFormClosing_, base.Text), //Text - base.Text
                    @"Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes) return;

            e.Cancel = true;
        }

        private void MenuExit_Click(object sender, EventArgs e) => Close();

        private void MenuOpenFile_Click(object sender, EventArgs e)
        {
            textBox.Text = OpenFile();
        }

        private string OpenFile()
        {
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    return new StreamReader(openFileDialog.FileName).ReadToEnd();

            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        private void MenuSaveFile_Click(object sender, EventArgs e)
        {
            if (textBox.Text.Trim().Equals(string.Empty)) return;
            try
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    File.WriteAllText(saveFileDialog.FileName, textBox.Text);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void Home_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode & Keys.Escape) == Keys.Escape) Close();
        }
        
        private void MenuClear_Click(object sender, EventArgs e)
        {
            textBoxInfo.Text = string.Empty;
        }

        private void AnalysisMenu_Click(object sender, EventArgs e)
        {
            //var thread = new Thread(ThreadAnalysisBruteforce);
            //thread.Start();
            textBoxInfo.Text += _data.AnalysisJamesReeds(textBox.Text);
        }

        private void ThreadAnalysisBruteforce()
        {
            Invoke((Action)(() =>
            {
                textBoxInfo.Text += _data.AnalysisBruteforce(this, textBox.Text);
            }));
        }

        internal void SetStatusInvoke()
        {
            Invoke((Action)(() =>
            {
                Int64 result;
                statusLabel.Text = Int64.TryParse(statusLabel.Text, out result) ? (result + 1).ToString() : "1";
            }));
        }
    }
}