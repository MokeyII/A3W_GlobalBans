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
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using A3W_Bans.Classes;

namespace A3W_Bans
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {

        public Search()
        {
            InitializeComponent();
        }

        public tBan banReponse { get; private set; }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            Classes.tBan bans = new tBan();
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://192.223.30.108:55071/");
                client.BaseAddress = new Uri("http://localhost:55071/");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = null;
                try
                {
                    tBan ban = new Classes.tBan();
                    ban.GuidOrIP = schGUID.Text;

                    string serializedBan = JsonConvert.SerializeObject(ban);

                    StringContent content = new StringContent(serializedBan, Encoding.UTF8, "application/json");

                    response = client.PostAsync("A3Bans/SearchBan", content).Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Player ban does not exist");
                }


                if (response.IsSuccessStatusCode)
                {
                    string strResponse = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        banReponse = JsonConvert.DeserializeObject<Classes.tBan>(strResponse);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                //   banReponse.BanReason = schReason.Text;
                //ID OWNER GUID REASON PROOF
                schBanID.Text = banReponse.BanID;
                schGUID.Text = banReponse.GuidOrIP;
                schReason.Text = banReponse.BanReason;
                schProof.Text = banReponse.Proof;
                //schOwnerID.Text = banReponse.OwnerID;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {


            {
                Login SqlLogin = new Login();
                SqlLogin.ShowDialog();
                {

                    tAuthenticationResponse authenticationResult = new Classes.tAuthenticationResponse();

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:55071/");

                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                        tCredential credential = new Classes.tCredential();
                        credential.Username = SqlLogin.txtSqlUserName.Text;
                        credential.Password = SqlLogin.txtSqlPassword.Password;

                        string serializedBan = JsonConvert.SerializeObject(credential);

                        StringContent content = new StringContent(serializedBan, Encoding.UTF8, "application/json");


                        HttpResponseMessage response = client.PostAsync("A3Bans/LogIn", content).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            string strResponse = response.Content.ReadAsStringAsync().Result;
                            authenticationResult = JsonConvert.DeserializeObject<tAuthenticationResponse>(strResponse);


                        }

                    }

                    if (authenticationResult.AuthenticationSuccess)
                    {
                        this.Hide();
                        Classes.tBan bans = new tBan();
                        using (var client = new HttpClient())
                        {
                            //client.BaseAddress = new Uri("http://192.223.30.108:55071/");
                            client.BaseAddress = new Uri("http://localhost:55071/");

                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage response = null;
                            try
                            {
                                tBan ban = new Classes.tBan();
                                ban.GuidOrIP = schGUID.Text;

                                string serializedBan = JsonConvert.SerializeObject(ban);

                                StringContent content = new StringContent(serializedBan, Encoding.UTF8, "application/json");

                                response = client.PostAsync("A3Bans/deleteBan", content).Result;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Player ban does not exist");
                            }


                            if (response.IsSuccessStatusCode)
                            {
                                string strResponse = response.Content.ReadAsStringAsync().Result;
                                try
                                {
                                    banReponse = JsonConvert.DeserializeObject<Classes.tBan>(strResponse);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show(authenticationResult.Message);
                    }
                }
            }
        }
    }
}
    
