using Domain.Repository;

namespace Domain.Services.Email;

public class EmailService : IEmailService
{
    private readonly IUserRepository _userRepository;

    public EmailService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return await _userRepository.ExistsByEmailAsync(email.Trim().ToLower());
    }
}
