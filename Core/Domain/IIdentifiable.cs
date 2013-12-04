using System;

namespace Core.Domain
{
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}