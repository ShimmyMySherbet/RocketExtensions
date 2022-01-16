using System;
using System.Linq;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using Rocket.Unturned.Skills;
using RocketExtensions.Utilities;
using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Color = UnityEngine.Color;

namespace RocketExtensions.Models
{
    /// <summary>
    /// Provides async methods to interact with a player from any thread.
    /// </summary>
    public class LDMPlayer
    {
        public LDMPlayer(IRocketPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException("IRocketPlayer player cannot be null");
            RocketPlayer = player;
        }

        internal LDMPlayer(Player player) : this(UnturnedPlayer.FromPlayer(player))
        {
        }

        public string DisplayName => UnturnedPlayer?.DisplayName ?? "Console";

        public bool IsAdmin => RocketPlayer.IsAdmin;

        public static LDMPlayer FromRocketPlayer(IRocketPlayer player) => new LDMPlayer(player);

        public static LDMPlayer FromPlayer(Player player) => new LDMPlayer(player);

        public static LDMPlayer FromName(string player) => new LDMPlayer(UnturnedPlayer.FromName(player));

        public bool IsConsole => RocketPlayer is ConsolePlayer || (RocketPlayer.IsAdmin && RocketPlayer.Id == "0");

        public Vector3 Position => Player?.transform?.position ?? Vector3.zero;

        public float Rotation => Player?.transform?.rotation.eulerAngles.y ?? 0;

        public InteractableVehicle CurrentVehicle => UnturnedPlayer?.CurrentVehicle;

        public CSteamID CSteamID => UnturnedPlayer?.CSteamID ?? CSteamID.Nil;

        public IRocketPlayer RocketPlayer { get; }

        /// <summary>
        /// The Player's Steam 64 ID, or 0
        /// </summary>
        public ulong PlayerID => UnturnedPlayer?.CSteamID.m_SteamID ?? 0;

        /// <summary>
        /// Player as UnturnedPlayer, or null
        /// </summary>
        public UnturnedPlayer UnturnedPlayer => RocketPlayer as UnturnedPlayer ?? null;

        /// <summary>
        /// The Player, or null
        /// </summary>
        public Player Player => UnturnedPlayer?.Player ?? null;

        /// <summary>
        /// The SteamPlayer, or null
        /// </summary>
        public SteamPlayer SteamPlayer => Player?.channel?.owner ?? null;

        /// <summary>
        /// Sends a message to the player
        /// </summary>
        public async Task MessageAsync(string message, Color? messageColor = null, bool rich = true)
        {
            if (messageColor == null)
            {
                messageColor = Color.green;
            }
            if (rich)
            {
                message = message.ReformatColor();
            }
            await ThreadTool.RunOnGameThreadAsync(UnturnedChat.Say, RocketPlayer, message, messageColor.Value, rich);
        }

        /// <summary>
        /// Cancels the command cooldown for a command.
        /// </summary>
        /// <returns>True if the cooldown was found and cancled</returns>
        public async Task<bool> CancelCooldownAsync(IRocketCommand command)
        {
            return await CooldownManager.CancelCooldownAsync(RocketPlayer, command);
        }

        /// <summary>
        /// Sets a command cooldown for a command
        /// </summary>
        /// <param name="cooldown">Cooldown time in seconds</param>
        /// <returns>True if a new cooldown was created, or false if an existing one was updated</returns>
        public async Task<bool> SetCooldownAsync(IRocketCommand command, uint cooldown)
        {
            return await CooldownManager.SetCooldownAsync(RocketPlayer, command, cooldown);
        }

        /// <summary>
        /// Gets the player's IP address, or 0.0.0.0
        /// </summary>
        public async Task<string> GetIPAsync()
        {
            if (SteamPlayer == null)
            {
                return "0.0.0.0";
            }
            return await ThreadTool.RunOnGameThreadAsync(() =>
            {
                var addr = SteamPlayer.getAddressString(false);
                if (string.IsNullOrEmpty(addr))
                {
                    return "0.0.0.0";
                }
                return addr;
            });
        }

