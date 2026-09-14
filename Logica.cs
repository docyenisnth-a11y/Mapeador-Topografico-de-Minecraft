using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MinecraftMapApp
{
    public class PontoMinecraft
    {
        public string Nome { get; set; } = string.Empty;

        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public string Nota { get; set; } = string.Empty;

        public Rectangle AreaNaTela { get; set; }
    }


    public partial class FormMapa : Form
    {
        // ==========================================
        // ENTER - EDITAR PONTO
        // ==========================================

        protected override bool ProcessCmdKey(
            ref Message msg,
            Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (pontoSelecionadoParaDistancia != null)
                {
                    AbrirEdicaoPonto(
                        pontoSelecionadoParaDistancia
                    );
                }

                // Impede o Enter de chegar
                // aos botões do menu.
                return true;
            }

            return base.ProcessCmdKey(
                ref msg,
                keyData
            );
        }


        // ==========================================
        // SISTEMA DE MUNDOS
        // ==========================================

        private GerenciadorMundos gerenciadorMundos =
            new GerenciadorMundos();

        private MundoMinecraft mundoAtual;


        private List<PontoMinecraft> pontosSuperficie =>
            mundoAtual.PontosSuperficie;


        private List<PontoMinecraft> pontosNether =>
            mundoAtual.PontosNether;


        // ==========================================
        // ESTADO DO MAPA
        // ==========================================

        private bool eSuperficie = true;


        private PontoMinecraft?
            pontoSelecionadoParaDistancia = null;

        private bool joaoMariaAtivo = false;
        private readonly List<PontoRota> pontosRotaAtual =
            new List<PontoRota>();

        private readonly List<PontoMinecraft> resultadosPesquisa =
            new List<PontoMinecraft>();

        private int indicePesquisa = -1;


        private float deslocamentoX = 650f;

        private float deslocamentoY = 350f;

        private float escala = 0.8f;


        private bool arrastando = false;


        private Point posicaoCliqueOriginal;


        private float deslocamentoOriginalX;

        private float deslocamentoOriginalY;


        // ==========================================
        // CONSTRUTOR
        // ==========================================

        public FormMapa()
        {
            // Se não houver mundos,
            // cria o mundo inicial.

            if (gerenciadorMundos.Mundos.Count == 0)
            {
                mundoAtual =
                    gerenciadorMundos.CriarMundo(
                        "Kyoko",
                        string.Empty
                    );
            }
            else
            {
                mundoAtual =
                    gerenciadorMundos.Mundos[0];
            }


            this.Size =
                new Size(1200, 800);


            this.Text =
                "Mapeamento Topografico de Minecraft Bedrock";


            this.DoubleBuffered = true;


            this.StartPosition =
                FormStartPosition.CenterScreen;


            this.KeyPreview = true;


            // ==========================================
            // MENU
            // ==========================================

            CriarMenuLateral();


            // ==========================================
            // EVENTOS DO MAPA
            // ==========================================

            this.Paint +=
                FormMapa_Paint;


            this.MouseDown +=
                FormMapa_MouseDown;


            this.MouseMove +=
                FormMapa_MouseMove;


            this.MouseUp +=
                FormMapa_MouseUp;


            this.MouseClick +=
                FormMapa_MouseClick;


            this.MouseWheel +=
                FormMapa_MouseWheel;


            this.KeyDown +=
                FormMapa_KeyDown;


            AtualizarNomeMundoNaInterface();
            AtualizarPesquisa();
        }


        private void LblMundoAtual_DoubleClick(object? sender, EventArgs e)
        {
            using GerenciarMundosForm janela =
                new GerenciarMundosForm(gerenciadorMundos);

            if (janela.ShowDialog(this) == DialogResult.OK &&
                janela.MundoSelecionado != null)
            {
                mundoAtual = janela.MundoSelecionado;
                gerenciadorMundos.SelecionarMundo(mundoAtual);
                eSuperficie = true;
                pontoSelecionadoParaDistancia = null;
                joaoMariaAtivo = false;
                pontosRotaAtual.Clear();
                AtualizarNomeMundoNaInterface();
                AtualizarPesquisa();
                Invalidate();
            }
        }

        // ==========================================
        // ATUALIZAR NOME DO MUNDO
        // ==========================================

        private void AtualizarNomeMundoNaInterface()
        {
            if (lblMundoAtual != null)
            {
                lblMundoAtual.Text =
                    $"🌎 MUNDO: {mundoAtual.Nome}";
            }
        }


        // ==========================================
        // ADICIONAR PONTO
        // ==========================================

        private void BtnAdicionar_Click(
            object? sender,
            EventArgs e)
        {
            using (
                NovoPontoForm janela =
                new NovoPontoForm())
            {
                if (
                    janela.ShowDialog(this)
                    ==
                    DialogResult.OK
                )
                {
                    PontoMinecraft novoPonto =
                        new PontoMinecraft
                        {
                            Nome = janela.NomePonto,
                            X = janela.XPonto,
                            Y = janela.YPonto,
                            Z = janela.ZPonto,
                            Nota = janela.NotaPonto
                        };

                    if (eSuperficie)
                    {
                        pontosSuperficie.Add(
                            novoPonto
                        );
                    }
                    else
                    {
                        pontosNether.Add(
                            novoPonto
                        );
                    }

                    // Salvar automaticamente

                    gerenciadorMundos
                        .SalvarMundoAtual();

                    pontoSelecionadoParaDistancia =
                        null;

                    this.Invalidate();
                }
            }
        }


        // ==========================================
        // DESENHAR MAPA
        // ==========================================

        private void FormMapa_Paint(
            object? sender,
            PaintEventArgs e)
        {
            Graphics g = e.Graphics;


            // ==========================================
            // FUNDO
            // ==========================================

            if (eSuperficie)
            {
                g.Clear(
                    Color.FromArgb(
                        34,
                        139,
                        34
                    )
                );
            }
            else
            {
                g.Clear(
                    Color.FromArgb(
                        74,
                        24,
                        24
                    )
                );
            }


            float cos30 =
                (float)Math.Cos(
                    Math.PI / 6
                );


            float sin30 =
                (float)Math.Sin(
                    Math.PI / 6
                );


            // ==========================================
            // GRADE DE CHUNKS (16x16 blocos)
            // ==========================================

            DesenharGradeChunks(g, cos30, sin30);


            // ==========================================
            // ROTAS JOÃO E MARIA
            // ==========================================

            DesenharRotas(g, cos30, sin30);

            // ==========================================
            // PONTOS
            // ==========================================

            List<PontoMinecraft> pontosAtuais =
                eSuperficie
                ? pontosSuperficie
                : pontosNether;


            foreach (var ponto in pontosAtuais)
            {
                int tX =
                    (int)
                    (
                        (
                            (ponto.X -
                            ponto.Z)
                            *
                            cos30
                            *
                            escala
                        )
                        +
                        deslocamentoX
                    );


                int tY =
                    (int)
                    (
                        (
                            (
                                (ponto.X +
                                ponto.Z)
                                *
                                sin30
                            )
                            -
                            ponto.Y
                        )
                        *
                        escala
                        +
                        deslocamentoY
                    );


                // Área clicável

                ponto.AreaNaTela =
                    new Rectangle(
                        tX - 10,
                        tY - 10,
                        20,
                        20
                    );


                // ==========================================
                // COR
                // ==========================================

                if (
                    ponto ==
                    pontoSelecionadoParaDistancia
                )
                {
                    g.FillEllipse(
                        Brushes.Yellow,
                        ponto.AreaNaTela
                    );
                }
                else
                {
                    g.FillEllipse(
                        Brushes.Crimson,
                        ponto.AreaNaTela
                    );
                }


                g.DrawEllipse(
                    Pens.White,
                    ponto.AreaNaTela
                );


                // ==========================================
                // TEXTO
                // ==========================================

                using (
                    Font fonteTexto =
                    new Font(
                        "Segoe UI",
                        9,
                        FontStyle.Bold))
                {
                    string textoExibicao =
                        $"{ponto.Nome} " +
                        $"[{ponto.X}, " +
                        $"{ponto.Y}, " +
                        $"{ponto.Z}]";


                    g.DrawString(
                        textoExibicao,
                        fonteTexto,
                        Brushes.White,
                        tX + 14,
                        tY - 6
                    );
                }
            }
        }


        // ==========================================
        // TECLADO
        // ==========================================

        private void FormMapa_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            // ==========================================
            // CTRL + N
            // ==========================================

            if (
                e.Control &&
                e.KeyCode == Keys.N
            )
            {
                e.Handled = true;

                e.SuppressKeyPress = true;

                if (joaoMariaAtivo)
                {
                    FinalizarRotaJoaoMaria();
                    lblRota.Text = "João e Maria: desligado";
                    lblRota.ForeColor = Color.DarkGray;
                    btnJoaoMaria.BackColor = Color.FromArgb(95, 70, 150);
                }

                eSuperficie =
                    !eSuperficie;


                pontoSelecionadoParaDistancia =
                    null;


                if (eSuperficie)
                {
                    lblInfoDimensao.Text =
                        "🌍 DIMENSAO: SUPERFICIE";


                    lblInfoDimensao.ForeColor =
                        Color.YellowGreen;
                }
                else
                {
                    lblInfoDimensao.Text =
                        "🔥 DIMENSAO: NETHER";


                    lblInfoDimensao.ForeColor =
                        Color.OrangeRed;
                }

                AtualizarPesquisa();

                this.Invalidate();

                return;
            }
        }


        // ==========================================
        // JOÃO E MARIA
        // ==========================================

        private void BtnJoaoMaria_Click(object? sender, EventArgs e)
        {
            if (!joaoMariaAtivo)
            {
                joaoMariaAtivo = true;
                pontosRotaAtual.Clear();

                lblRota.Text =
                    "João e Maria: ATIVO\nClique nos locais na ordem.";

                lblRota.ForeColor = Color.MediumPurple;
                btnJoaoMaria.BackColor = Color.FromArgb(130, 90, 190);
            }
            else
            {
                FinalizarRotaJoaoMaria();

                lblRota.Text = "João e Maria: desligado";
                lblRota.ForeColor = Color.DarkGray;
                btnJoaoMaria.BackColor = Color.FromArgb(95, 70, 150);
            }

            this.Invalidate();
        }

        private void FinalizarRotaJoaoMaria()
        {
            if (pontosRotaAtual.Count >= 2)
            {
                mundoAtual.RotasJoaoMaria.Add(
                    new RotaJoaoMaria
                    {
                        Nome =
                            $"Rota João e Maria {mundoAtual.RotasJoaoMaria.Count + 1}",
                        Superficie = eSuperficie,
                        Pontos = new List<PontoRota>(pontosRotaAtual)
                    });

                gerenciadorMundos.SalvarMundoAtual();
            }

            pontosRotaAtual.Clear();
            joaoMariaAtivo = false;
        }

        // ==========================================
        // EXCLUIR ROTA JOÃO E MARIA (clique direito no botão)
        // ==========================================

        private void BtnJoaoMaria_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            List<RotaJoaoMaria> rotas =
                mundoAtual.RotasJoaoMaria
                    .Where(r => r.Superficie == eSuperficie)
                    .ToList();

            if (rotas.Count == 0)
            {
                MessageBox.Show(
                    "Não há rotas João e Maria salvas nesta dimensão.");
                return;
            }

            ContextMenuStrip menuRotas = new ContextMenuStrip();

            foreach (RotaJoaoMaria rota in rotas)
            {
                ToolStripMenuItem item =
                    new ToolStripMenuItem($"Excluir: {rota.Nome}");

                item.Click += (s, args) =>
                {
                    mundoAtual.RotasJoaoMaria.Remove(rota);
                    gerenciadorMundos.SalvarMundoAtual();
                    this.Invalidate();
                };

                menuRotas.Items.Add(item);
            }

            menuRotas.Show(btnJoaoMaria, e.Location);
        }

        private void AdicionarPontoNaRota(PontoMinecraft ponto)
        {
            if (pontosRotaAtual.Count > 0)
            {
                PontoRota ultimo =
                    pontosRotaAtual[pontosRotaAtual.Count - 1];

                if (ultimo.X == ponto.X &&
                    ultimo.Y == ponto.Y &&
                    ultimo.Z == ponto.Z)
                    return;
            }

            pontosRotaAtual.Add(
                new PontoRota
                {
                    Nome = ponto.Nome,
                    X = ponto.X,
                    Y = ponto.Y,
                    Z = ponto.Z
                });

            double total =
                CalcularDistanciaRota(pontosRotaAtual);

            lblRota.Text =
                $"João e Maria: {pontosRotaAtual.Count} pontos\n" +
                $"Percurso: {total:F2} blocos";

            this.Invalidate();
        }

        private double CalcularDistanciaRota(
            List<PontoRota> pontos)
        {
            double total = 0;

            for (int i = 1; i < pontos.Count; i++)
            {
                int dx = pontos[i].X - pontos[i - 1].X;
                int dy = pontos[i].Y - pontos[i - 1].Y;
                int dz = pontos[i].Z - pontos[i - 1].Z;

                total += Math.Sqrt(
                    (double)dx * dx +
                    (double)dy * dy +
                    (double)dz * dz);
            }

            return total;
        }

        // ==========================================
        // PESQUISA
        // ==========================================

        private void TxtPesquisa_TextChanged(
            object? sender,
            EventArgs e)
        {
            AtualizarPesquisa();
        }

        private void TxtPesquisa_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                NavegarPesquisa(1);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void BtnPesquisaAnterior_Click(
            object? sender,
            EventArgs e)
        {
            NavegarPesquisa(-1);
        }

        private void BtnPesquisaProxima_Click(
            object? sender,
            EventArgs e)
        {
            NavegarPesquisa(1);
        }

        private void AtualizarPesquisa()
        {
            resultadosPesquisa.Clear();
            indicePesquisa = -1;

            string termo =
                txtPesquisa?.Text.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(termo))
            {
                if (lblResultadoPesquisa != null)
                    lblResultadoPesquisa.Text =
                        "Nenhuma pesquisa.";

                return;
            }

            List<PontoMinecraft> pontos =
                eSuperficie
                ? pontosSuperficie
                : pontosNether;

            foreach (PontoMinecraft ponto in pontos)
            {
                string nome = ponto.Nome;
                string coords =
                    $"{ponto.X} {ponto.Y} {ponto.Z}";
                string coordsVirgula =
                    $"{ponto.X},{ponto.Y},{ponto.Z}";

                if (nome.IndexOf(
                        termo,
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    coords.IndexOf(
                        termo,
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    coordsVirgula.IndexOf(
                        termo,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    resultadosPesquisa.Add(ponto);
                }
            }

            if (resultadosPesquisa.Count == 0)
            {
                lblResultadoPesquisa.Text = "0 resultados";
                return;
            }

            indicePesquisa = 0;
            PontoMinecraft primeiro = resultadosPesquisa[0];

            lblResultadoPesquisa.Text =
                $"1 de {resultadosPesquisa.Count}: {primeiro.Nome}";

            pontoSelecionadoParaDistancia = primeiro;
            CentralizarNoPonto(primeiro);
            this.Invalidate();
        }

        private void NavegarPesquisa(int direcao)
        {
            if (resultadosPesquisa.Count == 0)
            {
                AtualizarPesquisa();
                return;
            }

            indicePesquisa += direcao;

            if (indicePesquisa < 0)
                indicePesquisa = resultadosPesquisa.Count - 1;

            if (indicePesquisa >= resultadosPesquisa.Count)
                indicePesquisa = 0;

            PontoMinecraft ponto =
                resultadosPesquisa[indicePesquisa];

            lblResultadoPesquisa.Text =
                $"{indicePesquisa + 1} de {resultadosPesquisa.Count}: {ponto.Nome}";

            pontoSelecionadoParaDistancia = ponto;
            CentralizarNoPonto(ponto);
            this.Invalidate();
        }

        private void CentralizarNoPonto(PontoMinecraft ponto)
        {
            float cos30 = (float)Math.Cos(Math.PI / 6);
            float sin30 = (float)Math.Sin(Math.PI / 6);

            float mapaCentroX =
                250 + (this.ClientSize.Width - 250) / 2f;

            float mapaCentroY =
                this.ClientSize.Height / 2f;

            float relativoX =
                (ponto.X - ponto.Z) * cos30 * escala;

            float relativoY =
                ((ponto.X + ponto.Z) * sin30 - ponto.Y) * escala;

            deslocamentoX = mapaCentroX - relativoX;
            deslocamentoY = mapaCentroY - relativoY;
        }

        private PointF ConverterParaTela(
            int x,
            int y,
            int z,
            float cos30,
            float sin30)
        {
            return new PointF(
                ((x - z) * cos30 * escala) + deslocamentoX,
                (((x + z) * sin30) - y) * escala + deslocamentoY);
        }

        private (float x, float z) ConverterParaMundo(
            float px,
            float py,
            float cos30,
            float sin30)
        {
            float a = (px - deslocamentoX) / escala;
            float b = (py - deslocamentoY) / escala;

            float x = (a / cos30 + b / sin30) / 2f;
            float z = (b / sin30 - a / cos30) / 2f;

            return (x, z);
        }

        private void DesenharGradeChunks(
            Graphics g,
            float cos30,
            float sin30)
        {
            int largura = this.ClientSize.Width;
            int altura = this.ClientSize.Height;

            var c1 = ConverterParaMundo(0, 0, cos30, sin30);
            var c2 = ConverterParaMundo(largura, 0, cos30, sin30);
            var c3 = ConverterParaMundo(0, altura, cos30, sin30);
            var c4 = ConverterParaMundo(largura, altura, cos30, sin30);

            float xMin = Math.Min(Math.Min(c1.x, c2.x), Math.Min(c3.x, c4.x));
            float xMax = Math.Max(Math.Max(c1.x, c2.x), Math.Max(c3.x, c4.x));
            float zMin = Math.Min(Math.Min(c1.z, c2.z), Math.Min(c3.z, c4.z));
            float zMax = Math.Max(Math.Max(c1.z, c2.z), Math.Max(c3.z, c4.z));

            int passo = 16;

            // Se der zoom bem longe, dobra o espacamento pra nao
            // desenhar milhares de linhas (grade de chunks vira
            // grade de regioes maiores automaticamente).
            while (
                ((xMax - xMin) / passo > 250) ||
                ((zMax - zMin) / passo > 250)
            )
            {
                passo *= 2;
            }

            int xInicio = ((int)Math.Floor(xMin / 16)) * 16;
            int xFim = ((int)Math.Ceiling(xMax / 16)) * 16;
            int zInicio = ((int)Math.Floor(zMin / 16)) * 16;
            int zFim = ((int)Math.Ceiling(zMax / 16)) * 16;

            using (
                Pen penGrade =
                new Pen(
                    Color.FromArgb(50, 255, 255, 255),
                    1))
            {
                for (int x = xInicio; x <= xFim; x += passo)
                {
                    PointF a = ConverterParaTela(x, 0, zInicio, cos30, sin30);
                    PointF b = ConverterParaTela(x, 0, zFim, cos30, sin30);
                    g.DrawLine(penGrade, a, b);
                }

                for (int z = zInicio; z <= zFim; z += passo)
                {
                    PointF a = ConverterParaTela(xInicio, 0, z, cos30, sin30);
                    PointF b = ConverterParaTela(xFim, 0, z, cos30, sin30);
                    g.DrawLine(penGrade, a, b);
                }
            }
        }

        private void DesenharRotas(
            Graphics g,
            float cos30,
            float sin30)
        {
            foreach (RotaJoaoMaria rota in mundoAtual.RotasJoaoMaria)
            {
                if (rota.Superficie != eSuperficie ||
                    rota.Pontos.Count < 2)
                    continue;

                using (Pen pen =
                    new Pen(
                        Color.FromArgb(180, 255, 215, 0),
                        3))
                {
                    for (int i = 1; i < rota.Pontos.Count; i++)
                    {
                        PointF a = ConverterParaTela(
                            rota.Pontos[i - 1].X,
                            rota.Pontos[i - 1].Y,
                            rota.Pontos[i - 1].Z,
                            cos30,
                            sin30);

                        PointF b = ConverterParaTela(
                            rota.Pontos[i].X,
                            rota.Pontos[i].Y,
                            rota.Pontos[i].Z,
                            cos30,
                            sin30);

                        g.DrawLine(pen, a, b);
                    }
                }

                using (Font fonte =
                    new Font("Segoe UI", 8, FontStyle.Bold))
                {
                    for (int i = 0; i < rota.Pontos.Count; i++)
                    {
                        PointF p = ConverterParaTela(
                            rota.Pontos[i].X,
                            rota.Pontos[i].Y,
                            rota.Pontos[i].Z,
                            cos30,
                            sin30);

                        g.FillEllipse(
                            Brushes.Black,
                            p.X - 9,
                            p.Y - 9,
                            18,
                            18);

                        g.DrawString(
                            (i + 1).ToString(),
                            fonte,
                            Brushes.White,
                            p.X - 4,
                            p.Y - 7);
                    }
                }
            }

            // Mostra também a trilha que está sendo criada.
            if (pontosRotaAtual.Count >= 2)
            {
                using (Pen pen =
                    new Pen(Color.MediumPurple, 4))
                {
                    for (int i = 1; i < pontosRotaAtual.Count; i++)
                    {
                        PointF a = ConverterParaTela(
                            pontosRotaAtual[i - 1].X,
                            pontosRotaAtual[i - 1].Y,
                            pontosRotaAtual[i - 1].Z,
                            cos30,
                            sin30);

                        PointF b = ConverterParaTela(
                            pontosRotaAtual[i].X,
                            pontosRotaAtual[i].Y,
                            pontosRotaAtual[i].Z,
                            cos30,
                            sin30);

                        g.DrawLine(pen, a, b);
                    }
                }
            }
        }


        // ==========================================
        // ZOOM
        // ==========================================

        private void FormMapa_MouseWheel(
            object? sender,
            MouseEventArgs e)
        {
            if (e.X < 250)
            {
                return;
            }


            if (e.Delta > 0)
            {
                escala += 0.1f;
            }
            else
            {
                escala -= 0.1f;
            }


            if (escala < 0.1f)
            {
                escala = 0.1f;
            }


            this.Invalidate();
        }


        // ==========================================
        // COMEÇAR A ARRASTAR
        // ==========================================

        private void FormMapa_MouseDown(
            object? sender,
            MouseEventArgs e)
        {
            if (
                e.Button ==
                MouseButtons.Left &&
                e.X >= 250
            )
            {
                arrastando = true;


                posicaoCliqueOriginal =
                    e.Location;


                deslocamentoOriginalX =
                    deslocamentoX;


                deslocamentoOriginalY =
                    deslocamentoY;
            }
        }


        // ==========================================
        // ARRASTAR MAPA
        // ==========================================

        private void FormMapa_MouseMove(
            object? sender,
            MouseEventArgs e)
        {
            if (arrastando)
            {
                int diferencaX =
                    e.X -
                    posicaoCliqueOriginal.X;


                int diferencaY =
                    e.Y -
                    posicaoCliqueOriginal.Y;


                deslocamentoX =
                    deslocamentoOriginalX +
                    diferencaX;


                deslocamentoY =
                    deslocamentoOriginalY +
                    diferencaY;


                this.Invalidate();
            }
        }


        // ==========================================
        // PARAR DE ARRASTAR
        // ==========================================

        private void FormMapa_MouseUp(
            object? sender,
            MouseEventArgs e)
        {
            if (
                e.Button ==
                MouseButtons.Left
            )
            {
                arrastando = false;
            }
        }


        // ==========================================
        // CLIQUE NO MAPA
        // ==========================================

        private void FormMapa_MouseClick(
            object? sender,
            MouseEventArgs e)
        {
            // Ignora arrastamento

            if (
                Math.Abs(
                    e.X -
                    posicaoCliqueOriginal.X
                ) > 3 ||

                Math.Abs(
                    e.Y -
                    posicaoCliqueOriginal.Y
                ) > 3
            )
            {
                return;
            }


            List<PontoMinecraft> pontosAtuais =
                eSuperficie
                ? pontosSuperficie
                : pontosNether;


            foreach (var ponto in pontosAtuais)
            {
                if (
                    ponto.AreaNaTela.Contains(
                        e.Location)
                )
                {
                    if (joaoMariaAtivo && e.Clicks == 1)
                    {
                        AdicionarPontoNaRota(ponto);
                        return;
                    }

                    // ==========================================
                    // DUPLO CLIQUE
                    // ==========================================

                    if (e.Clicks >= 2)
                    {
                        AbrirEdicaoPonto(
                            ponto
                        );

                        return;
                    }


                    // ==========================================
                    // PRIMEIRO CLIQUE
                    // ==========================================

                    if (
                        pontoSelecionadoParaDistancia
                        == null
                    )
                    {
                        pontoSelecionadoParaDistancia =
                            ponto;


                        this.Invalidate();


                        MessageBox.Show(
                            $"Selecionado: {ponto.Nome}\n\n" +
                            "Clique em outro ponto para calcular a distancia.\n" +
                            "Pressione ENTER para editar."
                        );
                    }


                    // ==========================================
                    // MESMO PONTO
                    // ==========================================

                    else if (
                        pontoSelecionadoParaDistancia
                        == ponto
                    )
                    {
                        pontoSelecionadoParaDistancia =
                            null;


                        this.Invalidate();
                    }


                    // ==========================================
                    // OUTRO PONTO
                    // ==========================================

                    else
                    {
                        int deltaX =
                            ponto.X -
                            pontoSelecionadoParaDistancia.X;


                        int deltaY =
                            ponto.Y -
                            pontoSelecionadoParaDistancia.Y;


                        int deltaZ =
                            ponto.Z -
                            pontoSelecionadoParaDistancia.Z;


                        double distanciaBlocos =
                            Math.Sqrt(
                                (double)deltaX * deltaX +
                                (double)deltaY * deltaY +
                                (double)deltaZ * deltaZ
                            );


                        string resultado =
                            $"De: {pontoSelecionadoParaDistancia.Nome}\n" +
                            $"Ate: {ponto.Nome}\n\n" +
                            $"* Distancia Real: {distanciaBlocos:F2} blocos.\n" +
                            $"* Diferenca Altitude (Y): {Math.Abs(deltaY)} blocos.\n" +
                            $"* Deslocamento Plano: X:{Math.Abs(deltaX)} | Z:{Math.Abs(deltaZ)}";


                        MessageBox.Show(
                            resultado,
                            "Relatorio Topografico"
                        );


                        pontoSelecionadoParaDistancia =
                            null;


                        this.Invalidate();
                    }


                    return;
                }
            }
        }


        // ==========================================
        // ABRIR EDIÇÃO
        // ==========================================

        private void AbrirEdicaoPonto(
            PontoMinecraft ponto)
        {
            using (
                EditarPontoForm janela =
                new EditarPontoForm(ponto))
            {
                if (
                    janela.ShowDialog(this)
                    ==
                    DialogResult.OK
                )
                {
                    // ==================================
                    // EXCLUIR
                    // ==================================

                    if (
                        janela.PontoExcluido
                    )
                    {
                        List<PontoMinecraft>
                            pontosAtuais =
                            eSuperficie
                            ? pontosSuperficie
                            : pontosNether;


                        pontosAtuais.Remove(
                            ponto
                        );
                    }


                    // ==================================
                    // SALVAR
                    // ==================================

                    gerenciadorMundos
                        .SalvarMundoAtual();


                    pontoSelecionadoParaDistancia =
                        null;


                    this.Invalidate();
                }
            }
        }
    }
}