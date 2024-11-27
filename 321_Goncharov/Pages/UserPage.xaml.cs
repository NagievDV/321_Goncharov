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
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {

        private void UpdateUsers()
        {
            var currentUsers = Entities.GetContext().User.ToList();

            currentUsers = currentUsers.Where(x => x.FIO.ToLower().Contains(tbSearch.Text.ToLower())).ToList();

            if (chkbOnlyUsers.IsChecked.Value)
                currentUsers = currentUsers.Where(x => x.Role.Contains("Пользователь")).ToList();

            if (cbSort.SelectedIndex == 0)
                lvUsersList.ItemsSource = currentUsers.OrderBy(x => x.FIO).ToList();
            else lvUsersList.ItemsSource = currentUsers.OrderByDescending(x => x.FIO).ToList();
        }
            public UserPage()
            {
            InitializeComponent();
            cbSort.SelectedIndex = 0;
            chkbOnlyUsers.IsChecked = false;

            var currentUsers = Entities.GetContext().User.ToList();
            lvUsersList.ItemsSource = currentUsers;

            }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = "";
            cbSort.SelectedIndex = 0;
            chkbOnlyUsers.IsChecked = false;
            UpdateUsers();
        }

        private void chkbOnlyUsers_Checked(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }

        private void chkbOnlyUsers_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }
    }
}
