using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using A3W_Bans.Classes;
using System.Web;
using System.IO;

namespace A3W_Bans
{
    /// <summary>
    /// Interaction logic for SubmitBan.xaml
    /// </summary>
    /// 

    public partial class SubmitBan : Window
    {
        public SubmitBan()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += delegate { this.DragMove(); };
            txtID.MaxLength = 32;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
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
            // if ((guidMatch.Success && charMatch) || ipMatch.Success)
            var ipMatch = Regex.Match(txtID.Text, @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            var guidMatch = Regex.Match(txtID.Text, @"[a-z,A-Z,0-9]{32}");
            var charMatch = (txtID.Text.Any(Char.IsLetter) && txtID.Text.Any(Char.IsDigit));

            if ((guidMatch.Success && charMatch) || ipMatch.Success)
            {
                string str1 = "Banning this Player may result in the player NOT being able to play on many servers!@@If Bans are falsified, your user and all bans applied by you are subject to be removed from the database which will result in all bans being removed from any server using this tool.@@Are you sure you want to do this?";
                str1 = str1.Replace("@", " " + System.Environment.NewLine);
                var confirmResult = MessageBox.Show(str1, "CONFIRM BAN!!", MessageBoxButton.YesNo);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Sending response to WEB API");
                }

                Classes.tBan authenticationResult = new Classes.tBan();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:55071/");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    tBan submit = new Classes.tBan();
                    submit.GuidOrIP = txtID.Text;
                    submit.BanType = cmbBanType.Text;
                    submit.BanTime = txtBan.Text;
                    submit.BanReason = txtReason.Text;
                    submit.Proof = txtProof.Text;
                    submit.Proof= System.Net.WebUtility.HtmlEncode(submit.Proof);
                    //submit.Proof = submit.Proof.Replace("quot;", "    ");
                    //submit.Proof = submit.Proof.Replace("&lt;", "<");
                    //submit.Proof = submit.Proof.Replace("&", "``");
                    //submit.Proof = submit.Proof.Replace("gt;", ">");
                    //submit.Proof = submit.Proof.Replace("#39;", "|");
                    submit.Proof = submit.Proof.Replace(System.Environment.NewLine, "");


                    //Json convert takes a C# object/class and makes a Json string, server automatically takes the json string and gives you back a C# object
                    //Seamless integration that PHP cant do, yep
                    //This is just a helper library
                    string serializedBan = JsonConvert.SerializeObject(submit);

                    //application/json is content type
                    StringContent content = new StringContent(serializedBan, Encoding.UTF8, "application/json");
                    try
                    {
                        HttpResponseMessage response = client.PostAsync("A3Bans/SubmitBan", content).Result;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        MessageBox.Show("Your Ban Has Succesfully Posted");
                    }

                }
            }
        }

        private void btnCncl_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