        public async Task TriggerEffectAsync(ushort effectID)
        {
            if (UnturnedPlayer == null)
                return;
            await ThreadTool.RunOnGameThreadAsync(() =>
            {
                UnturnedPlayer.TriggerEffect(effectID);
            });
        }

        public async Task MaxSkillsAsync()
        {
            if (UnturnedPlayer == null)
                return;
            await ThreadTool.RunOnGameThreadAsync(() =>
            {
                UnturnedPlayer.MaxSkills();
            });
        }

        /// <summary>
        /// Gives a player an item
        /// </summary>
        /// <returns>True if the item was given, otherwise false</returns>
        public async Task<bool> TryGiveItemAsync(ushort itemID, byte amount)
        {
            if (UnturnedPlayer == null)
                return false;
            return await ThreadTool.RunOnGameThreadAsync(() =>
            {
                return UnturnedPlayer.GiveItem(itemID, amount);
            });
        }

        /// <summary>
        /// Gives a player an item
        /// </summary>
        /// <returns>True if the item was given, otherwise false</returns>
        public async Task<bool> TryGiveItemAsync(Item item)
        {
            if (UnturnedPlayer == null)
                return false;
            return await ThreadTool.RunOnGameThreadAsync(() =>
            {
                return UnturnedPlayer.GiveItem(item);
            });
        }

        /// <summary>
        /// Tries to spawn a vehicle in front of the player
        /// </summary>
        /// <returns>True on vehicle spawned</returns>
        public async Task<bool> TryGiveVehicleAsync(ushort vehicleID)
        {
            if (UnturnedPlayer == null)
                return false;
            return await ThreadTool.RunOnGameThreadAsync(() =>
            {
                return UnturnedPlayer.GiveVehicle(vehicleID);
            });
        }

        public async Task KickAsync(string reason)
        {
            await ThreadTool.RunOnGameThreadAsync(Provider.kick, CSteamID, reason);
        }

        public async Task BanAsync(CSteamID instigator, string reason, uint duration)
        {
            if (UnturnedPlayer == null)
                return;
            await ThreadTool.RunOnGameThreadAsync(() =>
            {
                UnturnedPlayer.Ban(instigator, reason, duration);
            });
        }

        public async Task BanAsync(string reason, uint duration) => await BanAsync(CSteamID.Nil, reason, duration);

        public async Task SetAdminAsync(bool admin)
        {
            if (UnturnedPlayer == null)
                return;

            await ThreadTool.RunOnGameThreadAsync(() =>
            {
                UnturnedPlayer.Admin(admin);
            });
        }

        public async Task SetAdminAsync(bool admin, UnturnedPlayer issuer)
        {
            if (UnturnedPlayer == null)
                return;

            await ThreadTool.RunOnGameThreadAsync(() =>
            {
                UnturnedPlayer.Admin(admin, issuer);
            });
        }

        public async Task SetAdminAsync(bool admin, LDMPlayer issuer)
        {
            if (UnturnedPlayer == null || issuer.UnturnedPlayer == null)
                return;

            await ThreadTool.RunOnGameThreadAsync(() =>
            {
                UnturnedPlayer.Admin(admin, issuer.UnturnedPlayer);
            });
        }

        /// <summary>
        /// Teleports a player to another player
        /// </summary>
        /// <param name="safe">Runs position checks when enabled. Disabling this will force the tp, even if it is into an object</param>
        /// <returns>True if the player was teleported (always true in unsafe mode)</returns>
        public async Task<bool> TeleportAsync(UnturnedPlayer target, bool safe = true)
        {
            if (Player == null)
            {
                return false;
            }

            return await ThreadTool.RunOnGameThreadAsync(() =>
            {
                if (safe)
                {
                    return Player.teleportToLocation(target.Position, target.Rotation);
                }
                else
                {
                    Player.teleportToLocationUnsafe(target.Position, target.Rotation);
                    return true;
                }
            });
        }

