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
        public T Get<T>(int index, T defaultValue) => Get<T>(index, defaultValue, paramName: null);

        public T Get<T>(int index) => Get<T>(index, paramName: null);

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