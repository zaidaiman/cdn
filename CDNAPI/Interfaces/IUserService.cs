public interface IUserService
{
    public Task<UserResponse?> CreateUserAsync(RegisterUserRequest data, string actionBy);
    public Task<UserResponse?> GetUserAsync(string username);
    public Task<UserResponse?> GetUserByIdAsync(string userId);
    public Task<UserResponse?> UpdateUserAsync(string username, UpdateUserRequest data, string actionBy);
    public Task<bool> DeleteUserAsync(string username, string actionBy);
    public Task<SearchUserResponse> SearchUserAsync(SearchUserRequest query);
}
