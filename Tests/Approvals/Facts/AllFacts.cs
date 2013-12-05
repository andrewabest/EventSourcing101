using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Autofac;
using Core.Domain;
using FluentAssertions;
using FluentAssertions.Types;
using NUnit.Framework;

namespace Tests.Approvals.Facts
{
    [TestFixture]
    public class AllFacts
    {
        private readonly IList<string> _blackListedIrregularVerbs = new List<string> { "Become", "Becomming", "Begin", "Come", "Coming", "Do", "Get", "Go", "Have", "Having", "Hide", "Hiding", "Make", "Making", "Pay", "Run", "Take", "Taking" };

        [Test, TestCaseSource(typeof(GetFacts))]
        public void ShouldBeNamedInPastTense(Type t)
        {
            if (t.Name.EndsWith("edFact"))
                return;

            if (t.Name.EndsWith("MadeFact"))
                return;

            foreach (var irregularVerb in _blackListedIrregularVerbs)
            {
                t.Name.Should().NotContainEquivalentOf(irregularVerb);
            }
        }

        [Test, TestCaseSource(typeof(GetFacts))]
        public void ShouldEndInFact(Type t)
        {
            t.Name.Should().EndWith("Fact");
        }

        [Test, TestCaseSource(typeof(GetFacts))]
        public void MustBeMarkedSerializable(Type entityType)
        {
            entityType.IsSerializable.Should().BeTrue("Facts must be marked serializable.");
        }

        public class GetFacts : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                var viewModelTypes =
                    typeof(AggregateRoot).Assembly.GetTypes()
                        .Where(t => !t.IsInterface && !t.IsAbstract && t.IsAssignableTo<IFact>());

                return viewModelTypes.GetEnumerator();
            }
        }
    }
}