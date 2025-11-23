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
    private readonly ServiceAuthor _serviceAuthor;
    private readonly ServiceGenre _serviceGenre;
    private readonly Action<object?> _navigate;

    private Book? _selectedBook;

    public Book? SelectedBook
    {
        get => _selectedBook;
        private set => SetField(ref _selectedBook,  value);
    }

    public ICommand UpdateBookCommand { get; set; }
    public ICommand DeleteBookCommand { get; set; }
    public ICommand RestockBookCommand { get; set; }
    public ICommand SellBookCommand { get; set; }
    public ICommand ChangeAuthorBookCommand { get; set; }
    public ICommand ChangeGenreBookCommand { get; set; }
    public ICommand NavigationBooksCommand { get; set; }

    public BookViewModel(ServiceBook serviceBook, Book? selectedBook, ServiceAuthor serviceAuthor, ServiceGenre serviceGenre, Action<object> navigate)
    {
        _serviceBook = serviceBook;
        _serviceAuthor = serviceAuthor;
        _serviceGenre = serviceGenre;
        _navigate = navigate;

        SelectedBook = selectedBook;

        UpdateBookCommand = new RelayCommand(
            execute: param =>
            {
                if (param is Book book)
                {
                    _serviceBook.UpdateBook(book);

                    if (SelectedBook != null)
                    {
                        SelectedBook.Title = book.Title;
                        SelectedBook.Country = book.Country;
                        SelectedBook.Price = book.Price;
                        SelectedBook.Stock = book.Stock;

                        SelectedBook.ChangeAuthor(book.Author);
                        SelectedBook.ChangeGenre(book.Genre);
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

                    _navigate(new BooksViewModel(_serviceBook, _serviceAuthor, _serviceGenre, _navigate));
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
                        SelectedBook.Stock = stockBookRequest.Book.Stock;
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
                        SelectedBook.Stock = stockBookRequest.Book.Stock;
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
                        SelectedBook.ChangeAuthor(authorBookRequest.Author);
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
                        SelectedBook.ChangeGenre(genreBookRequest.Genre);
                    }
                }
            },
            canExecute: param => param is GenreBookRequest genreBookRequest
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