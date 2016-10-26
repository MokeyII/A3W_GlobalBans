using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace A3W_Bans
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            txtSqlPassword.MaxLength = 16;
        }

        private void btnSqlLogin_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string myConnection = "datasource=127.0.0.1;port=3306;username=root;password=12345";
                MySqlConnection myConn = new MySqlConnection(myConnection);
                MySqlCommand SelectCommand = new MySqlCommand("select * from bans.admins where Username = '" + this.txtSqlUserName.Text + "'and Password= '" + this.txtSqlPassword.Text + "' ;", myConn);
                MySqlDataReader myReader;
                myConn.Open();
                myReader = SelectCommand.ExecuteReader();
                int count = 0;
                while (myReader.Read())
                {
                    count = count + 1;
                }
                if (count == 1)
                {
                    MessageBox.Show("You Have Succesfully Logged Into A3W Bans Database, Please remember all of your bans are subject for review.");
                    this.Hide();
                    SubmitBan SubBan = new SubmitBan();
                    SubBan.ShowDialog();
                }
                else if (count > 1)
                {
                    MessageBox.Show("Duplicate Username and Password!");
                }
                else
                    MessageBox.Show("Incorrect Username or Password!");
                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
