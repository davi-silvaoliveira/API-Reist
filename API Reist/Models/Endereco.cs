using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Reist.Connection;
using MySql.Data.MySqlClient;

namespace API_Reist.Models
{
    public class Endereco
    {
        public string cep { get; set; }
        public string uf { get; set; }
        public string cidade { get; set; }
        public string bairro { get; set; }
        public string logradouro { get; set; }
        public int numero { get; set; }

        public Endereco() { }

        public List<Endereco> ListarEnderecos()
        {
            using (Database DB = new Database())
            {
                var query = "select * from vw_listar_enderecos;";
                var retorno = DB.ReturnCommand(query);
                return Listar(retorno);
            }
        }

        public List<Endereco> Listar(MySqlDataReader retorno)
        {
            var enderecos = new List<Endereco>();
            while (retorno.Read())
            {
                var endereco = new Endereco()
                {
                    cep = retorno["cep"].ToString(),
                    uf = retorno["estado"].ToString(),
                    cidade = retorno["cidade"].ToString(),
                    bairro = retorno["bairro"].ToString(),
                    logradouro = retorno["logradouro"].ToString(),
                };
                enderecos.Add(endereco);
            }
            retorno.Close();
            return enderecos;
        }
    }
}
