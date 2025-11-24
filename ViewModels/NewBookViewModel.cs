using System.Collections.ObjectModel;
using System.Windows.Input;
using crud_csharp.Commands;
using crud_csharp.Helpers;
using crud_csharp.Models;
using crud_csharp.Services;
using crud_csharp.ViewModels.DTOs;

namespace crud_csharp.ViewModels;

public class NewBookViewModel : BaseViewModel
{
    private readonly ServiceBook _serviceBook;
    private readonly ServiceAuthor _serviceAuthor;
    private readonly ServiceGenre _serviceGenre;
    private readonly Action<object?> _navigate;

    private Book? _book;
    public Book Book
    {
        get => _book;
        set => SetField(ref _book, value);
    }

    public ObservableCollection<Author> Authors { get; set; }
    public ObservableCollection<Genre> Genres { get; set; }

    public ICommand AddBookCommand { get; set; }
    public ICommand NavigationBooksCommand { get; set; }

    public NewBookViewModel(ServiceBook serviceBook, ServiceAuthor serviceAuthor, ServiceGenre serviceGenre, Action<object> navigate)
    {
        _serviceBook = serviceBook;
        _serviceAuthor = serviceAuthor;
        _serviceGenre = serviceGenre;
        _navigate = navigate;

        Book = new Book();

        Authors = new ObservableCollection<Author>(_serviceAuthor.GetAllAuthors());
        Genres = new ObservableCollection<Genre>(_serviceGenre.GetAllGenres());

        AddBookCommand = new RelayCommand(
            execute: _ =>
            {
                _serviceBook.AddBook(Book);

                _navigate(new BooksViewModel(_serviceBook, _serviceAuthor, _serviceGenre, _navigate));
            },
            canExecute: _ =>  true
        );


        NavigationBooksCommand = new RelayCommand(
            execute: _ =>
            {
                _navigate(new BooksViewModel(_serviceBook, _serviceAuthor, _serviceGenre, _navigate));
            },
            canExecute: _ => true
        );
    }
}