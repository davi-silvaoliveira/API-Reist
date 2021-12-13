using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Reist.Connection;
using MySql.Data.MySqlClient;

namespace API_Reist.Models
{
    public class Pacote
    {
        public int id { get; set; } 
        public string descricao { get; set; }
        public int desconto { get; set; }
        public Viagem viagem { get; set; }

        public List<Pacote> BuscarPacotes(string origem, string destino, string dataIda, string dataVolta, int classe)
        {
            using (Database DB = new Database())
            {
                var query = "select * from vw_listar_pacotes where ori_city = '"+origem+"' and des_city = '"+destino+"' and classe = 1 and date(saida_ida) = date('"+dataIda+"') and date(saida_volta) = date('"+dataVolta+"');";
                var retorno = DB.ReturnCommand(query);
                return Listar(retorno);
            }
        }

        public List<Pacote> Listar(MySqlDataReader retorno)
        {
            var pacotes = new List<Pacote>();
            while (retorno.Read())
            {
                var passagemIda = new Passagem()
                {
                    saida = retorno["saida_ida"].ToString(),
                };

                var passagemVolta = new Passagem()
                {
                    saida = retorno["saida_volta"].ToString(),
                };

                var quarto = new Quarto()
                {
                    nome = retorno["nome"].ToString(),
                };

                var idaVolta = new IdaVolta()
                {
                    ida = passagemIda,
                    volta = passagemVolta,                    
                };

                var viagem = new Viagem()
                {
                    idaVolta = idaVolta,
                    ida = null,
                    quarto = quarto,
                };

                var pacote = new Pacote()
                {
                    id = int.Parse(retorno["id_pacote"].ToString()),
                    descricao = retorno["descricao"].ToString(),
                    desconto = int.Parse(retorno["desconto"].ToString()),
                    viagem = viagem,
                };
                pacotes.Add(pacote);
            }
            retorno.Close();
            return pacotes;
        }
    }
}
