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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using A3W_Bans.Classes;

namespace A3W_Bans
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            //HERE
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
      
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            List<tBan> bans = new List<Classes.tBan>();

            string myConnection = "datasource=127.0.0.1;port=3306;username=root;password=12345";
            MySqlConnection myConn = new MySqlConnection(myConnection);
            MySqlCommand SelectCommand = new MySqlCommand("select GUID,BanTime,Reason from bans.bans order by BanType asc", myConn);

            MySqlDataReader myReader;
            myConn.Open();
            myReader = SelectCommand.ExecuteReader();

            while (myReader.Read())
            {
                tBan newBan = new tBan();

                newBan.GuidOrIP = myReader["GUID"].ToString();
                newBan.BanTime = myReader["BanTime"].ToString();
                newBan.BanReason = myReader["Reason"].ToString();
               // newBan.BanType = myReader["BanType"].ToString();
               // newBan.BanProof = myReader["Proof"].ToString();

                bans.Add(newBan);
              

            }
            myReader.Close();
            myConn.Close();

            dgBansList.ItemsSource = bans;


        }

        private void btnMainSubmitBan_Click(object sender, RoutedEventArgs e)
        {
            Login SqlLogin = new Login();
            SqlLogin.ShowDialog();
        }
    }
}
