using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Reist.Connection;
using MySql.Data.MySqlClient;

namespace API_Reist.Models
{
    public class Viagem
    {
        public int id { get; set; }
        public int quantidade_pessoas { get; set; }
        public int quantidade_quartos { get; set; }
        public string checkin { get; set; }
        public string checkout { get; set; }
        public Quarto quarto { get; set; }
        public Passagem ida { get; set; }
        public IdaVolta idaVolta { get; set; }

        public void InserirViagemIda()
        {
            using (Database DB = new Database())
            {
                var query = "insert into viagem values(default, null, null, " + this.ida.id + ", null, null, " + this.quantidade_pessoas + ", null); select @@identity;";
                MySqlDataReader retorno = DB.ReturnCommand(query);
                retorno.Read();

                int idViagem = int.Parse(retorno["@@identity"].ToString());
                this.id = idViagem;
            }
        }

        public void InserirViagemIdaVolta()
        {
            using (Database DB = new Database())
            {
                var query = "insert into viagem values(default, null, null, " + this.idaVolta.ida.id + ", " + this.idaVolta.volta.id + ", null, " + this.quantidade_pessoas + ", null); select @@identity;";
                MySqlDataReader retorno = DB.ReturnCommand(query);
                retorno.Read();

                int idViagem = int.Parse(retorno["@@identity"].ToString());
                this.id = idViagem;
            }
        }

        public void InserirViagemQuarto()
        {
            using (Database DB = new Database())
            {
                var query = "insert into viagem values(default, '" + this.checkin + " 00:00:00', '" + this.checkout + " 00:00:00', null, null, " + this.quantidade_quartos + ", " + this.quantidade_pessoas + ", " + this.quarto.id + "); select @@identity;";
                MySqlDataReader retorno = DB.ReturnCommand(query);
                retorno.Read();

                int idViagem = int.Parse(retorno["@@identity"].ToString());
                this.id = idViagem;
            }
        }
    }
}
