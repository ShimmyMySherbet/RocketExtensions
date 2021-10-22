using Rocket.API;
using Rocket.Core;

namespace RocketExtensions.Models
{
    public static class CooldownManager
    {
        private static object GetCooldownItem(IRocketPlayer player, IRocketCommand command)
        {
            dynamic r = R.Commands;
            dynamic cooldowns = r.cooldown;
            foreach (dynamic cooldown in cooldowns)
            {
                if (cooldown.Player == player && cooldown.Command == command)
                {
                    return cooldown;
                }
            }

            return null;
        }

        public static bool CancelCooldown(IRocketPlayer player, IRocketCommand command)
        {
            var cooldown = GetCooldownItem(player, command);
            if (cooldown == null)
            {
                return false;
            }
            RemoveCooldown(cooldown);
            return true;
        }

        private static void RemoveCooldown(object cooldown)
        {
            if (cooldown != null)
            {
                dynamic r = R.Commands;

                if (r.cooldown.Contains(cooldown))
                {
                    r.cooldown.Remove(cooldown);
                }
            }
        }
    }
}