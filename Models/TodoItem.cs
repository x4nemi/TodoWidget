using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoApp.Models;

public partial class TodoItem : ObservableObject
{
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private bool _isCompleted;
}
