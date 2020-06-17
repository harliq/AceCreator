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

        public HudCombo ChoiceJSON { get; set; }
        public HudButton CommandConvertJSON { get; set; }
        public HudButton ButtonOpenJSON { get; set; }

        public HudCombo ChoiceSQL { get; set; }
        public HudButton CommandConvertSQL { get; set; }
        public HudButton ButtonOpenSQL { get; set; }

        public HudTextBox TextboxCreateWCID { get; set; }
        public HudButton ButtonCreateWCID { get; set; }
        public HudButton ButtonCreateInvWCID { get; set; }
        public HudButton ButtonCreateInstantWCID { get; set; }

        public HudTextBox TextboxExportJsonWCID { get; set; }
        public HudButton ButtonExportJSON { get; set; }
        public HudTextBox TextboxExportSQLWCID { get; set; }
        public HudButton ButtonExportSQL { get; set; }
        public HudButton ButtonYotesWCIDLookUp { get; set; }
        public HudButton ButtonPCAPSWCIDLookUp { get; set; }


        public HudStaticText LabelGetInfo { get; set; }

        public HudButton ButtonRemoveInstace { get; set; }
        public HudButton ButtonMyLocation { get; set; }
        public HudButton ButtonDeleteItem { get; set; }
        public HudButton CommandRefreshFilesList { get; set; }
        public HudButton ButtonGetInfo { get; set; }

        // ComboBox Change Events
        public void ChoiceJSON_Change(object sender, EventArgs e)
        {
            try
            {
                TextboxCreateWCID = (HudTextBox)view["TextboxCreateWCID"];
                string tsplit = ((HudStaticText)ChoiceJSON[ChoiceJSON.Current]).Text;
                TextboxCreateWCID.Text = tsplit.Split(' ')[0];
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ChoiceSQL_Change(object sender, EventArgs e)
        {
            try
            {
                TextboxCreateWCID = (HudTextBox)view["TextboxCreateWCID"];
                string tsplit = ((HudStaticText)ChoiceSQL[ChoiceSQL.Current]).Text;
                TextboxCreateWCID.Text = tsplit.Split(' ')[0];
            }
            catch (Exception ex) { Util.LogError(ex); }

        }

        // Button Events

        public void ButtonConvertJSON_Click(object sender, EventArgs e)
        {
            try
            {
                string tsplit = ((HudStaticText)ChoiceJSON[ChoiceJSON.Current]).Text;
                TextboxCreateWCID = (HudTextBox)view["TextboxCreateWCID"];
                Util.SendChatCommand(@"/import-json " + tsplit.Split(' ')[0]);
                
                TextboxCreateWCID.Text = tsplit.Split(' ')[0];
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonConvertSQL_Click(object sender, EventArgs e)
        {
            try
            {
                string tsplit = ((HudStaticText)ChoiceSQL[ChoiceSQL.Current]).Text;
                TextboxCreateWCID = (HudTextBox)view["TextboxCreateWCID"];
                Util.SendChatCommand(@"/import-sql " + tsplit.Split(' ')[0]);
                
                TextboxCreateWCID.Text = tsplit.Split(' ')[0];
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonOpenJSON_Click(object sender, EventArgs e)
        {

            try
            {
                System.Diagnostics.Process.Start(Globals.PathJSON + @"\" + ((HudStaticText)ChoiceJSON[ChoiceJSON.Current]).Text);
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonOpenSQL_Click(object sender, EventArgs e)
        {

            try
            {
                System.Diagnostics.Process.Start(Globals.PathSQL + @"\" + ((HudStaticText)ChoiceSQL[ChoiceSQL.Current]).Text);
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonCreateWCID_Click(object sender, EventArgs e)
        {
            try
            {
                Util.SendChatCommand(@"/create " + TextboxCreateWCID.Text);
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonCreateInvWCID_Click(object sender, EventArgs e)
        {
            try
            {

                Util.SendChatCommand(@"/ci " + TextboxCreateWCID.Text);
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonCreateInstantWCID_Click(object sender, EventArgs e)
        {
            try
            {

                Util.SendChatCommand(@"/createinst " + TextboxCreateWCID.Text);
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonExportJSON_Click(object sender, EventArgs e)
        {
            try
            {
                Util.SendChatCommand(@"/export-json " + TextboxExportJsonWCID.Text);
                JsonChoiceListLoadFiles();
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonExportSQL_Click(object sender, EventArgs e)
        {
            try
            {

                Util.SendChatCommand(@"/export-sql " + TextboxExportSQLWCID.Text);
                SqlChoiceListLoadFiles();
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonYotesWCIDLookUp_Click(object sender, EventArgs e)
        {

            Globals.ButtonCommand = "YotesLookup";
            try
            {
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += GetInfoWaitForItemUpdate;
                Util.WriteToChat(Globals.YotesWCID);

            }
            catch (Exception ex) { Util.LogError(ex); }

        }

        public void ButtonPCAPSWCIDLookUp_Click(object sender, EventArgs e)
        {
            Globals.ButtonCommand = "PCAPsLookup";
            try
            {

                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += GetInfoWaitForItemUpdate;
                // https://github.com/ACEmulator/ACE-PCAP-Exports/search?q=filename:
            }
            catch (Exception ex) { Util.LogError(ex); }

        }

        public void ButtonRemoveInstace_Click(object sender, EventArgs e)
        {

            try
            {
                Globals.ButtonCommand = "/removeinst";
                CommandWait(sender, e);

            }
            catch (Exception ex) { Util.LogError(ex); }

        }

        public void ButtonMyLocation_Click(object sender, EventArgs e)
        {

            try
            {
                Util.SendChatCommand("/loc");

            }
            catch (Exception ex) { Util.LogError(ex); }

        }


        public void ButtonDeleteItem_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/delete";
                CommandWait(sender, e);

            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonRefreshFilesList_Click(object sender, EventArgs e)
        {
            try
            {
                Util.WriteToChat("Reloading FileLists");
                RefreshAllLists();

                // Util.WriteToChat("Text= " + ((HudStaticText)ChoiceSQL[ChoiceSQL.Current]).Text);
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonGetInfo_Click(object sender, EventArgs e)
        {
            
            try
            {
                Globals.ButtonCommand = "/getinfo";
                CommandWait(sender, e);

            }
            catch (Exception ex) { Util.LogError(ex); }

        }

    }
}
