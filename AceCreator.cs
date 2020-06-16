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
    //Attaches events from core
    [WireUpBaseEvents]

    //View (UI) handling
    [MVView("AceCreator.mainView.xml")]
    [MVWireUpControlEvents]

    // FriendlyName is the name that will show up in the plugins list of the decal agent (the one in windows, not in-game)
    // View is the path to the xml file that contains info on how to draw our in-game plugin. The xml contains the name and icon our plugin shows in-game.
    // The view here is SamplePlugin.mainView.xml because our projects default namespace is SamplePlugin, and the file name is mainView.xml.
    // The other key here is that mainView.xml must be included as an embeded resource. If its not, your plugin will not show up in-game.
    [FriendlyName("AceCreator")]
    public partial class AceCreator : PluginBase
    {
        //  For seeing ID has completed
        WorldObject WO;
        private readonly AceItem aceItem = new AceItem();

        private ChatMessages cm = new ChatMessages();
        public static HudTabView TabView { get; private set; }

        private static VirindiViewService.ViewProperties properties;
        private static VirindiViewService.ControlGroup controls;
        private static VirindiViewService.HudView view;

        /// <summary>
        /// This is called when the plugin is started up. This happens only once.
        /// </summary>
        protected override void Startup()
        {
            try
            {
                // This initializes our static Globals class with references to the key objects your plugin will use, Host and Core.
                // The OOP way would be to pass Host and Core to your objects, but this is easier.
                System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\AceContentCreator\");
                CoreManager.Current.ChatBoxMessage += new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);
                Globals.Init("AceCreator", Host, Core);
                LoadWindow();
                LoadPathSettings();
                //JsonChoiceListLoadFiles();
                //SqlChoiceListLoadFiles();
                RefreshAllLists();
                LoadParentObjectsFile();
                //Initialize the view.
                // MVWireupHelper.WireupStart(this, Host);

            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        /// <summary>
        /// This is called when the plugin is shut down. This happens only once.
        /// </summary>
        protected override void Shutdown()
        {
            try
            {
                // Save Path Settings
                //Destroy the view.
                MVWireupHelper.WireupEnd(this);
            }
            catch (Exception ex) { Util.LogError(ex); }
        }

        public void LoadWindow()
        {
            // Create the view
            VirindiViewService.XMLParsers.Decal3XMLParser parser = new VirindiViewService.XMLParsers.Decal3XMLParser();
            parser.ParseFromResource("AceCreator.mainView.xml", out properties, out controls);
            view = new VirindiViewService.HudView(properties, controls);
            // view.Title = "ACE Content Creator - Version " + typeof(AceCreator).Assembly.GetName().Version;
            // Get the file version for the notepad.
            string assemblyFolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo( assemblyFolder+ @"\AceCreator.dll");
            //view.Title = "ACE Content Creator - Version " + typeof(AceCreator).Assembly.GetName().Version;
            view.Title = "ACE Content Creator - Version " + myFileVersionInfo.FileVersion;

            // In order to have some sort of organization and to keep the clutter down, 
            // the varibles for each tab are declared in their corresponding TabFiles, along with the other control events.
            
            // ***** Content Tab *****
            ChoiceJSON = (HudCombo)view["ChoiceJSON"];
            ChoiceJSON.Change += new EventHandler(ChoiceJSON_Change);

            CommandConvertJSON = view != null ? (HudButton)view["CommandConvertJSON"] : new HudButton();
            CommandConvertJSON.Hit += new EventHandler(ButtonConvertJSON_Click);

            ButtonOpenJSON = view != null ? (HudButton)view["ButtonOpenJSON"] : new HudButton();
            ButtonOpenJSON.Hit += new EventHandler(ButtonOpenJSON_Click);

            ChoiceSQL = (HudCombo)view["ChoiceSQL"];
            ChoiceSQL.Change += new EventHandler(ChoiceSQL_Change);

            CommandConvertSQL = view != null ? (HudButton)view["CommandConvertSQL"] : new HudButton();
            CommandConvertSQL.Hit += new EventHandler(ButtonConvertSQL_Click);

            ButtonOpenSQL = view != null ? (HudButton)view["ButtonOpenSQL"] : new HudButton();
            ButtonOpenSQL.Hit += new EventHandler(ButtonOpenSQL_Click);

            TextboxCreateWCID = (HudTextBox)view["TextboxCreateWCID"];

            ButtonCreateWCID = view != null ? (HudButton)view["ButtonCreateWCID"] : new HudButton();
            ButtonCreateWCID.Hit += new EventHandler(ButtonCreateWCID_Click);

            ButtonCreateInvWCID = view != null ? (HudButton)view["ButtonCreateInvWCID"] : new HudButton();
            ButtonCreateInvWCID.Hit += new EventHandler(ButtonCreateInvWCID_Click);

            ButtonCreateInstantWCID = view != null ? (HudButton)view["ButtonCreateInstantWCID"] : new HudButton();
            ButtonCreateInstantWCID.Hit += new EventHandler(ButtonCreateInstantWCID_Click);

            TextboxExportJsonWCID = (HudTextBox)view["TextboxExportJsonWCID"];

            ButtonExportJSON = view != null ? (HudButton)view["ButtonExportJSON"] : new HudButton();
            ButtonExportJSON.Hit += new EventHandler(ButtonExportJSON_Click);

            TextboxExportSQLWCID = (HudTextBox)view["TextboxExportSQLWCID"];

            ButtonExportSQL = view != null ? (HudButton)view["ButtonExportSQL"] : new HudButton();
            ButtonExportSQL.Hit += new EventHandler(ButtonExportSQL_Click);

            ButtonYotesWCIDLookUp = view != null ? (HudButton)view["ButtonYotesWCIDLookUp"] : new HudButton();
            ButtonYotesWCIDLookUp.Hit += new EventHandler(ButtonYotesWCIDLookUp_Click);

            ButtonPCAPSWCIDLookUp = view != null ? (HudButton)view["ButtonPCAPSWCIDLookUp"] : new HudButton();
            ButtonPCAPSWCIDLookUp.Hit += new EventHandler(ButtonPCAPSWCIDLookUp_Click);            

            LabelGetInfo = (HudStaticText)view["LabelGetInfo"]; 

            ButtonRemoveInstace = view != null ? (HudButton)view["ButtonRemoveInstace"] : new HudButton();
            ButtonRemoveInstace.Hit += new EventHandler(ButtonRemoveInstace_Click);

            ButtonMyLocation = view != null ? (HudButton)view["ButtonMyLocation"] : new HudButton();
            ButtonMyLocation.Hit += new EventHandler(ButtonMyLocation_Click);

            ButtonDeleteItem = view != null ? (HudButton)view["ButtonDeleteItem"] : new HudButton();
            ButtonDeleteItem.Hit += new EventHandler(ButtonDeleteItem_Click);

            CommandRefreshFilesList = view != null ? (HudButton)view["CommandRefreshFilesList"] : new HudButton();
            CommandRefreshFilesList.Hit += new EventHandler(ButtonRefreshFilesList_Click);

            ButtonGetInfo = view != null ? (HudButton)view["ButtonGetInfo"] : new HudButton();
            ButtonGetInfo.Hit += new EventHandler(ButtonGetInfo_Click);

            // ***** Nudge Tab *****
            ButtonNudgeN = view != null ? (HudButton)view["ButtonNudgeN"] : new HudButton();
            ButtonNudgeN.Hit += new EventHandler(ButtonNudgeN_Click);

            ButtonNudgeNE = view != null ? (HudButton)view["ButtonNudgeNE"] : new HudButton();
            ButtonNudgeNE.Hit += new EventHandler(ButtonNudgeNE_Click);

            ButtonNudgeE = view != null ? (HudButton)view["ButtonNudgeE"] : new HudButton();
            ButtonNudgeE.Hit += new EventHandler(ButtonNudgeE_Click);

            ButtonNudgeSE = view != null ? (HudButton)view["ButtonNudgeSE"] : new HudButton();
            ButtonNudgeSE.Hit += new EventHandler(ButtonNudgeSE_Click);

            ButtonNudgeS = view != null ? (HudButton)view["ButtonNudgeS"] : new HudButton();
            ButtonNudgeS.Hit += new EventHandler(ButtonNudgeS_Click);

            ButtonNudgeSW = view != null ? (HudButton)view["ButtonNudgeSW"] : new HudButton();
            ButtonNudgeSW.Hit += new EventHandler(ButtonNudgeSW_Click);

            ButtonNudgeW = view != null ? (HudButton)view["ButtonNudgeW"] : new HudButton();
            ButtonNudgeW.Hit += new EventHandler(ButtonNudgeW_Click);

            ButtonNudgeNW = view != null ? (HudButton)view["ButtonNudgeNW"] : new HudButton();
            ButtonNudgeNW.Hit += new EventHandler(ButtonNudgeNW_Click);

            ButtonNudgeUp = view != null ? (HudButton)view["ButtonNudgeUp"] : new HudButton();
            ButtonNudgeUp.Hit += new EventHandler(ButtonNudgeUp_Click);

            ButtonNudgeDown = view != null ? (HudButton)view["ButtonNudgeDown"] : new HudButton();
            ButtonNudgeDown.Hit += new EventHandler(ButtonNudgeDown_Click);


            TextboxNudgeValueCustom = (HudTextBox)view["TextboxNudgeValueCustom"];

            ButtonRotateN = view != null ? (HudButton)view["ButtonRotateN"] : new HudButton();
            ButtonRotateN.Hit += new EventHandler(ButtonRotateN_Click);

            ButtonRotateE = view != null ? (HudButton)view["ButtonRotateE"] : new HudButton();
            ButtonRotateE.Hit += new EventHandler(ButtonRotateE_Click);

            ButtonRotateS = view != null ? (HudButton)view["ButtonRotateS"] : new HudButton();
            ButtonRotateS.Hit += new EventHandler(ButtonRotateS_Click);

            ButtonRotateW = view != null ? (HudButton)view["ButtonRotateW"] : new HudButton();
            ButtonRotateW.Hit += new EventHandler(ButtonRotateW_Click);


            ButtonRotateNE = view != null ? (HudButton)view["ButtonRotateNE"] : new HudButton();
            ButtonRotateNE.Hit += new EventHandler(ButtonRotateNE_Click);

            ButtonRotateSE = view != null ? (HudButton)view["ButtonRotateSE"] : new HudButton();
            ButtonRotateSE.Hit += new EventHandler(ButtonRotateSE_Click);

            ButtonRotateSW = view != null ? (HudButton)view["ButtonRotateSW"] : new HudButton();
            ButtonRotateSW.Hit += new EventHandler(ButtonRotateSW_Click);

            ButtonRotateNW = view != null ? (HudButton)view["ButtonRotateNW"] : new HudButton();
            ButtonRotateNW.Hit += new EventHandler(ButtonRotateNW_Click);


            ButtonFreeRotate = view != null ? (HudButton)view["ButtonFreeRotate"] : new HudButton();
            ButtonFreeRotate.Hit += new EventHandler(ButtonFreeRotate_Click);

            TextboxFreeRotate = (HudTextBox)view["TextboxFreeRotate"];


            // ***** LandBlocks Tab *****
            ChoiceLandblockJSON = (HudCombo)view["ChoiceLandblockJSON"];
            //ChoiceLandblockJSON.Change += new EventHandler(ChoiceLandblockJSON_Change);

            ButtonImportLandblockJSON = view != null ? (HudButton)view["ButtonImportLandblockJSON"] : new HudButton();
            ButtonImportLandblockJSON.Hit += new EventHandler(ButtonImportLandblockJSON_Click);

            ButtonEditLandblockJSON = view != null ? (HudButton)view["ButtonEditLandblockJSON"] : new HudButton();
            ButtonEditLandblockJSON.Hit += new EventHandler(ButtonEditLandblockJSON_Click);

            ChoiceLandblockSQL = (HudCombo)view["ChoiceLandblockSQL"];
            //ChoiceLandblockSQL.Change += new EventHandler(ChoiceLandblockSQL_Change);

            ButtonImportLandblockSQL = view != null ? (HudButton)view["ButtonImportLandblockSQL"] : new HudButton();
            ButtonImportLandblockSQL.Hit += new EventHandler(ButtonImportLandblockSQL_Click);

            ButtonEditLandblockSQL = view != null ? (HudButton)view["ButtonEditLandblockSQL"] : new HudButton();
            ButtonEditLandblockSQL.Hit += new EventHandler(ButtonEditLandblockSQL_Click);

            ButtonReloadLandblock = view != null ? (HudButton)view["ButtonReloadLandblock"] : new HudButton();
            ButtonReloadLandblock.Hit += new EventHandler(ButtonReloadLandblock_Click);

            ButtonClearCache = view != null ? (HudButton)view["ButtonClearCache"] : new HudButton();
            ButtonClearCache.Hit += new EventHandler(ButtonClearCache_Click);

            ButtonGetCurrentLandblock = view != null ? (HudButton)view["ButtonGetCurrentLandblock"] : new HudButton();
            ButtonGetCurrentLandblock.Hit += new EventHandler(ButtonGetCurrentLandblock_Click);

            ButtonExportLandblock = view != null ? (HudButton)view["ButtonExportLandblock"] : new HudButton();
            ButtonExportLandblock.Hit += new EventHandler(ButtonExportLandblock_Click);

            TextboxFreeRotate = (HudTextBox)view["TextboxFreeRotate"];



            // ***** Quests/Recipes Tab *****
            ChoiceQuestJSON = (HudCombo)view["ChoiceQuestJSON"];
            ButtonImportQuestJSON = view != null ? (HudButton)view["ButtonImportQuestJSON"] : new HudButton();
            ButtonImportQuestJSON.Hit += new EventHandler(ButtonImportQuestJSON_Click);
            ButtonEditQuestJSON = view != null ? (HudButton)view["ButtonEditQuestJSON"] : new HudButton();
            ButtonEditQuestJSON.Hit += new EventHandler(ButtonEditQuestJSON_Click);

            ChoiceQuestSQL = (HudCombo)view["ChoiceQuestSQL"];
            ButtonImportQuestSQL = view != null ? (HudButton)view["ButtonImportQuestSQL"] : new HudButton();
            ButtonImportQuestSQL.Hit += new EventHandler(ButtonImportQuestSQL_Click);
            ButtonEditQuestSQL = view != null ? (HudButton)view["ButtonEditQuestSQL"] : new HudButton();
            ButtonEditQuestSQL.Hit += new EventHandler(ButtonEditQuestSQL_Click);

            ChoiceRecipeJSON = (HudCombo)view["ChoiceRecipeJSON"];
            ButtonImportRecipeJSON = view != null ? (HudButton)view["ButtonImportRecipeJSON"] : new HudButton();
            ButtonImportRecipeJSON.Hit += new EventHandler(ButtonImportRecipeJSON_Click);
            ButtonEditRecipeJSON = view != null ? (HudButton)view["ButtonEditRecipeJSON"] : new HudButton();
            ButtonEditRecipeJSON.Hit += new EventHandler(ButtonEditRecipeJSON_Click);

            ChoiceRecipeSQL = (HudCombo)view["ChoiceRecipeSQL"];
            ButtonImportRecipeSQL = view != null ? (HudButton)view["ButtonImportRecipeSQL"] : new HudButton();
            ButtonImportRecipeSQL.Hit += new EventHandler(ButtonImportRecipeSQL_Click);

            ButtonEditRecipeSQL = view != null ? (HudButton)view["ButtonEditRecipeSQL"] : new HudButton();
            ButtonEditRecipeSQL.Hit += new EventHandler(ButtonEditRecipeSQL_Click);


            // ***** Advanced Tab *****


            ChoiceGenerator = (HudCombo)view["ChoiceGenerator"];
            ChoiceGenerator.Change += new EventHandler(ChoiceGenerator_Change);

            TextboxGeneratorWCID = (HudTextBox)view["TextboxGeneratorWCID"];
            

            ButtonCreateGenerator = view != null ? (HudButton)view["ButtonCreateGenerator"] : new HudButton();
            ButtonCreateGenerator.Hit += new EventHandler(ButtonCreateGenerator_Click);

            ButtonEditGeneratorList = view != null ? (HudButton)view["ButtonEditGeneratorList"] : new HudButton();
            ButtonEditGeneratorList.Hit += new EventHandler(ButtonEditGeneratorList_Click);

            ButtonRefreshGeneratorList = view != null ? (HudButton)view["ButtonRefreshGeneratorList"] : new HudButton();
            ButtonRefreshGeneratorList.Hit += new EventHandler(ButtonRefreshGeneratorList_Click);

            ChoiceChildList = (HudCombo)view["ChoiceChildList"];
            ChoiceChildList.Change += new EventHandler(ChoiceChildList_Change);

            ButtonGetParentGUID = view != null ? (HudButton)view["ButtonGetParentGUID"] : new HudButton();
            ButtonGetParentGUID.Hit += new EventHandler(ButtonGetParentGUID_Click);

            ButtonLinkChild = view != null ? (HudButton)view["ButtonLinkChild"] : new HudButton();
            ButtonLinkChild.Hit += new EventHandler(ButtonLinkChild_Click);

            TextboxParentGUID = (HudTextBox)view["TextboxParentGUID"];
            TextboxChildWCID = (HudTextBox)view["TextboxChildWCID"];

            ButtonCreateMob = view != null ? (HudButton)view["ButtonCreateMob"] : new HudButton();
            ButtonCreateMob.Hit += new EventHandler(ButtonCreateMob_Click);
            
            ButtonAdvancedRemoveInst = view != null ? (HudButton)view["ButtonAdvancedRemoveInst"] : new HudButton();
            ButtonAdvancedRemoveInst.Hit += new EventHandler(ButtonRemoveInstace_Click);

            // ***** Paths Tab *****
            TextBoxPathJSON = (HudTextBox)view["TextboxPathJSON"];
            TextBoxPathSQL = (HudTextBox)view["TextboxPathSQL"];
            TextboxPathLandBlockJSON = (HudTextBox)view["TextboxPathLandBlockJSON"];
            TextboxPathLandBlockSQL = (HudTextBox)view["TextboxPathLandBlockSQL"];

            TextboxPathQuestJSON = (HudTextBox)view["TextboxPathQuestJSON"];
            TextboxPathQuestSQL = (HudTextBox)view["TextboxPathQuestSQL"];

            TextboxPathRecipeJSON = (HudTextBox)view["TextboxPathRecipeJSON"];
            TextboxPathRecipeSQL = (HudTextBox)view["TextboxPathRecipeSQL"];


            ButtonSavePaths = view != null ? (HudButton)view["ButtonSavePaths"] : new HudButton();
            ButtonSavePaths.Hit += new EventHandler(ButtonSavePaths_Click);

            ButtonLoadINI = view != null ? (HudButton)view["ButtonLoadINI"] : new HudButton();
            ButtonLoadINI.Hit += new EventHandler(ButtonLoadINI_Click);

            ButtonOpenINI = view != null ? (HudButton)view["ButtonOpenINI"] : new HudButton();
            ButtonOpenINI.Hit += new EventHandler(ButtonOpenINI_Click);

            ButtonACCWiki = view != null ? (HudButton)view["ButtonACCWiki"] : new HudButton();
            ButtonACCWiki.Hit += new EventHandler(ButtonACCWiki_Click);

            // Making some stuff not seen
            //ButtonYotesWCIDLookUp.Visible = false;

        }

        [BaseEvent("LoginComplete", "CharacterFilter")]
        private void CharacterFilter_LoginComplete(object sender, EventArgs e)
        {
            try
            {
                Util.WriteToChat("Plugin now online. Server population: " + Core.CharacterFilter.ServerPopulation);

                Util.WriteToChat("CharacterFilter_LoginComplete");

                //InitSampleList();

                // Subscribe to events here

                // Globals.Core.WorldFilter.ChangeObject += new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject2);

            }
            catch (Exception ex) { Util.LogError(ex); }
        }

        [BaseEvent("Logoff", "CharacterFilter")]
        private void CharacterFilter_Logoff(object sender, Decal.Adapter.Wrappers.LogoffEventArgs e)
        {
            try
            {
                // Unsubscribe to events here, but know that this event is not gauranteed to happen. I've never seen it not fire though.
                // This is not the proper place to free up resources, but... its the easy way. It's not proper because of above statement.

                Globals.Core.WorldFilter.ChangeObject -= new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
            }
            catch (Exception ex) { Util.LogError(ex); }
        }

        // Note that there are several ways to latch on to decals events.
        // You can use the BaseEvent attribute, or you can latch on to the same event as shown in CharacterFilter_LoginComplete, above.
        // The BaseEvent method will only work in this class as it is derived from PluginBase. You will need to use the += and -= method in your other objects.
        [BaseEvent("ChangeObject", "WorldFilter")]
        public void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
        {
            try
            {
                // This can get very spammy so I filted it to just print on ident received

                // Util.WriteToChat("WorldFilter_ChangeObject: " + e.Changed.Name + " " + e.Change);
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
  
        // Methods
        private void Current_ChatBoxMessage(object sender, ChatTextInterceptEventArgs e)
        {
            try
            {

                if (ChatMessages.GetWeenieInfo(e.Text, out string wcid, out string guid))
                {
                    TextboxExportJsonWCID = (HudTextBox)view["TextboxExportJsonWCID"];
                    TextboxExportSQLWCID = (HudTextBox)view["TextboxExportSQLWCID"];
                    TextboxParentGUID = (HudTextBox)view["TextboxParentGUID"];                    

                    LabelGetInfo = (HudStaticText)view["LabelGetInfo"];
                    LabelGetInfo.Text = e.Text;
                    TextboxExportJsonWCID.Text = wcid;
                    TextboxExportSQLWCID.Text = wcid;
                    TextboxParentGUID.Text = guid;

                    Globals.YotesWCID = wcid;
                    if (Globals.ButtonCommand == "YotesLookup")
                    {
                        Util.WriteToChat("Opening Browser");
                        Globals.ButtonCommand = "";
                        System.Diagnostics.Process.Start("http://ac.yotesfan.com/weenies/items/" + wcid);
                        Globals.ButtonCommand = "NONE";
                    }
                    if (Globals.ButtonCommand == "PCAPsLookup")
                    {
                        Util.WriteToChat("Opening Browser");
                        Globals.ButtonCommand = "";
                        System.Diagnostics.Process.Start("https://github.com/ACEmulator/ACE-PCAP-Exports/search?q=filename:" + wcid);
                        Globals.ButtonCommand = "NONE";
                    }
                    // Util.WriteToChat(e.Text);
                }
                if (ChatMessages.FileExport(e.Text))
                {
                    JsonChoiceListLoadFiles();
                    SqlChoiceListLoadFiles();
                    Util.WriteToChat("ListRefresh");
                }
                if (ChatMessages.LogMyLocations(e.Text, out string location))
                {
                    if (TextboxCreateWCID.Text == "")
                        Util.LogLocation("BlankWCID, " + location);
                    else
                        Util.LogLocation(TextboxCreateWCID.Text + ", " + location);
                }
                if (ChatMessages.GetCurrentLandblock(e.Text, out string landblock))
                {
                    TextboxCurrentLandblock = (HudTextBox)view["TextboxCurrentLandblock"];

                    TextboxCurrentLandblock.Text = landblock;
                }
                //if (ChatMessages.GetParentGUID(e.Text, out string guid))
                //{
                //    TextboxParentGUID = (HudTextBox)view["TextboxParentGUID"];
                //    TextboxParentGUID.Text = guid;
                //}
            }
            catch (Exception ex)
            {
                Util.WriteToChat("ChatBoxMessage Error - " + ex);
            }
        }
        private void JsonChoiceListLoadFiles()
        {
            ChoiceJSON = (HudCombo)view["ChoiceJSON"];
            Util.WriteToChat(Globals.PathJSON);
            ChoiceJSON.Clear();
            string filespath = Globals.PathJSON;
            DirectoryInfo d = new DirectoryInfo(filespath);
            FileInfo[] files = d.GetFiles("*.json");

            foreach (var file in files)
            {
                // Util.WriteToChat(file.Name);
                ChoiceJSON.AddItem(file.Name, file);

            }
        }
        private void SqlChoiceListLoadFiles()
        {
            Util.WriteToChat(Globals.PathSQL);
            ChoiceSQL = (HudCombo)view["ChoiceSQL"];
            ChoiceChildList = (HudCombo)view["ChoiceChildList"];

            // ICombo addfile = JSONFileList.Add(File.AppendAllText)
            ChoiceSQL.Clear();
            ChoiceChildList.Clear();

            string filespath = Globals.PathSQL;
            DirectoryInfo d = new DirectoryInfo(filespath);
            FileInfo[] files = d.GetFiles("*.sql");

            foreach (var file in files)
            {
                // Util.WriteToChat(file.Name);
                ChoiceSQL.AddItem(file.Name, file.Name);
                ChoiceChildList.AddItem(file.Name, file.Name);

            }
        }        
        private void LoadPathSettings()
        {
            Dictionary<string, string> pathsettings = Util.LoadSetttings();
            TextBoxPathJSON = (HudTextBox)view["TextboxPathJSON"];
            TextBoxPathSQL = (HudTextBox)view["TextboxPathSQL"];
            TextboxPathLandBlockJSON = (HudTextBox)view["TextboxPathLandBlockJSON"];
            TextboxPathLandBlockSQL = (HudTextBox)view["TextboxPathLandBlockSQL"];

            if (pathsettings.ContainsKey("weenie_jsonpath")) //  && pathsettings["jsonpath"] != "")
            {
                TextBoxPathJSON.Text = pathsettings["weenie_jsonpath"];
                Globals.PathJSON = pathsettings["weenie_jsonpath"];
                Util.WriteToChat(pathsettings["weenie_jsonpath"]);
            }
            if (pathsettings.ContainsKey("weenie_sqlpath")) // && pathsettings["sqlpath"] != "")
            {
                TextBoxPathSQL.Text = pathsettings["weenie_sqlpath"];
                Globals.PathSQL = pathsettings["weenie_sqlpath"];
                Util.WriteToChat(pathsettings["weenie_sqlpath"]);
            }
            if (pathsettings.ContainsKey("landblock_jsonpath")) // && pathsettings["sqlpath"] != "")
            {
                TextboxPathLandBlockJSON.Text = pathsettings["landblock_jsonpath"];
                Globals.PathLandBlockJSON = pathsettings["landblock_jsonpath"];
                Util.WriteToChat(pathsettings["landblock_jsonpath"]);
            }
            if (pathsettings.ContainsKey("landblock_sqlpath")) // && pathsettings["sqlpath"] != "")
            {
                TextboxPathLandBlockSQL.Text = pathsettings["landblock_sqlpath"];
                Globals.PathLandBlockSQL = pathsettings["landblock_sqlpath"];
                Util.WriteToChat(pathsettings["landblock_sqlpath"]);
            }
            if (pathsettings.ContainsKey("quest_jsonpath")) 
            {
                TextboxPathQuestJSON.Text = pathsettings["quest_jsonpath"];
                Globals.PathQuestJSON = pathsettings["quest_jsonpath"];
                Util.WriteToChat(pathsettings["quest_jsonpath"]);
            }
            if (pathsettings.ContainsKey("quest_sqlpath")) 
            {
                TextboxPathQuestSQL.Text = pathsettings["quest_sqlpath"];
                Globals.PathQuestSQL = pathsettings["quest_sqlpath"];
                Util.WriteToChat(pathsettings["quest_sqlpath"]);
            }

            if (pathsettings.ContainsKey("recipe_jsonpath")) 
            {
                TextboxPathRecipeJSON.Text = pathsettings["recipe_jsonpath"];
                Globals.PathRecipeJSON = pathsettings["recipe_jsonpath"];
                Util.WriteToChat(pathsettings["recipe_jsonpath"]);
            }
            if (pathsettings.ContainsKey("recipe_sqlpath")) 
            {
                TextboxPathRecipeSQL.Text = pathsettings["recipe_sqlpath"];
                Globals.PathRecipeSQL = pathsettings["recipe_sqlpath"];
                Util.WriteToChat(pathsettings["recipe_sqlpath"]);
            }
            RefreshAllLists();
        }
        private void SavePathSettings(object sender, EventArgs e)
        {
            Util.SaveIni(TextBoxPathJSON.Text, TextBoxPathSQL.Text, TextboxPathLandBlockJSON.Text, TextboxPathLandBlockSQL.Text,
                         TextboxPathQuestJSON.Text, TextboxPathQuestSQL.Text, TextboxPathRecipeJSON.Text, TextboxPathRecipeSQL.Text);
        }
         private void GetInfoWaitForItemUpdate(object sender, ChangeObjectEventArgs e)
        {
            try
            {
                if (e.Changed.Id == aceItem.id)
                {

                    Util.SendChatCommand("/getinfo");
                    CoreManager.Current.WorldFilter.ChangeObject -= GetInfoWaitForItemUpdate;
                }
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        private void DeleteItemWaitForItemUpdate(object sender, ChangeObjectEventArgs e)
        {
            try
            {
                if (e.Changed.Id == aceItem.id)
                {
                    Util.SendChatCommand(Globals.ButtonCommand);
                    CoreManager.Current.WorldFilter.ChangeObject -= DeleteItemWaitForItemUpdate;
                    Globals.ButtonCommand = "NONE";
                }
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        private void WaitForItemUpdate(object sender, ChangeObjectEventArgs e)
        {
            try
            {
                if (e.Changed.Id == aceItem.id)
                {
                    Util.SendChatCommand(Globals.ButtonCommand);
                    CoreManager.Current.WorldFilter.ChangeObject -= WaitForItemUpdate;
                    Globals.ButtonCommand = "NONE";
                }
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        private void LookupYotesWaitForItemUpdate(object sender, ChangeObjectEventArgs e)
        {
            try
            {
                if (e.Changed.Id == aceItem.id)
                {
                    CoreManager.Current.WorldFilter.ChangeObject -= LookupYotesWaitForItemUpdate;                 
                }
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        private void LoadLandBlockJSONChoiceList()
        {

            if (Globals.PathLandBlockJSON == "")
            {
                Util.WriteToChat("JSON Landblock Path blank, Ignoring");
                return;
            }
            ChoiceLandblockJSON = (HudCombo)view["ChoiceLandblockJSON"];
            Util.WriteToChat(Globals.PathLandBlockJSON);
            ChoiceLandblockJSON.Clear();
            string filespath = Globals.PathLandBlockJSON;
            DirectoryInfo d = new DirectoryInfo(filespath);
            FileInfo[] files = d.GetFiles("*.json");

            foreach (var file in files)
            {
                // Util.WriteToChat(file.Name);
                ChoiceLandblockJSON.AddItem(file.Name, file);

            }
        }
        private void LoadLandBlockSQLChoiceList()
        {

            if (Globals.PathLandBlockSQL == "")
            {
                Util.WriteToChat("SQL Landblock Path blank, Ignoring");
                return;
            }
            ChoiceLandblockSQL = (HudCombo)view["ChoiceLandblockSQL"];
            Util.WriteToChat(Globals.PathLandBlockSQL);
            ChoiceLandblockSQL.Clear();
            string filespath = Globals.PathLandBlockSQL;
            DirectoryInfo d = new DirectoryInfo(filespath);
            FileInfo[] files = d.GetFiles("*.sql");

            foreach (var file in files)
            {
                // Util.WriteToChat(file.Name);
                ChoiceLandblockSQL.AddItem(file.Name, file);

            }
        }



        private void LoadQuestJSONChoiceList()
        {

            if (Globals.PathQuestJSON == "")
            {
                Util.WriteToChat("JSON Quest Path blank, Ignoring");
                return;
            }
            ChoiceQuestJSON = (HudCombo)view["ChoiceQuestJSON"];
            Util.WriteToChat(Globals.PathQuestJSON);
            ChoiceQuestJSON.Clear();
            string filespath = Globals.PathQuestJSON;
            DirectoryInfo d = new DirectoryInfo(filespath);
            FileInfo[] files = d.GetFiles("*.json");

            foreach (var file in files)
            {
                // Util.WriteToChat(file.Name);
                ChoiceQuestJSON.AddItem(file.Name, file);

            }
        }
        private void LoadQuestSQLChoiceList()
        {

            if (Globals.PathQuestSQL == "")
            {
                Util.WriteToChat("SQL Quest Path blank, Ignoring");
                return;
            }
            ChoiceQuestSQL = (HudCombo)view["ChoiceQuestSQL"];
            Util.WriteToChat(Globals.PathQuestSQL);
            ChoiceQuestSQL.Clear();
            string filespath = Globals.PathQuestSQL;
            DirectoryInfo d = new DirectoryInfo(filespath);
            FileInfo[] files = d.GetFiles("*.sql");

            foreach (var file in files)
            {
                // Util.WriteToChat(file.Name);
                ChoiceQuestSQL.AddItem(file.Name, file);

            }
        }



        private void LoadRecipeJSONChoiceList()
        {

            if (Globals.PathRecipeJSON == "")
            {
                Util.WriteToChat("JSON Recipe Path blank, Ignoring");
                return;
            }
            ChoiceRecipeJSON = (HudCombo)view["ChoiceRecipeJSON"];
            Util.WriteToChat(Globals.PathRecipeJSON);
            ChoiceRecipeJSON.Clear();
            string filespath = Globals.PathRecipeJSON;
            DirectoryInfo d = new DirectoryInfo(filespath);
            FileInfo[] files = d.GetFiles("*.json");

            foreach (var file in files)
            {
                // Util.WriteToChat(file.Name);
                ChoiceRecipeJSON.AddItem(file.Name, file);

            }
        }
        private void LoadRecipeSQLChoiceList()
        {

            if (Globals.PathRecipeSQL == "")
            {
                Util.WriteToChat("SQL Recipe Path blank, Ignoring");
                return;
            }
            ChoiceRecipeSQL = (HudCombo)view["ChoiceRecipeSQL"];
            Util.WriteToChat(Globals.PathRecipeSQL);
            ChoiceRecipeSQL.Clear();
            string filespath = Globals.PathRecipeSQL;
            DirectoryInfo d = new DirectoryInfo(filespath);
            FileInfo[] files = d.GetFiles("*.sql");

            foreach (var file in files)
            {
                // Util.WriteToChat(file.Name);
                ChoiceRecipeSQL.AddItem(file.Name, file);

            }
        }
        private void RefreshAllLists()
        {
            JsonChoiceListLoadFiles();
            SqlChoiceListLoadFiles();
            LoadLandBlockJSONChoiceList();
            LoadLandBlockSQLChoiceList();
            LoadQuestJSONChoiceList();
            LoadQuestSQLChoiceList();
            LoadRecipeJSONChoiceList();
            LoadRecipeSQLChoiceList();
        }

        public void LoadParentObjectsFile()
        {
            ChoiceGenerator = (HudCombo)view["ChoiceGenerator"];
            ChoiceGenerator.Clear();
            try
            {
                string assemblyFolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string filePath = System.IO.Path.Combine(assemblyFolder, "parentobjects.ini");

                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();

                    using (StreamWriter writer = new StreamWriter(filePath, false))
                    {
                        writer.WriteLine("1154 Linkable Monster Generator");
                        writer.WriteLine("4219 Linkable Monster Generator ( 7 Min. )");
                        writer.WriteLine("7923 Linkable Monster Generator ( 3 Min. )");
                        writer.WriteLine("7924 Linkable Monster Generator ( 5 Min. )");

                        writer.WriteLine("7925 Linkable Monster Generator ( 10 Min.)");
                        writer.WriteLine("7926 Linkable Monster Generator ( 20 Min.)");
                        writer.WriteLine("7932 Linkable Monster Generator ( 4 Min. )");
                        writer.WriteLine("21120	Linkable Monster Generator");
                        writer.WriteLine("24129	Linkable Monster Generator ( 2 Min.)");
                        writer.Close();
                    }
                }

                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in File.ReadAllLines(filePath))
                {
                    ChoiceGenerator.AddItem(line, line);
                }

            }
            catch (Exception ex)
            {
                                
                Util.WriteToChat(ex.Message);

            }
            
        }
    }
}
