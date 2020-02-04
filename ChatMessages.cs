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
        public ChatMessages()
        {
            // Weenie WCID
            WeenieGetInfo.Add(new Regex("^WeenieClassId:.*"));

            // Weenie Class Name
            // WeenieGetInfo.Add(new Regex("^WeenieClassName: (?<wcname>.+)$"));
        }
        public static bool GetWeenieInfo(string text, out string wcid)
        {
            foreach (Regex regex in WeenieGetInfo)
            {
                if (regex.IsMatch(text))
                {

                    Util.WriteToChat("CapGroup1= " + RegExGroup(@"WeenieClassId: (.*)", text));
                    wcid = RegExGroup(@"WeenieClassId: (.*)", text);
                    return true;
                    

                }
            }
            wcid = "False";
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
    }
}