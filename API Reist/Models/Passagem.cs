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
        public string classe { get; set; }
        //public int assentos_economica { get; set; }
        public int assentos { get; set; }
        //public string preco_economica { get; set; }
        public string preco { get; set; }

        public List<Passagem> BuscarPassagensIda(string origem, string destino, string data)
        {
            using (Database DB = new Database())
            {
                var query = "select * from vw_buscar_passagem where date(saida) = date('"+data+ "') and ori_uf = '"+origem+"' and des_uf = '"+destino+"'";
                var retorno = DB.ReturnCommand(query);
                return Listar(retorno);
            }
        }

        /*public List<Passagem> BuscarPassagensIdaVolta(string origem, string destino, string dataIda, string dataVolta)
        {
            using (Database DB = new Database())
            {
                var query = "select * from vw_buscar_passagem where date(saida) = date('" + data + "') and ori_uf = '" + origem + "' and des_uf = '" + destino + "'";
                var retorno = DB.ReturnCommand(query);
                return Listar(retorno);
            }
        }*/

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

                //var clas;
                //if (int.Parse(retorno["assentos_executiva"].ToString()) == 1)                    

                var passagem = new Passagem()
                {
                    origem = Origem,
                    destino = Destino,
                    saida = retorno["saida"].ToString(),
                    chegada = retorno["chegada"].ToString(),
                    assentos = int.Parse(retorno["assentos"].ToString()),
                    preco = retorno["preco"].ToString(),
                    //classe = clas,                    
                };

                if (int.Parse(retorno["classe"].ToString()) == 2)
                    passagem.classe = "Executiva";
                else
                    passagem.classe = "Econômica";

                passagens.Add(passagem);
            }
            retorno.Close();
            return passagens;
        }
    }
}
