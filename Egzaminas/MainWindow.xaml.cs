using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Egzaminas
{
    public partial class MainWindow : Window
    {
        private PasswordManager passwordManager;
        private Timer timer;
        public MainWindow()
        {
            InitializeComponent();
            passwordManager = new PasswordManager();
            timer = new Timer();
        }

        private void CreatePassword_Click(object sender, RoutedEventArgs e)
        {
            string password = PasswordTextBox.Text;
            if (!string.IsNullOrEmpty(password))
            {
                string hash = passwordManager.CreatePasswordHash(password);
                HashTextBlock.Text = "Hash: " + hash;
            }
        }

        private async void BruteForceCrack_Click(object sender, RoutedEventArgs e)
        {
            string hash = HashTextBlock.Text.Replace("Hash: ", "");
            if (!string.IsNullOrEmpty(hash))
            {
                if (!int.TryParse(ThreadCountTextBox.Text, out int maxThreads) || maxThreads <= 0)
                {
                    MessageBox.Show("Please enter a valid number of threads.");
                    return;
                }
                var bruteForceCracker = new BruteForceCracker(hash);

                timer.Start();
                string crackedPassword = await Task.Run(() => bruteForceCracker.BruteForcePassword(maxThreads));
                timer.Stop();

                if (string.IsNullOrEmpty(crackedPassword))
                {
                    ResultTextBlock.Text = "Password not found.";
                }
                else
                {
                    ResultTextBlock.Text = "Password: " + crackedPassword;
                }

                TimeTextBlock.Text = "Time taken: " + timer.GetElapsedTime() + " seconds";
            }
        }
    }
}