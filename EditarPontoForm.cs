using System;
using System.Drawing;
using System.Windows.Forms;

namespace MinecraftMapApp
{
    public class EditarPontoForm : Form
    {
        private TextBox txtNome = new TextBox();
        private TextBox txtX = new TextBox();
        private TextBox txtY = new TextBox();
        private TextBox txtZ = new TextBox();
        private TextBox txtNota = new TextBox();

        private Button btnSalvar = new Button();
        private Button btnExcluir = new Button();
        private Button btnCancelar = new Button();

        private PontoMinecraft ponto;

        public bool PontoExcluido { get; private set; }

        public EditarPontoForm(PontoMinecraft ponto)
        {
            this.ponto = ponto;

            this.Text = "Editar Ponto";
            this.Size = new Size(450, 480);
            this.StartPosition =
                FormStartPosition.CenterParent;

            this.FormBorderStyle =
                FormBorderStyle.FixedDialog;

            this.MaximizeBox = false;
            this.MinimizeBox = false;

            CriarInterface();
            CarregarDados();
        }

        private void CriarInterface()
        {
            Label titulo = new Label
            {
                Text = "EDITAR PONTO",
                Location = new Point(25, 20),
                Size = new Size(380, 30),
                Font = new Font(
                    "Segoe UI",
                    14,
                    FontStyle.Bold)
            };

            Label lblNome = new Label
            {
                Text = "Nome:",
                Location = new Point(25, 65),
                Size = new Size(100, 20)
            };

            txtNome.Location =
                new Point(25, 88);

            txtNome.Size =
                new Size(380, 25);

            Label lblX = new Label
            {
                Text = "X:",
                Location = new Point(25, 125),
                Size = new Size(100, 20)
            };

            txtX.Location =
                new Point(25, 148);

            txtX.Size =
                new Size(115, 25);

            Label lblY = new Label
            {
                Text = "Y:",
                Location = new Point(155, 125),
                Size = new Size(100, 20)
            };

            txtY.Location =
                new Point(155, 148);

            txtY.Size =
                new Size(115, 25);

            Label lblZ = new Label
            {
                Text = "Z:",
                Location = new Point(285, 125),
                Size = new Size(120, 20)
            };

            txtZ.Location =
                new Point(285, 148);

            txtZ.Size =
                new Size(120, 25);

            Label lblNota = new Label
            {
                Text = "Nota:",
                Location = new Point(25, 190),
                Size = new Size(100, 20)
            };

            txtNota.Location =
                new Point(25, 213);

            txtNota.Size =
                new Size(380, 100);

            txtNota.Multiline = true;
            txtNota.ScrollBars =
                ScrollBars.Vertical;

            btnSalvar.Text =
                "SALVAR ALTERAÇÕES";

            btnSalvar.Location =
                new Point(25, 340);

            btnSalvar.Size =
                new Size(180, 40);

            btnSalvar.BackColor =
                Color.FromArgb(46, 139, 87);

            btnSalvar.ForeColor =
                Color.White;

            btnSalvar.FlatStyle =
                FlatStyle.Flat;

            btnSalvar.Click +=
                BtnSalvar_Click;

            btnExcluir.Text =
                "EXCLUIR PONTO";

            btnExcluir.Location =
                new Point(215, 340);

            btnExcluir.Size =
                new Size(190, 40);

            btnExcluir.BackColor =
                Color.FromArgb(150, 50, 50);

            btnExcluir.ForeColor =
                Color.White;

            btnExcluir.FlatStyle =
                FlatStyle.Flat;

            btnExcluir.Click +=
                BtnExcluir_Click;

            btnCancelar.Text =
                "CANCELAR";

            btnCancelar.Location =
                new Point(25, 395);

            btnCancelar.Size =
                new Size(380, 35);

            btnCancelar.Click +=
                BtnCancelar_Click;

            this.Controls.Add(titulo);

            this.Controls.Add(lblNome);
            this.Controls.Add(txtNome);

            this.Controls.Add(lblX);
            this.Controls.Add(txtX);

            this.Controls.Add(lblY);
            this.Controls.Add(txtY);

            this.Controls.Add(lblZ);
            this.Controls.Add(txtZ);

            this.Controls.Add(lblNota);
            this.Controls.Add(txtNota);

            this.Controls.Add(btnSalvar);
            this.Controls.Add(btnExcluir);
            this.Controls.Add(btnCancelar);
        }

        private void CarregarDados()
        {
            txtNome.Text = ponto.Nome;
            txtX.Text = ponto.X.ToString();
            txtY.Text = ponto.Y.ToString();
            txtZ.Text = ponto.Z.ToString();
            txtNota.Text = ponto.Nota;
        }

        private void BtnSalvar_Click(
            object? sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                txtNome.Text))
            {
                MessageBox.Show(
                    "O ponto precisa ter um nome."
                );

                return;
            }

            if (!int.TryParse(
                txtX.Text,
                out int x) ||

                !int.TryParse(
                    txtY.Text,
                    out int y) ||

                !int.TryParse(
                    txtZ.Text,
                    out int z))
            {
                MessageBox.Show(
                    "As coordenadas precisam ser números inteiros."
                );

                return;
            }

            ponto.Nome =
                txtNome.Text.Trim();

            ponto.X = x;
            ponto.Y = y;
            ponto.Z = z;

            ponto.Nota =
                txtNota.Text;

            PontoExcluido = false;

            this.DialogResult =
                DialogResult.OK;

            this.Close();
        }

        private void BtnExcluir_Click(
            object? sender,
            EventArgs e)
        {
            DialogResult resposta =
                MessageBox.Show(
                    $"Deseja realmente excluir o ponto \"{ponto.Nome}\"?",
                    "Excluir Ponto",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

            if (resposta ==
                DialogResult.Yes)
            {
                PontoExcluido = true;

                this.DialogResult =
                    DialogResult.OK;

                this.Close();
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