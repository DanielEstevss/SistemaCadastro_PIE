using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCadastro_PIE
{
    internal class Pessoa
    {
        private string _nome;
        private string _dataNascimento;
        private char _sexo;
        private string _cpf;
        private string _email;
        private string _telefone;
        private string _endereco;

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public string DataNascimento
        {
            get { return _dataNascimento; }
            set { _dataNascimento = value; }
        }

        public char Sexo
        {
            get { return _sexo; }
            set { _sexo = value; }
        }

        public string Cpf
        {
            get { return _cpf; }
            set { _cpf = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Telefone
        {
            get { return _telefone; }
            set { _telefone = value; }
        }

        public string Endereco
        {
            get { return _endereco; }
            set { _endereco = value; }
        }
    }
}
