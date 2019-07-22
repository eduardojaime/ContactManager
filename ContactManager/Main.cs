using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace ContactManager
{
    public partial class Main : Form
    {
        myInfo userInfo;
        string connectionString = "Server=ibtcontactsed.mssql.somee.com;Database=ibtcontactsed;User Id=ibtcollege_SQLLogin_1;Password=q1r2pjjpzo;";
        public Main(myInfo infoFromLogin)
        {
            InitializeComponent();
            userInfo = infoFromLogin;
            // Example of use
            lblMessage.Text = userInfo.loginMessage;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            GetContacts();
        }

        private void GetContacts()
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            try
            {
                myConnection.Open();
                // Step 1 Define query
                string query = "SELECT * FROM Contact ORDER BY LastName";
                // Step 2 Create and initialize command object
                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandText = query;
                // Step 3 Create datatable that will contain the data
                DataTable myContacts = new DataTable();
                myContacts.Columns.Add("ID");
                myContacts.Columns.Add(new DataColumn("FirstName"));
                myContacts.Columns.Add(new DataColumn("LastName"));
                myContacts.Columns.Add(new DataColumn("Phone"));
                myContacts.Columns.Add(new DataColumn("Email"));
                // Step 4 Fill in the datatable with information from the database
                SqlDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    DataRow contact = myContacts.NewRow();
                    contact["ID"] = myReader["ID"];
                    contact["FirstName"] = myReader["FirstName"];
                    contact["LastName"] = myReader["LastName"];
                    contact["Phone"] = myReader["Phone"];
                    contact["Email"] = myReader["Email"];
                    myContacts.Rows.Add(contact);
                }
                // Step 5 Bind datatable to datagridview
                dgContacts.DataSource = myContacts;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // Basic validation
            txtFirstName.Text = txtFirstName.Text.Trim().Replace(";", "");
            txtLastName.Text = txtLastName.Text.Trim().Replace(";", "");
            txtEmailAddr.Text = txtEmailAddr.Text.Trim().Replace(";", "");
            txtPhoneNbr.Text = txtPhoneNbr.Text.Trim().Replace(";", "");

            if (txtFirstName.Text == string.Empty)
            {
                MessageBox.Show("Please enter a name.");
                return;
            }
            if (txtLastName.Text == string.Empty)
            {
                MessageBox.Show("Please enter a last name.");
                return;
            }
            if (txtEmailAddr.Text == string.Empty)
            {
                MessageBox.Show("Please enter an email address.");
                return;
            }

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            try
            {
                myConnection.Open();
                string query = "INSERT INTO Contact VALUES ('{0}', '{1}', '{2}', '{3}')";
                query = String.Format(query,
                    txtFirstName.Text,
                    txtLastName.Text,
                    txtPhoneNbr.Text,
                    txtEmailAddr.Text);
                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandText = query;
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
            txtFirstName.Text = string.Empty;
            txtEmailAddr.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtPhoneNbr.Text = string.Empty;
            GetContacts();
        }
    }
}
