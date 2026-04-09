using Avalonia.Controls;
using Avalonia.Input;
using TodoApp.ViewModels;

namespace TodoApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var input = this.FindControl<TextBox>("InputBox");
        if (input != null)
            input.KeyDown += (_, e) =>
            {
                if (e.Key == Key.Enter && DataContext is MainWindowViewModel vm && vm.AddTodoCommand.CanExecute(null))
                    vm.AddTodoCommand.Execute(null);
            };
    }
}
