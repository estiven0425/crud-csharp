using System.Windows.Controls;
using crud_csharp.Data;
using crud_csharp.Services;
using crud_csharp.ViewModels;

namespace crud_csharp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        var context = new AppDbContext();

        DataContext = new MainViewModel(
            new ServiceBook(context),
            new ServiceAuthor(context),
            new ServiceGenre(context)
        );
    }
}