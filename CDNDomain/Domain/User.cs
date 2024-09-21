using System.ComponentModel.DataAnnotations;
using NanoidDotNet;

public enum UserStatus
{
    Active = 1,
    Inactive = 2,
    Deleted = 3,
}

public class User : BaseModel
{
    [Key]
    public string Id { get; set; } = Nanoid.Generate(Nanoid.Alphabets.LettersAndDigits, 12);
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public List<string>? Skillsets { get; set; }
    public List<string>? Hobby { get; set; }
    public string? StatusName { get; set; }

    private UserStatus _status;
    public UserStatus Status
    {
        get
        {
            return _status;
        }
        set
        {
            _status = value;
            StatusName = Enum.GetName(typeof(UserStatus), value);
        }
    }
}
