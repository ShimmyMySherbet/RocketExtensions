using System;

namespace RocketExtensions.Models.Exceptions
{
    public sealed class InvalidArgumentException : Exception
    {
        private string m_message;
        public override string Message => m_message;
        public InvalidArgumentException(string name)
        {
            m_message = $"Invalid value for field {name}";
        }

        public InvalidArgumentException(int index)
        {
            m_message = $"Invalid value at command position {index + 1}";
        }
    }
}