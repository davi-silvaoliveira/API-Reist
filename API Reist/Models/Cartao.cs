using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Reist.Connection;
using MySql.Data.MySqlClient;

namespace API_Reist.Models
{
    public class Cartao
    {
        public int id { get; set; }
        public string numero { get; set; }
        public string validade { get; set; }
        public string codSeguranca { get; set; }
        public string tipo { get; set; }
        public string bandeira { get; set; }

        public void Inserir()
        {
            using (Database DB = new Database())
            {
                var query = "call cadastrar_cartao(" + this.numero + ", '" + this.validade + "', " + this.codSeguranca + ", '" + this.tipo + "', '" + this.bandeira + "');";
                MySqlCommand cmd = new MySqlCommand(query, DB.connection);
                cmd.ExecuteNonQuery();
                SetId(this.numero);
            }
        }

        public void SetId(string numeroCartao)
        {
            using (Database DB = new Database())
            {
                var query = "select id_cartao from cartao where numero = " + numeroCartao + ";";
                MySqlDataReader retorno = DB.ReturnCommand(query);
                retorno.Read();

                int idCartao = int.Parse(retorno["id_cartao"].ToString());
                this.id = idCartao;
            }
        }
    }
}
