using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http;
using A3W_Bans.Classes;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace A3W_Bans
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //DOnt make this static
        HttpClient client = new HttpClient();
        public MainWindow()
        {

            //HERE
            InitializeComponent();
            this.MouseLeftButtonDown += delegate { this.DragMove(); };
            this.Loaded += MainWindow_Loaded;
        }
        private List<tBan> getBanListFromController()
        {
            Classes.tBanListReponse banListReponse = new tBanListReponse();
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://192.223.30.108:55071/");
                client.BaseAddress = new Uri("http://localhost:55071/");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = null;
                try
                {
                    response = client.GetAsync("A3Bans/MainPage").Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Webserver is currently Down!");
                }


                if (response.IsSuccessStatusCode)
                {
                    string strResponse = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        banListReponse = JsonConvert.DeserializeObject<Classes.tBanListReponse>(strResponse);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
            return banListReponse.banList;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dgBansList.ItemsSource = getBanListFromController();
        }


        private void btnMainSubmitBan_Click(object sender, RoutedEventArgs e)
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
                    SubmitBan SubBan = new SubmitBan();
                    SubBan.ShowDialog();
                }
                else
                {
                    MessageBox.Show(authenticationResult.Message);
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            dgBansList.ItemsSource = getBanListFromController();
        }

        private void dgBansList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnDLSql_Click(object sender, RoutedEventArgs e)
        {
            List<tBan> tempList = getBanListFromController();


                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "a3bans";
                dlg.DefaultExt = ".text";
                dlg.Filter = "Text documents (.txt)|*.txt";
                dlg.CreatePrompt = true;
                dlg.OverwritePrompt = true;

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    // Save document
                    string filename = dlg.FileName;
                }

                System.IO.StreamWriter file = new System.IO.StreamWriter(dlg.FileName, false);

            for (int i = 0; i < tempList.Count; i++)
            {
                tBan newBan = new tBan();

                file.Write(newBan.GuidOrIP = tempList[i].GuidOrIP.ToString());
                file.Write(" ");
                file.Write(newBan.BanTime = tempList[i].BanTime.ToString());
                file.Write(" ");
                file.WriteLine(newBan.BanReason = tempList[i].BanReason.ToString());
            }

                file.Close();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Search BanSearch = new Search();
            BanSearch.ShowDialog();
        }
    
    }
}

