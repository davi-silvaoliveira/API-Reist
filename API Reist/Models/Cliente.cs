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
    public class Cliente
    {
        public string cpf { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public string celular { get; set; }
        public string sexo { get; set; }
        //public DateTime nascimento { get; set; }
        public string nascimento { get; set; }
        public string numero { get; set; }
        public Endereco endereco { get; set; }
        public Cliente() { }

        public void Insert()
        {
            Hash hash = new Hash(SHA512.Create());

            using (Database DB = new Database())
            {
                var query = string.Format("call cadastrar_cliente({0}, '{1}', '{2}', '{3}', '{4}', '{5}', str_to_date('{6}', '%d/%m/%Y'), {7}, '{8}', " +
                    "'{9}', '{10}', '{11}','{12}');", this.cpf, this.nome, this.email, hash.Criptografar(this.senha), this.celular,this.sexo, this.nascimento,
                    this.endereco.cep, this.endereco.logradouro, this.endereco.bairro, this.endereco.cidade, this.endereco.uf, this.numero);

                MySqlCommand cmd = new MySqlCommand(query, DB.connection);
                cmd.ExecuteNonQuery();

                //return this;
            }
        }

        public List<Cliente> ListarClientes()
        {
            using (Database DB = new Database())
            {
                var query = "select cpf, nome, email, senha, celular, sexo, vw.cep, vw.logradouro, vw.bairro, vw.cidade, " +
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
                    //cpf = long.Parse(retorno["cpf"].ToString()),
                    email = retorno["email"].ToString(),
                    senha = retorno["senha"].ToString(),
                    //celular = long.Parse(retorno["celular"].ToString()),
                    sexo = retorno["sexo"].ToString(),
                };
                clientes.Add(cliente);
            }
            retorno.Close();
            return clientes;
        }

        /*public bool Autenticar()
        {
            using (Database database = new Database())
            {
                Hash hash = new Hash(SHA512.Create());
                //this.senha = hash.Criptografar(this.senha);

                string command = string.Format("select * from cliente where email = '{0}';", this.email);
                MySqlDataReader reader = database.ReturnCommand(command);
                reader.Read();

                if (reader.HasRows)
                {
                    return hash.Verificar(this.senha, reader["senha"].ToString());
                }
                else
                {
                    command = string.Format("select * from funcionario where email = '{0}';", this.email);
                    reader = database.ReturnCommand(command); reader.Read();
                    if (reader.HasRows)
                    {
                        this.nivel = int.Parse(reader["acesso"].ToString());
                        return hash.Verificar(this.senha, reader["senha"].ToString());
                    }
                    else
                        reader.Close(); return false;
                }
            }*/


        public class Hash
        {
            private HashAlgorithm algorithm;
            public Hash(HashAlgorithm algorithm)
            {
                this.algorithm = algorithm;
            }

            //
            public string Criptografar(string senha)
            {
                var codificado = Encoding.UTF8.GetBytes(senha);
                var criptografado = this.algorithm.ComputeHash(codificado);

                var builder = new StringBuilder();
                foreach (var caracter in criptografado)
                {
                    builder.Append(caracter.ToString("X2"));
                }

                return builder.ToString();
            }

            //
            public bool Verificar(string senhaDigitada, string senhaCodificada)
            {
                return this.Criptografar(senhaDigitada) == senhaCodificada;
            }
        }
    }
}

