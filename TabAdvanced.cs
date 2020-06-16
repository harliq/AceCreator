using System;
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

        public HudTextBox TextboxParentGUID { get; set; }
        public HudTextBox TextboxChildWCID { get; set; }

        


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
            Globals.ButtonCommand = "GetInfo";
            try
            {
                Util.SendChatCommand("/createinst -p " + TextboxParentGUID.Text +" -c " + TextboxChildWCID.Text);
              
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
    }
}
