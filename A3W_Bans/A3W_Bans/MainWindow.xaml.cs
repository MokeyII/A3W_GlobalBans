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
using System.Windows.Forms;
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
            this.MouseLeftButtonDown += delegate { this.DragMove(); };
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            List<tBan> bans = new List<Classes.tBan>();

            string dbConnection = "datasource=127.0.0.1;port=3306;username=root;password=12345";
            MySqlConnection conDataBase = new MySqlConnection(dbConnection);
            MySqlCommand SelectCommand = new MySqlCommand("select GUID,BanTime,Reason from bans.bans order by BanType asc", conDataBase);

            MySqlDataReader dbReader;
            conDataBase.Open();
            dbReader = SelectCommand.ExecuteReader();

            while (dbReader.Read())
            {
                tBan newBan = new tBan();

                newBan.GuidOrIP = dbReader["GUID"].ToString();
                newBan.BanTime = dbReader["BanTime"].ToString();
                newBan.BanReason = dbReader["Reason"].ToString();

                bans.Add(newBan);

   
            }
            dbReader.Close();
            conDataBase.Close();

            dgBansList.ItemsSource = bans;

        }

        private void btnMainSubmitBan_Click(object sender, RoutedEventArgs e)
        {
            Login SqlLogin = new Login();
            SqlLogin.ShowDialog();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            List<tBan> bans = new List<Classes.tBan>();

            string dbConnection = "datasource=127.0.0.1;port=3306;username=root;password=12345";
            MySqlConnection conDataBase = new MySqlConnection(dbConnection);
            MySqlCommand SelectCommand = new MySqlCommand("select GUID,BanTime,Reason from bans.bans order by BanType asc", conDataBase);

            MySqlDataReader dbReader;
            conDataBase.Open();
            dbReader = SelectCommand.ExecuteReader();

            while (dbReader.Read())
            {
                tBan newBan = new tBan();

                newBan.GuidOrIP = dbReader["GUID"].ToString();
                newBan.BanTime = dbReader["BanTime"].ToString();
                newBan.BanReason = dbReader["Reason"].ToString();

                bans.Add(newBan);


            }
            dbReader.Close();
            conDataBase.Close();

            dgBansList.ItemsSource = bans;
        }

        private void dgBansList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

