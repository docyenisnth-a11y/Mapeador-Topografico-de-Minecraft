using System;
using System.Drawing;
using System.Windows.Forms;

namespace MinecraftMapApp
{
    public partial class FormMapa : Form
    {
        private Button btnAdicionar = new Button();

        private Label lblInfoDimensao = new Label();
        private Label lblMundoAtual = new Label();

        // Pesquisa
        private TextBox txtPesquisa = new TextBox();
        private Button btnPesquisaAnterior = new Button();
        private Button btnPesquisaProxima = new Button();
        private Label lblResultadoPesquisa = new Label();

        // João e Maria
        private Button btnJoaoMaria = new Button();
        private Label lblRota = new Label();

        private void CriarMenuLateral()
        {
            Panel panelMenu = new Panel
            {
                Size = new Size(250, this.Height),
                Dock = DockStyle.Left,
                BackColor = Color.FromArgb(35, 35, 38)
            };
            this.Controls.Add(panelMenu);

            lblInfoDimensao.Text = "🌍 DIMENSAO: SUPERFICIE";
            lblInfoDimensao.ForeColor = Color.YellowGreen;
            lblInfoDimensao.Location = new Point(20, 15);
            lblInfoDimensao.Size = new Size(210, 20);
            lblInfoDimensao.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            panelMenu.Controls.Add(lblInfoDimensao);

            lblMundoAtual.Text = $"🌎 MUNDO: {mundoAtual.Nome}";
            lblMundoAtual.ForeColor = Color.White;
            lblMundoAtual.Location = new Point(20, 38);
            lblMundoAtual.Size = new Size(210, 20);
            lblMundoAtual.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblMundoAtual.Cursor = Cursors.Hand;
            lblMundoAtual.DoubleClick += LblMundoAtual_DoubleClick;
            panelMenu.Controls.Add(lblMundoAtual);

            Label lblPesquisa = new Label
            {
                Text = "Pesquisar:",
                ForeColor = Color.LightGray,
                Location = new Point(20, 68),
                Size = new Size(210, 18)
            };
            panelMenu.Controls.Add(lblPesquisa);

            txtPesquisa.Location = new Point(20, 88);
            txtPesquisa.Size = new Size(150, 23);
            txtPesquisa.TextChanged += TxtPesquisa_TextChanged;
            txtPesquisa.KeyDown += TxtPesquisa_KeyDown;
            panelMenu.Controls.Add(txtPesquisa);

            btnPesquisaAnterior.Text = "◀";
            btnPesquisaAnterior.Location = new Point(175, 87);
            btnPesquisaAnterior.Size = new Size(25, 25);
            btnPesquisaAnterior.Click += BtnPesquisaAnterior_Click;
            panelMenu.Controls.Add(btnPesquisaAnterior);

            btnPesquisaProxima.Text = "▶";
            btnPesquisaProxima.Location = new Point(205, 87);
            btnPesquisaProxima.Size = new Size(25, 25);
            btnPesquisaProxima.Click += BtnPesquisaProxima_Click;
            panelMenu.Controls.Add(btnPesquisaProxima);

            lblResultadoPesquisa.Text = "Nenhuma pesquisa.";
            lblResultadoPesquisa.ForeColor = Color.DarkGray;
            lblResultadoPesquisa.Location = new Point(20, 116);
            lblResultadoPesquisa.Size = new Size(210, 18);
            lblResultadoPesquisa.Font = new Font("Segoe UI", 8);
            panelMenu.Controls.Add(lblResultadoPesquisa);

            btnAdicionar.Text = "+ Adicionar Ponto";
            btnAdicionar.Location = new Point(20, 145);
            btnAdicionar.Size = new Size(210, 40);
            btnAdicionar.BackColor = Color.FromArgb(46, 139, 87);
            btnAdicionar.ForeColor = Color.White;
            btnAdicionar.FlatStyle = FlatStyle.Flat;
            btnAdicionar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAdicionar.Click += BtnAdicionar_Click;
            panelMenu.Controls.Add(btnAdicionar);

            // Ferramenta de rota fica logo abaixo de Adicionar Ponto.
            btnJoaoMaria.Text = "JOÃO E MARIA";
            btnJoaoMaria.Location = new Point(20, 195);
            btnJoaoMaria.Size = new Size(210, 40);
            btnJoaoMaria.BackColor = Color.FromArgb(95, 70, 150);
            btnJoaoMaria.ForeColor = Color.White;
            btnJoaoMaria.FlatStyle = FlatStyle.Flat;
            btnJoaoMaria.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            btnJoaoMaria.Click += BtnJoaoMaria_Click;
            btnJoaoMaria.MouseUp += BtnJoaoMaria_MouseUp;
            panelMenu.Controls.Add(btnJoaoMaria);


            lblRota.Text = "João e Maria: desligado";
            lblRota.ForeColor = Color.DarkGray;
            lblRota.Location = new Point(20, 245);
            lblRota.Size = new Size(210, 38);
            lblRota.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            panelMenu.Controls.Add(lblRota);
        }
    }
}