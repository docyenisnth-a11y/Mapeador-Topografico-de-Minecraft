using System;
using System.Drawing;
using System.Windows.Forms;

namespace MinecraftMapApp
{
    public class NovoMundoForm : Form
    {
        private TextBox txtNome = new TextBox();
        private TextBox txtSeed = new TextBox();

        private Button btnCriar = new Button();
        private Button btnCancelar = new Button();

        public string NomeMundo { get; private set; } = string.Empty;
        public string SeedMundo { get; private set; } = string.Empty;

        public NovoMundoForm()
        {
            this.Text = "Novo Mundo";
            this.Size = new Size(420, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            CriarInterface();
        }

        private void CriarInterface()
        {
            Label lblTitulo = new Label
            {
                Text = "CRIAR NOVO MUNDO",
                Location = new Point(25, 20),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };

            Label lblNome = new Label
            {
                Text = "Nome do mundo:",
                Location = new Point(25, 70),
                Size = new Size(150, 20)
            };

            txtNome.Location = new Point(25, 92);
            txtNome.Size = new Size(350, 25);

            Label lblSeed = new Label
            {
                Text = "Seed:",
                Location = new Point(25, 130),
                Size = new Size(150, 20)
            };

            txtSeed.Location = new Point(25, 152);
            txtSeed.Size = new Size(350, 25);

            btnCriar.Text = "CRIAR";
            btnCriar.Location = new Point(200, 205);
            btnCriar.Size = new Size(175, 35);
            btnCriar.BackColor = Color.FromArgb(46, 139, 87);
            btnCriar.ForeColor = Color.White;
            btnCriar.FlatStyle = FlatStyle.Flat;
            btnCriar.Click += BtnCriar_Click;

            btnCancelar.Text = "CANCELAR";
            btnCancelar.Location = new Point(25, 205);
            btnCancelar.Size = new Size(160, 35);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Click += BtnCancelar_Click;

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblNome);
            this.Controls.Add(txtNome);
            this.Controls.Add(lblSeed);
            this.Controls.Add(txtSeed);
            this.Controls.Add(btnCriar);
            this.Controls.Add(btnCancelar);
        }

        public void Preencher(string nome, string seed)
        {
            txtNome.Text = nome;
            txtSeed.Text = seed;
        }

        private void BtnCriar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show(
                    "Digite um nome para o mundo.",
                    "Novo Mundo"
                );

                txtNome.Focus();
                return;
            }

            NomeMundo = txtNome.Text.Trim();
            SeedMundo = txtSeed.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancelar_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}