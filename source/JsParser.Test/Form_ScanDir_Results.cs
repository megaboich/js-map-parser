using JsParser.Core.Parsers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JsParser.Test
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

        private bool analyzeJsFileIsNotObfuscated(string fileContent)
        {
            var totalSymbolsCount = fileContent.Length;
            var charGroups = fileContent.GroupBy(c => c);

            Func<char, int> countSymbols = (symbol) =>
            {
                var symbols = charGroups.FirstOrDefault(g => g.Key == symbol);
                if (symbols != null)
                {
                    return symbols.Count();
                }
                return 0;
            };

            var spacesCount = countSymbols(' ');
            var tabsCount = countSymbols('\t');
            var returnCount = countSymbols('\r');
            var newlineCount = countSymbols('\n');

            var totalSeparators = spacesCount + tabsCount + returnCount + newlineCount;

            //Assumption if separators are more then 10% then file is not minified
            return (totalSeparators > totalSymbolsCount * 0.1);
        }

        private void ScanDir(string dir)
        {
            var outDir = "C:\\js_parser_units_output\\scan_results";
            Directory.CreateDirectory(outDir + "\\min");
            Directory.CreateDirectory(outDir + "\\src");
            var jsFiles = Directory.GetFiles(dir, "*.js", SearchOption.AllDirectories);
            Parallel.ForEach(jsFiles, fullFileName =>
            {
                if (_stopAll)
                {
                    return;
                }

                ++_startedCount;
                

                Application.DoEvents();

                var source = File.ReadAllText(fullFileName);
                var isNotMinified = analyzeJsFileIsNotObfuscated(source);
                var actualResult = (new JavascriptParser(new JavascriptParserSettings())).Parse(source, fullFileName);

                var fileName = Path.GetFileName(fullFileName);
                lock (_lock)
                {
                    File.AppendAllText(outDir + (isNotMinified ? "\\src\\" : "\\min\\") + fileName, source);
                }

                if (actualResult.InternalErrors != null && actualResult.InternalErrors.Count > 0)
                {
                    lock (_lock)
                    {
                        File.AppendAllText(outDir + (isNotMinified ? "err_src" : "err_min") + ".log", fileName + Environment.NewLine);
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
