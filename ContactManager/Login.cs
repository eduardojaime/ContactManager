using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Step 1 Import ADO.NET controls
using System.Data.SqlClient;
// Also import data controls: DataTable
using System.Data;

namespace ContactManager
{
    public partial class Login : Form
    {
        string connectionString = "Server=ibtcontactsed.mssql.somee.com;Database=ibtcontactsed;User Id=ibtcollege_SQLLogin_1;Password=q1r2pjjpzo;";
        public Login()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            try
            {
                // OPEN THE CONNECTION
                myConnection.Open();
                // PERFORM THE COMMAND
                SqlCommand myCommand = new SqlCommand();
                myCommand.CommandText = String.Format(
                    "SELECT * FROM Login WHERE UserId ='{0}' AND Password='{1}'",
                    txtUsername.Text.Trim().Replace("\"", "").Replace(";", ""),
                    txtPassword.Text.Trim().Replace("\"", "").Replace(";", "")
                    );
                myCommand.Connection = myConnection;

                SqlDataReader myDataReader = myCommand.ExecuteReader();
                if (myDataReader.Read())
                {
                    // Set the data you want to pass
                    myInfo infoFromLogin = new myInfo();
                    infoFromLogin.loginMessage = "Welcome!! You are " + txtUsername.Text;

                    // Pass it on!
                    Main newForm = new Main(infoFromLogin);
                    newForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Not found!");
                }
            }
            catch (Exception ex)
            {
                // CATCH ANY ERRORS AND DEBUG
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // ALWAYS CLOSE THE CONNECTION
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }
    }
}
