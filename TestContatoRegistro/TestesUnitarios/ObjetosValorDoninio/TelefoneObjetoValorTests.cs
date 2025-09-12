using ContatoRegistro.Doninio.Excessoes;
using ContatoRegistro.Doninio.ObjetosValor;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestContatoRegistro.TestesUnitarios.ObjetosValorDoninio
{
    public sealed class TelefoneObjetoValorTests
    {
        [Theory]
        [InlineData("11987654321")] 
        [InlineData("21987654321")] 
        [InlineData("(11) 98765-4321")] 
        public void Ctor_ComTelefoneValido_NaoDeveLancar(string numero)
        {
            Action act = () => _ = new TelefoneObjetoValor(numero);
            act.Should().NotThrow();
        }


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("123")]
        [InlineData("abcdefghijk")]
        public void Ctor_ComTelefoneInvalido_DeveLancarExcessaoDominio(string numero)
        {
            Action act = () => _ = new TelefoneObjetoValor(numero);
            act.Should().Throw<ExcessaoDominio>();
        }


        [Fact]
        public void Equals_MesmoTelefone_DeveSerIgual()
        {
            var a = new TelefoneObjetoValor("11987654321");
            var b = new TelefoneObjetoValor("11987654321");
            (a == b).Should().BeTrue();
        }
    }
}
