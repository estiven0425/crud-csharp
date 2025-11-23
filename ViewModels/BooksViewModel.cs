using System.Collections.ObjectModel;
using System.Windows.Input;
using crud_csharp.Commands;
using crud_csharp.Helpers;
using crud_csharp.Models;
using crud_csharp.Services;
using crud_csharp.ViewModels.DTOs;

namespace crud_csharp.ViewModels;

public class BooksViewModel : BaseViewModel
{
    private readonly ServiceBook _serviceBook;
    private readonly ServiceAuthor _serviceAuthor;
    private readonly ServiceGenre _serviceGenre;
    private readonly Action<object> _navigate;

    public ObservableCollection<Book> Books { get; set; }

    private Book? _selectedBook;
    public Book? SelectedBook
    {
        get => _selectedBook;
        set => SetField(ref _selectedBook, value);
    }

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set => SetField(ref _searchText, value);
    }

    public ICommand GetBookByTitleCommand { get; set; }
    public ICommand NavigationNewBookCommand { get; set; }
    public ICommand NavigationBookCommand { get; set; }
    public ICommand NavigationMainCommand { get; set; }

    public BooksViewModel(ServiceBook serviceBook, ServiceAuthor serviceAuthor, ServiceGenre serviceGenre, Action<object> navigate)
    {
        _serviceBook = serviceBook;
        _serviceAuthor = serviceAuthor;
        _serviceGenre = serviceGenre;
        _navigate = navigate;

        Books = new ObservableCollection<Book>(_serviceBook.GetAllBooks());

        GetBookByTitleCommand = new RelayCommand(
            execute: _ =>
            {
                if (!ValidationHelper.IsInvalidText(SearchText))
                {
                    var book = _serviceBook.GetBookByTitle(SearchText);
                    if (book != null)
                    {
                        SelectedBook = book;
                    }
                }
            },
            canExecute: param =>!ValidationHelper.IsInvalidText(SearchText)
        );

        NavigationNewBookCommand = new RelayCommand(
            execute: _ =>
            {
                // _navigate(new NewBookViewModel(_serviceBook, _navigate));
            },
            canExecute: _ => true
        );

        NavigationBookCommand = new RelayCommand(
            execute: _ =>
            {
                _navigate(new BookViewModel(_serviceBook, SelectedBook, _serviceAuthor, _serviceGenre, _navigate));
            },
            canExecute: _ => true
        );

        NavigationMainCommand = new RelayCommand(
            execute: _ =>
            {
                _navigate(new MainViewModel(_serviceBook, _serviceAuthor, _serviceGenre, navigate));
            },
            canExecute: _ => true
        );
    }
}