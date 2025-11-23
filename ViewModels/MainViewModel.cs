using System.Collections.ObjectModel;
using System.Windows.Input;
using crud_csharp.Commands;
using crud_csharp.Helpers;
using crud_csharp.Models;
using crud_csharp.Services;

namespace crud_csharp.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly ServiceBook _serviceBook;
    private readonly ServiceAuthor _serviceAuthor;
    private readonly ServiceGenre _serviceGenre;

    public ObservableCollection<Book>  Books { get; set; }
    public ObservableCollection<Author> Authors { get; set; }
    public ObservableCollection<Genre> Genres { get;  set; }

    private Book? _selectedBook;
    public Book? SelectedBook
    {
        get =>  _selectedBook;
        set => SetField(ref _selectedBook, value);
    }

    private Author? _selectedAuthor;
    public Author? SelectedAuthor
    {
        get => _selectedAuthor;
        set => SetField(ref _selectedAuthor, value);

    }

    private Genre? _selectedGenre;
    public Genre? SelectedGenre
    {
        get => _selectedGenre;
        set => SetField(ref _selectedGenre, value);
    }

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set => SetField(ref _searchText, value);
    }

    private bool _searchInBooks = true;
    public bool SearchInBooks
    {
        get => _searchInBooks;
        set => SetField(ref _searchInBooks, value);
    }

    private bool _searchInAuthors;
    public bool SearchInAuthors
    {
        get => _searchInAuthors;
        set => SetField(ref _searchInAuthors, value);
    }

    private bool _searchInGenres;
    public bool SearchInGenres
    {
        get => _searchInGenres;
        set => SetField(ref _searchInGenres, value);
    }

    public ICommand SelectedSearchCommand { get; set; }
    public ICommand NavigationBooksCommand { get; set; }
    public ICommand NavigationBookCommand { get; set; }
    public ICommand NavigationAuthorsCommand { get; set; }
    public ICommand NavigationAuthorCommand { get; set; }
    public ICommand NavigationGenresCommand { get; set; }
    public ICommand NavigationGenreCommand { get; set; }

    private object? _currentView;
    public object? CurrentView
    {
        get => _currentView;
        set => SetField(ref _currentView, value);
    }

    public MainViewModel(ServiceBook serviceBook, ServiceAuthor serviceAuthor, ServiceGenre serviceGenre, Action<object> navigate)
    {
        _serviceBook = serviceBook;
        _serviceAuthor = serviceAuthor;
        _serviceGenre = serviceGenre;

        CurrentView = this;

        Books = new ObservableCollection<Book>(_serviceBook.GetAllBooks().OrderByDescending(book => book.Id));
        Authors = new ObservableCollection<Author>(_serviceAuthor.GetAllAuthors().OrderByDescending(author => author.Id));
        Genres = new ObservableCollection<Genre>(_serviceGenre.GetAllGenres().OrderByDescending(genre => genre.Id));

        SelectedSearchCommand = new RelayCommand(
            execute: _ =>
            {
                if (!ValidationHelper.IsInvalidText(SearchText))
                {
                    if (SearchInBooks)
                    {
                        var book = _serviceBook.GetBookByTitle(SearchText);
                        if (book != null)
                        {
                            SelectedAuthor = null;
                            SelectedGenre = null;
                            SelectedBook = book;
                        }
                    }
                    else if (SearchInAuthors)
                    {
                        var author = _serviceAuthor.GetAuthorByName(SearchText);
                        if (author != null)
                        {
                            SelectedBook = null;
                            SelectedGenre = null;
                            SelectedAuthor = author;
                        }
                    }
                    else if (SearchInGenres)
                    {
                        var genre = _serviceGenre.GetGenreByName(SearchText);
                        if (genre != null)
                        {
                            SelectedBook = null;
                            SelectedAuthor = null;
                            SelectedGenre = genre;
                        }
                    }
                }
            },
            canExecute: _ => !ValidationHelper.IsInvalidText(SearchText)
        );

        NavigationBooksCommand = new RelayCommand(
            execute: _ =>
            {
                navigate(new BooksViewModel(_serviceBook, _serviceAuthor, _serviceGenre, navigate));
            },
            canExecute: _ => true
        );

        NavigationBookCommand = new RelayCommand(
            execute: _ =>
            {
                CurrentView = new BookViewModel(_serviceBook, SelectedBook, _serviceAuthor, _serviceGenre, navigate);
            },
            canExecute: _ => true
        );

        NavigationAuthorsCommand = new RelayCommand(
            execute: _ =>
            {
                // CurrentView = new AuthorsViewModel(_serviceAuthor, navigate);
            },
            canExecute: _ => true
        );

        NavigationAuthorCommand = new RelayCommand(
            execute: _ =>
            {
                // CurrentView = new AuthorViewModel(_serviceAuthor, SelectedAuthor, navigate);
            },
            canExecute: _ => true
        );

        NavigationGenresCommand = new RelayCommand(
            execute: _ =>
            {
                // CurrentView = new GenresViewModel(_serviceGenre, navigate);
            },
            canExecute: _ => true
        );

        NavigationGenreCommand = new RelayCommand(
            execute: _ =>
            {
                // CurrentView = new GenreViewModel(_serviceGenre, SelectedGenre, navigate);
            },
            canExecute: _ => true
        );
    }
}