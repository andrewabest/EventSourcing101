using System;

namespace Core
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }
}