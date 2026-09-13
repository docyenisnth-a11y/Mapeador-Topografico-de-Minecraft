using System;
using System.Drawing;
using System.Windows.Forms;

namespace MinecraftMapApp
{
    public class NovoPontoForm : Form
    {
        private TextBox txtNome = new TextBox();
        private TextBox txtX = new TextBox();
        private TextBox txtY = new TextBox();
        private TextBox txtZ = new TextBox();
        private TextBox txtNota = new TextBox();

        private Button btnMarcar = new Button();
        private Button btnCancelar = new Button();

        public string NomePonto { get; private set; } = string.Empty;
        public int XPonto { get; private set; }
        public int YPonto { get; private set; }
        public int ZPonto { get; private set; }
        public string NotaPonto { get; private set; } = string.Empty;

        public NovoPontoForm()
        {
            this.Text = "Novo Ponto";
            this.Size = new Size(420, 500);
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
                Text = "MARCAR NOVO PONTO",
                Location = new Point(25, 20),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };

            Label lblNome = new Label
            {
                Text = "Nome do local:",
                Location = new Point(25, 65),
                Size = new Size(200, 20)
            };

            txtNome.Location = new Point(25, 87);
            txtNome.Size = new Size(350, 25);

            Label lblX = new Label
            {
                Text = "Coordenada X (Leste/Oeste):",
                Location = new Point(25, 125),
                Size = new Size(350, 18)
            };

            txtX.Location = new Point(25, 146);
            txtX.Size = new Size(350, 25);
            txtX.Text = "0";

            Label lblY = new Label
            {
                Text = "Coordenada Y (Altitude):",
                Location = new Point(25, 183),
                Size = new Size(350, 18)
            };

            txtY.Location = new Point(25, 204);
            txtY.Size = new Size(350, 25);
            txtY.Text = "64";

            Label lblZ = new Label
            {
                Text = "Coordenada Z (Norte/Sul):",
                Location = new Point(25, 241),
                Size = new Size(350, 18)
            };

            txtZ.Location = new Point(25, 262);
            txtZ.Size = new Size(350, 25);
            txtZ.Text = "0";

            Label lblNota = new Label
            {
                Text = "Nota (opcional):",
                Location = new Point(25, 299),
                Size = new Size(350, 18)
            };

            txtNota.Location = new Point(25, 320);
            txtNota.Size = new Size(350, 25);

            btnMarcar.Text = "MARCAR PONTO";
            btnMarcar.Location = new Point(200, 360);
            btnMarcar.Size = new Size(175, 40);
            btnMarcar.BackColor = Color.FromArgb(46, 139, 87);
            btnMarcar.ForeColor = Color.White;
            btnMarcar.FlatStyle = FlatStyle.Flat;
            btnMarcar.Click += BtnMarcar_Click;

            btnCancelar.Text = "CANCELAR";
            btnCancelar.Location = new Point(25, 360);
            btnCancelar.Size = new Size(160, 40);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Click += BtnCancelar_Click;

            this.Controls.Add(lblTitulo);
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
            this.Controls.Add(btnMarcar);
            this.Controls.Add(btnCancelar);
        }

        private void BtnMarcar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show(
                    "Digite um nome para o ponto.",
                    "Novo Ponto"
                );

                txtNome.Focus();
                return;
            }

            if (!int.TryParse(txtX.Text, out int x) ||
                !int.TryParse(txtY.Text, out int y) ||
                !int.TryParse(txtZ.Text, out int z))
            {
                MessageBox.Show(
                    "Preencha as coordenadas com numeros inteiros!",
                    "Novo Ponto"
                );

                return;
            }

            NomePonto = txtNome.Text.Trim();
            XPonto = x;
            YPonto = y;
            ZPonto = z;
            NotaPonto = txtNota.Text.Trim();

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