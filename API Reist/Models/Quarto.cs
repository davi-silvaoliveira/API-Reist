using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Reist.Connection;
using MySql.Data.MySqlClient;

namespace API_Reist.Models
{
    public class Quarto
    {
        public int id { get; set; }
        public string nome { get; set; }
        public int quantidade { get; set; }
        public int capacidade { get; set; }
        public string descricao { get; set; }
        public float diaria { get; set; }
        public float precoTotal { get; set; }
        public long hotel { get; set; }

        public List<Quarto> ListarQuartos(long id, string checkin, string checkout, int q)
        {
            using (Database DB = new Database())
            {
                var query = "select * from quarto where cnpj_hotel = " + id + ";";
                var retorno = DB.ReturnCommand(query);
                return Listar(retorno, checkin, checkout, q);
            }
        }

        public Quarto BuscarQuarto(int id)
        {
            using (Database database = new Database())
            {
                string command = "select * from vw_buscar_quarto where id = " + id + "";
                MySqlDataReader retorno = database.ReturnCommand(command);
                retorno.Read();

                var quarto = new Quarto()
                {
                    id = id,
                    nome = retorno["nome"].ToString(),
                    diaria = float.Parse(retorno["precoDiaria"].ToString()),
                };

                return quarto;
            }
        }

        public void verificar_reservas(string checkin, string checkout)
        {
            using (Database database = new Database())
            {
                string command = "call verificar_reservas(" + this.id + ", '"+checkin+"', '"+checkout+"', @ocupados); select @ocupados as a;";
                MySqlDataReader retorno = database.ReturnCommand(command);
                retorno.Read();
                int ocupados = int.Parse(retorno["a"].ToString());
                this.quantidade = this.quantidade - ocupados;
            }
        }

        public List<Quarto> Listar(MySqlDataReader retorno, string checkin, string checkout, int q)
        {
            var quartos = new List<Quarto>();
            while (retorno.Read())
            {
                var quarto = new Quarto()
                {
                    id = int.Parse(retorno["id"].ToString()),
                    hotel = long.Parse(retorno["cnpj_hotel"].ToString()),
                    nome = retorno["nome"].ToString(),
                    descricao = retorno["descricao"].ToString(),
                    quantidade = int.Parse(retorno["quantidade"].ToString()),

                    capacidade = int.Parse(retorno["capacidade"].ToString()),
                    diaria = float.Parse(retorno["precoDiaria"].ToString()),
                };

                quarto.verificar_reservas(checkin, checkout);
                quarto.CalcularPreco(q);
                quartos.Add(quarto);
            }
            retorno.Close();
            return quartos;
        }

        public void CalcularPreco(int quartos)
        {
            float total = this.diaria * quartos;
            this.precoTotal = total;
        }
    }
}
