using System;
using System.Drawing;
using System.Windows.Forms;

namespace MinecraftMapApp
{
    public class GerenciarMundosForm : Form
    {
        private GerenciadorMundos gerenciador;
        private ListBox listaMundos = new ListBox();

        private Button btnAbrir = new Button();
        private Button btnExcluir = new Button();
        private Button btnCancelar = new Button();
        private ContextMenuStrip menuContexto = new ContextMenuStrip();
        private ToolStripMenuItem itemEditar = new ToolStripMenuItem("Editar informações");
        private ToolStripMenuItem itemExcluir = new ToolStripMenuItem("Excluir mundo");

        public MundoMinecraft? MundoSelecionado { get; private set; }

        public GerenciarMundosForm(
            GerenciadorMundos gerenciador)
        {
            this.gerenciador = gerenciador;

            this.Text = "Meus Mundos";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            CriarInterface();
            AtualizarLista();
        }

        private void CriarInterface()
        {
            Label titulo = new Label
            {
                Text = "MEUS MUNDOS",
                Location = new Point(25, 20),
                Size = new Size(400, 30),
                Font = new Font(
                    "Segoe UI",
                    14,
                    FontStyle.Bold)
            };

            listaMundos.Location =
                new Point(25, 65);

            listaMundos.Size =
                new Size(430, 230);

            listaMundos.Font =
                new Font(
                    "Segoe UI",
                    11);

            listaMundos.DoubleClick +=
                BtnAbrir_Click;
            listaMundos.MouseUp += ListaMundos_MouseUp;

            itemEditar.Click += ItemEditar_Click;
            itemExcluir.Click += ItemExcluir_Click;
            menuContexto.Items.Add(itemEditar);
            menuContexto.Items.Add(new ToolStripSeparator());
            menuContexto.Items.Add(itemExcluir);
            listaMundos.ContextMenuStrip = menuContexto;

            btnAbrir.Text = "ABRIR";
            btnAbrir.Location =
                new Point(25, 315);
            btnAbrir.Size =
                new Size(135, 40);

            btnAbrir.BackColor =
                Color.FromArgb(46, 139, 87);

            btnAbrir.ForeColor =
                Color.White;

            btnAbrir.FlatStyle =
                FlatStyle.Flat;

            btnAbrir.Click +=
                BtnAbrir_Click;

            btnExcluir.Text = "EXCLUIR";
            btnExcluir.Location =
                new Point(180, 315);
            btnExcluir.Size =
                new Size(135, 40);

            btnExcluir.BackColor =
                Color.FromArgb(150, 50, 50);

            btnExcluir.ForeColor =
                Color.White;

            btnExcluir.FlatStyle =
                FlatStyle.Flat;

            btnExcluir.Click +=
                BtnExcluir_Click;

            btnCancelar.Text = "FECHAR";
            btnCancelar.Location =
                new Point(335, 315);
            btnCancelar.Size =
                new Size(120, 40);

            btnCancelar.Click +=
                BtnCancelar_Click;

            this.Controls.Add(titulo);
            this.Controls.Add(listaMundos);
            this.Controls.Add(btnAbrir);
            this.Controls.Add(btnExcluir);
            this.Controls.Add(btnCancelar);
        }

        private void AtualizarLista()
        {
            listaMundos.Items.Clear();

            foreach (
                MundoMinecraft mundo
                in gerenciador.Mundos)
            {
                listaMundos.Items.Add(
                    $"🌎 {mundo.Nome}    |    Criado em {mundo.DataCriacao:dd/MM/yyyy}"
                );
            }

            if (listaMundos.Items.Count > 0)
            {
                listaMundos.SelectedIndex = 0;
            }
        }

        private void ListaMundos_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int indice = listaMundos.IndexFromPoint(e.Location);
                if (indice >= 0)
                {
                    listaMundos.SelectedIndex = indice;
                    menuContexto.Show(listaMundos, e.Location);
                }
            }
        }

        private MundoMinecraft? ObterMundoSelecionado()
        {
            if (listaMundos.SelectedIndex < 0 || listaMundos.SelectedIndex >= gerenciador.Mundos.Count)
                return null;

            return gerenciador.Mundos[listaMundos.SelectedIndex];
        }

        private void ItemEditar_Click(object? sender, EventArgs e)
        {
            MundoMinecraft? mundo = ObterMundoSelecionado();
            if (mundo == null) return;

            using NovoMundoForm janela = new NovoMundoForm();
            janela.Text = "Editar Mundo";
            janela.Preencher(mundo.Nome, mundo.Seed);

            if (janela.ShowDialog(this) == DialogResult.OK)
            {
                string nomeAntigo = mundo.Nome;
                mundo.Nome = janela.NomeMundo;
                mundo.Seed = janela.SeedMundo;
                gerenciador.RenomearMundo(nomeAntigo, mundo);
                AtualizarLista();
            }
        }

        private void ItemExcluir_Click(object? sender, EventArgs e)
        {
            BtnExcluir_Click(sender, e);
        }

        private void BtnAbrir_Click(
            object? sender,
            EventArgs e)
        {
            if (listaMundos.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Selecione um mundo primeiro."
                );

                return;
            }

            MundoSelecionado =
                gerenciador.Mundos[
                    listaMundos.SelectedIndex
                ];

            this.DialogResult =
                DialogResult.OK;

            this.Close();
        }

        private void BtnExcluir_Click(
            object? sender,
            EventArgs e)
        {
            if (listaMundos.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Selecione um mundo primeiro."
                );

                return;
            }

            MundoMinecraft mundo =
                gerenciador.Mundos[
                    listaMundos.SelectedIndex
                ];

            DialogResult resposta =
                MessageBox.Show(
                    $"Tem certeza que deseja excluir o mundo \"{mundo.Nome}\"?\n\n" +
                    "Todos os pontos desse mundo serão apagados.",
                    "Excluir Mundo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

            if (resposta ==
                DialogResult.Yes)
            {
                gerenciador.ExcluirMundo(
                    mundo.Nome
                );

                AtualizarLista();
            }
        }

        private void BtnCancelar_Click(
            object? sender,
            EventArgs e)
        {
            this.DialogResult =
                DialogResult.Cancel;

            this.Close();
        }
    }
}