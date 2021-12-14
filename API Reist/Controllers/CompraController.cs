using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Reist.Models;

namespace API_Reist.Controllers
{
    public class CompraController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public Cartao ReturnCompra()
        {
            Cartao cartao = new Cartao();
            return cartao;

        }

        [HttpPost]
        public bool FinalizarCompraIda([FromBody] Compra compra)
        {
            Passagem passagem = new Passagem();
            Viagem viagem = new Viagem();
            int id = compra.ida;
            int pessoas = compra.quantPessoas;
            //id = compra.viagem.ida.id;

            passagem = passagem.BuscarPassagem(id);
            //passagem.CalcularPreco(pessoas);
            viagem.ida = passagem;
            viagem.quantidade_pessoas = pessoas;

            compra.viagem = viagem;
            //compra.viagem.quantidade_pessoas = pessoas;
            //compra.cartao.validade = dataValidade;

            try
            {
                compra.InserirCompraIda();
                return true;
            }
            catch
            {
                return false;
            }

        }

        [HttpPost]
        public bool FinalizarCompraIdaVolta([FromBody] Compra compra)
        {
            Passagem passagemIda = new Passagem();
            Passagem passagemVolta = new Passagem();
            IdaVolta idaVolta = new IdaVolta();
            Viagem viagem = new Viagem();
            int idIda = compra.ida;
            int idVolta = compra.volta;
            int pessoas = compra.quantPessoas;
            //id = compra.viagem.ida.id;

            passagemIda = passagemIda.BuscarPassagem(idIda);
            passagemVolta = passagemVolta.BuscarPassagem(idVolta);
            passagemIda.CalcularPreco(compra.quantPessoas);
            passagemVolta.CalcularPreco(compra.quantPessoas);
            idaVolta.ida = passagemIda; idaVolta.volta = passagemVolta;
            viagem.idaVolta = idaVolta;
            viagem.quantidade_pessoas = pessoas;
            compra.viagem = viagem;
            //compra.viagem.quantidade_pessoas = pessoas;
            //compra.cartao.validade = dataValidade;

            try
            {
                compra.InserirCompraIdaVolta();
                return true;
            }
            catch
            {
                return false;
            }

        }

        [HttpPost]
        public bool FinalizarCompraHospedagem([FromBody] Compra compra)
        {
            Quarto quarto = new Quarto();
            Viagem viagem = new Viagem();
            int idQuarto = compra.quarto;
            int pessoas = compra.quantPessoas;
            int quartos = compra.quantQuartos;
            string checkin = compra.checkin;
            string checkout = compra.checkout;

            viagem.quantidade_pessoas = pessoas;
            viagem.quantidade_quartos = quartos;
            viagem.checkin = checkin;
            viagem.checkout = checkout;
            compra.viagem = viagem;

            quarto = quarto.BuscarQuarto(idQuarto);
            compra.viagem.quarto = quarto;

            //compra.viagem.quantidade_pessoas = pessoas;
            //compra.cartao.validade = dataValidade;

            try
            {
                compra.InserirCompraQuarto();
                return true;
            }
            catch
            {
                return false;
            }

        }

        [HttpPost]
        public bool FinalizarCompraPacote([FromBody] Compra compra)
        {
            Pacote pacote = new Pacote();
            int idPacote = compra.idPacote;

            pacote = pacote.Buscar(idPacote);
            compra.pacote = pacote;

            //compra.viagem.quantidade_pessoas = pessoas;
            //compra.cartao.validade = dataValidade;

            try
            {
                compra.InserirCompraPacote();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
