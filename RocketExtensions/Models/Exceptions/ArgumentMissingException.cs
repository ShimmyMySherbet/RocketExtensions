using System;

namespace RocketExtensions.Models.Exceptions
{
    public sealed class ArgumentMissingException : Exception
    {
        private string m_message;
        public override string Message => m_message;

        public ArgumentMissingException(string name)
        {
            m_message = $"Missing Argument {name}";
        }

        public ArgumentMissingException(int index)
        {
            m_message = $"Missing Argument at position {index + 1}";
        }
    }
}