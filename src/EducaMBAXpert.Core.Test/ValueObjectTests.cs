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
            // Arrange
            var amount = 100m;
            var currency = "BRL";

            // Act
            var a = new Money(amount, currency);
            var b = new Money(amount, currency);
            var saoIguais = a.Equals(b);

            // Assert
            Assert.Equal(a, b);
            Assert.True(saoIguais);
        }

        [Fact(DisplayName = "ValueObjects com valores diferentes devem ser diferentes")]
        [Trait("ValueObject", "Core")]
        public void ValueObjects_Diferentes_DevemSerDiferentes()
        {
            // Arrange
            var a = new Money(100, "BRL");
            var b = new Money(200, "USD");

            // Act
            var saoIguais = a.Equals(b);

            // Assert
            Assert.NotEqual(a, b);
            Assert.False(saoIguais);
        }
    }
}
