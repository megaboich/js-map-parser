using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using JsParserCore.Parsers;

namespace JsParserTest
{
    public partial class Form_ScanDir_Results : Form
    {
        private static volatile bool _stopAll = false;
        private static volatile int _startedCount = 0;
        private static volatile int _finishedCount = 0;
        string _pathToSearch;
        private static object _lock = new object();

        public Form_ScanDir_Results(string pathToSearch)
        {
            InitializeComponent();

            _pathToSearch = pathToSearch;
        }


        private void Form_ScanDir_Results_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();

            ScanDir(_pathToSearch);
        }

        private void ScanDir(string dir)
        {
            var jsFiles = Directory.GetFiles(dir, "*.js");
            Parallel.ForEach(jsFiles, file =>
            {
                if (_stopAll)
                {
                    return;
                }

                ++_startedCount;
                

                Application.DoEvents();

                var source = File.ReadAllText(file);

                var actualResult = (new JavascriptParser(new JavascriptParserSettings())).Parse(source);

                if (actualResult.InternalErrors != null && actualResult.InternalErrors.Count > 0)
                {
                    lock (_lock)
                    {
                        File.AppendAllText("C:\\js_err.log", file + Environment.NewLine);
                    }
                }

                ++_finishedCount;
                
            });

            _stopAll = false;
            textBox1.AppendText("END");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _stopAll = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelStatus.Text = "Started: " + _startedCount;
            label1.Text = "Finished: " + _finishedCount;
        }
    }
}
