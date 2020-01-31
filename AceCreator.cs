using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
    public class AceCreator: PluginBase
    {
        public HudCombo ChoiceJSON { get; set; }
        public HudCombo ChoiceSQL { get; set; }
        public HudButton CommandConvertSQL { get; set; }
        public HudButton CommandConvertJSON { get; set; }
        public HudButton CommandRefreshFilesList { get; set; }
        public HudTextBox TextboxCreateWCID { get; set; }
        public HudButton ButtonCreateWCID { get; set; }
        public HudButton ButtonCreateInvWCID { get; set; }
        public HudButton ButtonCreateInstantWCID { get; set; }

        public HudTextBox TextboxExportJsonWCID { get; set; }
        public HudButton ButtonExportJSON { get; set; }
        public HudTextBox TextboxExportSQLWCID { get; set; }
        public HudButton ButtonExportSQL { get; set; }


        public HudTextBox TextBoxPathJSON { get; set; }
        public HudTextBox TextBoxPathSQL { get; set; }
        public HudButton ButtonSavePaths { get; set; }
        public HudButton ButtonTest { get; set; }


        private static VirindiViewService.ViewProperties properties;
        private static VirindiViewService.ControlGroup controls;
        private static VirindiViewService.HudView view;
        public static HudTabView TabView { get; private set; }

        /// <summary>
        /// This is called when the plugin is started up. This happens only once.
        /// </summary>
        protected override void Startup()
        {
            try
            {
                // This initializes our static Globals class with references to the key objects your plugin will use, Host and Core.
                // The OOP way would be to pass Host and Core to your objects, but this is easier.
                Globals.Init("AceCreator", Host, Core);
                LoadWindow();
                LoadPathSettings();
                JsonChoiceList();
                SqlChoiceList();
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
            view.Title = "ACE Content Creator - Version " + typeof(AceCreator).Assembly.GetName().Version;

            // Content Tab
            ChoiceJSON = (HudCombo)view["ChoiceJSON"];
            ChoiceJSON.Change += new EventHandler(ChoiceJSON_Change);
            ChoiceSQL = (HudCombo)view["ChoiceSQL"];
            ChoiceSQL.Change += new EventHandler(ChoiceSQL_Change);

            CommandConvertSQL = view != null ? (HudButton)view["CommandConvertSQL"] : new HudButton();
            CommandConvertSQL.Hit += new EventHandler(ButtonConvertSQL_Click);

            CommandConvertJSON = view != null ? (HudButton)view["CommandConvertJSON"] : new HudButton();
            CommandConvertJSON.Hit += new EventHandler(ButtonConvertJSON_Click);

            CommandRefreshFilesList = view != null ? (HudButton)view["CommandRefreshFilesList"] : new HudButton();
            CommandRefreshFilesList.Hit += new EventHandler(ButtonRefreshFilesList_Click);

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
            


            // Paths Tab
            TextBoxPathJSON = (HudTextBox)view["TextboxPathJSON"];
            TextBoxPathSQL = (HudTextBox)view["TextboxPathSQL"];

            ButtonSavePaths = view != null ? (HudButton)view["ButtonSavePaths"] : new HudButton();
            ButtonSavePaths.Hit += new EventHandler(ButtonSavePaths_Click);

            ButtonTest = view != null ? (HudButton)view["ButtonTest"] : new HudButton();
            ButtonTest.Hit += new EventHandler(ButtonTest_Click);

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

                Globals.Core.WorldFilter.ChangeObject -= new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject2);
            }
            catch (Exception ex) { Util.LogError(ex); }
        }


        // Note that there are several ways to latch on to decals events.
        // You can use the BaseEvent attribute, or you can latch on to the same event as shown in CharacterFilter_LoginComplete, above.
        // The BaseEvent method will only work in this class as it is derived from PluginBase. You will need to use the += and -= method in your other objects.
        [BaseEvent("ChangeObject", "WorldFilter")]
        void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
        {
            try
            {
                // This can get very spammy so I filted it to just print on ident received
                // if (e.Change == WorldChangeType.IdentReceived)
                // Util.WriteToChat("WorldFilter_ChangeObject: " + e.Changed.Name + " " + e.Change);
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        void WorldFilter_ChangeObject2(object sender, ChangeObjectEventArgs e)
        {
        }


        public void ButtonConvertSQL_Click(object sender, EventArgs e)
        {
            try
            {
                TextboxCreateWCID = (HudTextBox)view["TextboxCreateWCID"];
                Util.SendChatCommand(@"/import-sql " + ((HudStaticText)ChoiceSQL[ChoiceSQL.Current]).Text);                
                Util.WriteToChat("Imported SQL= " + ((HudStaticText)ChoiceSQL[ChoiceSQL.Current]).Text);

                string tsplit = ((HudStaticText)ChoiceSQL[ChoiceSQL.Current]).Text;
                TextboxCreateWCID.Text = tsplit.Split(' ')[0];
            }
            catch (Exception ex) { Util.LogError(ex); }

        }

        public void ButtonConvertJSON_Click(object sender, EventArgs e)
        {
            try
            {
                TextboxCreateWCID = (HudTextBox)view["TextboxCreateWCID"];
                Util.SendChatCommand(@"/import-json " + ((HudStaticText)ChoiceJSON[ChoiceJSON.Current]).Text);
                Util.WriteToChat("Imported JSON= " + ((HudStaticText)ChoiceJSON[ChoiceJSON.Current]).Text);

                string tsplit = ((HudStaticText)ChoiceJSON[ChoiceJSON.Current]).Text;
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

                Util.WriteToChat("Text= " + ((HudStaticText)ChoiceSQL[ChoiceSQL.Current]).Text);
            }
            catch (Exception ex) { Util.LogError(ex); }

        }

        private void JsonChoiceList()
        {
            ChoiceJSON = (HudCombo)view["ChoiceJSON"];
            Util.WriteToChat(Globals.PathJSON);
            // ICombo addfile = JSONFileList.Add(File.AppendAllText)
            ChoiceJSON.Clear();
            string filespath = Globals.PathJSON;
            DirectoryInfo d = new DirectoryInfo(filespath);
            FileInfo[] files = d.GetFiles("*.json");

            foreach (var file in files)
            {
                Util.WriteToChat(file.Name);
                ChoiceJSON.AddItem(file.Name,file);

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
                Util.WriteToChat(file.Name);
                ChoiceSQL.AddItem(file.Name, file.Name);

            }
        }
        private void LoadPathSettings()
        {
            Dictionary<string, string> pathsettings = Util.LoadSetttings();
            TextBoxPathJSON = (HudTextBox)view["TextboxPathJSON"];
            TextBoxPathSQL = (HudTextBox)view["TextboxPathSQL"];

            if (pathsettings.ContainsKey("jsonpath")) //  && pathsettings["jsonpath"] != "")
            {
                TextBoxPathJSON.Text = pathsettings["jsonpath"];
                Globals.PathJSON = pathsettings["jsonpath"];
                Util.WriteToChat(pathsettings["jsonpath"]);
            }

            if (pathsettings.ContainsKey("sqlpath")) // && pathsettings["sqlpath"] != "")
            {
                TextBoxPathSQL.Text = pathsettings["sqlpath"];
                Globals.PathSQL = pathsettings["sqlpath"];
                Util.WriteToChat(pathsettings["sqlpath"]);
            }

           
        }
        private void SavePathSettings(object sender, EventArgs e)
        {
            //EmailSender = new Email(email.Text, host.Text, int.Parse(port.Text), ss1.Checked, pass.Text);
            //DiscordSender = new Discord(discordurl.Text);
            Util.SaveIni(TextBoxPathJSON.Text, TextBoxPathSQL.Text);
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
        public void ButtonSavePaths_Click(object sender, EventArgs e)
        {
            try
            {
                Util.WriteToChat("Writing Ini File");
                Util.SaveIni(TextBoxPathJSON.Text, TextBoxPathSQL.Text);
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonTest_Click(object sender, EventArgs e)
        {
            try
            {
                LoadPathSettings();
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
            }
            catch (Exception ex) { Util.LogError(ex); }

        }
        public void ButtonExportSQL_Click(object sender, EventArgs e)
        {
            try
            {
                
                Util.SendChatCommand(@"/export-sql " + TextboxExportJsonWCID.Text);
            }
            catch (Exception ex) { Util.LogError(ex); }

        }

        //Globals.Host.Actions.RequestId(Globals.Host.Actions.CurrentSelection);


    }

}
