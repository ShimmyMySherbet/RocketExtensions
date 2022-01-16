using System;
using System.Collections.Concurrent;
using System.Linq;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using RocketExtensions.Interfaces;
using SDG.Unturned;

namespace RocketExtensions.Models
{
    /// <summary>
    /// Used to parse strings
    /// </summary>
    public static class StringTypeConverter
    {
        private static readonly string[] m_ValsTrue = { "true", "enabled", "on", "active", "t", "1", "yes", "yep" };
        private static readonly string[] m_ValsFalse = { "false", "disabled", "off", "unactive", "f", "0", "no", "nah" };

        private static ConcurrentDictionary<Type, IStringParser> m_CustomParsers = new ConcurrentDictionary<Type, IStringParser>();

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
                    return EParseResult.PlayerNotFound;
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
            else if (t == typeof(ItemAsset))
            {
                if (ushort.TryParse(input, out var itid))
                {
                    var ast = Assets.find(EAssetType.ITEM, itid);
                    if (ast != null && ast is ItemAsset itemasset)
                    {
                        result = (T)(object)itemasset;
                        return EParseResult.Parsed;
                    }
                }
                else
                {
                    var byName = Assets.find(EAssetType.ITEM)
                        .Where(x => x is ItemAsset asts
                        && (asts.itemName != null
                        && asts.itemName.IndexOf(input, StringComparison.InvariantCultureIgnoreCase) != -1));
                    if (byName.Any())
                    {
                        result = (T)(object)byName.First();
                        return EParseResult.Parsed;
                    }
                }
                return EParseResult.ParseFailed;
            }
            else if (t == typeof(VehicleAsset))
            {
                if (ushort.TryParse(input, out var vhid))
                {
                    var ast = Assets.find(EAssetType.VEHICLE, vhid);
                    if (ast != null && ast is VehicleAsset vehicleasset)
                    {
                        result = (T)(object)vehicleasset;
                        return EParseResult.Parsed;
                    }
                }
                else
                {
                    var byName = Assets.find(EAssetType.ITEM)
                        .Where(x => x is VehicleAsset asts
                        && (asts.vehicleName != null
                        && asts.vehicleName.IndexOf(input, StringComparison.InvariantCultureIgnoreCase) != -1));
                    if (byName.Any())
                    {
                        result = (T)(object)byName.First();
                        return EParseResult.Parsed;
                    }
                }
                return EParseResult.ParseFailed;
            }
            else if (t == typeof(AnimalAsset))
            {
                if (ushort.TryParse(input, out var assetID))
                {
                    var ast = Assets.find(EAssetType.ANIMAL, assetID);
                    if (ast != null && ast is AnimalAsset instanceAsset)
                    {
                        result = (T)(object)instanceAsset;
                        return EParseResult.Parsed;
                    }
                }
                else
                {
                    var byName = Assets.find(EAssetType.ANIMAL)
                        .Where(x => x is AnimalAsset asts
                        && (asts.animalName != null
                        && asts.animalName.IndexOf(input, StringComparison.InvariantCultureIgnoreCase) != -1));
                    if (byName.Any())
                    {
                        result = (T)(object)byName.First();
                        return EParseResult.Parsed;
                    }
                }
                return EParseResult.ParseFailed;
            }
            else if (t == typeof(LocationNode))
            {
                var match = LevelNodes.nodes.FirstOrDefault(x => x is LocationNode ln && !string.IsNullOrEmpty(input) && ln.name.IndexOf(input, StringComparison.InvariantCultureIgnoreCase) != -1);
                if (match != null)
                {
                    result = (T)(object)match;
                    return EParseResult.Parsed;
                }
                return EParseResult.ParseFailed;
            }
            else
            {
                var parser = GetParser(t);
                try
                {
                    if (parser != null)
                    {
                        result = parser.Parse<T>(input, out var epr);
                        return epr;
                    }
                }
                catch (Exception ex)  // Don't want a broken parser breaking all RocketExtensions commands
                {
                    Logger.LogError($"Error occurred while running custom parser of type {parser.GetType().FullName}");
                    Logger.LogError(ex.Message);
                    Logger.LogError(ex.StackTrace);
                }
            }

            return EParseResult.InvalidType;
        }

        public static Player ParsePlayer(string handle)
        {
            if (ulong.TryParse(handle, out var uid))
            {
                var pl = PlayerTool.getSteamPlayer(uid).player;
                return pl;
            }
            return PlayerTool.getPlayer(handle);
        }

        public static void RegisterParser(IStringParser parser)
        {
            if (parser == null)
            {
                throw new ArgumentNullException("parser");
            }
            m_CustomParsers[parser.Type] = parser;
        }

        public static void DeregisterParser(IStringParser parser)
        {
            if (m_CustomParsers.TryGetValue(parser.Type, out var psr))
            {
                if (psr.GetType() == parser.GetType())
                {
                    m_CustomParsers.TryRemove(psr.GetType(), out _);
                }
            }
        }

        public static IStringParser GetParser(Type type)
        {
            if (m_CustomParsers.TryGetValue(type, out var psr))
            {
                return psr;
            }
            return null;
        }
    }
}