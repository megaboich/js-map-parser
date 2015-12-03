using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JsParser.UI.Helpers;
using JsParser.UI.Properties;
using System.Collections.Specialized;

namespace JsParser.UI.UI
{
    public partial class JsParserSettingsControl : UserControl
    {
        private Font _defaultTreeFont;

        public JsParserSettingsControl()
        {
            InitializeComponent();

            _defaultTreeFont = Font;
        }

        public Font DefaultTreeFont
        {
            get { return _defaultTreeFont; }
            set { _defaultTreeFont = value; }
        }

        public void InitSettings()
        {
            chTrackActiveItem.Checked = Settings.Default.TrackActiveItem;
            edExtensions.Text = string.Join(" ", Settings.Default.Extensions.OfType<string>().Select(s => s.Replace(".", "")).ToArray());
            chScriptStripEnabled.Checked = Settings.Default.ScriptStripEnabled;
            edScriptStripEtensions.Text = string.Join(" ", Settings.Default.ScriptStripExtensions.OfType<string>().Select(s => s.Replace(".", "")).ToArray());

            taggedFuncLabel1.ForeColor = Settings.Default.taggedFunction1Color;
            taggedFuncLabel1.Font = Settings.Default.taggedFunction1Font ?? _defaultTreeFont;
            taggedFuncLabel2.ForeColor = Settings.Default.taggedFunction2Color;
            taggedFuncLabel2.Font = Settings.Default.taggedFunction2Font ?? _defaultTreeFont;
            taggedFuncLabel3.ForeColor = Settings.Default.taggedFunction3Color;
            taggedFuncLabel3.Font = Settings.Default.taggedFunction3Font ?? _defaultTreeFont;
            taggedFuncLabel4.ForeColor = Settings.Default.taggedFunction4Color;
            taggedFuncLabel4.Font = Settings.Default.taggedFunction4Font ?? _defaultTreeFont;
            taggedFuncLabel5.ForeColor = Settings.Default.taggedFunction5Color;
            taggedFuncLabel5.Font = Settings.Default.taggedFunction5Font ?? _defaultTreeFont;
            taggedFuncLabel6.ForeColor = Settings.Default.taggedFunction6Color;
            taggedFuncLabel6.Font = Settings.Default.taggedFunction6Font ?? _defaultTreeFont;

            numericUpDownMaxParametersLength.Value = Settings.Default.MaxParametersLength;
            numericUpDownMaxParametersLengthInFunctionChain.Value = Settings.Default.MaxParametersLengthInFunctionChain;

            chSendStatistics.Checked = Settings.Default.SendStatistics;
            chFixAspNet.Checked = Settings.Default.FixAspNetTags;
            edFixAspNetExtensions.Text = string.Join(" ", Settings.Default.FixAspNetTagsExtensions.OfType<string>().Select(s => s.Replace(".", "")).ToArray());
            chFixRazor.Checked = Settings.Default.FixRazorSyntax;
            edFixRazorExtensions.Text = string.Join(" ", Settings.Default.FixRazorSyntaxExtensions.OfType<string>().Select(s => s.Replace(".", "")).ToArray());

            edToDoKeyWords.Text = String.Join(", ", Settings.Default.ToDoKeywords.OfType<string>().ToArray());

            chShowErrorNotificationOnTopOfTheEditor.Checked = Settings.Default.ShowErrorsNotificationOnTopOfEditor;

            var themesSerialized = Settings.Default.ThemeSettingsSerialized;
        }

        private StringCollection ReadListOfExtensionsFromTextBoxText(string textBoxText)
        {
            var list = new StringCollection();
            list.AddRange(textBoxText.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries));
            return list;
        }

