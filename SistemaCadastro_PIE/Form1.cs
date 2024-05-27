using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SistemaCadastro_PIE
{
    public partial class Form1 : Form
    {
        SqlConnection conexao;

        ConnectionString acesso = new ConnectionString();

        private int idItemEscolhido;
        public Form1()
        {
            InitializeComponent();

            listCadastros.View = View.Details;
            listCadastros.LabelEdit = true;
            listCadastros.AllowColumnReorder = true;
            listCadastros.FullRowSelect = true;
            listCadastros.GridLines = true;

            listCadastros.Columns.Add("Id", 30, HorizontalAlignment.Left);
            listCadastros.Columns.Add("Nome", 150, HorizontalAlignment.Left);
            listCadastros.Columns.Add("Data de Nascimento", 90, HorizontalAlignment.Left);
            listCadastros.Columns.Add("Sexo", 30, HorizontalAlignment.Left);
            listCadastros.Columns.Add("CPF", 100, HorizontalAlignment.Left);
            listCadastros.Columns.Add("Email", 150, HorizontalAlignment.Left);
            listCadastros.Columns.Add("Telefone", 100, HorizontalAlignment.Left);
            listCadastros.Columns.Add("Endereço", 150, HorizontalAlignment.Left);

            
        }

        // Método associado ao botão "Cadastrar"
        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            // Verifica se os campos estão devidamente preenchidos
            if (txtNome.Text == "")
            {
                MessageBox.Show("Preencha o campo Nome");
                txtNome.Focus();
                return;
            }

            if (txtDataNascimento.Text == "  /  /")
            {
                MessageBox.Show("Preencha o campo Data de Nascimento");
                txtDataNascimento.Focus();
                return;
            }

            if (txtCPF.Text == "   .   .   -")
            {
                MessageBox.Show("Preencha o campo CPF");
                txtCPF.Focus();
                return;
            }

            if (txtEmail.Text == "")
            {
                MessageBox.Show("Preencha o campo Email");
                txtEmail.Focus();
                return;
            }

            if (txtTelefone.Text == "(  )      -")
            {
                MessageBox.Show("Preencha o campo Telefone");
                txtTelefone.Focus();
                return;
            }

            if (txtEndereco.Text == "")
            {
                MessageBox.Show("Preencha o campo Endereço");
                txtEndereco.Focus();
                return;
            }

            // Verifica qual opçãp do campo "Sexo" foi Marcado
            char sexoEscolhido;

            if (radioMasculino.Checked)
            {
                sexoEscolhido = 'M';
            }

            else if (radioFeminino.Checked)
            {
                sexoEscolhido = 'F';
            }

            else
            {
                sexoEscolhido = 'O';
            }


            // Cria um objeto do tipo Pessoa
            Pessoa pessoa = new Pessoa();

            pessoa.Nome = txtNome.Text;
            pessoa.DataNascimento = txtDataNascimento.Text;
            pessoa.Sexo = sexoEscolhido;
            pessoa.Cpf = txtCPF.Text;
            pessoa.Email = txtEmail.Text;
            pessoa.Telefone = txtTelefone.Text;
            pessoa.Endereco = txtEndereco.Text;


            // Inserção de cadastro no banco de dados                   
            try
            {
                conexao = new SqlConnection(acesso.connectionstring);

                string query = "INSERT INTO Cadastros (Nome, DataNascimento, Sexo, Cpf, Email, Telefone, Endereco) " +
                               "VALUES ('" + pessoa.Nome + "', '" + pessoa.DataNascimento + "', '" + pessoa.Sexo + "', '" + pessoa.Cpf + "', '" + pessoa.Email + "' , '" + pessoa.Telefone + "' , '" + pessoa.Endereco + "')";

                SqlCommand comando = new SqlCommand(query, conexao);

                conexao.Open();

                comando.ExecuteReader();

                MessageBox.Show("Cadastro inserido com sucesso no Banco de Dados");
            }

            catch (Exception)
            {
                MessageBox.Show("Erro ao inserir pessoa no Banco de Dados");
            }

            finally
            {
                conexao.Close();
            }

            // Método que limpa os campos após o cadastro
            LimparCampos();
        }


        // Evento associado a busca de cadastros no banco de dados
        private void btnBuscar_Click(object sender, EventArgs e)
        {            
            try
            {
                string busca = "%" + txtBusca.Text + "%";

                conexao = new SqlConnection(acesso.connectionstring);

                string query = "SELECT * " +
                               "FROM Cadastros " +
                               "WHERE Nome LIKE @busca OR Endereco LIKE @busca";

                SqlCommand comando = new SqlCommand(query, conexao);

                comando.Parameters.AddWithValue("@busca", busca);

                conexao.Open();

                SqlDataReader reader = comando.ExecuteReader();

                listCadastros.Items.Clear();

                while (reader.Read())
                {
                    string[] linha =
                    {
                        reader.GetInt32(0).ToString(),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.GetString(5),
                        reader.GetString(6),
                        reader.GetString(7)
                    };

                    var linhaListview = new ListViewItem(linha);

                    listCadastros.Items.Add(linhaListview);
                }
            }

            catch (Exception)
            {
                MessageBox.Show("Erro na busca");
            }

            finally
            {
                conexao.Close();
            }
        }


        // Evento responsável por preencher os campos com as informações de cadastro presentes na list
        private void listCadastros_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itensSelecionados = listCadastros.SelectedItems;

            foreach (ListViewItem item in itensSelecionados)
            {
                idItemEscolhido = int.Parse(item.SubItems[0].Text);

                txtNome.Text = item.SubItems[1].Text;

                txtDataNascimento.Text = item.SubItems[2].Text;

                switch (item.SubItems[3].Text)
                {
                    case "M":
                        radioMasculino.Checked = true;
                        break;

                    case "F":
                        radioFeminino.Checked = true;
                        break;

                    case "O":
                        radioOutro.Checked = true;
                        break;
                }

                txtCPF.Text = item.SubItems[4].Text;

                txtEmail.Text = item.SubItems[5].Text;

                txtTelefone.Text = item.SubItems[6].Text;

                txtEndereco.Text = item.SubItems[7].Text;
            }
        }


        // Evento associado a atualização de informações do cadastro no banco de dados
        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            char sexoEscolhido;

            if (radioMasculino.Checked)
            {
                sexoEscolhido = 'M';
            }

            else if (radioFeminino.Checked)
            {
                sexoEscolhido = 'F';
            }

            else
            {
                sexoEscolhido = 'O';
            }

            

            try
            {
                using (conexao = new SqlConnection(acesso.connectionstring))
                {
                    conexao.Open();

                    string query = "UPDATE Cadastros SET " +
                                   "Nome = @Nome, DataNascimento = @DataNascimento, Sexo = @Sexo, Cpf = @Cpf, Email = @Email, Telefone = @Telefone, Endereco = @Endereco " +
                                   "WHERE Id = @Id";


                    using (SqlCommand comando = new SqlCommand(query, conexao))
                    {
                        comando.Parameters.AddWithValue("@Nome", txtNome.Text);
                        comando.Parameters.AddWithValue("@DataNascimento", txtDataNascimento.Text);
                        comando.Parameters.AddWithValue("@Sexo", sexoEscolhido);
                        comando.Parameters.AddWithValue("@Cpf", txtCPF.Text);
                        comando.Parameters.AddWithValue("@Email", txtEmail.Text);
                        comando.Parameters.AddWithValue("@Telefone", txtTelefone.Text);
                        comando.Parameters.AddWithValue("@Endereco", txtEndereco.Text);
                        comando.Parameters.AddWithValue("@Id", idItemEscolhido);


                        int linhasAfetadas = comando.ExecuteNonQuery();

                        listCadastros.Items.Clear();

                        LimparCampos();

                        MessageBox.Show("Informações atualizadas com sucesso");
                    }
                }
            } 
     
            catch (Exception)
            {
                MessageBox.Show("Erro ao atualizar as informações");
            }

            finally
            {
                conexao.Close();
            }
    }


        // Evento responsável por excluir cadastros do banco de dados
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult confirmacaoExclusão = MessageBox.Show("Tem certeza que deseja excluir esse cadastro ?", "Observação", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if(confirmacaoExclusão == DialogResult.Yes)
                {                   
                    using (conexao = new SqlConnection(acesso.connectionstring))
                    {
                        conexao.Open();

                        string query = "DELETE FROM Cadastros WHERE Id = @Id";


                        using (SqlCommand comando = new SqlCommand(query, conexao))
                        {
                            
                            comando.Parameters.AddWithValue("@Id", idItemEscolhido);


                            int linhasAfetadas = comando.ExecuteNonQuery();

                            listCadastros.Items.Clear();

                            LimparCampos();

                            MessageBox.Show("Cadastro excluído com sucesso");
                        }
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show("Erro ao excluir cadastro");
            }

            finally
            {
                conexao.Close();
            }
        }


        // Método para limpar os campos
        private void LimparCampos()
        {
            txtNome.Text = "";
            txtDataNascimento.Text = "";
            radioMasculino.Checked = true;
            radioFeminino.Checked = false;
            radioFeminino.Checked = false;
            txtCPF.Text = "";
            txtEmail.Text = "";
            txtTelefone.Text = "";
            txtEndereco.Text = "";

            txtNome.Focus();
        }
    }
}
