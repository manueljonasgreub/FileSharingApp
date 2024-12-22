using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using FSAClient.Classes;

namespace FSAClient
{
    /// <summary>
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class Setup : Page
    {
        private IPServices iPService = new IPServices();

        public Setup()
        {
            InitializeComponent();
            GetClientInfo();
        }

        private async void GetClientInfo()
        {
            try
            {
                IPAddress localIP = iPService.GetLocalIP();
                IPAddress publicIP = await iPService.GetPublicIP();

                TextBoxLocalIP.Text = localIP.ToString();
                TextBoxPublicIP.Text = publicIP.ToString();
            }
            catch
            {
            }
        }

        private void ButtonFinishSetup_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxLocalMode.IsChecked == true)
            {
                TextBoxPublicIP.Text = TextBoxLocalIP.Text;
                TextBoxPublicPort.Text = TextBoxLocalPort.Text;
            }

            try
            {
                if (UserData.StoreUserData(TextBoxName.Text, TextBoxLocalIP.Text, TextBoxLocalPort.Text,
                    TextBoxPublicIP.Text, TextBoxPublicPort.Text))
                {
                    var newPage = new FSA(TextBoxServerAddress.Text);
                    MainWindow.Instance.NavigateToPage(newPage);
                }
                else throw new Exception();
            }
            catch
            {
                MessageBox.Show("Ungültige Eingaben erkannt.", "Error");
            }
        }

        private void CheckBoxLocalMode_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxPublicIP.IsEnabled = false;
            TextBoxPublicPort.IsEnabled = false;
        }

        private void CheckBoxLocalMode_Unchecked(object sender, RoutedEventArgs e)
        {
            TextBoxPublicIP.IsEnabled = true;
            TextBoxPublicPort.IsEnabled = true;
        }
    }
}