using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Models;

namespace TodoApp.ViewModels;

public partial class TodoDetailViewModel : ObservableObject
{
    [ObservableProperty] private TodoItem _item;

    public Action? OnGoBack { get; set; }

    public TodoDetailViewModel(TodoItem item)
    {
        _item = item;
    }

    [RelayCommand]
    private void GoBack() => OnGoBack?.Invoke();
}
