using System;
using System.Text;
using System.Windows;
using System.Net.Http;
using System.Net.Http.Headers;
using A3W_Bans.Classes;
using Newtonsoft.Json;
//Gay

//I hate community edition

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
            this.MouseLeftButtonDown += delegate { this.DragMove(); };
            txtSqlPassword.MaxLength = 16;

        }

        private void btnSqlLogin_Click(object sender, RoutedEventArgs e)
        {

            tAuthenticationResponse authenticationResult = new Classes.tAuthenticationResponse();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:55071/");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                tCredential credential = new Classes.tCredential();
                credential.Username = txtSqlUserName.Text;
                credential.Password = txtSqlPassword.Password;

                string serializedBan = JsonConvert.SerializeObject(credential);

                StringContent content = new StringContent(serializedBan, Encoding.UTF8, "application/json");


                HttpResponseMessage response = client.PostAsync("A3Bans/LogIn", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string strResponse = response.Content.ReadAsStringAsync().Result;
                    authenticationResult = JsonConvert.DeserializeObject<tAuthenticationResponse>(strResponse);

                    
                }

            }

            if(authenticationResult.AuthenticationSuccess)
            {
                MessageBox.Show("You Have Succesfully Logged Into A3W Bans.");
                this.Hide();
                //SubmitBan SubBan = new SubmitBan();
                //SubBan.ShowDialog();
            }
            else
            {
                MessageBox.Show(authenticationResult.Message);
            }
        }

        public void userNameclr(object sender, RoutedEventArgs e)
        {
            txtSqlUserName.Text = string.Empty;
            txtSqlUserName.GotFocus -= userNameclr;
            txtSqlPassword.Clear();
        }

        private void btnSqlCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }

}
