using Microsoft.EntityFrameworkCore;
using Pizzaria1000Video.Data;
using Pizzaria1000Video.Dto;
using Pizzaria1000Video.Models;

namespace Pizzaria1000Video.Services.Pizza
{
    public class PizzaService : IPizzaInterface
    {
        private readonly AppDbContext _context;
        private readonly string _sistema;
        public PizzaService(AppDbContext context, IWebHostEnvironment sistema)
        {
            _context = context;
            _sistema = sistema.WebRootPath;
        }


        public string GeraCaminhoArquivo(IFormFile foto)
        {
            var codigoUnico = Guid.NewGuid().ToString();
            var nomeCaminhoImagem = foto.FileName.Replace(" ", "").ToLower() + codigoUnico + ".png";

            var caminhoParaSalvarImagens = _sistema + "\\imagem\\";


            if (!Directory.Exists(caminhoParaSalvarImagens))
            {
                Directory.CreateDirectory(caminhoParaSalvarImagens);
            }

            using (var stream = File.Create(caminhoParaSalvarImagens + nomeCaminhoImagem))
            {
                foto.CopyToAsync(stream).Wait();
            }

            return nomeCaminhoImagem;


        }

        public async Task<PizzaModel> CriarPizza(PizzaCriacaoDto pizzaCriacaoDto, IFormFile foto)
        {
            try
            {
                var nomeCaminhoImagem = GeraCaminhoArquivo(foto);

                var pizza = new PizzaModel
                {
                    Sabor = pizzaCriacaoDto.Sabor,
                    Descricao = pizzaCriacaoDto.Descricao,
                    Valor = pizzaCriacaoDto.Valor,
                    Capa = nomeCaminhoImagem
                };

                _context.Add(pizza);
                await _context.SaveChangesAsync();

                return pizza;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PizzaModel>> GetPizzas()
        {
            try
            {

                return await _context.Pizzas.ToListAsync();

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PizzaModel> GetPizzaPorId(int id)
        {
            try
            {

                return await _context.Pizzas.FirstOrDefaultAsync(pizza => pizza.Id == id);

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PizzaModel> EditarPizza(PizzaModel pizza, IFormFile? foto)
        {
            try
            {
                var pizzaBanco = await _context.Pizzas.AsNoTracking().FirstOrDefaultAsync(pizzaBD => pizzaBD.Id == pizza.Id);

                var nomeCaminhoImagem = "";

                if(foto != null)
                {
                    string caminhoCapaExistente = _sistema + "\\imagem\\" + pizzaBanco.Capa;

                    if(File.Exists(caminhoCapaExistente))
                    {
                        File.Delete(caminhoCapaExistente);
                    }

                    nomeCaminhoImagem = GeraCaminhoArquivo(foto);
                }


                pizzaBanco.Sabor = pizza.Sabor;
                pizzaBanco.Descricao = pizza.Descricao;
                pizzaBanco.Valor = pizza.Valor;

                if(nomeCaminhoImagem != "")
                {
                    pizzaBanco.Capa = nomeCaminhoImagem;
                }
                else
                {
                    pizzaBanco.Capa = pizza.Capa;
                }

                _context.Update(pizzaBanco);
                await _context.SaveChangesAsync();

                return pizza;

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PizzaModel> RemoverPizza(int id)
        {
            try
            {
                var pizza = await _context.Pizzas.FirstOrDefaultAsync(pizzabanco => pizzabanco.Id == id);   

                _context.Remove(pizza);
                await _context.SaveChangesAsync();

                return pizza;


            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PizzaModel>> GetPizzasFiltro(string? pesquisar)
        {
            try
            {

                var pizzas = await _context.Pizzas.Where(pizzaBanco => pizzaBanco.Sabor.Contains(pesquisar)).ToListAsync();
                return pizzas;

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
