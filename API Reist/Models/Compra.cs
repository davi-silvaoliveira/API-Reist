using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Reist.Connection;
using MySql.Data.MySqlClient;

namespace API_Reist.Models
{
    public class Compra
    {
        public int id { get; set; }
        public string cpf { get; set; }
        public int parcelas { get; set; }
        public int idPacote { get; set; }
        public int ida { get; set; }
        public int volta { get; set; }
        public int quarto { get; set; }
        public int quantQuartos { get; set; }
        public int quantPessoas { get; set; }
        public string checkin { get; set; }
        public string checkout { get; set; }
        public float valorCompra { get; set; }
        public string dataPagamento { get; set; }
        public Cartao cartao { get; set; }
        public Pacote pacote { get; set; }
        public Viagem viagem { get; set; }

        public void InserirCompraIda()
        {
            cartao.Inserir();
            viagem.InserirViagemIda();
            InserirPagamento();
        }

        public void InserirCompraIdaVolta()
        {
            cartao.Inserir();
            viagem.InserirViagemIdaVolta();
            InserirPagamento();
        }

        public void InserirCompraQuarto()
        {
            cartao.Inserir();
            viagem.InserirViagemQuarto();
            InserirPagamento();
        }
        public void InserirCompraPacote()
        {
            cartao.Inserir();
            InserirPagamentoPacote();
        }

        public void InserirPagamento()
        {
            using (Database DB = new Database())
            {
                //this.cpf = 
                var query = "insert into pagamento_compra values(default, " + this.cpf + ", null, " + this.viagem.id + ", " + this.cartao.id + ", "+this.parcelas+", curdate(), "+this.valorCompra+");";
                MySqlCommand cmd = new MySqlCommand(query, DB.connection);
                cmd.ExecuteNonQuery();

                //return this;
            }
        }

        public void InserirPagamentoPacote()
        {
            using (Database DB = new Database())
            {
                //this.cpf = 
                var query = "insert into pagamento_compra values(default, " + this.cpf + ", " + this.pacote.id + ", null, " + this.cartao.id + ", " + this.parcelas + ", curdate(), " + this.valorCompra + ");";
                MySqlCommand cmd = new MySqlCommand(query, DB.connection);
                cmd.ExecuteNonQuery();

                //return this;
            }
        }
    }
}
