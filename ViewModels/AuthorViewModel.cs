using System.Collections.ObjectModel;
using System.Windows.Input;
using crud_csharp.Commands;
using crud_csharp.Helpers;
using crud_csharp.Models;
using crud_csharp.Services;

namespace crud_csharp.ViewModels;

public class AuthorViewModel : BaseViewModel
{
    private readonly ServiceAuthor _serviceAuthor;

    public ObservableCollection<Author> Authors { get; set; }

    private Author? _selectedAuthor;
    public Author? SelectedAuthor
    {
        get => _selectedAuthor;
        set => SetField(ref _selectedAuthor, value);
    }

    public ICommand GetAuthorByNameCommand {  get; set; }
    public ICommand AddAuthorCommand {  get; set; }
    public ICommand UpdateAuthorCommand {  get; set; }
    public ICommand DeleteAuthorCommand {  get; set; }

    public AuthorViewModel(ServiceAuthor serviceAuthor)
    {
        _serviceAuthor = serviceAuthor;

        Authors = new ObservableCollection<Author>(_serviceAuthor.GetAllAuthors());

        GetAuthorByNameCommand = new RelayCommand(
            execute: param =>
            {
                if (param is String name && !ValidationHelper.IsInvalidText(name))
                {
                    var author = _serviceAuthor.GetAuthorByName(name);
                    if (author != null)
                    {
                        SelectedAuthor = author;
                    }
                }
            },
            canExecute: param => param is String name &&  !ValidationHelper.IsInvalidText(name)
        );

        AddAuthorCommand = new RelayCommand(
            execute: param =>
            {
                if (param is Author author)
                {
                    _serviceAuthor.AddAuthor(author);

                    Authors.Add(author);
                    SelectedAuthor = author;
                }
            },
            canExecute: param => param is Author author
        );

        UpdateAuthorCommand = new RelayCommand(
            execute: param =>
            {
                if (param is Author author)
                {
                    _serviceAuthor.UpdateAuthor(author);

                    if (SelectedAuthor != null)
                    {
                        int oldAuthor = Authors.IndexOf(SelectedAuthor);
                        if (oldAuthor >= 0)
                        {
                            Authors[oldAuthor].Name = author.Name;
                            Authors[oldAuthor].Age = author.Age;
                            Authors[oldAuthor].Country = author.Country;
                            Authors[oldAuthor].Description = author.Description;
                            Authors[oldAuthor].Email = author.Email;
                            Authors[oldAuthor].Phone = author.Phone;
                        }

                        SelectedAuthor = Authors[oldAuthor];
                    }
                }
            },
            canExecute: param => param is Author author
        );

        DeleteAuthorCommand = new RelayCommand(
            execute: param =>
            {
                if (param is Author author)
                {
                    _serviceAuthor.DeleteAuthor(author);

                    Authors.Remove(author);
                    SelectedAuthor = null;
                }
            },
            canExecute: param => param is Author author
        );
    }
}