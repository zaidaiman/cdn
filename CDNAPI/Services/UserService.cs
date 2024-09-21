using Microsoft.EntityFrameworkCore;
using Log = Helpers.Log;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly ApplicationDbContext _context;
    private readonly Mapper _mapper;
    public UserService(ILogger<UserService> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
        _mapper = new Mapper();
    }

    public async Task<UserResponse?> GetUserByIdAsync(string userId)
    {
        return await _context.User.AsNoTracking()
            .Where(u => u.Id == userId && u.Status == UserStatus.Active)
            .Select(u => _mapper.MapUserResponse(u))
            .FirstOrDefaultAsync();
    }

    public async Task<UserResponse?> GetUserAsync(string username)
    {
        return await _context.User.AsNoTracking()
            .Where(u => u.Username == username && u.Status == UserStatus.Active)
            .Select(u => _mapper.MapUserResponse(u))
            .FirstOrDefaultAsync();
    }

    private async Task<User?> _GetUserAsync(string username)
    {
        return await _context.User
            .Where(u => u.Username == username && u.Status == UserStatus.Active)
            .FirstOrDefaultAsync();
    }

    public async Task<UserResponse?> CreateUserAsync(RegisterUserRequest data, string actionBy)
    {
        var existingUser = await _context.User.Where(u => u.Username == data.Username).FirstOrDefaultAsync();
        if (existingUser != null)
        {
            Log.Error(_logger, GetType().Name, $"User {data.Username} already exists.");
            throw new Exception($"User {data.Username} already exists.");
        }

        Log.Information(_logger, GetType().Name, $"Creating user {data.Username}");
        var newUser = new User
        {
            Email = data.Email,
            Username = data.Username,
            PhoneNumber = data.PhoneNumber,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = actionBy,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = actionBy,
            Status = UserStatus.Active
        };

        if (data.Skillsets?.Count > 0) newUser.Skillsets = data.Skillsets;
        if (data.Hobby?.Count > 0) newUser.Hobby = data.Hobby;

        await _context.User.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return _mapper.MapUserResponse(newUser);
    }

    public async Task<UserResponse?> UpdateUserAsync(string username, UpdateUserRequest data, string actionBy)
    {
        var user = await _GetUserAsync(username);
        if (user == null) return null;

        Log.Information(_logger, GetType().Name, $"Updating user {username}");

        if (!string.IsNullOrEmpty(data.Email)) user.Email = data.Email;
        if (data.Skillsets?.Count > 0) user.Skillsets = data.Skillsets;
        if (data.Hobby?.Count > 0) user.Hobby = data.Hobby;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = actionBy;

        await _context.SaveChangesAsync();
        return _mapper.MapUserResponse(user);
    }

    public async Task<bool> DeleteUserAsync(string username, string actionBy)
    {
        var user = await _GetUserAsync(username);
        if (user == null) return false;

        Log.Information(_logger, GetType().Name, $"Deleting user {username}");
        _context.User.Remove(user);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<SearchUserResponse> SearchUserAsync(SearchUserRequest req)
    {
        var query = _context.User.AsNoTracking().Where(u => u.Status == UserStatus.Active);

        if (!string.IsNullOrEmpty(req.Search))
        {
            query = query.Where(u =>
                        u.Username!.Contains(req.Search) ||
                        u.Email!.Contains(req.Search) ||
                        u.Skillsets!.Any(s => s.Contains(req.Search)) ||
                        u.Hobby!.Any(h => h.Contains(req.Search)));
        }

        if (req.Sort?.Count > 0)
        {
            var orderedQuery = query.OrderBy(u => 0);
            foreach (var sort in req.Sort)
            {
                if (sort.Contains("username"))
                {
                    orderedQuery = sort.StartsWith('-')
                        ? orderedQuery.ThenBy(u => u.Username)
                        : orderedQuery.ThenByDescending(u => u.Username);
                }

                if (sort.Contains("email"))
                {
                    orderedQuery = sort.StartsWith('-')
                        ? orderedQuery.ThenBy(u => u.Email)
                        : orderedQuery.ThenByDescending(u => u.Email);
                }

                if (sort.Contains("createdAt"))
                {
                    orderedQuery = sort.StartsWith('-')
                        ? orderedQuery.ThenBy(u => u.CreatedAt)
                        : orderedQuery.ThenByDescending(u => u.CreatedAt);
                }

                if (sort.Contains("updatedAt"))
                {
                    orderedQuery = sort.StartsWith('-')
                        ? orderedQuery.ThenBy(u => u.UpdatedAt)
                        : orderedQuery.ThenByDescending(u => u.UpdatedAt);
                }
            }

            query = orderedQuery;
        }

        var total = await query.CountAsync();
        var items = await query.Skip((req.Page - 1) * req.Size).Take(req.Size)
                        .Select(u => _mapper.MapUserResponse(u))
                        .ToListAsync();

        return new SearchUserResponse
        {
            Items = items,
            Page = req.Page,
            Size = req.Size,
            Total = total,
            TotalPage = (int)Math.Ceiling((double)total / req.Size),
        };
    }
}
