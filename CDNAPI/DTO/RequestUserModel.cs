using System.ComponentModel.DataAnnotations;

public class RegisterUserRequest
{
    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? PhoneNumber { get; set; }

    [NotEmptyIfNotNull]
    public List<string>? Skillsets { get; set; }

    [NotEmptyIfNotNull]
    public List<string>? Hobby { get; set; }
}

public class UpdateUserRequest
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    [NotEmptyIfNotNull]
    public List<string>? Skillsets { get; set; }

    [NotEmptyIfNotNull]
    public List<string>? Hobby { get; set; }
}

public class SearchUserRequest
{
    public string? Search { get; set; }
    public List<string>? Sort { get; set; }
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}
