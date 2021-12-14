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
        public int id { get; set; }
        public string saida { get; set; }
        public Local origem { get; set; }
        public Local destino { get; set; }
        public string chegada { get; set; }
        public string classe { get; set; }
        //public int assentos_economica { get; set; }
        public int assentos { get; set; }
        //public string preco_economica { get; set; }
        public string preco { get; set; }

        public List<Passagem> BuscarPassagensIda(string origem, string destino, string data, int classe, int pessoas)
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
                return Listar(retorno, pessoas);
            }
        }

        public List<IdaVolta> BuscarPassagensIdaVolta(string origem, string destino, string dataIda, string dataVolta, int classe, int p)
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
                return ListarIdaVolta(retorno, p);
            }
        }

        public void verificar_assentos()
        {
            using (Database database = new Database())
            {
                string command = "call verificar_assentos("+this.id+", @assentos_ocupados); select @assentos_ocupados as a;";
                MySqlDataReader retorno = database.ReturnCommand(command);
                retorno.Read();
                int ocupados = int.Parse(retorno["a"].ToString());
                this.assentos = this.assentos - ocupados;
                //return assentos;
            }
        }

        public Passagem BuscarPassagem(int id)
        {
            using (Database database = new Database())
            {
                string command = "select * from vw_buscar_passagem_ida where id_passagem = "+id+"";
                MySqlDataReader retorno = database.ReturnCommand(command);
                retorno.Read();

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
                    id = id,
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

                passagem.verificar_assentos();

                return passagem;
            }
        }

        public List<Passagem> Listar(MySqlDataReader retorno, int p)
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
                    id = int.Parse(retorno["id_passagem"].ToString()),
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

                passagem.CalcularPreco(p);

                passagem.verificar_assentos();

                passagens.Add(passagem);
            }

            retorno.Close();
            return passagens;
        }

        public List<IdaVolta> ListarIdaVolta(MySqlDataReader retorno, int p)
        {            
            var idaVoltas = new List<IdaVolta>();
            while (retorno.Read())
            {                
                int idIda = int.Parse(retorno["id_ida"].ToString());
                int idVolta = int.Parse(retorno["id_volta"].ToString());

                var passagemIda = BuscarPassagem(idIda);
                var passagemVolta = BuscarPassagem(idVolta);

                var IdaVolta = new IdaVolta()
                {
                    ida = passagemIda,
                    volta = passagemVolta,
                };

                IdaVolta.PrecoIdaVolta(p);

                idaVoltas.Add(IdaVolta);               
            }
            retorno.Close();
            return idaVoltas;
        }

        public void CalcularPreco(int pessoas)
        {
            float total = float.Parse(this.preco) * pessoas;
            this.preco = total.ToString();
        }
    }    

    public class IdaVolta
    {
        public float precoFinal { get; set; }
        public Passagem ida { get; set; }
        public Passagem volta { get; set; }


        public void PrecoIdaVolta(int pessoas)
        {
            ida.CalcularPreco(pessoas);
            volta.CalcularPreco(pessoas);


            float total = float.Parse(ida.preco) + float.Parse(volta.preco);
            this.precoFinal = total;
        }
    }
}
