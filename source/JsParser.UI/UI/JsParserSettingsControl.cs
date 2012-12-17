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
            edExtensions.Lines = Settings.Default.Extensions.OfType<string>().ToArray();

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

            chCheckForVersionUpdates.Checked = Settings.Default.CheckForVersionUpdates;
            chSendStatistics.Checked = Settings.Default.SendStatistics;
            chUseVSColors.Checked = Settings.Default.UseVSColorTheme;
        }

        public void SaveSettings()
        {
            Settings.Default.UseVSColorTheme = chUseVSColors.Checked;
            Settings.Default.TrackActiveItem = chTrackActiveItem.Checked;
            Settings.Default.Extensions = new System.Collections.Specialized.StringCollection();
            Settings.Default.Extensions.AddRange(edExtensions.Lines);

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

            Settings.Default.CheckForVersionUpdates = chCheckForVersionUpdates.Checked;
            Settings.Default.SendStatistics = chSendStatistics.Checked;
            if (Settings.Default.SendStatistics)
            {
                //If user checks this item once - then politic are approved.
                Settings.Default.SendStatisticsPolitic = "Approved";
            }

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
    }
}
