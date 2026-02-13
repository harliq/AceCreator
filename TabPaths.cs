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
        public HudTextBox TextBoxPathJSON { get; set; }
        public HudTextBox TextBoxPathSQL { get; set; }
        public HudTextBox TextboxPathLandBlockJSON { get; set; }
        public HudTextBox TextboxPathLandBlockSQL { get; set; }

        public HudTextBox TextboxPathQuestJSON { get; set; }
        public HudTextBox TextboxPathQuestSQL { get; set; }

        public HudTextBox TextboxPathRecipeJSON { get; set; }
        public HudTextBox TextboxPathRecipeSQL { get; set; }


        public HudButton ButtonSavePaths { get; set; }
        public HudButton ButtonLoadINI { get; set; }
        public HudButton ButtonOpenINI { get; set; }
        
        public HudButton ButtonFindPath { get; set; }



        // Button Events
        public void ButtonSavePaths_Click(object sender, EventArgs e)
        {
            try
            {
                Util.WriteToChat("Writing Ini File");
                Util.SaveIni(TextBoxPathJSON.Text, TextBoxPathSQL.Text, TextboxPathLandBlockJSON.Text, TextboxPathLandBlockSQL.Text,
             TextboxPathQuestJSON.Text, TextboxPathQuestSQL.Text, TextboxPathRecipeJSON.Text, TextboxPathRecipeSQL.Text);
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonLoadINI_Click(object sender, EventArgs e)
        {
            try
            {
                LoadPathSettings();
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonOpenINI_Click(object sender, EventArgs e)
        {
            try
            {
                string assemblyFolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string filePath = System.IO.Path.Combine(assemblyFolder, "acecreator.ini");
                System.Diagnostics.Process.Start(filePath);
                Util.WriteToChat(filePath);

            }
            catch (Exception ex) { Util.LogError(ex); }
        }

        public void ButtonFindPath_Click(object sender, EventArgs e)
        {
            try
            {
                Util.WriteToChat("If you have not changed the folder, or exported an weenie this may not show correctly.  Also, this will not be the correct path if server is not on same machine as client.");
                Util.WriteToChat("This is the base Content Folder:");
                Util.SendChatCommand("/fetchstring content_folder");
                Util.WriteToChat("You will need to add the subfolder to the path");
            }
            catch (Exception ex) { Util.LogError(ex); }
        }

       

    }
}
