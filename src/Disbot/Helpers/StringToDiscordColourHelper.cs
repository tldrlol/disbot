using System.Collections.Generic;
using Discord;

namespace Disbot.Helpers
{
    public static class StringToDiscordColourHelper
    {
        private static Dictionary<string, Color> _colours = new Dictionary<string, Color>
        {
            ["white"] = Color.Default,
            ["blue"] = Color.Blue,
            ["darkblue"] = Color.DarkBlue,
            ["black"] = Color.DarkerGrey,
            ["green"] = Color.Green,
            ["darkgreen"] = Color.DarkGreen,
            ["darkgrey"] = Color.DarkGrey,
            ["darkgray"] = Color.DarkGrey,
            ["lightgrey"] = Color.LightGrey,
            ["lightgray"] = Color.LightGrey,
            ["magenta"] = Color.Magenta,
            ["darkmagenta"] = Color.DarkMagenta,
            ["orange"] = Color.Orange,
            ["darkorange"] =  Color.DarkOrange,
            ["lightorange"] = Color.LightOrange,
            ["purple"] = Color.Purple,
            ["darkpruple"] = Color.DarkPurple,
            ["red"] = Color.Red,
            ["darkred"] = Color.DarkRed,
            ["teal"] = Color.Teal,
            ["darkteal"] = Color.DarkTeal,
            ["gold"] = Color.Gold,
        };

        public static Color FromName(string rawName)
        {
            var normalizedName = rawName.ToLower();

            if (_colours.ContainsKey(normalizedName))
            {
                return _colours[normalizedName];
            }

            return Color.Default;
        }
    }
}
