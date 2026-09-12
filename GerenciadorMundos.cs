using System;
using System.Collections.Generic;
using System.Linq;

namespace MinecraftMapApp
{
    public class GerenciadorMundos
    {
        public List<MundoMinecraft> Mundos { get; private set; }
            = new List<MundoMinecraft>();

        public MundoMinecraft? MundoAtual { get; private set; }

        private GerenciadorSalvamento salvamento;

        public GerenciadorMundos()
        {
            salvamento = new GerenciadorSalvamento();

            CarregarMundos();
        }

        public MundoMinecraft CriarMundo(
            string nome,
            string seed)
        {
            MundoMinecraft novoMundo =
                new MundoMinecraft
                {
                    Nome = nome,
                    Seed = seed
                };

            Mundos.Add(novoMundo);

            MundoAtual = novoMundo;

            salvamento.SalvarMundo(novoMundo);

            return novoMundo;
        }

        public void SelecionarMundo(MundoMinecraft mundo)
        {
            if (Mundos.Contains(mundo))
            {
                MundoAtual = mundo;
            }
        }

        public void SelecionarMundo(string nome)
        {
            MundoMinecraft? mundo =
                Mundos.FirstOrDefault(
                    m => m.Nome.Equals(
                        nome,
                        StringComparison.OrdinalIgnoreCase
                    )
                );

            if (mundo != null)
            {
                MundoAtual = mundo;
            }
        }

        public void RenomearMundo(string nomeAntigo, MundoMinecraft mundo)
        {
            salvamento.ExcluirMundo(nomeAntigo);
            salvamento.SalvarMundo(mundo);
        }

        public void SalvarMundoAtual()
        {
            if (MundoAtual != null)
            {
                salvamento.SalvarMundo(
                    MundoAtual
                );
            }
        }

        public void SalvarTodos()
        {
            foreach (MundoMinecraft mundo in Mundos)
            {
                salvamento.SalvarMundo(mundo);
            }
        }

        public void ExcluirMundo(string nome)
        {
            MundoMinecraft? mundo =
                Mundos.FirstOrDefault(
                    m => m.Nome.Equals(
                        nome,
                        StringComparison.OrdinalIgnoreCase
                    )
                );

            if (mundo != null)
            {
                Mundos.Remove(mundo);

                salvamento.ExcluirMundo(
                    mundo.Nome
                );

                if (MundoAtual == mundo)
                {
                    MundoAtual =
                        Mundos.FirstOrDefault();
                }
            }
        }

        private void CarregarMundos()
        {
            Mundos =
                salvamento.CarregarMundos();

            if (Mundos.Count > 0)
            {
                MundoAtual = Mundos[0];
            }
        }
    }
}