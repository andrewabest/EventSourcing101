using System;

namespace Core.Domain
{
    public interface IFact
    {
        Guid AggregateRootId { get; set; }
        Guid UnitOfWorkId { get; }
        int UnitOfWorkSequenceNumber { get; }
        DateTimeOffset Timestamp { get; }
        Guid? CreatedBy { get; }

        string AggregateRootTypeName { get; }
        Guid Id { get; set; }
        Guid FactId { get; }

        void SetUnitOfWorkDetails(Guid unitOfWorkId, int unitOfWorkSequenceNumber, DateTimeOffset timestamp,
            Guid? userId);
    }
}