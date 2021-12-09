using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Reist.Connection;
using MySql.Data.MySqlClient;

namespace API_Reist.Models
{
    public class Local
    {
        public string nome { get; set; }
        public string sigla { get; set; }
        public Endereco endereco { get; set; }
        public Local() { }

        public Local BuscarPorCidade(string nome)
        {
            using (Database database = new Database())
            {
                string command = "select * from vw_buscar_locais where cidade like '%" + nome + "%';";
                MySqlDataReader reader = database.ReturnCommand(command);
                reader.Read();

                if (reader.HasRows)
                {

                    this.nome = reader["nome_local"].ToString();
                    this.sigla = reader["sigla"].ToString();

                    Endereco enderecoObj = new Endereco();

                    enderecoObj.cep = reader["cep"].ToString();
                    enderecoObj.uf = reader["estado"].ToString();
                    enderecoObj.cidade = reader["cidade"].ToString();
                    enderecoObj.bairro = reader["bairro"].ToString();
                    enderecoObj.logradouro = reader["logradouro"].ToString();
                    /*var numero = reader["numero_endereco"];

                    if (numero != null)
                        enderecoObj.numero = int.Parse(reader["numero_endereco"].ToString());
                    else
                        enderecoObj.numero = 0;*/

                    this.endereco = enderecoObj;
                    return this;

                }
                else
                {
                    return null;
                }
            }
        }

        public Local Buscar(string nome)
        {
            using (Database database = new Database())
            {
                string command = "select * from vw_buscar_locais where nome_local like '%"+nome+"%';";
                MySqlDataReader reader = database.ReturnCommand(command);
                reader.Read();

                if (reader.HasRows)
                {

                    this.nome = reader["nome_local"].ToString();
                    this.sigla = reader["sigla"].ToString();

                    Endereco enderecoObj = new Endereco();

                    enderecoObj.cep = reader["cep"].ToString();
                    enderecoObj.uf = reader["estado"].ToString();
                    enderecoObj.cidade = reader["cidade"].ToString();
                    enderecoObj.bairro = reader["bairro"].ToString();
                    enderecoObj.logradouro = reader["logradouro"].ToString();
                    /*var numero = reader["numero_endereco"];

                    if (numero != null)
                        enderecoObj.numero = int.Parse(reader["numero_endereco"].ToString());
                    else
                        enderecoObj.numero = 0;*/

                    this.endereco = enderecoObj;
                    return this;

                }
                else
                    return BuscarPorCidade(nome);                
            }
        }
    }
}
