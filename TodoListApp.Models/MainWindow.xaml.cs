using System.Windows;
using TodoListApp.ViewModels;

namespace TodoListApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}