using System.Collections.ObjectModel;
using System.Windows.Input;
using crud_csharp.Commands;
using crud_csharp.Helpers;
using crud_csharp.Models;
using crud_csharp.Services;

namespace crud_csharp.ViewModels;

public class GenreViewModel : BaseViewModel
{
    private readonly ServiceGenre _serviceGenre;

    public ObservableCollection<Genre>  Genres { get; set; }

    private Genre? _selectedGenre;
    public Genre? SelectedGenre
    {
        get => _selectedGenre;
        set => SetField(ref _selectedGenre, value);
    }

    public ICommand GetGenreByNameCommand { get; set; }
    public ICommand AddGenreCommand { get; set; }
    public ICommand UpdateGenreCommand { get; set; }
    public ICommand DeleteGenreCommand { get; set; }

    public GenreViewModel(ServiceGenre serviceGenre)
    {
        _serviceGenre = serviceGenre;

        Genres = new ObservableCollection<Genre>(_serviceGenre.GetAllGenres());

        GetGenreByNameCommand = new RelayCommand(
            execute: param =>
            {
                if (param is String name && !ValidationHelper.IsInvalidText(name))
                {
                    var genre = _serviceGenre.GetGenreByName(name);
                    if (genre != null)
                    {
                        SelectedGenre = genre;
                    }
                }
            },
            canExecute: param => param is String name && !ValidationHelper.IsInvalidText(name)
        );

        AddGenreCommand = new RelayCommand(
            execute: param =>
            {
                if (param is Genre genre)
                {
                    _serviceGenre.AddGenre(genre);

                    Genres.Add(genre);
                    SelectedGenre = genre;
                }
            },
            canExecute: param => param is Genre genre
        );

        UpdateGenreCommand = new RelayCommand(
            execute: param =>
            {
                if (param is Genre genre)
                {
                    _serviceGenre.UpdateGenre(genre);

                    if (SelectedGenre != null)
                    {
                        int oldGenre = Genres.IndexOf(SelectedGenre);
                        if (oldGenre >= 0)
                        {
                            Genres[oldGenre].Name =  genre.Name;
                            Genres[oldGenre].Description  = genre.Description;
                        }

                        SelectedGenre = Genres[oldGenre];
                    }
                }
            },
            canExecute: param => param is Genre genre
        );

        DeleteGenreCommand = new RelayCommand(
            execute: param =>
            {
                if (param is Genre genre)
                {
                    _serviceGenre.DeleteGenre(genre);

                    Genres.Remove(genre);
                    SelectedGenre = null;
                }
            },
            canExecute: param => param is Genre genre);
    }
}