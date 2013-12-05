using System;

namespace Core.Domain
{
    [Serializable]
    public abstract class Entity : IIdentifiable, IAppendFacts
    {
        private readonly IAppendFacts _parent;

        public IAppendFacts Parent { get { return _parent; } }

        public Guid Id { get; protected set; }

        protected Entity(IAppendFacts parent)
        {
            _parent = parent;
        }

        public void Append(IFact fact)
        {
            _parent.Append(fact);
        }
    }
}