        public void SaveSettings()
        {
            Settings.Default.TrackActiveItem = chTrackActiveItem.Checked;
            Settings.Default.Extensions = ReadListOfExtensionsFromTextBoxText(edExtensions.Text);

            Settings.Default.ScriptStripEnabled = chScriptStripEnabled.Checked;
            Settings.Default.ScriptStripExtensions = ReadListOfExtensionsFromTextBoxText(edScriptStripEtensions.Text);

            Settings.Default.taggedFunction1Color = taggedFuncLabel1.ForeColor;
            Settings.Default.taggedFunction1Font = taggedFuncLabel1.Font;
            Settings.Default.taggedFunction2Color = taggedFuncLabel2.ForeColor;
            Settings.Default.taggedFunction2Font = taggedFuncLabel2.Font;
            Settings.Default.taggedFunction3Color = taggedFuncLabel3.ForeColor;
            Settings.Default.taggedFunction3Font = taggedFuncLabel3.Font;
            Settings.Default.taggedFunction4Color = taggedFuncLabel4.ForeColor;
            Settings.Default.taggedFunction4Font = taggedFuncLabel4.Font;
            Settings.Default.taggedFunction5Color = taggedFuncLabel5.ForeColor;
            Settings.Default.taggedFunction5Font = taggedFuncLabel5.Font;
            Settings.Default.taggedFunction6Color = taggedFuncLabel6.ForeColor;
            Settings.Default.taggedFunction6Font = taggedFuncLabel6.Font;

            Settings.Default.MaxParametersLength = Convert.ToInt32(numericUpDownMaxParametersLength.Value);
            Settings.Default.MaxParametersLengthInFunctionChain = Convert.ToInt32(numericUpDownMaxParametersLengthInFunctionChain.Value);

            Settings.Default.SendStatistics = chSendStatistics.Checked;
            if (Settings.Default.SendStatistics)
            {
                //If user checks this item once - then politic are approved.
                Settings.Default.SendStatisticsPolitic = "Approved";
            }

            Settings.Default.FixAspNetTags = chFixAspNet.Checked;
            Settings.Default.FixAspNetTagsExtensions = ReadListOfExtensionsFromTextBoxText(edFixAspNetExtensions.Text);
            Settings.Default.FixRazorSyntax = chFixRazor.Checked;
            Settings.Default.FixRazorSyntaxExtensions = ReadListOfExtensionsFromTextBoxText(edFixRazorExtensions.Text);

            Settings.Default.ToDoKeywords = new System.Collections.Specialized.StringCollection();
            Settings.Default.ToDoKeywords.AddRange(edToDoKeyWords.Text.Split(new[] { ',', ';' }).Select(p => p.Trim()).ToArray());

            Settings.Default.ShowErrorsNotificationOnTopOfEditor = chShowErrorNotificationOnTopOfTheEditor.Checked;

            Settings.Default.Save();
        }

        private void btnForceSendStatistics_Click(object sender, EventArgs e)
        {
            StatisticsManager.Instance.SubmitStatisticsToServer(force: true);
        }

        private void ShowFontDialogForLabel(string lblName)
        {
            Label lbl = (Label)Controls.Find(lblName, true).First();
            fontDialog1.AllowScriptChange = false;
            fontDialog1.AllowVerticalFonts = false;
            fontDialog1.ShowColor = true;
            fontDialog1.ShowEffects = true;
            fontDialog1.FontMustExist = true;

            fontDialog1.Font = lbl.Font;
            fontDialog1.Color = lbl.ForeColor;

            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                lbl.Font = fontDialog1.Font;
                lbl.ForeColor = fontDialog1.Color;
            }
        }

        private void ShowColorDialogForLabel(string lblName)
        {
            Label lbl = (Label)Controls.Find(lblName, true).First();
            colorDialog1.Color = lbl.ForeColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                lbl.ForeColor = colorDialog1.Color;
            }
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            var lblName = "taggedFuncLabel" + (string)((Control)sender).Tag;
            ShowFontDialogForLabel(lblName);
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            var lblName = "taggedFuncLabel" + (string)((Control)sender).Tag;
            ShowColorDialogForLabel(lblName);
        }

        private void chScriptStripEnabled_CheckedChanged(object sender, EventArgs e)
        {
            edScriptStripEtensions.Enabled = chScriptStripEnabled.Checked;
        }

        private void chFixAspNet_CheckedChanged(object sender, EventArgs e)
        {
            edFixAspNetExtensions.Enabled = chFixAspNet.Checked;
        }

        private void chFixRazor_CheckedChanged(object sender, EventArgs e)
        {
            edFixRazorExtensions.Enabled = chFixRazor.Checked;
        }

        private void btnAddCustomTheme_Click(object sender, EventArgs e)
        {

        }

        private void btnEditTheme_Click(object sender, EventArgs e)
        {

        }

        private void btnRemoveTheme_Click(object sender, EventArgs e)
        {

        }
    }
}
