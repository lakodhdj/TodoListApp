using System.Windows;
using TodoListApp.Models;

namespace TodoListApp
{
    public partial class EditTaskWindow : Window
    {

        public TodoItem Task { get; set; }

        public EditTaskWindow(TodoItem task)
        {
            InitializeComponent();
            Task = task;
            DataContext = Task;
            Owner = Application.Current.MainWindow;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}