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
        public string nome { get; set; }
        public int quantidade { get; set; }
        public int capacidade { get; set; }
        public string descricao { get; set; }
        public float diaria { get; set; }
        public long hotel { get; set; }

        public List<Quarto> ListarQuartos(long id)
        {
            using (Database DB = new Database())
            {
                var query = "select * from quarto where cnpj_hotel = " + id + ";";
                var retorno = DB.ReturnCommand(query);
                return Listar(retorno);
            }
        }

        public List<Quarto> Listar(MySqlDataReader retorno)
        {
            var quartos = new List<Quarto>();
            while (retorno.Read())
            {
                var quarto = new Quarto()
                {
                    hotel = long.Parse(retorno["cnpj_hotel"].ToString()),
                    nome = retorno["nome"].ToString(),
                    descricao = retorno["descricao"].ToString(),
                    quantidade = int.Parse(retorno["quantidade"].ToString()),
                    capacidade = int.Parse(retorno["capacidade"].ToString()),
                    diaria = float.Parse(retorno["precoDiaria"].ToString()),
                };
                quartos.Add(quarto);
            }
            retorno.Close();
            return quartos;
        }
    }
}
