using System;
using System.IO;
using System.Reflection;
using Decal.Adapter;
using Decal.Adapter.Wrappers;
using MyClasses.MetaViewWrappers;
using VirindiViewService;
using VirindiViewService.Controls;

/*
 * Created by Mag-nus. 8/19/2011, VVS added by Virindi-Inquisitor.
 * 
 * No license applied, feel free to use as you wish. H4CK TH3 PL4N3T? TR45H1NG 0UR R1GHT5? Y0U D3C1D3!
 * 
 * Notice how I use try/catch on every function that is called or raised by decal (by base events or user initiated events like buttons, etc...).
 * This is very important. Don't crash out your users!
 * 
 * In 2.9.6.4+ Host and Core both have Actions objects in them. They are essentially the same thing.
 * You sould use Host.Actions though so that your code compiles against 2.9.6.0 (even though I reference 2.9.6.5 in this project)
 * 
 * If you add this plugin to decal and then also create another plugin off of this sample, you will need to change the guid in
 * Properties/AssemblyInfo.cs to have both plugins in decal at the same time.
 * 
 * If you have issues compiling, remove the Decal.Adapater and VirindiViewService references and add the ones you have locally.
 * Decal.Adapter should be in C:\Games\Decal 3.0\
 * VirindiViewService should be in C:\Games\VirindiPlugins\VirindiViewService\
*/

namespace AceCreator
{

	public class PluginCore 
    {


		//[MVControlEvent("UseSelectedItem", "Click")]
		//void UseSelectedItem_Click(object sender, MVControlEventArgs e)
		//{
		//	try
		//	{
		//		if (Globals.Host.Actions.CurrentSelection == 0 || Globals.Core.WorldFilter[Globals.Host.Actions.CurrentSelection] == null)
		//		{
		//			Util.WriteToChat("UseSelectedItem no item selected");

		//			return;
		//		}
                
		//		Util.WriteToChat("UseSelectedItem " + Globals.Core.WorldFilter[Globals.Host.Actions.CurrentSelection].Name);

		//		Globals.Host.Actions.UseItem(Globals.Host.Actions.CurrentSelection, 0);
		//	}
		//	catch (Exception ex) { Util.LogError(ex); }
		//}

		//[MVControlEvent("ToggleAttack", "Click")]
		//void ToggleAttack_Click(object sender, MVControlEventArgs e)
		//{
		//	try
		//	{
		//		Util.WriteToChat("ToggleAttack");

		//		if (Globals.Host.Actions.CombatMode == CombatState.Peace)
		//			Globals.Host.Actions.SetCombatMode(CombatState.Melee);
		//		else
		//			Globals.Host.Actions.SetCombatMode(CombatState.Peace);
		//	}
		//	catch (Exception ex) { Util.LogError(ex); }
		//}

		//[MVControlEvent("CommandRefreshFilesList", "Click")]
		//void CommandRefreshFilesList_Click(object sender, MVControlEventArgs e)
		//{
		//	try
		//	{
  //              Util.WriteToChat("Reloading FileLists");
  //              //Util.SendChatCommand("/vt refresh");
  //              //JsonChoiceList();
  //              //SqlChoiceList();
  //              //InitSampleList();
  //              //Util.WriteToChat(Directory.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\temp\");

  //          }
  //          catch (Exception ex) { Util.LogError(ex); }
		//}





        //[MVControlReference("ChoiceJSON")]
        //private ICombo JSONFileList = null;

        //[MVControlReference("ChoiceSQL")]
        //private ICombo SQLFileList = null;



        //[MVControlReference("SampleList")]
        //private IList SampleList = null;







        //private void InitSampleList()
        //{
        //          string filespath = @"C:\Users\Harli\Source\Repos\harliq\DecalPluginTemplates\SamplePlugin-VVS\bin\Debug\temp";
        //          DirectoryInfo d = new DirectoryInfo(filespath);
        //          FileInfo[] files = d.GetFiles("*.json");

