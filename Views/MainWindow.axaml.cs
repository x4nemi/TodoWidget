using Avalonia.Controls;
using Avalonia.Input;

namespace TodoApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Allow pressing Enter in the input box to add a todo
        var input = this.FindControl<TextBox>("InputBox");
        if (input != null)
            input.KeyDown += (_, e) =>
            {
                if (e.Key == Key.Enter)
                    this.FindControl<Button>("AddButton")?.Command?.Execute(null);
            };
    }
}
