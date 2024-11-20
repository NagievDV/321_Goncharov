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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _321_Goncharov.Pages
{
    /// <summary>
    /// Interaction logic for AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        private User _currentUser = new User();
        public AddUserPage(User selectedUser)
        {
            InitializeComponent();

            if (selectedUser != null) _currentUser = selectedUser;

            DataContext = _currentUser;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentUser.Login))
                errors.AppendLine("Укажите логин!");
            if (string.IsNullOrWhiteSpace(_currentUser.Password))
                errors.AppendLine("Укажите пароль!");
            if ((_currentUser.Role == null) || (cmbRole.Text == ""))
                errors.AppendLine("Выберите роль!");
            else
                _currentUser.Role = cmbRole.Text;
            if (string.IsNullOrWhiteSpace(_currentUser.FIO))
                errors.AppendLine("Укажите Ф.И.О.!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentUser.ID == 0)
                Entities.GetContext().User.Add(_currentUser);

            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }


        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void tbLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbLoginHint.Visibility = Visibility.Visible;
            if (tbLogin.Text.Length > 0)
            {
                tbLoginHint.Visibility = Visibility.Hidden;
            }
        }

        private void tbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbPasswordHint.Visibility = Visibility.Visible;
            if (tbPassword.Text.Length > 0)
            {
                tbPasswordHint.Visibility = Visibility.Hidden;
            }
        }

        private void tbFIO_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbFIOHint.Visibility = Visibility.Visible;
            if (tbFIO.Text.Length > 0)
            {
                tbFIOHint.Visibility = Visibility.Hidden;
            }
        }

        private void tbPhoto_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbPhotoHint.Visibility = Visibility.Visible;
            if (tbPhoto.Text.Length > 0)
            {
                tbPhotoHint.Visibility = Visibility.Hidden;
            }
        }
    }
}
