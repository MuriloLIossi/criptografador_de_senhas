using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace criptografar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txtResult.Enabled = false;
            txtResult.ReadOnly = true;
        }

        private void txtResult_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCriptografar_Click(object sender, EventArgs e)
        {
            try
            {
                Sql.conector.Open();

                string senha = txtSenha.Text;
                string senhaEncript = BCrypt.Net.BCrypt.HashPassword(senha, workFactor: 12);

                string insert = $"INSERT INTO senha(senha_string, senha_numeric) VALUES (@senha, @senhaEncript)";
                using (SqlCommand cmd = new SqlCommand(insert, Sql.conector))
                {
                    cmd.Parameters.AddWithValue("@senha", senha);
                    cmd.Parameters.AddWithValue("@senhaEncript", senhaEncript);

                    cmd.ExecuteNonQuery();
                    Sql.conector.Close();
                }

                Sql.conector.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            string senha = txtSenha.Text;
            try
            {
                Sql.conector.Open();

                string query = "SELECT senha_numeric FROM senha WHERE senha_string = @senha";
                using (SqlCommand cmd = new SqlCommand(query, Sql.conector))
                {
                    cmd.Parameters.AddWithValue("@senha", senha);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Sql.verificado = reader["senha_numeric"].ToString();
                        Sql.conector.Close();
                    }
                    else
                    {
                        MessageBox.Show("Senha não encontrada");
                        Sql.conector.Close();
                    }
                }

                if (Sql.verificado != null)
                {
                    bool senhaCorreta = BCrypt.Net.BCrypt.Verify(txtSenha.Text, Sql.verificado);
                    if (senhaCorreta)
                    {
                        MessageBox.Show("Senha correta");
                        txtResult.Text = Sql.verificado;
                    }
                    else
                    {
                        MessageBox.Show("Senha incorreta");
                    }

                    Sql.conector.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
