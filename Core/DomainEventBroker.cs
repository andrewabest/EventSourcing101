using System.Collections;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using Core.Domain;

namespace Core
{
    internal class DomainEventBroker : IDomainEventBroker
    {
        private readonly ILifetimeScope _lifetimeScope;

        public DomainEventBroker(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        // TODO: Implement IHandle in here too.
        public void Raise(IFact fact, IUnitOfWork unitOfWork)
        {
            var handlerType = typeof(IHandleDuringUnitOfWork<>).MakeGenericType(fact.GetType());
            var handlersType = typeof(IEnumerable<>).MakeGenericType(handlerType);
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var handlers = (IEnumerable)scope.Resolve(handlersType, new Parameter[] { new TypedParameter(typeof(IUnitOfWork), unitOfWork) });
                foreach (var handler in handlers)
                    ((dynamic)handler).Handle((dynamic)fact);
            }
        }
    }
}