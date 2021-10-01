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

        public T Get<T>(int index, T defaultValue)
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
            else if (result == EParseResult.ParseFailed)
            {
                return defaultValue;
            }
            else
            {
                return res;
            }
        }


        public T Get<T>(int index) => Get<T>(index); // Backward Compatability

        public T Get<T>(int index, string paramName = null)
        {
            if (index >= m_Items.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            var value = m_Items[index];

            var result = StringTypeConverter.Parse<T>(value, out var res);

            if (result == EParseResult.InvalidType)
            {
                throw new InvalidCastException($"Type {typeof(T).Name} is not valid for automatic string parsing");
            }
            else if (result == EParseResult.ParseFailed)
            {
                if (!string.IsNullOrEmpty(paramName))
                {
                    throw new InvalidArgumentException(paramName);

                } else
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