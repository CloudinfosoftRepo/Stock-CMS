namespace Stock_CMS.ServiceInterface
{
    public interface IOTPService
    {
        Task SendOtpAsync(string email);
        Task<bool> VerifyOtpAsync(string email, string otp);

    }
}
