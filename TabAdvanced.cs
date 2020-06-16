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
            Globals.ButtonCommand = "GetInfo";
            try
            {
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += GetInfoWaitForItemUpdate;
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
    }
}
