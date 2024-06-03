using CodeMobileChallenge.Commands;
using CodeMobileChallenge.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace CodeMobileChallenge.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        private string email;
        private string password;
        private string errorMessage;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }
        private bool IsValidPassword(string password)
        {
            return password.Length >= 8;
        }

        private void Login()
        {
            if (IsValidEmail(Email) && IsValidPassword(Password))
            {
                ErrorMessage = string.Empty;
                MessageBox.Show($"Logged in successfull as {Email}");
                HomeView homeView = new HomeView();
                Application.Current.MainWindow.Content = homeView;
            }
            else
            {
                ErrorMessage = "Email or password incorrect";
            }
        }
        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
        }
        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login, CanLogin);
        }
    }
}
