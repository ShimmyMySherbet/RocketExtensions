using System.Text.RegularExpressions;

namespace RocketExtensions.Utilities
{
    public static class ColorReformatter
    {
        public static readonly Regex ColorOpeningMatch = new Regex(@"\[color=[a-zA-Z0-9#]+\]");
        public static readonly Regex ColorClosing = new Regex(@"\[/color\]");

        public static string ReformatColor(this string key)
        {
            var openings = ColorOpeningMatch.Matches(key);
            for (int i = 0; i < openings.Count; i++)
            {
                var opening = openings[i];
                if (!opening.Success)
                    continue;
                var color = opening.Value.Substring(7);
                color = color.Substring(0, color.Length - 1);

                // Same size, no need to worry about adjusting offsets
                key = key
                    .Remove(opening.Index, opening.Length)
                    .Insert(opening.Index, $"<color={color}>");
            }
            key = ColorClosing.Replace(key, "</color>", 20);
            return key;
        }
    }
}