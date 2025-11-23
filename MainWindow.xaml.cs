using System.Windows;
using crud_csharp.Data;
using crud_csharp.Services;
using crud_csharp.ViewModels;

namespace crud_csharp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var context = new AppDbContext();

        MainViewModel mainViewModel = null!;
        mainViewModel = new MainViewModel(
            new ServiceBook(context),
            new ServiceAuthor(context),
            new ServiceGenre(context),
            navigate: viewModel => mainViewModel.CurrentView = viewModel
        );

        DataContext = mainViewModel;
    }
}