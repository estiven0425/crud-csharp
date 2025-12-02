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

    private int _stockAmount;
    public int StockAmount
    {
        get => _stockAmount;
        set
        {
            _stockAmount = value;

            OnPropertyChanged();

            CurrentStockRequest = new StockBookRequest { Book = SelectedBook, Amount = _stockAmount };
        }

    }

    private int _sellAmount;
    public int SellAmount
    {
        get => _sellAmount;
        set
        {
            _sellAmount = value;

            OnPropertyChanged();

            CurrentSellRequest = new StockBookRequest { Book = SelectedBook, Amount = _sellAmount };
        }

    }

    private StockBookRequest _currentStockRequest;
    public StockBookRequest CurrentStockRequest
    {
        get => _currentStockRequest;
        private set => SetField(ref _currentStockRequest, value);
    }

    private StockBookRequest _currentSellRequest;
    public StockBookRequest CurrentSellRequest
    {
        get => _currentSellRequest;
        private set => SetField(ref _currentSellRequest, value);
    }

    public ObservableCollection<Author> Authors { get; set; }
    public ObservableCollection<Genre> Genres { get; set; }

    public string ErrorMessage  { get; set; } = string.Empty;

    public ICommand UpdateBookCommand { get; set; }
    public ICommand DeleteBookCommand { get; set; }
    public ICommand RestockBookCommand { get; set; }
    public ICommand SellBookCommand { get; set; }
    public ICommand NavigationBooksCommand { get; set; }

    public BookViewModel(ServiceBook serviceBook, Book? selectedBook, ServiceAuthor serviceAuthor, ServiceGenre serviceGenre, Action<object> navigate)
    {
        _serviceBook = serviceBook;
        _serviceAuthor = serviceAuthor;
        _serviceGenre = serviceGenre;
        _navigate = navigate;

        SelectedBook = selectedBook;

        Authors = new ObservableCollection<Author>(_serviceAuthor.GetAllAuthors());
        Genres = new ObservableCollection<Genre>(_serviceGenre.GetAllGenres());

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

                        _navigate(new BooksViewModel(_serviceBook, _serviceAuthor, _serviceGenre, navigate));
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

                    _navigate(new BooksViewModel(_serviceBook, _serviceAuthor, _serviceGenre, navigate));
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
                        _navigate(new BooksViewModel(_serviceBook, _serviceAuthor, _serviceGenre, navigate));
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
                        _navigate(new BooksViewModel(_serviceBook, _serviceAuthor, _serviceGenre, navigate));
                    }
                }
            },
            canExecute: param => param is StockBookRequest restockBookRequest && !ValidationHelper.IsInvalidNumber(restockBookRequest.Amount)
        );

        NavigationBooksCommand = new RelayCommand(
            execute: _ =>
            {
                _navigate(new BooksViewModel(_serviceBook, _serviceAuthor, _serviceGenre, navigate));
            },
            canExecute: _ => true
        );
    }
}