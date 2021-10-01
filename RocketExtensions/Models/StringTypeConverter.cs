using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System.Linq;

namespace RocketExtensions.Models
{
    /// <summary>
    /// Used to parse strings
    /// </summary>
    public static class StringTypeConverter
    {
        private static readonly string[] m_ValsTrue = { "true", "enabled", "on", "active", "t", "1", "yes" };
        private static readonly string[] m_ValsFalse = { "false", "disabled", "off", "unactive", "f", "0", "no" };

        public static EParseResult Parse<T>(string input, out T result)
        {
            result = default(T);
            var t = typeof(T);
            if (t == typeof(string))
            {
                result = (T)(object)input;
                return EParseResult.Parsed;
            }
            else if (t == typeof(sbyte))
            {
                if (sbyte.TryParse(input, out var intr))
                {
                    result = (T)(object)intr;
                    return EParseResult.Parsed;
                }
                else
                {
                    return EParseResult.ParseFailed;
                }
            }
            else if (t == typeof(byte))
            {
                if (byte.TryParse(input, out var intr))
                {
                    result = (T)(object)intr;
                    return EParseResult.Parsed;
                }
                else
                {
                    return EParseResult.ParseFailed;
                }
            }
            else if (t == typeof(short))
            {
                if (short.TryParse(input, out var intr))
                {
                    result = (T)(object)intr;
                    return EParseResult.Parsed;
                }
                else
                {
                    return EParseResult.ParseFailed;
                }
            }
            else if (t == typeof(ushort))
            {
                if (ushort.TryParse(input, out var intr))
                {
                    result = (T)(object)intr;
                    return EParseResult.Parsed;
                }
                else
                {
                    return EParseResult.ParseFailed;
                }
            }
            else if (t == typeof(int))
            {
                if (int.TryParse(input, out var intr))
                {
                    result = (T)(object)intr;
                    return EParseResult.Parsed;
                }
                else
                {
                    return EParseResult.ParseFailed;
                }
            }
            else if (t == typeof(uint))
            {
                if (uint.TryParse(input, out var intr))
                {
                    result = (T)(object)intr;
                    return EParseResult.Parsed;
                }
                else
                {
                    return EParseResult.ParseFailed;
                }
            }
            else if (t == typeof(float))
            {
                if (float.TryParse(input, out var intr))
                {
                    result = (T)(object)intr;
                    return EParseResult.Parsed;
                }
                else
                {
                    return EParseResult.ParseFailed;
                }
            }
            else if (t == typeof(double))
            {
                if (double.TryParse(input, out var intr))
                {
                    result = (T)(object)intr;
                    return EParseResult.Parsed;
                }
                else
                {
                    return EParseResult.ParseFailed;
                }
            }
            else if (t == typeof(long))
            {
                if (long.TryParse(input, out var intr))
                {
                    result = (T)(object)intr;
                    return EParseResult.Parsed;
                }
                else
                {
                    return EParseResult.ParseFailed;
                }
            }
            else if (t == typeof(ulong))
            {
                if (ulong.TryParse(input, out var intr))
                {
                    result = (T)(object)intr;
                    return EParseResult.Parsed;
                }
                else
                {
                    return EParseResult.ParseFailed;
                }
            }
            else if (t == typeof(Player))
            {
                var pl = ParsePlayer(input);
                if (pl != null)
                {
                    result = (T)(object)pl;
                    return EParseResult.Parsed;
                }
                else
                {
                    return EParseResult.ParseFailed;
                }
            }
            else if (t == typeof(UnturnedPlayer))
            {
                var pl = ParsePlayer(input);
                if (pl != null)
                {
                    result = (T)(object)UnturnedPlayer.FromPlayer(pl);
                    return EParseResult.Parsed;
                }
                else
                {
                    return EParseResult.PlayerNotFound;
                }
            }
            else if (t == typeof(SteamPlayer))
            {
                var pl = ParsePlayer(input);
                if (pl != null)
                {
                    result = (T)(object)pl.channel.owner;
                    return EParseResult.Parsed;
                }
                else
                {
                    return EParseResult.PlayerNotFound;
                }
            }
            else if (t == typeof(bool))
            {
                var lower = input.ToLowerInvariant();

                if (m_ValsTrue.Contains(lower))
                {
                    result = (T)(object)true;
                    return EParseResult.Parsed;
                }
                else if (m_ValsFalse.Contains(lower))
                {
                    result = (T)(object)false;
                    return EParseResult.Parsed;
                }
                return EParseResult.ParseFailed;
            }

            return EParseResult.InvalidType;
        }

        public static Player ParsePlayer(string handle)
        {
            if (ulong.TryParse(handle, out var uid))
            {
                var pl = PlayerTool.getPlayer(new CSteamID(uid));
                return pl;
            }

            return PlayerTool.getPlayer(handle);
        }
    }
}