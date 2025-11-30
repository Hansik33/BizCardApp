namespace BizCardApp.ViewModels;

public partial class BusinessCardViewModel : BaseViewModel
{
    private int _id;
    public int Id
    {
        get => _id;
        set
        {
            if (SetProperty(ref _id, value))
                OnPropertyChanged(nameof(IsDirty));
        }
    }

    public string FullName => $"{FirstName} {LastName}".Trim();

    public string SnapshotFullName
    {
        get
        {
            if (IsDirty && _snapshot != null)
            {
                var snapName = $"{_snapshot.FirstName} {_snapshot.LastName}".Trim();
                return $"* {snapName}";
            }
            else if (_snapshot != null)
                return $"{_snapshot.FirstName} {_snapshot.LastName}".Trim();
            else
                return $"{FirstName} {LastName}".Trim();

        }
    }

    private string _firstName = string.Empty;
    public string FirstName
    {
        get => _firstName;
        set
        {
            if (SetProperty(ref _firstName, value))
            {
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(SnapshotFullName));
                OnPropertyChanged(nameof(IsDirty));
            }
        }
    }

    private string _lastName = string.Empty;
    public string LastName
    {
        get => _lastName;
        set
        {
            if (SetProperty(ref _lastName, value))
            {
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(SnapshotFullName));
                OnPropertyChanged(nameof(IsDirty));
            }
        }
    }

    private string? _company;
    public string? Company
    {
        get => _company;
        set
        {
            if (SetProperty(ref _company, value))
            {
                OnPropertyChanged(nameof(SnapshotFullName));
                OnPropertyChanged(nameof(IsDirty));
            }
        }
    }

    private string? _jobTitle;
    public string? JobTitle
    {
        get => _jobTitle;
        set
        {
            if (SetProperty(ref _jobTitle, value))
            {
                OnPropertyChanged(nameof(SnapshotFullName));
                OnPropertyChanged(nameof(IsDirty));
            }
        }
    }

    private string? _phone;
    public string? Phone
    {
        get => _phone;
        set
        {
            if (SetProperty(ref _phone, value))
            {
                OnPropertyChanged(nameof(SnapshotFullName));
                OnPropertyChanged(nameof(IsDirty));
            }
        }
    }

    private string? _email;
    public string? Email
    {
        get => _email;
        set
        {
            if (SetProperty(ref _email, value))
            {
                OnPropertyChanged(nameof(SnapshotFullName));
                OnPropertyChanged(nameof(IsDirty));
            }
        }
    }

    private string? _address;
    public string? Address
    {
        get => _address;
        set
        {
            if (SetProperty(ref _address, value))
            {
                OnPropertyChanged(nameof(SnapshotFullName));
                OnPropertyChanged(nameof(IsDirty));
            }
        }
    }

    private BusinessCardViewModel? _snapshot;

    public void TakeSnapshot()
    {
        _snapshot = new BusinessCardViewModel
        {
            Id = this.Id,
            FirstName = this.FirstName,
            LastName = this.LastName,
            Company = this.Company,
            JobTitle = this.JobTitle,
            Phone = this.Phone,
            Email = this.Email,
            Address = this.Address
        };
        OnPropertyChanged(nameof(SnapshotFullName));
        OnPropertyChanged(nameof(IsDirty));
    }

    public bool IsDirty =>
        _snapshot == null ||
        Id != _snapshot.Id ||
        FirstName != _snapshot.FirstName ||
        LastName != _snapshot.LastName ||
        Company != _snapshot.Company ||
        JobTitle != _snapshot.JobTitle ||
        Phone != _snapshot.Phone ||
        Email != _snapshot.Email ||
        Address != _snapshot.Address;
}