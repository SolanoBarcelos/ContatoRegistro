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
    public sealed class NomeObjetoValorTests
    {
        [Theory]
        [InlineData("Maria Silva")]
        [InlineData("João")]
        [InlineData("A B C")]
        public void Ctor_ComNomeValido_NaoDeveLancar(string nome)
        {
            Action act = () => _ = new NomeObjetoValor(nome);
            act.Should().NotThrow();
        }


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Ctor_ComNomeVazio_DeveLancarExcessaoDominio(string nome)
        {
            Action act = () => _ = new NomeObjetoValor(nome);
            act.Should().Throw<ExcessaoDominio>();
        }


        [Fact]
        public void Equals_MesmoNome_DeveSerIgual()
        {
            var a = new NomeObjetoValor("Maria Silva");
            var b = new NomeObjetoValor("Maria Silva");
            (a == b).Should().BeTrue();
        }
    }
}

