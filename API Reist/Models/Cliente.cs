using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Reist.Connection;
using MySql.Data.MySqlClient;

namespace API_Reist.Models
{
    public class Cliente
    {
        public long cpf { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public long celular { get; set; }
        public string sexo { get; set; }
        //public DateTime nascimento { get; set; }
        //public string nascimento { get; set; }
        public Endereco endereco { get; set; }

        public List<Cliente> ListarClientes()
        {
            using (Database DB = new Database())
            {
                var query = "select cpf, nome, email, senha, celular, sexo, vw.cep, vw.logradouro, vw.bairro, vw.cidade, "+
                    "vw.estado from cliente inner join vw_listar_enderecos as vw where vw.cep = endereco;";
                var retorno = DB.ReturnCommand(query);
                return Listar(retorno);
            }
        }

        public List<Cliente> Listar(MySqlDataReader retorno)
        {
            var clientes = new List<Cliente>();
            //var enderec = new Endereco();
            while (retorno.Read())
            {
                var enderecoObj = new Endereco()
                {
                    cep = retorno["cep"].ToString(),
                    uf = retorno["estado"].ToString(),
                    cidade = retorno["cidade"].ToString(),
                    bairro = retorno["bairro"].ToString(),
                    logradouro = retorno["logradouro"].ToString(),
                    
                };

                var cliente = new Cliente()
                {
                    endereco = enderecoObj,
                    nome = retorno["nome"].ToString(),
                    cpf = long.Parse(retorno["cpf"].ToString()),
                    email = retorno["email"].ToString(),
                    senha = retorno["senha"].ToString(),
                    celular = long.Parse(retorno["celular"].ToString()),
                    sexo = retorno["sexo"].ToString(),
                };
                clientes.Add(cliente);
            }
            retorno.Close();
            return clientes;
        }
    }
}
