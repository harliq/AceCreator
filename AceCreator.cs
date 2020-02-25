﻿using System;
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
    public class AceCreator : PluginBase
    {
        private ChatMessages cm = new ChatMessages();
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

        // LandBlocksTab
        public HudCombo ChoiceLandblockJSON { get; set; }
        public HudButton ButtonImportLandblockJSON { get; set; }
        public HudButton ButtonEditLandblockJSON { get; set; }

        public HudCombo ChoiceLandblockSQL { get; set; }
        public HudButton ButtonImportLandblockSQL { get; set; }
        public HudButton ButtonEditLandblockSQL { get; set; }

        public HudButton ButtonReloadLandblock { get; set; }
        public HudButton ButtonClearCache { get; set; }

        // Paths Tab
        public HudTextBox TextBoxPathJSON { get; set; }
        public HudTextBox TextBoxPathSQL { get; set; }
        public HudTextBox TextboxPathLandBlockJSON { get; set; }
        public HudTextBox TextboxPathLandBlockSQL { get; set; }


        public HudButton ButtonSavePaths { get; set; }
        public HudButton ButtonLoadINI { get; set; }
        public HudButton ButtonOpenINI { get; set; }


        


        private static VirindiViewService.ViewProperties properties;
        private static VirindiViewService.ControlGroup controls;
        private static VirindiViewService.HudView view;
        public static HudTabView TabView { get; private set; }

        //  For seeing ID has completed
        WorldObject WO;
        private readonly AceItem aceItem = new AceItem();

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
                JsonChoiceList();
                SqlChoiceList();
                LoadLandBlockJSONChoiceList();
                LoadLandBlockSQLChoiceList();
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

            // Content Tab
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

            // ***** LandBlocksTab *****

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
            


            // Paths Tab
            TextBoxPathJSON = (HudTextBox)view["TextboxPathJSON"];
            TextBoxPathSQL = (HudTextBox)view["TextboxPathSQL"];
            TextboxPathLandBlockJSON = (HudTextBox)view["TextboxPathLandBlockJSON"];
            TextboxPathLandBlockSQL = (HudTextBox)view["TextboxPathLandBlockSQL"];


            

            ButtonSavePaths = view != null ? (HudButton)view["ButtonSavePaths"] : new HudButton();
            ButtonSavePaths.Hit += new EventHandler(ButtonSavePaths_Click);

            ButtonLoadINI = view != null ? (HudButton)view["ButtonLoadINI"] : new HudButton();
            ButtonLoadINI.Hit += new EventHandler(ButtonLoadINI_Click);

            ButtonOpenINI = view != null ? (HudButton)view["ButtonOpenINI"] : new HudButton();
            ButtonOpenINI.Hit += new EventHandler(ButtonOpenINI_Click);
            
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

        private void Current_ChatBoxMessage(object sender, ChatTextInterceptEventArgs e)
        {
            try
            {

                if (ChatMessages.GetWeenieInfo(e.Text, out string wcid))
                {
                    TextboxExportJsonWCID = (HudTextBox)view["TextboxExportJsonWCID"];
                    TextboxExportSQLWCID = (HudTextBox)view["TextboxExportSQLWCID"];

                    LabelGetInfo = (HudStaticText)view["LabelGetInfo"];
                    LabelGetInfo.Text = e.Text;
                    TextboxExportJsonWCID.Text = wcid;
                    TextboxExportSQLWCID.Text = wcid;
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
                    JsonChoiceList();
                    SqlChoiceList();
                    Util.WriteToChat("ListRefresh");
                }
                if (ChatMessages.LogMyLocations(e.Text, out string location))
                {
                    if (TextboxCreateWCID.Text == "")
                        Util.LogLocation("BlankWCID, " + location);
                    else
                        Util.LogLocation(TextboxCreateWCID.Text + ", " + location);
                }
            }
            catch (Exception ex)
            {
                Util.WriteToChat("ChatBoxMessage Error - " + ex);
            }
        }
        private void JsonChoiceList()
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
        private void SqlChoiceList()
        {
            Util.WriteToChat(Globals.PathSQL);
            ChoiceSQL = (HudCombo)view["ChoiceSQL"];
            // ICombo addfile = JSONFileList.Add(File.AppendAllText)
            ChoiceSQL.Clear();
            string filespath = Globals.PathSQL;
            DirectoryInfo d = new DirectoryInfo(filespath);
            FileInfo[] files = d.GetFiles("*.sql");

            foreach (var file in files)
            {
                // Util.WriteToChat(file.Name);
                ChoiceSQL.AddItem(file.Name, file.Name);

            }
        }

        private void LoadLandBlockJSONChoiceList()
        {


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

        }
        private void SavePathSettings(object sender, EventArgs e)
        {
            Util.SaveIni(TextBoxPathJSON.Text, TextBoxPathSQL.Text, TextboxPathLandBlockJSON.Text, TextboxPathLandBlockSQL.Text);
        }
        // ComboBox Change
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
        // Buttons
        public void ButtonConvertSQL_Click(object sender, EventArgs e)
        {
            try
            {
                string tsplit = ((HudStaticText)ChoiceSQL[ChoiceSQL.Current]).Text;
                TextboxCreateWCID = (HudTextBox)view["TextboxCreateWCID"];
                Util.SendChatCommand(@"/import-sql " + tsplit.Split(' ')[0]);
                // Util.WriteToChat("Imported SQL= " + ((HudStaticText)ChoiceSQL[ChoiceSQL.Current]).Text);
                TextboxCreateWCID.Text = tsplit.Split(' ')[0];
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonConvertJSON_Click(object sender, EventArgs e)
        {
            try
            {
                string tsplit = ((HudStaticText)ChoiceJSON[ChoiceJSON.Current]).Text;
                TextboxCreateWCID = (HudTextBox)view["TextboxCreateWCID"];
                Util.SendChatCommand(@"/import-json " + tsplit.Split(' ')[0]);
                // Util.WriteToChat("Imported JSON= " + ((HudStaticText)ChoiceJSON[ChoiceJSON.Current]).Text);                
                TextboxCreateWCID.Text = tsplit.Split(' ')[0];
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonRefreshFilesList_Click(object sender, EventArgs e)
        {
            try
            {
                Util.WriteToChat("Reloading FileLists");

                JsonChoiceList();
                SqlChoiceList();
                LoadLandBlockJSONChoiceList();
                LoadLandBlockSQLChoiceList();


                // Util.WriteToChat("Text= " + ((HudStaticText)ChoiceSQL[ChoiceSQL.Current]).Text);
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonSavePaths_Click(object sender, EventArgs e)
        {
            try
            {
                Util.WriteToChat("Writing Ini File");
                Util.SaveIni(TextBoxPathJSON.Text, TextBoxPathSQL.Text, TextboxPathLandBlockJSON.Text, TextboxPathLandBlockSQL.Text);
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
                JsonChoiceList();
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonExportSQL_Click(object sender, EventArgs e)
        {
            try
            {

                Util.SendChatCommand(@"/export-sql " + TextboxExportSQLWCID.Text);
                SqlChoiceList();
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonGetInfo_Click(object sender, EventArgs e)
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
        public void ButtonDeleteItem_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.ButtonCommand = "/delete";
                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += DeleteItemWaitForItemUpdate;
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
        public void ButtonRemoveInstace_Click(object sender, EventArgs e)
        {

            try
            {
                Globals.ButtonCommand = "/removeinst";

                WO = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];
                aceItem.name = WO.Name;
                aceItem.id = WO.Id;

                Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);
                CoreManager.Current.WorldFilter.ChangeObject += DeleteItemWaitForItemUpdate;
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonMyLocation_Click(object sender, EventArgs e)
        {

            try
            {
                Util.SendChatCommand("/loc");
                // LabelCurrentLandblock = (HudStaticText)view["LabelCurrentLandblock"];
                // var myLandCell = CoreManager.Current.Actions.Landcell;
                // LabelCurrentLandblock.Text = CoreManager.Current.Actions.Landcell.ToString("X");
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
        public void ButtonImportLandblockJSON_Click(object sender, EventArgs e)
        {
            try
            {
                // string tsplit = ((HudStaticText)ChoiceLandblockJSON[ChoiceLandblockJSON.Current]).Text;
                string wcid = ((HudStaticText)ChoiceLandblockJSON[ChoiceLandblockJSON.Current]).Text.Replace(".json","");
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
    }
}
