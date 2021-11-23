using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Reist.Connection;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;

namespace API_Reist.Models
{
    public class Passagem
    {
        public string saida { get; set; }
        public Local origem { get; set; }
        public Local destino { get; set; }
        public string chegada { get; set; }
        public int assentos_economica { get; set; }
        public int assentos_executiva { get; set; }
        public string preco_economica { get; set; }
        public string preco_executiva { get; set; }

        public List<Passagem> ListarPassagens()
        {
            using (Database DB = new Database())
            {
                var query = "select * from vw_buscar_passagem;";
                var retorno = DB.ReturnCommand(query);
                return Listar(retorno);
            }
        }

        public List<Passagem> Listar(MySqlDataReader retorno)
        {
            var passagens = new List<Passagem>();
            while (retorno.Read())
            {
                var enderecoObjOrigem = new Endereco()
                {
                    uf = retorno["ori_uf"].ToString(),
                };

                var enderecoObjDestino = new Endereco()
                {
                    uf = retorno["des_uf"].ToString(),
                };

                var Origem = new Local()
                {
                    nome = retorno["origem"].ToString(),
                    endereco = enderecoObjOrigem
                };

                var Destino = new Local()
                {
                    nome = retorno["destino"].ToString(),
                    endereco = enderecoObjDestino
                };

                var passagem = new Passagem()
                {
                    origem = Origem,
                    destino = Destino,
                    saida = retorno["saida"].ToString(),
                    chegada = retorno["chegada"].ToString(),
                    assentos_economica = int.Parse(retorno["assentos_economica"].ToString()),
                    assentos_executiva = int.Parse(retorno["assentos_executiva"].ToString()),
                    preco_economica = retorno["preco_economia"].ToString(),
                };
                passagens.Add(passagem);
            }
            retorno.Close();
            return passagens;
        }
    }
}
