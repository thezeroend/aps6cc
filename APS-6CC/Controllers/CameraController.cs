using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APS_6CC.Controllers
{
    public class CameraController : Controller
    {
        private readonly IHostingEnvironment _environment;
        public CameraController(IHostingEnvironment hostingEnvironment)
        {
            _environment = hostingEnvironment;
        }

        public IActionResult CapturaImagem()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CapturaImagemPost()
        {
            try
            {
                //obtem a coleção de arquivos enviada via Post no formulário
                var arquivos = HttpContext.Request.Form.Files;
                //verifica se existem arquivos
                if (arquivos != null)
                {
                    foreach (var arquivo in arquivos)
                    {
                        if (arquivo.Length > 0)
                        {
                            // obtem o nome do arquivo
                            var nomeArquivo = arquivo.FileName;
                            // Gera um Guid para definir um arquivo com nome unico
                            var nomeArquivoUnico = Convert.ToString(Guid.NewGuid());
                            // Obtém a extensão do arquivo
                            var arquivoExtensao = Path.GetExtension(nomeArquivo);
                            // Concatena o nome do arquivo unico + a extensão
                            var novoNomeArquivo = string.Concat(nomeArquivoUnico, arquivoExtensao);
                            //  Gera o caminho para armazenar a imagem na pasta criada
                            var caminhoArquivo = Path.Combine(_environment.WebRootPath, "ImagensCapturadas")
+ $@"\{novoNomeArquivo}";
                            if (!string.IsNullOrEmpty(caminhoArquivo))
                            {
                                // armazena a imagem na pasta definida
                                SalvaImagemLocal(arquivo, caminhoArquivo);
                            }
                        }
                    }
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void SalvaImagemLocal(IFormFile arquivo, string nomeArquivo)
        {
            //recebe o arquivo a ser salvo e o seu nome
            using (FileStream fs = System.IO.File.Create(nomeArquivo))
            {
                //copia a imagem na pasta
                arquivo.CopyTo(fs);
                fs.Flush();
            }
        }
    }
}