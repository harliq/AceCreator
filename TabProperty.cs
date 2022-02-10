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

        public HudCombo ChoicePropertyType { get; set; }

        public HudButton ButtonSetProperty { get; set; }

        public HudTextBox TextboxProperty { get; set; }
        public HudTextBox TextboxPropertyValue { get; set; }

        public HudButton ButtonPropertyDump { get; set; }

        public HudCheckBox CheckBoxSavePropertyDump { get; set; }

        public HudButton ButtonDebug { get; set; }

        

        public void ButtonSetProperty_Click(object sender, EventArgs e)
        {
            
            string propertyType = "TYPE_VALUE";
            if(string.IsNullOrEmpty(TextboxProperty.Text))
            {
                Util.WriteToChat("Property Box is empty");
                return;
            }
            //Bool
            if (((HudStaticText)ChoicePropertyType[ChoicePropertyType.Current]).Text == "Bool")
            {
                //Util.WriteToChat("Boolean");
                if (BoolTypes.TryGetValue(Convert.ToInt32(TextboxProperty.Text), out string tempPropertyType));
                    propertyType = tempPropertyType;
            }
            //Int
            if (((HudStaticText)ChoicePropertyType[ChoicePropertyType.Current]).Text == "Int")
            {
                //Util.WriteToChat("Int32");
                if (IntTypes.TryGetValue(Convert.ToInt32(TextboxProperty.Text), out string tempPropertyType));
                propertyType = tempPropertyType;
            }
            //Int64
            if (((HudStaticText)ChoicePropertyType[ChoicePropertyType.Current]).Text == "Int64")
            {
                //Util.WriteToChat("Boolean");
                if (Int64Types.TryGetValue(Convert.ToInt32(TextboxProperty.Text), out string tempPropertyType));
                propertyType = tempPropertyType;
            }
            //Float
            if (((HudStaticText)ChoicePropertyType[ChoicePropertyType.Current]).Text == "Float")
            {
                //Util.WriteToChat("Boolean");
                if (FloatTypes.TryGetValue(Convert.ToInt32(TextboxProperty.Text), out string tempPropertyType));
                propertyType = tempPropertyType;
            }
            //String
            if (((HudStaticText)ChoicePropertyType[ChoicePropertyType.Current]).Text == "String")
            {
                //Util.WriteToChat("Boolean");
                if (StringTypes.TryGetValue(Convert.ToInt32(TextboxProperty.Text), out string tempPropertyType));
                propertyType = tempPropertyType;
            }
            if (string.IsNullOrEmpty(TextboxPropertyValue.Text))
            {
                Util.WriteToChat("Property Value Box is empty");
                return;
            }
            try
            {
                string setPropertyCommand = "/setproperty Property" + ((HudStaticText)ChoicePropertyType[ChoicePropertyType.Current]).Text + "." + propertyType + " " + TextboxPropertyValue.Text;
                Globals.ButtonCommand = setPropertyCommand;
                CommandWait(sender, e);

                // Util.WriteToChat("/setproperty Property" + ((HudStaticText)ChoicePropertyType[ChoicePropertyType.Current]).Text + "." + propertyType + " " + TextboxPropertyValue.Text);
                Util.WriteToChat(setPropertyCommand);

            }
            catch (Exception ex) { Util.LogError(ex); }

            
            
        }
        public void ButtonPropertyDump_Click(object sender, EventArgs e)
        {
            try
            {
                //Util.WriteToChat("CheckBox Value= " + CheckBoxSavePropertyDump.Checked);
                
                if (CheckBoxSavePropertyDump.Checked == true)
                {
                    Globals.LogChat = true;

                }
                Globals.ButtonCommand = "/propertydump";
                CommandWait(sender, e);
            }
            catch (Exception ex) { Util.LogError(ex); }
        }
        public void ButtonDebug_Click(object sender, EventArgs e)
        {
            Util.SendChatCommand("/attackable on");

            Util.SendChatCommand("/create 53365 10"); // Wind Fury
            Util.SendChatCommand("/create 72000 5");  // Night Brier
            Util.SendChatCommand("/create 72001 5");  // Nor
            Util.SendChatCommand("/create 72002 5");  // Zer 
            Util.SendChatCommand("/ci 8391");  // Beer

            Util.WriteToChat("Here you go you show off!");
                        
        }
    }
}
