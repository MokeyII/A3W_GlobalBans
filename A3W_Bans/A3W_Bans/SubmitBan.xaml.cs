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
    /// Interaction logic for SubmitBan.xaml
    /// </summary>
    public partial class SubmitBan : Window
    {
        public SubmitBan()
        {
            InitializeComponent();
            txtID.MaxLength = 32;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string constring = "datasource=127.0.0.1;port=3306;username=root;password=12345;database=bans";
            string Query = "insert into bans (GUID, BanTime, Reason, Proof,Bantype) Values('" + this.txtID.Text + "','" + this.txtBan.Text + "','" + this.txtReason.Text + "','" + this.txtProof.Text + "','" + this.cmbBanType.Text + "') ;";
            MySqlConnection conDataBase = new MySqlConnection(constring);
            MySqlCommand cmdDataBase = new MySqlCommand(Query, conDataBase);
            MySqlDataReader myReader;

            if (txtID.Text.Length < 32)
            {
                MessageBox.Show("Invalid GUID");
                return;
            }
            if (txtBan.Text.Length < 2)
            {
                MessageBox.Show("Please enter ''-1'' under ban time to ensure a permanant ban");
                return;
            }
            if (txtReason.Text.Length < 10)
            {
                MessageBox.Show("Please Enter a Valid Reason, Ensure you add your Appeal Process to the reason. I.E. ''Hacking | Appeal your ban @ www.yoursitehere.com''");
                return;
            }
            if (txtProof.Text.Length < 10)
            {
                MessageBox.Show("Please Enter Valid Proof, I.E. Script Logs, Youtube Link, Sound File Link, etc...");
                return;
            }
            else
                try
                {
                    string str1 = "Banning this Player may result in the player NOT being able to play on many servers!@@If Bans are falsified, your user and all bans applied by you are subject to be removed from the database which will result in all bans being removed from any server using this tool.@@Are you sure you want to do this?";
                    str1 = str1.Replace("@", " " + System.Environment.NewLine);
                    var confirmResult = MessageBox.Show(str1, "CONFIRM BAN!!", MessageBoxButton.YesNo);
                    if (confirmResult == MessageBoxResult.Yes)
                    {
                        conDataBase.Open();
                        myReader = cmdDataBase.ExecuteReader();
                        MessageBox.Show("Ban Submitted and Applied!");
                        while (myReader.Read())
                        {

                        }
                        conDataBase.Close();
                        return;
                    }
                    else
                    {
                        return;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }
    }
}
