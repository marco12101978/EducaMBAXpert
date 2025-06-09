using EducaMBAXpert.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EducaMBAXpert.Core.Test
{
    public class ValueObjectTests
    {
        private class Money : ValueObject
        {
            public decimal Amount { get; }
            public string Currency { get; }

            public Money(decimal amount, string currency)
            {
                Amount = amount;
                Currency = currency;
            }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return Amount;
                yield return Currency;
            }
        }

        [Fact(DisplayName = "ValueObjects com mesmos valores devem ser iguais")]
        [Trait("ValueObject", "Core")]
        public void ValueObjects_Iguais_DevemSerIguais()
        {
            var a = new Money(100, "BRL");
            var b = new Money(100, "BRL");

            Assert.Equal(a, b);
            Assert.True(a.Equals(b));
        }

        [Fact(DisplayName = "ValueObjects com valores diferentes devem ser diferentes")]
        [Trait("ValueObject", "Core")]
        public void ValueObjects_Diferentes_DevemSerDiferentes()
        {
            var a = new Money(100, "BRL");
            var b = new Money(200, "USD");

            Assert.NotEqual(a, b);
            Assert.False(a.Equals(b));
        }
    }
}
