using System;

namespace RocketExtensions.Models.Exceptions
{
    public class PlayerNotFoundException : Exception
    {
        private string m_message;
        public override string Message => m_message;

        public PlayerNotFoundException(string handle)
        {
            m_message = $"Failed to find player by Name/ID '{handle}'";
        }
    }
}