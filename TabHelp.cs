using System;
using System.Collections.Generic;
using System.Text;
using Decal.Adapter;
using VirindiViewService.Controls;


namespace AceCreator
{
    public partial class AceCreator : PluginBase
    {
        public HudButton ButtonACCWiki { get; set; }
        public HudButton ButtonACEWiki { get; set; }


        public void ButtonACCWiki_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/harliq/AceCreator/wiki");

            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonACEWiki_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/ACEmulator/ACE/wiki/Content-Creation");

            }
            catch (Exception ex) { Util.LogError(ex); }
        }
    }
}
