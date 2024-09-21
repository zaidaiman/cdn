public class SearchUserResponse
{
    public IEnumerable<UserResponse>? Items { get; set; }
    public int Page { get; set; }
    public int TotalPage { get; set; }
    public int Size { get; set; }
    public int Total { get; set; }
}

public class UserResponse
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public List<string>? Skillsets { get; set; }
    public List<string>? Hobby { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
