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

        public List<Passagem> BuscarPassagensIda(string origem, string destino, string data, int classe)
        {
            using (Database DB = new Database())
            {
                string query;
                if (classe == 1 || classe == 2)
                    query = "select * from vw_buscar_passagem_ida where date(saida) = date('" + data + "') and ori_city = '" + origem + "' and des_city = '" + destino + "' and " +
                        "classe = " + classe + "";
                else
                    query = "select * from vw_buscar_passagem_ida where date(saida) = date('" + data + "') and ori_city = '" + origem + "' and des_city = '" + destino + "'";

                var retorno = DB.ReturnCommand(query);
                return Listar(retorno);
            }
        }

        public List<IdaVolta> BuscarPassagensIdaVolta(string origem, string destino, string dataIda, string dataVolta, int classe)
        {
            using (Database DB = new Database())
            {
                string query;
                if (classe == 1 || classe == 2)
                    query = "select id_ida, id_volta from vw_buscar_passagem_ida_volta where date(saida_ida) = date('" + dataIda + "') and date(saida_volta) = date('" + dataVolta + "') " +
                        "and ori_city = '" + origem + "' and des_city = '" + destino + "' and " +
                        "classe = " + classe + ";";
                else
                    query = "select id_ida, id_volta from vw_buscar_passagem_ida_volta where date(saida_ida) = date('" + dataIda + "') and date(saida_volta) = date('" + dataVolta + "') " +
                        "and ori_city = '" + origem + "' and des_city = '" + destino + "';";
                var retorno = DB.ReturnCommand(query);

                retorno.Close();
                return ListarPorID(retorno);
            }
        }

        public List<Passagem> Listar(MySqlDataReader retorno)
        {
            var passagens = new List<Passagem>();
            while (retorno.Read())
            {
                var enderecoOrigem = new Endereco()
                {
                    uf = retorno["ori_uf"].ToString(),
                    cidade = retorno["ori_city"].ToString(),
                };

                var enderecoDestino = new Endereco()
                {
                    uf = retorno["des_uf"].ToString(),
                    cidade = retorno["des_city"].ToString(),
                };

                var Origem = new Local()
                {
                    sigla = retorno["origem"].ToString(),
                    endereco = enderecoOrigem
                };

                var Destino = new Local()
                {
                    sigla = retorno["destino"].ToString(),
                    endereco = enderecoDestino
                };

                var passagem = new Passagem()
                {
                    origem = Origem,
                    destino = Destino,
                    saida = retorno["saida"].ToString(),
                    chegada = retorno["chegada"].ToString(),
                    assentos = int.Parse(retorno["assentos"].ToString()),
                    preco = retorno["preco"].ToString(),
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

        public List<IdaVolta> ListarPorID(MySqlDataReader retorno)
        {            
            var idaVoltas = new List<IdaVolta>();
            //retorno.Ope
            while (retorno.Read())
            {
                using (Database DB = new Database())
                {
                    string query = "select * from vw_buscar_passagem_ida where id_passagem = " + retorno["id_ida"].ToString() + ";";
                    var retornoLocal = DB.ReturnCommand(query);

                    while (retornoLocal.Read())
                    {

                        var enderecoOrigem = new Endereco()
                        {
                            uf = retornoLocal["ori_uf"].ToString(),
                            cidade = retornoLocal["ori_city"].ToString(),
                        };

                        var enderecoDestino = new Endereco()
                        {
                            uf = retornoLocal["des_uf"].ToString(),
                            cidade = retornoLocal["des_city"].ToString(),
                        };

                        var Origem = new Local()
                        {
                            sigla = retornoLocal["origem"].ToString(),
                            endereco = enderecoOrigem
                        };

                        var Destino = new Local()
                        {
                            sigla = retornoLocal["destino"].ToString(),
                            endereco = enderecoDestino
                        };

                        var passagemIda = new Passagem()
                        {
                            origem = Origem,
                            destino = Destino,
                            saida = retornoLocal["saida"].ToString(),
                            chegada = retornoLocal["chegada"].ToString(),
                            assentos = int.Parse(retornoLocal["assentos"].ToString()),
                            preco = retornoLocal["preco"].ToString(),
                        };

                        if (int.Parse(retornoLocal["classe"].ToString()) == 2)
                            passagemIda.classe = "Executiva";
                        else
                            passagemIda.classe = "Econômica";

                        var idaVolta = new IdaVolta()
                        {
                            ida = passagemIda,
                            volta = null,
                        };

                        idaVoltas.Add(idaVolta);
                    }
                    retornoLocal.Close();
                }
            
                retorno.Close();
                //retornoLocal.Close();
                //return idaVoltas;
            }
            return idaVoltas;
        }

        public class IdaVolta
        {
            public Passagem ida { get; set; }
            public Passagem volta { get; set; }

        }
    }
}