        // IListRow row = SampleList.Add();
        //Util.WriteToChat(Directory.GetCurrentDirectory() + @"\temp\");


        //foreach (var file in files)
        //{
        //    Util.WriteToChat(file.Name);
        //    IListRow row = SampleList.Add();
        //    row[1][0] = file.Name;
        //    //SampleList.Add(file.Name, file.Name);
        //    //SampleList
        //    //row[0] = file.Name;
        //}

        //foreach (WorldObject worldObject in Globals.Core.WorldFilter.GetByContainer(Globals.Core.CharacterFilter.Id))
        //{
        //IListRow row = SampleList.Add();

        // Notice we're using an index of 1 for the second parameter. That's because this is a DecalControls.IconColumn column.

        //row[0][1] = file
        //row[1][0] = worldObject.Name;

        // Also note that you can create an empty column. In our mainView.xml we have:
        // <column progid="DecalControls.TextColumn" name="colF" />
        // It is column index 5 and has no size associated with it. You can use this column to store an id of an item, or other misc data that you can use
        // later to grab info about the row, or maybe its sort order, etc..

        // To clear the list you can do:
        // SampleList.Clear();


        // If we want to check if this item is equipped, we could do the following
        //if (worldObject.Values(LongValueKey.EquippedSlots) > 0)
        //{
        //}

        // This check will pass if the object is wielded
        // Take note that someone else might be wielding this object. If you want to know if its wielded by YOU, you need to add another check.
        //if (worldObject.Values(LongValueKey.Slot, -1) == -1)
        //{
        //}

        // You can get an items current mana, but only if the item has been id'd, otherwise it will just return 0.
        //if (worldObject.HasIdData)
        //{
        //	int currentMana = worldObject.Values(LongValueKey.CurrentMana);
        //}

        // But also note that we don't know how long ago this item has been id'd. Maybe it was id'd an hour ago? The mana data would be erroneous.
        // So, we could get fresh id data for the object with the following:
        // Globals.Host.Actions.RequestId(worldObject.Id);

        // But now note that it may take a second or so to get that id data. So if we did the following:
        // Globals.Host.Actions.RequestId(worldObject.Id);
        // worldObject.Values(LongValueKey.CurrentMana) <-- This would still be the OLD information and not the new because the above statement hasn't finished.
        //}
        //}

        //[MVControlReference("SampleListText")]
        //private IStaticText SampleListText = null;

        //[MVControlReference("SampleListCheckBox")]
        //private ICheckBox SampleListCheckBox = null;

        //[MVControlEvent("SampleListCheckBox", "Change")]
        //void SampleListCheckBox_Change(object sender, MVCheckBoxChangeEventArgs e)
        //{
        //	try
        //	{
        //		Util.WriteToChat("SampleListCheckBox_Change " + SampleListCheckBox.Checked);

        //		//SampleListText.Text = SampleListCheckBox.Checked.ToString();
        //	}
        //	catch (Exception ex) { Util.LogError(ex); }
        //}


        //[MVControlEvent("CommandConvertJSON", "Click")]
        //void CommandConvertJSON_Click(object sender, MVControlEventArgs e)
        //{
        //    //jsonFileName = ["JSONFileList"];
            
        //    try
        //    {
        //        // Util.WriteToChat("File Selected " + HudStaticText(JSONFileList.Selected);
        //        Util.WriteToChat("File Text " + JSONFileList.Text);
        //        Util.WriteToChat("File Name " + JSONFileList.Name);
        //        Util.WriteToChat("File Data " + JSONFileList.Data);



        //        //Util.SendChatCommand("/vt refresh");
        //        //ChoiceList();
        //        //InitSampleList();
        //        //Util.WriteToChat(Directory.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\temp\");

        //    }
        //    catch (Exception ex) { Util.LogError(ex); }
        //}

    }
}
