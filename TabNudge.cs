using System;
using System.Collections.Generic;
using System.Text;
using Decal.Adapter;
using Decal.Adapter.Wrappers;
using MyClasses.MetaViewWrappers;
using VirindiViewService;
using VirindiViewService.Controls;

namespace AceCreator
{
    public partial class AceCreator : PluginBase
    {

        
        public HudTextBox TextboxNudgeValueCustom { get; set; }
        public HudButton ButtonNudgeN { get; set; }

        public HudButton ButtonNudgeNE { get; set; }
        public HudButton ButtonNudgeE { get; set; }
        public HudButton ButtonNudgeSE { get; set; }

        public HudButton ButtonNudgeS { get; set; }

        public HudButton ButtonNudgeSW { get; set; }
        public HudButton ButtonNudgeW { get; set; }
        public HudButton ButtonNudgeNW { get; set; }

        public HudButton ButtonNudgeUp { get; set; }
        public HudButton ButtonNudgeDown { get; set; }

        public HudButton ButtonRotateN { get; set; }
        public HudButton ButtonRotateE { get; set; }
        public HudButton ButtonRotateS { get; set; }
        public HudButton ButtonRotateW { get; set; }
        public HudButton ButtonFreeRotate { get; set; }

        public HudTextBox TextboxFreeRotate { get; set; }

        // Button Events
        public void ButtonNudgeN_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/nudge n " + TextboxNudgeValueCustom.Text;
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += WaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonNudgeNE_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/nudge ne " + TextboxNudgeValueCustom.Text;
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += WaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonNudgeE_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/nudge e " + TextboxNudgeValueCustom.Text;
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += WaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonNudgeSE_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/nudge se " + TextboxNudgeValueCustom.Text;
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += WaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonNudgeS_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/nudge s " + TextboxNudgeValueCustom.Text;
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += WaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonNudgeSW_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/nudge sw " + TextboxNudgeValueCustom.Text;
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += WaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonNudgeW_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/nudge w " + TextboxNudgeValueCustom.Text;
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += WaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonNudgeNW_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/nudge nw " + TextboxNudgeValueCustom.Text;
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += WaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonRotateN_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/rotate n";
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += WaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonRotateE_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/rotate e";
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += WaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }
        }

        public void ButtonRotateS_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/rotate s";
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += WaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }
        }

        public void ButtonRotateW_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/rotate w";
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += WaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonFreeRotate_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/rotate " + TextboxFreeRotate.Text;
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += WaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }
        }

        public void ButtonNudgeUp_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/nudge up " + TextboxNudgeValueCustom.Text;
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += WaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonNudgeDown_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/nudge down " + TextboxNudgeValueCustom.Text;
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += WaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
    }
}
