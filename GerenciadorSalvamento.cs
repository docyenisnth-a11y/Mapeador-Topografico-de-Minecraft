using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MinecraftMapApp
{
    public class GerenciadorSalvamento
    {
        private readonly string pastaMundos;

        public GerenciadorSalvamento()
        {
            string pastaDados = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData
                ),
                "MinecraftMapApp"
            );

            pastaMundos = Path.Combine(
                pastaDados,
                "Mundos"
            );

            Directory.CreateDirectory(pastaMundos);
        }

        public void SalvarMundo(MundoMinecraft mundo)
        {
            try
            {
                string nomeArquivo = LimparNomeArquivo(mundo.Nome);

                string caminho = Path.Combine(
                    pastaMundos,
                    nomeArquivo + ".json"
                );

                string json = JsonSerializer.Serialize(
                    mundo,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }
                );

                File.WriteAllText(caminho, json);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Erro ao salvar o mundo:\n\n" + ex.Message,
                    "Erro de Salvamento"
                );
            }
        }

        public List<MundoMinecraft> CarregarMundos()
        {
            List<MundoMinecraft> mundos =
                new List<MundoMinecraft>();

            try
            {
                string[] arquivos =
                    Directory.GetFiles(
                        pastaMundos,
                        "*.json"
                    );

                foreach (string arquivo in arquivos)
                {
                    try
                    {
                        string json =
                            File.ReadAllText(arquivo);

                        MundoMinecraft? mundo =
                            JsonSerializer.Deserialize<MundoMinecraft>(
                                json
                            );

                        if (mundo != null)
                        {
                            mundos.Add(mundo);
                        }
                    }
                    catch
                    {
                        // Ignora arquivos de mundo corrompidos
                        // para não impedir os demais de carregar.
                    }
                }
            }
            catch
            {
                // Se a pasta não existir, ela será criada
                // automaticamente pelo construtor.
            }

            return mundos;
        }

        public void ExcluirMundo(string nome)
        {
            try
            {
                string nomeArquivo =
                    LimparNomeArquivo(nome);

                string caminho =
                    Path.Combine(
                        pastaMundos,
                        nomeArquivo + ".json"
                    );

                if (File.Exists(caminho))
                {
                    File.Delete(caminho);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Erro ao excluir o mundo:\n\n" +
                    ex.Message,
                    "Erro"
                );
            }
        }

        private string LimparNomeArquivo(string nome)
        {
            foreach (
                char caractere
                in Path.GetInvalidFileNameChars()
            )
            {
                nome = nome.Replace(
                    caractere.ToString(),
                    "_"
                );
            }

            return nome.Trim();
        }
    }
}