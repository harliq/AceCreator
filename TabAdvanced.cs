using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Decal.Adapter;
using VirindiViewService.Controls;

namespace AceCreator
{
    public partial class AceCreator : PluginBase
    {
        public HudButton ButtonGetParentGUID { get; set; }
        public HudButton ButtonLinkChild { get; set; }

        public HudButton ButtonCreateGenerator { get; set; }
        public HudButton ButtonEditGeneratorList { get; set; }
        public HudButton ButtonRefreshGeneratorList { get; set; }
        public HudButton ButtonAdvancedRemoveInst { get; set; }
        public HudButton ButtonCreateMob { get; set; }


        public HudButton ButtonAdvancedAddEncounter { get; set; }
        public HudButton ButtonAdvancedRemoveEncounter { get; set; }
        public HudTextBox TextBoxEncounterWCID { get; set; }


        public HudTextBox TextboxGeneratorWCID { get; set; }

        public HudTextBox TextboxParentGUID { get; set; }
        public HudTextBox TextboxChildWCID { get; set; }


        

        public HudCombo ChoiceGenerator { get; set; }
        public HudCombo ChoiceChildList { get; set; }

        
        // ComboBox Change Events
        public void ChoiceChildList_Change(object sender, EventArgs e)
        {
            try
            {
                TextboxChildWCID = (HudTextBox)view["TextboxChildWCID"];
                string tsplit = ((HudStaticText)ChoiceChildList[ChoiceChildList.Current]).Text;
                TextboxChildWCID.Text = tsplit.Split(' ')[0];
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ChoiceGenerator_Change(object sender, EventArgs e)
        {
            try
            {
                TextboxGeneratorWCID = (HudTextBox)view["TextboxGeneratorWCID"];
                string tsplit = ((HudStaticText)ChoiceGenerator[ChoiceGenerator.Current]).Text;
                TextboxGeneratorWCID.Text = tsplit.Split(' ')[0];
            }
            catch (Exception ex) { Util.LogError(ex); }
        }

        // Button Events
        public void ButtonGetParentGUID_Click(object sender, EventArgs e)
        {
            
            try
            {
                Globals.ButtonCommand = "/getinfo";
                CommandWait(sender, e);

            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonLinkChild_Click(object sender, EventArgs e)
        {
            
            try
            {
                Util.SendChatCommand("/createinst -p " + TextboxParentGUID.Text +" -c " + TextboxChildWCID.Text);
              
            }
            catch (Exception ex) { Util.LogError(ex); }
        }

        public void ButtonCreateGenerator_Click(object sender, EventArgs e)
        {

            try
            {
                Util.SendChatCommand("/createinst " + TextboxGeneratorWCID.Text);
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonEditGeneratorList_Click(object sender, EventArgs e)
        {
            try
            {
                string assemblyFolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);                
                string filePath = System.IO.Path.Combine(assemblyFolder, "parentobjects.ini");
                System.Diagnostics.Process.Start(filePath);
                Util.WriteToChat(filePath);

            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonRefreshGeneratorList_Click(object sender, EventArgs e)
        {

            try
            {
                LoadParentObjectsFile();

            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonCreateMob_Click(object sender, EventArgs e)
        {

            try
            {
                Util.SendChatCommand("/createinst -p GUID -c 1");

            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonAdvancedRemoveEncounter_Click(object sender, EventArgs e)
        {

            try
            {
                Globals.ButtonCommand = "/removeenc";
                CommandWait(sender, e);

            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonAdvancedAddEncounter_Click(object sender, EventArgs e)
        {

            try
            {
                Util.SendChatCommand("/addenc " + TextBoxEncounterWCID.Text);
            }
            catch (Exception ex) { Util.LogError(ex); }
        }

    }
}
