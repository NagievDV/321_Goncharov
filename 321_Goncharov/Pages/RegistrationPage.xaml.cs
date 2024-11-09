using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }

        private static bool ContainsRussianLetters(string input)
        {
            // Регулярное выражение для поиска русских букв (кириллица)
            Regex russianLetterPattern = new Regex("[А-Яа-яЁё]");
            return russianLetterPattern.IsMatch(input);
        }
        public static bool ContainsDigit(string input)
        {
            // Регулярное выражение для поиска хотя бы одной цифры
            Regex digitPattern = new Regex(@"\d");
            return digitPattern.IsMatch(input);
        }
        private bool ValidatePassword(string password)
        {
            if (password.Length < 6) return false;
            else if (ContainsRussianLetters(password)) return false;
            else if (!ContainsDigit(password)) return false;

            return true;
        }

        private bool FreeLogin(string login)
        {
            using (var db = new Entities())
            {
                var user = db.User
                .AsNoTracking()
                .FirstOrDefault(u => u.Login == login);

                if (user != null)
                {
                    return false;
                }

            }
            return true;
        }
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
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

        private void pbPasswordConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtHintPasswordConfirm.Visibility = Visibility.Visible;
            if (pbPasswordConfirm.Password.Length > 0)
            {
                txtHintPasswordConfirm.Visibility = Visibility.Hidden;
            }

        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbLogin.Text) ||
                string.IsNullOrEmpty(pbPassword.Password) ||
                string.IsNullOrEmpty(pbPasswordConfirm.Password) ||
                string.IsNullOrEmpty(tbFIO.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (!FreeLogin(tbLogin.Text))
            {
                MessageBox.Show("Пользователь с таким логином уже существует!");
                return;
            }

            if (!ValidatePassword(pbPassword.Password))
            {
                MessageBox.Show("Придумайте более безопасный пароль\n(6 символов или более, только латиница, минимум одна цифра)!");
                return;
            }

            if (pbPassword.Password != pbPasswordConfirm.Password)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }

            Entities db = new Entities();
            User userObject = new User
            {
                FIO = tbFIO.Text,
                Login = tbLogin.Text,
                Password = GetHash(pbPassword.Password),
                Role = cbRole.Text
            };
            db.User.Add(userObject);
            db.SaveChanges();

            MessageBox.Show("Пользователь успешно зарегистрирован!");
            NavigationService?.GoBack();

        }
    }
}
