using System;

namespace Core.Domain
{
    [Serializable]
    public abstract class Fact<T> : IFact
        where T : IAggregateRoot
    {
        public Guid AggregateRootId { get; set; }
        public Guid UnitOfWorkId { get; set; }
        public int UnitOfWorkSequenceNumber { get; set; }

        public Guid Id { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid FactId { get; set; }


        protected Fact()
        {
            FactId = Guid.NewGuid();
        }

        public void SetUnitOfWorkDetails(Guid unitOfWorkId, int unitOfWorkSequenceNumber, DateTimeOffset timestamp, Guid? userId)
        {
            if (unitOfWorkId == Guid.Empty) throw new ArgumentException("unitOfWorkId", "An ID for the current unit of work must be provided!");

            UnitOfWorkId = unitOfWorkId;
            UnitOfWorkSequenceNumber = unitOfWorkSequenceNumber;
            Timestamp = timestamp;
            CreatedBy = userId;
        }

        public string AggregateRootTypeName
        {
            get { return typeof(T).FullName; }
        }
    }
}