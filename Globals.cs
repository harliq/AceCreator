﻿using System;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace AceCreator
{
	public static class Globals
	{
		public static void Init(string pluginName, PluginHost host, CoreManager core)
		{
			PluginName = pluginName;

			Host = host;

			Core = core;
		}

        

        public static string PluginName { get; private set; }

		public static PluginHost Host { get; private set; }

		public static CoreManager Core { get; private set; }

        public static string PathJSON;
        public static string PathSQL;

        public static string PathLandBlockJSON;
        public static string PathLandBlockSQL;

        public static string PathQuestJSON;
        public static string PathQuestSQL;

        public static string PathRecipeJSON;
        public static string PathRecipeSQL;

        public static string YotesWCID;
        public static string ButtonCommand;

        public static bool LogChat = false;
    }
}