        /// <summary>
        /// Teleports a player to a position
        /// </summary>
        /// <param name="safe">Runs position checks when enabled. Disabling this will force the tp, even if it is into an object</param>
        /// <returns>True if the player was teleported (always true in unsafe mode)</returns>
        public async Task<bool> TeleportAsync(Vector3 position, float rotation, bool safe = true)
        {
            if (Player == null)
            {
                return false;
            }

            return await ThreadTool.RunOnGameThreadAsync(() =>
            {
                if (safe)
                {
                    return Player.teleportToLocation(position, rotation);
                }
                else
                {
                    Player.teleportToLocationUnsafe(position, rotation);
                    return true;
                }
            });
        }

        public async Task HealAsync(byte amount)
        {
            if (UnturnedPlayer == null)
            {
                return;
            }
            await ThreadTool.RunOnGameThreadAsync(() =>
            {
                UnturnedPlayer.Heal(amount);
            });
        }

        public async Task<EPlayerKill> DamageAsync(byte amount, Vector3 direction, EDeathCause cause, ELimb limb, CSteamID damageDealer)
        {
            if (UnturnedPlayer == null)
            {
                return EPlayerKill.NONE;
            }
            return await ThreadTool.RunOnGameThreadAsync(() =>
            {
                return UnturnedPlayer.Damage(amount, direction, cause, limb, damageDealer);
            });
        }

        public async Task SetSkillAsync(UnturnedSkill skill, byte level)
        {
            if (UnturnedPlayer == null)
            {
                return;
            }
            await ThreadTool.RunOnGameThreadAsync(() =>
            {
                UnturnedPlayer.SetSkillLevel(skill, level);
            });
        }

        public async Task SendBrowserRequestAsync(string url, string message)
        {
            if (Player == null)
            {
                return;
            }
            await ThreadTool.RunOnGameThreadAsync(() => Player.sendBrowserRequest(message, url));
        }

        /// <summary>
        /// Relays a player to another server
        /// </summary>
        public async Task RelayAsync(uint ip, ushort port, string password, bool showMenu = true)
        {
            if (Player == null)
            {
                return;
            }

            await ThreadTool.RunOnGameThreadAsync(() => Player.sendRelayToServer(ip, port, password, showMenu));
        }

        /// <summary>
        /// Relays a player to another server
        /// </summary>
        public async Task RelayAsync(string ip, ushort port, string password, bool showMenu = true)
        {
            if (Player == null)
            {
                return;
            }

            var strs = ip.Split('.').ToArray();
            if (strs.Length != 4)
            {
                throw new InvalidCastException($"Cannot convert ip '{ip}' into a numeric IP address");
            }
            uint ipAddr = 0;
            try
            {
                var b1 = byte.Parse(strs[0]);
                var b2 = byte.Parse(strs[1]);
                var b3 = byte.Parse(strs[2]);
                var b4 = byte.Parse(strs[3]);
                var buffer = new byte[] { b1, b2, b3, b4 };
                ipAddr = BitConverter.ToUInt32(buffer, 0);
            }
            catch (FormatException)
            {
                throw new InvalidCastException($"Cannot convert ip '{ip}' into a numeric IP address");
            }

            await ThreadTool.RunOnGameThreadAsync(() => Player.sendRelayToServer(ipAddr, port, password, showMenu));
        }

        /// <summary>
        /// Toggles a plugin widget flag
        /// </summary>
        public async Task SetPluginWidgetFlagAsync(EPluginWidgetFlags flag, bool active)
        {
            if (Player == null)
            {
                return;
            }
            await ThreadTool.RunOnGameThreadAsync(() => Player.setPluginWidgetFlag(flag, active));
        }
    }
}