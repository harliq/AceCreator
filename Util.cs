using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Decal.Adapter;
using AceCreator.Lib;

namespace AceCreator
{
	public static class Util
	{
		public static void LogError(Exception ex)
		{
			try
			{
				using (StreamWriter writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\" + Globals.PluginName + " errors.txt", true))
				{
					writer.WriteLine("============================================================================");
					writer.WriteLine(DateTime.Now.ToString());
					writer.WriteLine("Error: " + ex.Message);
					writer.WriteLine("Source: " + ex.Source);
					writer.WriteLine("Stack: " + ex.StackTrace);
					if (ex.InnerException != null)
					{
						writer.WriteLine("Inner: " + ex.InnerException.Message);
						writer.WriteLine("Inner Stack: " + ex.InnerException.StackTrace);
					}
					writer.WriteLine("============================================================================");
					writer.WriteLine("");
					writer.Close();
				}
			}
			catch
			{
			}
		}
        
		public static void WriteToChat(string message)
		{
			try
			{
				Globals.Host.Actions.AddChatText("<{" + Globals.PluginName + "}>: " + message, 5);
			}
			catch (Exception ex) { LogError(ex); }
		}
        /// <summary>
        /// This will first attempt to send the messages to all plugins. If no plugins set e.Eat to true on the message, it will then simply call InvokeChatParser.
        /// </summary>
        /// <param name="chatcommand"></param>
        public static void SendChatCommand(string chatcommand)
        {
            try
            {
                DecalProxy.DispatchChatToBoxWithPluginIntercept(chatcommand);
            }
            catch (Exception ex) { LogError(ex); }
        }

        public static void SaveIni(string jsonpath, string sqlpath)
        {
            try
            {
                string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string filePath = Path.Combine(assemblyFolder, "acecreator.ini");

                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();

                    using (StreamWriter writer = new StreamWriter(filePath, false))
                    {
                        writer.WriteLine("jsonpath=");
                        writer.WriteLine("sqlpath=");
                        writer.Close();
                    }
                }

                string[] lines = File.ReadAllLines(filePath);

                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    writer.WriteLine("jsonpath=" + jsonpath);
                    writer.WriteLine("sqlpath=" + sqlpath);
                    writer.Close();
                    WriteToChat("Path Settings Saved to acecreator.ini");
                }
            }
            catch (Exception ex)
            {
                WriteToChat(ex.Message);
            }
        }

        internal static Dictionary<string, string> LoadSetttings()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(assemblyFolder, "acecreator.ini");
            Dictionary<string, string> temp = new Dictionary<string, string>();

            if (!File.Exists(filePath))
                SaveIni("", "");

            foreach (string line in File.ReadAllLines(filePath))
            {
                WriteToChat(line);
                string[] templineinfo = line.Split('=');
                    temp.Add(templineinfo[0], templineinfo[1]);
            }
            return temp;
        }
    }
}
