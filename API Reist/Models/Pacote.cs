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
        public int id_viagem { get; set; }
        public string descricao { get; set; }
        public int desconto { get; set; }
        public float precoFinal { get; set; }
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

        public Pacote Buscar(int id)
        {
            using (Database database = new Database())
            {
                string command = "select * from vw_listar_pacotes where id_pacote = " + id + "";
                MySqlDataReader retorno = database.ReturnCommand(command);
                retorno.Read();

                var passagemIda = new Passagem()
                {
                    id = int.Parse(retorno["ida"].ToString()),
                    saida = retorno["saida_ida"].ToString(),
                };

                var passagemVolta = new Passagem()
                {
                    id = int.Parse(retorno["volta"].ToString()),
                    saida = retorno["saida_volta"].ToString(),
                };

                var quarto = new Quarto()
                {
                    id = int.Parse(retorno["id_hotel"].ToString()),
                    nome = retorno["nome"].ToString(),
                };

                passagemIda = passagemIda.BuscarPassagem(passagemIda.id);
                passagemVolta = passagemVolta.BuscarPassagem(passagemVolta.id);
                quarto = quarto.BuscarQuarto(quarto.id);

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
                    id_viagem = int.Parse(retorno["id_viagem"].ToString()),
                    id = int.Parse(retorno["id_pacote"].ToString()),
                    descricao = retorno["descricao"].ToString(),
                    desconto = int.Parse(retorno["desconto"].ToString()),
                    viagem = viagem,
                };

                
                return pacote;
            }
        }

        public List<Pacote> Listar(MySqlDataReader retorno)
        {
            var pacotes = new List<Pacote>();
            while (retorno.Read())
            {
                var passagemIda = new Passagem()
                {
                    id = int.Parse(retorno["ida"].ToString()),
                    saida = retorno["saida_ida"].ToString(),
                };

                var passagemVolta = new Passagem()
                {
                    id = int.Parse(retorno["volta"].ToString()),
                    saida = retorno["saida_volta"].ToString(),
                };

                var quarto = new Quarto()
                {
                    id = int.Parse(retorno["id_hotel"].ToString()),
                    nome = retorno["nome"].ToString(),
                };

                passagemIda = passagemIda.BuscarPassagem(passagemIda.id);
                passagemVolta = passagemVolta.BuscarPassagem(passagemVolta.id);
                quarto = quarto.BuscarQuarto(quarto.id);

                var idaVolta = new IdaVolta()
                {
                    ida = passagemIda,
                    volta = passagemVolta,                    
                };

                idaVolta.PrecoIdaVolta(int.Parse(retorno["quantidade_pessoas"].ToString()));
                quarto.CalcularPreco(int.Parse(retorno["quantidade_quartos"].ToString()));

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

                pacote.CalcularPreco();

                pacote.CalcularPreco();
                pacotes.Add(pacote);
            }
            retorno.Close();
            return pacotes;
        }

        public void CalcularPreco(/*int pessoas, int quartos*/)
        {
            float total = viagem.idaVolta.precoFinal + viagem.quarto.precoTotal;
            var descon = (this.desconto / 100) * total;
            total = total - descon;
            this.precoFinal = total;
        }
    }
}
