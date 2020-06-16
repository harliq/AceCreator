using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using AceCreator;
namespace AceCreator
{
    public class ChatMessages
    {
        public static Collection<Regex> WeenieGetInfo = new Collection<Regex>();
        public static Collection<Regex> ExportFiles = new Collection<Regex>();
        public static Collection<Regex> MyLocations = new Collection<Regex>();
        public static Collection<Regex> MyLandblock = new Collection<Regex>();
        public static Collection<Regex> ParentGUID = new Collection<Regex>();

        public ChatMessages()
        {
            // Weenie WCID
            WeenieGetInfo.Add(new Regex("^GUID:.*"));
            ExportFiles.Add(new Regex("^Exported.*"));
            MyLocations.Add(new Regex("^Your location is:.*"));
            MyLandblock.Add(new Regex("^CurrentLandblock:.*"));
            ParentGUID.Add(new Regex("^GUID:.*"));
        }
        public static bool GetWeenieInfo(string text, out string wcid)
        {
            foreach (Regex regex in WeenieGetInfo)
            {
                if (regex.IsMatch(text))
                {

                    // Util.WriteToChat("CapGroup1= " + RegExGroup(@"WeenieClassId: (.*)", text));

                    wcid = RegExGroup(@"WeenieClassId: (.*)", text);
                    return true;

                }
            }
            wcid = "False";
            return false;
        }

        public static bool LogMyLocations(string text, out string location)
        {
            foreach (Regex regex in MyLocations)
            {
                if (regex.IsMatch(text))
                {

                    location = RegExGroup("^Your location is: (.*)", text);
                    return true;
                    
                }
            }
            location = "False";
            return false;
        }

        public static bool GetCurrentLandblock(string text, out string landblock)
        {
            foreach (Regex regex in MyLandblock)
            {
                if (regex.IsMatch(text))
                {

                    landblock = RegExGroup("^CurrentLandblock: (.*)", text);
                    return true;

                }
            }
            landblock = "False";
            return false;
        }
        public static bool GetParentGUID(string text, out string guid)
        {
            foreach (Regex regex in MyLandblock)
            {
                if (regex.IsMatch(text))
                {

                    guid = RegExGroup("^GUID: (.*)", text);
                    return true;

                }
            }
            guid = "False";
            return false;
        }

        public static string RegExGroup(string pattern, string text)
        {

            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);

            // match the regex pattern against string
            Match m = r.Match(text);
            Group g = m.Groups[1];
            string mystring = g.Value;

            return mystring;
        }
        public static bool FileExport(string text)
        {
            foreach (Regex regex in ExportFiles)
            {
                if (regex.IsMatch(text))
                {
                        return true;
                }
            }
            return false;
        }
    }
}