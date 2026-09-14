using System;
using System.Collections.Generic;

namespace MinecraftMapApp
{
    public class PontoRota
    {
        public string Nome { get; set; } = string.Empty;
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }

    public class RotaJoaoMaria
    {
        public string Nome { get; set; } = "Rota João e Maria";
        public bool Superficie { get; set; } = true;
        public bool Fim { get; set; } = false;
        public List<PontoRota> Pontos { get; set; } = new List<PontoRota>();
    }

    public class MundoMinecraft
    {
        public string Nome { get; set; } = string.Empty;

        public string Seed { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public float CameraX { get; set; } = 650f;

        public float CameraY { get; set; } = 350f;

        public float CameraEscala { get; set; } = 0.8f;

        public List<PontoMinecraft> PontosSuperficie { get; set; } = new List<PontoMinecraft>();

        public List<PontoMinecraft> PontosNether { get; set; } = new List<PontoMinecraft>();

        public List<PontoMinecraft> PontosCaverna { get; set; } = new List<PontoMinecraft>();

        public List<PontoMinecraft> PontosFim { get; set; } = new List<PontoMinecraft>();

        public List<RotaJoaoMaria> RotasJoaoMaria { get; set; } = new List<RotaJoaoMaria>();
    }
}