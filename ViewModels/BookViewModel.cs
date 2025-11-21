using System.Collections.ObjectModel;
using System.Windows.Input;
using crud_csharp.Commands;
using crud_csharp.Helpers;
using crud_csharp.Models;
using crud_csharp.Services;
using crud_csharp.ViewModels.DTOs;

namespace crud_csharp.ViewModels;

public class BookViewModel : BaseViewModel
{
    private readonly ServiceBook _serviceBook;

    public ObservableCollection<Book> Books { get; set; }

    private Book? _selectedBook;
    public Book? SelectedBook
    {
        get => _selectedBook;
        set => SetField(ref _selectedBook, value);
    }

    public ICommand GetBookByTitleCommand { get; }
    public ICommand AddBookCommand { get; }
    public ICommand UpdateBookCommand { get; }
    public ICommand DeleteBookCommand { get; }
    public ICommand RestockBookCommand { get; }
    public ICommand SellBookCommand { get; }
    public ICommand ChangeAuthorBookCommand { get; }
    public ICommand ChangeGenreBookCommand { get; }

    public BookViewModel(ServiceBook serviceBook)
    {
        _serviceBook = serviceBook;

        Books = new ObservableCollection<Book>(_serviceBook.GetAllBooks());

        GetBookByTitleCommand = new RelayCommand(
            execute: param =>
            {
                if (param is String title && !ValidationHelper.IsInvalidText(title))
                {
                    var book = _serviceBook.GetBookByTitle(title);
                    if (book != null)
                    {
                        SelectedBook = book;
                    }
                }
            },
            canExecute: param => param is String title && !ValidationHelper.IsInvalidText(title)
        );

        AddBookCommand = new RelayCommand(
            execute: param =>
            {
                if (param is Book book)
                {
                    _serviceBook.AddBook(book);

                    Books.Add(book);
                    SelectedBook = book;
                }
            },
            canExecute: param =>  param is Book book
        );

        UpdateBookCommand = new RelayCommand(
            execute: param =>
            {
                if (param is Book book)
                {
                    _serviceBook.UpdateBook(book);

                    if (SelectedBook != null)
                    {
                        int oldBook = Books.IndexOf(SelectedBook);
                        if (oldBook >= 0)
                        {
                            Books[oldBook].Title = book.Title;
                            Books[oldBook].Country =  book.Country;
                            Books[oldBook].Price =  book.Price;
                            Books[oldBook].Stock =  book.Stock;

                            Books[oldBook].ChangeAuthor(book.Author);
                            Books[oldBook].ChangeGenre(book.Genre);
                        }

                        SelectedBook = Books[oldBook];
                    }
                }
            },
            canExecute: param =>  param is Book book
        );

        DeleteBookCommand = new RelayCommand(
            execute: param =>
            {
                if (param is Book book)
                {
                    _serviceBook.DeleteBook(book);

                    Books.Remove(book);
                    SelectedBook = null;
                }
            },
            canExecute: param =>  param is Book book
        );

        RestockBookCommand = new RelayCommand(
            execute: param =>
            {
                if (param is StockBookRequest stockBookRequest)
                {
                    _serviceBook.RestockBook(stockBookRequest.Book, stockBookRequest.Amount);

                    if (SelectedBook != null)
                    {
                        int oldBook = Books.IndexOf(SelectedBook);
                        if (oldBook >= 0)
                        {
                            Books[oldBook].Stock = stockBookRequest.Book.Stock;
                            SelectedBook = Books[oldBook];
                        }
                    }
                }
            },
            canExecute: param => param is StockBookRequest restockBookRequest && !ValidationHelper.IsInvalidNumber(restockBookRequest.Amount)
        );

        SellBookCommand = new RelayCommand(
            execute: param =>
            {
                if (param is StockBookRequest stockBookRequest)
                {
                    _serviceBook.SellBook(stockBookRequest.Book, stockBookRequest.Amount);

                    if (SelectedBook != null)
                    {
                        int oldBook = Books.IndexOf(SelectedBook);
                        if (oldBook >= 0)
                        {
                            Books[oldBook].Stock = stockBookRequest.Book.Stock;
                            SelectedBook = Books[oldBook];
                        }
                    }
                }
            },
            canExecute: param => param is StockBookRequest restockBookRequest && !ValidationHelper.IsInvalidNumber(restockBookRequest.Amount)
        );

        ChangeAuthorBookCommand = new RelayCommand(
            execute: param =>
            {
                if (param is AuthorBookRequest authorBookRequest)
                {
                    _serviceBook.ChangeAuthorBook(authorBookRequest.Book, authorBookRequest.Author);

                    if (SelectedBook != null)
                    {
                        int oldBook = Books.IndexOf(SelectedBook);
                        if (oldBook >= 0)
                        {
                            Books[oldBook].ChangeAuthor(authorBookRequest.Author);
                            SelectedBook = Books[oldBook];
                        }
                    }
                }
            },
            canExecute: param => param is AuthorBookRequest authorBookRequest
        );

        ChangeGenreBookCommand = new RelayCommand(
            execute: param =>
            {
                if (param is GenreBookRequest genreBookRequest)
                {
                    _serviceBook.ChangeGenreBook(genreBookRequest.Book, genreBookRequest.Genre);

                    if (SelectedBook != null)
                    {
                        int oldBook = Books.IndexOf(SelectedBook);
                        if (oldBook >= 0)
                        {
                            Books[oldBook].ChangeGenre(genreBookRequest.Genre);
                            SelectedBook = Books[oldBook];
                        }
                    }
                }
            },
            canExecute: param => param is GenreBookRequest genreBookRequest
        );
    }
}