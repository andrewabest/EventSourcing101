using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Core.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Approvals.Entities
{
    public class AllEntities
    {
        [Test, TestCaseSource("GetEntities")]
        public void MustBeMarkedSerializable(Type entityType)
        {
            entityType.IsSerializable.Should().BeTrue("Entities must be marked serializable.");
        }

        [Test, TestCaseSource("GetEntities")]
        public void ApplyMethods_MustBePublic(Type entityType)
        {
            //this just checks it has the attribute, at some point we might want to check if they actually are.
            entityType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance)
                .Count(m => m.Name == "Apply")
                .Should().Be(0, "Apply methods on entities should be public");
        }

        [Test, TestCaseSource("GetEntities")]
        public void Properties_MustHavePrivateSetters(Type entityType)
        {
            foreach (var setter in entityType.GetProperties().Select(p => p.GetSetMethod()))
            {
                (setter == null).Should().BeTrue("Entity properties must not have public setters");
            }
        }

        private static IEnumerable<Type> GetEntities()
        {
            var viewModelTypes =
                typeof(Entity).Assembly.GetTypes()
                    .Where(t => !t.IsInterface && t.IsAssignableTo<IIdentifiable>());

            return viewModelTypes.ToArray();
        }
        
    }
}