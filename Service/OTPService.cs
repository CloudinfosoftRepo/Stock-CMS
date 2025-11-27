using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;
using Stock_CMS.ViewModel;

namespace Stock_CMS.Service
{
    public class OTPService : IOTPService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<OTPService> _logger;
        private readonly SmtpSettingsViewModel _smtpSettings;

        public OTPService(IUserRepository userRepository, ILogger<OTPService> logger, IOptions<SmtpSettingsViewModel> smtpOptions)
        {
            _userRepository = userRepository;
            _logger = logger;
            _smtpSettings = smtpOptions.Value;
        }

        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async Task SendOtpEmail(string email, string otp)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.UserName));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Your OTP Code";

            message.Body = new TextPart("plain")
            {
                Text = $"Your OTP code is: {otp}\nThis code will expire in 5 minutes."
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendOtpAsync(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
                throw new Exception("User not found.");

            // Assuming UserDto has these added properties:
            user.CurrentOtp = GenerateOtp();
            user.ExpiryTime = DateTime.UtcNow.AddMinutes(5);
            user.FailedAttempts = 0;
            user.IsLock = false;

            // The update method expects IEnumerable<UserDto>, so wrap user in a list:
            await _userRepository.UpdateUser(new List<UserDto> { user });
            await SendOtpEmail(email, user.CurrentOtp);
        }

        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
                return false;

            if (user.IsLock == true)
                return false;

            if (user.ExpiryTime == null || user.ExpiryTime < DateTime.UtcNow)
                return false;

            if (user.CurrentOtp != otp)
            {
                user.FailedAttempts++;

                if (user.FailedAttempts >= 3)
                    user.IsLock = true;

                await _userRepository.UpdateUser(new List<UserDto> { user });
                return false;
            }

            // OTP is valid
            user.CurrentOtp = null;
            user.ExpiryTime = null;
            user.FailedAttempts = 0;

            await _userRepository.UpdateUser(new List<UserDto> { user });
            return true;
        }
    }
}
