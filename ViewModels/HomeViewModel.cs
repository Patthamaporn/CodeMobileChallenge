using CodeMobileChallenge.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace CodeMobileChallenge.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public ICommand HomeCommand { get; }

        public HomeViewModel()
        {
            //HomeCommand = new RelayCommand(Home);
        }

        private void Home()
        {
        }
    }
}
