using System;
using RocketExtensions.Models;

namespace RocketExtensions.Interfaces
{
    public interface IStringParser
    {
        Type Type { get; }

        T Parse<T>(string input, out EParseResult parseResult);
    }
}