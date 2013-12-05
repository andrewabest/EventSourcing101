using System;
using Core.Domain.Aggregates.CustomerAggregate;
using Core.Domain.Aggregates.CustomerAggregate.Facts;
using Core.Extensions;

namespace Core.Domain.Handlers
{
    public class NameChangeHandler : IHandleDuringUnitOfWork<CustomerNameChangedFact>
    {
        private readonly IRepository<Customer> _customerRepository;

        public NameChangeHandler(
            Func<IUnitOfWork, IRepository<Customer>> customerRepositoryFactory,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepositoryFactory(unitOfWork);
        }

        public void Handle(CustomerNameChangedFact fact)
        {
            if (fact.Name.IsNotAndrew())
            {
                var customer = _customerRepository.GetById(fact.AggregateRootId);

                customer.ChangeName("Andrew");
            }
        }
    }
}