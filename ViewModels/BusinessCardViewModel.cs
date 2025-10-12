namespace BizCardApp.ViewModels;

public partial class BusinessCardViewModel : BaseViewModel
{
    public string FullName => $"{FirstName} {LastName}".Trim();

    private string _firstName = string.Empty;
    public string FirstName
    {
        get => _firstName;
        set
        {
            if (SetProperty(ref _firstName, value))
                OnPropertyChanged(nameof(FullName));
        }
    }

    private string _lastName = string.Empty;
    public string LastName
    {
        get => _lastName;
        set
        {
            if (SetProperty(ref _lastName, value))
                OnPropertyChanged(nameof(FullName));
        }
    }

    private string? _company;
    public string? Company
    {
        get => _company;
        set => SetProperty(ref _company, value);
    }

    private string? _jobTitle;
    public string? JobTitle
    {
        get => _jobTitle;
        set => SetProperty(ref _jobTitle, value);
    }

    private string? _phone;
    public string? Phone
    {
        get => _phone;
        set => SetProperty(ref _phone, value);
    }

    private string? _email;
    public string? Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    private string? _address;
    public string? Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }
}