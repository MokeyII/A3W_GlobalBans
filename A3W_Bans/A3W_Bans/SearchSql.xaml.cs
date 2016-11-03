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
using A3W_Bans.Classes;

namespace A3W_Bans
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class SearchSql : Window
    {
        public SearchSql()
        {
            InitializeComponent();
        }
       
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string constring = "datasource=192.223.30.108;port=3306;username=banadmin;password=12345;database=a3bans";
            string selectQuery = "DELETE FROM `bans` WHERE `bans`.`GUID` = ('" + this.txtGUID.Text + "') ;";
            MySqlConnection conDataBase = new MySqlConnection(constring);
            MySqlCommand cmdDataBase = new MySqlCommand(selectQuery, conDataBase);
            MySqlDataReader dbReader;


            dbReader = cmdDataBase.ExecuteReader();
            if (dbReader.Read())
            {
                txtReason.Text = dbReader.GetString("Reason");
                txtProof.Text = dbReader.GetString("Proof");
            }
                

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
