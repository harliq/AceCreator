using System;
using Decal.Adapter;
using VirindiViewService.Controls;

namespace AceCreator
{
    public partial class AceCreator : PluginBase
    {

        public HudCombo ChoiceQuestJSON { get; set; }
        public HudButton ButtonImportQuestJSON { get; set; }
        public HudButton ButtonEditQuestJSON { get; set; }

        public HudCombo ChoiceQuestSQL { get; set; }
        public HudButton ButtonImportQuestSQL { get; set; }
        public HudButton ButtonEditQuestSQL { get; set; }

        public HudCombo ChoiceRecipeJSON { get; set; }
        public HudButton ButtonImportRecipeJSON { get; set; }
        public HudButton ButtonEditRecipeJSON { get; set; }

        public HudCombo ChoiceRecipeSQL { get; set; }
        public HudButton ButtonImportRecipeSQL { get; set; }
        public HudButton ButtonEditRecipeSQL { get; set; }



        public void ButtonImportQuestJSON_Click(object sender, EventArgs e)
        {

            try
            {
                string wcid = ((HudStaticText)ChoiceQuestJSON[ChoiceQuestJSON.Current]).Text.Replace(".json", "");
                Util.SendChatCommand(@"/import-json " + wcid + " quest");
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonEditQuestJSON_Click(object sender, EventArgs e)
        {

            try
            {
                System.Diagnostics.Process.Start(Globals.PathQuestJSON + @"\" + ((HudStaticText)ChoiceQuestJSON[ChoiceQuestJSON.Current]).Text);
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonImportQuestSQL_Click(object sender, EventArgs e)
        {

            try
            {
                string wcid = ((HudStaticText)ChoiceQuestSQL[ChoiceQuestSQL.Current]).Text.Replace(".sql", "");
                Util.SendChatCommand(@"/import-sql " + wcid + " quest");
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonEditQuestSQL_Click(object sender, EventArgs e)
        {

            try
            {
                System.Diagnostics.Process.Start(Globals.PathQuestSQL + @"\" + ((HudStaticText)ChoiceQuestSQL[ChoiceQuestSQL.Current]).Text);
            }
            catch (Exception ex) { Util.LogError(ex); }

        }


        public void ButtonImportRecipeJSON_Click(object sender, EventArgs e)
        {

            try
            {
                string wcid = ((HudStaticText)ChoiceRecipeJSON[ChoiceRecipeJSON.Current]).Text.Replace(".json", "");
                Util.SendChatCommand(@"/import-json " + wcid + " recipe");
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonEditRecipeJSON_Click(object sender, EventArgs e)
        {

            try
            {
                System.Diagnostics.Process.Start(Globals.PathRecipeJSON + @"\" + ((HudStaticText)ChoiceRecipeJSON[ChoiceRecipeJSON.Current]).Text);
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonImportRecipeSQL_Click(object sender, EventArgs e)
        {

            try
            {
                string wcid = ((HudStaticText)ChoiceRecipeSQL[ChoiceRecipeSQL.Current]).Text.Replace(".sql", "");
                Util.SendChatCommand(@"/import-sql " + wcid + " recipe");
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonEditRecipeSQL_Click(object sender, EventArgs e)
        {

            try
            {
                System.Diagnostics.Process.Start(Globals.PathRecipeSQL + @"\" + ((HudStaticText)ChoiceRecipeSQL[ChoiceRecipeSQL.Current]).Text);
            }
            catch (Exception ex) { Util.LogError(ex); }

        }

    }
}
