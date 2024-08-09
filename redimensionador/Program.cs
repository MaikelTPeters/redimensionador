using System;
using System.Drawing;
using System.Threading;

namespace redimensionador
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando redimensionador");

            Thread thread = new Thread(Redimensionar);
            thread.Start();
        }
        static void Redimensionar()
        {
            #region "Diretorios"
            string entrada = "Arquivo_Entrada";
            string redimensionado = "Arquivo_Redimensionado";
            string finalizado = "Arquivo_Finalizado";

            if (!Directory.Exists(entrada))
            {
                Directory.CreateDirectory(entrada);
            }
            if (!Directory.Exists(redimensionado))
            {
                Directory.CreateDirectory(redimensionado);
            }
            if (!Directory.Exists(finalizado))
            {
                Directory.CreateDirectory(finalizado);
            }
            #endregion

            FileStream fileStream;
            FileInfo fileInfo;

            while (true)
            {
                //Programa vai olhar para a pasta de entrada
                //Se tiver arquivo, ele ira redimensionar
                var arquivoEntrada = Directory.EnumerateFiles(entrada);

                //ler o tamanho que ira redimensionar
                int novaAltura1 = 200;

                foreach (var arquivo in arquivoEntrada)
                {
                    fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fileInfo = new FileInfo(arquivo);

                    string caminho = Environment.CurrentDirectory + @"\" + redimensionado + @"\" + DateTime.Now.Microsecond.ToString() + "_" + fileInfo.Name;

                    //Redimensiona + Copia os arquivos redimensionados para a pasta de redimensionados
                    Redimensionador(Image.FromStream(fileStream), novaAltura1, caminho);

                    //Fecha o arquivo
                    fileStream.Close();

                    //Move o arquivo para a pasta finalizados
                    string caminhoFinalizado = Environment.CurrentDirectory + @"\" + finalizado + @"\" + fileInfo.Name;
                    fileInfo.MoveTo(caminhoFinalizado);
                }
                
                Thread.Sleep(new TimeSpan(0, 0, 5));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagem">Imagem a ser redimensionada</param>
        /// <param name="altura">Altura que a ser redimensionada</param>
        /// <param name="caminho">Caminho aonde sera gravado o arquivo redimensionado</param>
        /// <returns></returns>
        static void Redimensionador(Image imagem, int altura, string caminho)
        {
            double radio = (double)altura / imagem.Height;
            int novaLargura = (int)(imagem.Width * radio);
            int novaAltura = (int)(imagem.Height * radio);

            Bitmap novaImage = new Bitmap(novaLargura, novaAltura);

            using(Graphics g = Graphics.FromImage(novaImage))
            {
                g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);
            }

            novaImage.Save(caminho);
            imagem.Dispose();
        }
    }
}