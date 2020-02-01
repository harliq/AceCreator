using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;

namespace AceCreator
{
    class ChatMessages
    {

        public static Collection<Regex> WeenieGetInfo = new Collection<Regex>();
        public ChatMessages()
        {
            // Weenie WCID
            WeenieGetInfo.Add(new Regex("^WeenieClassId:.*"));

            // Weenie Class Name
            // WeenieGetInfo.Add(new Regex("^WeenieClassName: (?<wcname>.+)$"));
        }
        public static bool GetWeenieInfo(string text)
        {
            foreach (Regex regex in WeenieGetInfo)
            {
                if (regex.IsMatch(text))
                    // Util.WriteToChat("GetWeenieInfo True");
                    return true;
            }

            return false;
        }
    }
}