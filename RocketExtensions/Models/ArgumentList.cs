using RocketExtensions.Models.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RocketExtensions.Models
{
    public class ArgumentList : IEnumerable<string>
    {
        private List<string> m_Items = new List<string>();

        public ArgumentList(string[] arguments)
        {
            m_Items = arguments.ToList();
        }

        // Backward Compatability

        /// <summary>
        /// Parses an argument.
        /// Supports primitive types, and <see cref="SDG.Unturned.Player"/>, <see cref="SDG.Unturned.SteamPlayer"/>, and <see cref="Rocket.Unturned.Player.UnturnedPlayer"/>
        /// Will throw and send a user-friendly message on invalid argument, and player not found.
        /// </summary>
        /// <param name="index">Index of the parameter</param>
        /// <param name="defaultValue">Default value to be returned instead of the argument was not supplied.</param>
        /// <returns>Parsed Value</returns>
        public T Get<T>(int index, T defaultValue) => Get<T>(index, defaultValue, paramName: null);

        /// <summary>
        /// Parses an argument.
        /// Supports primitive types, and <see cref="SDG.Unturned.Player"/>, <see cref="SDG.Unturned.SteamPlayer"/>, and <see cref="Rocket.Unturned.Player.UnturnedPlayer"/>
        /// Will throw and send a user-friendly message on invalid argument, and player not found, and argument missing.
        /// </summary>
        /// <param name="index">Index of the parameter</param>
        /// <returns>Parsed Value</returns>
        public T Get<T>(int index) => Get<T>(index, paramName: null);

        /// <summary>
        /// Parses an argument.
        /// Supports primitive types, and <see cref="SDG.Unturned.Player"/>, <see cref="SDG.Unturned.SteamPlayer"/>, and <see cref="Rocket.Unturned.Player.UnturnedPlayer"/>
        /// Will throw and send a user-friendly message on invalid argument, and player not found.
        /// </summary>
        /// <param name="index">Index of the parameter</param>
        /// <param name="defaultValue">Default value to be returned instead of the argument was not supplied.</param>
        /// <param name="paramName">The parameter name to be used in User Friendly error messages</param>
        /// <returns>Parsed Value</returns>
        public T Get<T>(int index, T defaultValue, string paramName = null)
        {
            if (index >= m_Items.Count)
            {
                return defaultValue;
            }

            var value = m_Items[index];

            var result = StringTypeConverter.Parse<T>(value, out var res);

            if (result == EParseResult.InvalidType)
            {
                throw new InvalidCastException($"Type {typeof(T).Name} is not valid for automatic string parsing");
            }
            else if (result == EParseResult.PlayerNotFound)
            {
                return defaultValue;
            }
            else if (result == EParseResult.ParseFailed)
            {
                if (!string.IsNullOrEmpty(paramName))
                {
                    throw new InvalidArgumentException(paramName);
                }
                else
                {
                    throw new InvalidArgumentException(index);
                }
            }
            else
            {
                return res;
            }
        }

        /// <summary>
        /// Parses an argument.
        /// Supports primitive types, and <see cref="SDG.Unturned.Player"/>, <see cref="SDG.Unturned.SteamPlayer"/>, and <see cref="Rocket.Unturned.Player.UnturnedPlayer"/>
        /// Will throw and send a user-friendly message on invalid argument, and player not found, and argument missing.
        /// </summary>
        /// <param name="index">Index of the parameter</param>
        /// <param name="paramName">The parameter name to be used in User Friendly error messages</param>
        /// <returns>Parsed Value</returns>
        public T Get<T>(int index, string paramName = null)
        {
            if (index >= m_Items.Count)
            {
                if (!string.IsNullOrEmpty(paramName))
                {
                    throw new ArgumentMissingException(paramName);
                }
                else
                {
                    throw new ArgumentMissingException(index);
                }
            }

            var value = m_Items[index];

            var result = StringTypeConverter.Parse<T>(value, out var res);

            if (result == EParseResult.InvalidType)
            {
                throw new InvalidCastException($"Type {typeof(T).Name} is not valid for automatic string parsing");
            }
            else if (result == EParseResult.PlayerNotFound)
            {
                throw new PlayerNotFoundException(value);
            }
            else if (result == EParseResult.ParseFailed)
            {
                if (!string.IsNullOrEmpty(paramName))
                {
                    throw new InvalidArgumentException(paramName);
                }
                else
                {
                    throw new InvalidArgumentException(index);
                }
            }
            else
            {
                return res;
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }
    }
}