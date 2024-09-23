using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace criptografar
{
    public class Sql
    {
        public static SqlConnection conector = new SqlConnection(@"Data Source=MURILOIOSSI\IOSSI; Initial Catalog = CRIPTOGRAFAR_SENHA; Integrated Security = true");

        public static string criptografado;
        public static string resultado;
        public static string verificado;
    }
}
