using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Decal.Adapter;
using Decal.Adapter.Wrappers;
using MyClasses.MetaViewWrappers;
using VirindiViewService;
using VirindiViewService.Controls;

namespace AceCreator
{
    public partial class AceCreator : PluginBase
    {

        
        public HudCombo ChoiceLandblockJSON { get; set; }
        public HudButton ButtonImportLandblockJSON { get; set; }
        public HudButton ButtonEditLandblockJSON { get; set; }

        public HudCombo ChoiceLandblockSQL { get; set; }
        public HudButton ButtonImportLandblockSQL { get; set; }
        public HudButton ButtonEditLandblockSQL { get; set; }

        public HudButton ButtonReloadLandblock { get; set; }
        public HudButton ButtonClearCache { get; set; }



        // Button Events
        public void ButtonImportLandblockJSON_Click(object sender, EventArgs e)
        {
            try
            {
                // string tsplit = ((HudStaticText)ChoiceLandblockJSON[ChoiceLandblockJSON.Current]).Text;
                string wcid = ((HudStaticText)ChoiceLandblockJSON[ChoiceLandblockJSON.Current]).Text.Replace(".json", "");
                // TextboxCreateWCID = (HudTextBox)view["TextboxCreateWCID"];
                Util.SendChatCommand(@"/import-json " + wcid + " landblock");
                // Util.WriteToChat("Imported JSON= " + ((HudStaticText)ChoiceJSON[ChoiceJSON.Current]).Text);                
                // TextboxCreateWCID.Text = tsplit.Split(' ')[0];
            }
            catch (Exception ex) { Util.LogError(ex); }
        }

        public void ButtonImportLandblockSQL_Click(object sender, EventArgs e)
        {
            try
            {
                // string tsplit = ((HudStaticText)ChoiceLandblockSQL[ChoiceLandblockSQL.Current]).Text;
                string wcid = ((HudStaticText)ChoiceLandblockSQL[ChoiceLandblockSQL.Current]).Text.Replace(".sql", "");
                //TextboxCreateWCID = (HudTextBox)view["TextboxCreateWCID"];
                Util.SendChatCommand(@"/import-sql " + wcid + " landblock");
                // Util.WriteToChat("Imported JSON= " + ((HudStaticText)ChoiceJSON[ChoiceJSON.Current]).Text);                
                // TextboxCreateWCID.Text = tsplit.Split(' ')[0];
            }
            catch (Exception ex) { Util.LogError(ex); }
        }

        public void ButtonEditLandblockJSON_Click(object sender, EventArgs e)
        {

            try
            {
                System.Diagnostics.Process.Start(Globals.PathLandBlockJSON + @"\" + ((HudStaticText)ChoiceLandblockJSON[ChoiceLandblockJSON.Current]).Text);
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonEditLandblockSQL_Click(object sender, EventArgs e)
        {

            try
            {
                System.Diagnostics.Process.Start(Globals.PathLandBlockSQL + @"\" + ((HudStaticText)ChoiceLandblockSQL[ChoiceLandblockSQL.Current]).Text);
            }
            catch (Exception ex) { Util.LogError(ex); }

        }

        public void ButtonReloadLandblock_Click(object sender, EventArgs e)
        {

            try
            {
                Util.SendChatCommand("/reload-landblock");
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonClearCache_Click(object sender, EventArgs e)
        {

            try
            {
                Util.SendChatCommand("/clearcache");
            }
            catch (Exception ex) { Util.LogError(ex); }

        }




        // Methods


    }
}
