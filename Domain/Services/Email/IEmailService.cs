namespace Domain.Services.Email;

public interface IEmailService
{
    public abstract Task<bool> EmailExistsAsync(string email);
}
