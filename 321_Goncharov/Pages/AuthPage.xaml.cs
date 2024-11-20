using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace _321_Goncharov.Pages
{
    /// <summary>
    /// Interaction logic for AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }

        public AuthPage()
        {
            InitializeComponent();
        }

        private void tbLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintLogin.Visibility = Visibility.Visible;
            if (tbLogin.Text.Length > 0)
            {
                txtHintLogin.Visibility = Visibility.Hidden;
            }

        }

        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtHintPassword.Visibility = Visibility.Visible;
            if (pbPassword.Password.Length > 0)
            {
                txtHintPassword.Visibility = Visibility.Hidden;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(tbLogin.Text) || string.IsNullOrEmpty(pbPassword.Password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }
            using (var db = new Entities())
            {
                var passwordHash = GetHash(pbPassword.Password);
                var user = db.User
                .AsNoTracking()
                .FirstOrDefault(u => u.Login == tbLogin.Text && u.Password == passwordHash);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с такими данными не найден!");
                    return;
                }
                else
                {
                    MessageBox.Show("Пользователь успешно найден!");

                    switch (user.Role.ToLower())
                    {
                        case "администратор":
                            NavigationService?.Navigate(new AdminPage());
                            break;
                        case "пользователь":
                            NavigationService?.Navigate(new UserPage());
                            break;
                    }
                }

            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegistrationPage());
        }
    }
}
