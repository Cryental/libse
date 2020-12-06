﻿using System;
using Nikse.SubtitleEdit.Core.Common;
using Nikse.SubtitleEdit.Core.Interfaces;

namespace Nikse.SubtitleEdit.Core.Forms.FixCommonErrors
{
    public class FixShortLinesAll : IFixCommonError
    {
        public void Fix(Subtitle subtitle, IFixCallbacks callbacks)
        {
            var language = Configuration.Settings.Language.FixCommonErrors;
            string fixAction = language.MergeShortLineAll;
            int noOfShortLines = 0;
            for (int i = 0; i < subtitle.Paragraphs.Count; i++)
            {
                Paragraph p = subtitle.Paragraphs[i];
                if (callbacks.AllowFix(p, fixAction))
                {
                    string s = HtmlUtil.RemoveHtmlTags(p.Text, true);
                    if (s.Contains(Environment.NewLine) && s.Replace(Environment.NewLine, " ").Replace("  ", " ").CountCharacters(false, Configuration.Settings.General.IgnoreArabicDiacritics) < Configuration.Settings.General.MergeLinesShorterThan)
                    {
                        s = Utilities.AutoBreakLine(p.Text, callbacks.Language);
                        if (s != p.Text)
                        {
                            string oldCurrent = p.Text;
                            p.Text = s;
                            noOfShortLines++;
                            callbacks.AddFixToListView(p, fixAction, oldCurrent, p.Text);
                        }
                    }
                }
            }
            callbacks.UpdateFixStatus(noOfShortLines, language.RemoveLineBreaks, string.Format(language.XLinesUnbreaked, noOfShortLines));
        }
    }
}
