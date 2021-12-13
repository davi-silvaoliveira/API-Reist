using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Reist.Connection;
using MySql.Data.MySqlClient;

namespace API_Reist.Models
{
    public class Compra
    {
        public int id { get; set; }
        public int parcelas { get; set; }
        public string dataPagamento { get; set; }
        public Pacote pacote { get; set; }
        public Viagem viagem { get; set; }

        public void ComprarPacote()
        {
            using (Database DB = new Database())
            {
                var query = string.Format("call cadastrar_cliente({0}, '{1}', '{2}', '{3}', '{4}', '{5}', str_to_date('{6}', '%d/%m/%Y'), {7}, '{8}', " +
                    "'{9}', '{10}', '{11}','{12}');", this.cpf, this.nome, this.email, hash.Criptografar(this.senha), this.celular, this.sexo, this.nascimento,
                    this.endereco.cep, this.endereco.logradouro, this.endereco.bairro, this.endereco.cidade, this.endereco.uf, this.endereco.numero);

                MySqlCommand cmd = new MySqlCommand(query, DB.connection);
                cmd.ExecuteNonQuery();

                //return this;
            }
        }

        public void ComprarHospedagem()
        {

        }

        public void ComprarIda()
        {

        }

        public void ComprarIdaVolta()
        {

        }
    }
}
