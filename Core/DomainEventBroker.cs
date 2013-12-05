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

        public void Raise(IFact fact)
        {
            RaiseForBaseHandlers(fact);

            var handlerType = typeof(IHandle<>).MakeGenericType(fact.GetType());
            var handlersType = typeof(IEnumerable<>).MakeGenericType(handlerType);
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var handlers = (IEnumerable)scope.Resolve(handlersType);

                foreach (var handler in handlers)
                    ((dynamic)handler).Handle((dynamic)fact);
            }
        }

        // TODO: If you want better polymorphic fact handling adapt the Caliburn.Micro EventBroker
        private void RaiseForBaseHandlers(IFact fact)
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var handlers = (IEnumerable)scope.Resolve(typeof(IEnumerable<IHandle<IFact>>));

                foreach (var handler in handlers)
                    ((dynamic)handler).Handle((dynamic)fact);
            }
        }

        public void RaiseWithinUnitOfWork(IFact fact, IUnitOfWork unitOfWork)
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