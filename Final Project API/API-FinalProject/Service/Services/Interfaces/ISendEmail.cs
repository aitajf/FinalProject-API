
namespace Service.Services.Interfaces
{
    public interface ISendEmail
    {
        void Send(string from, string displayName, string to, string messageBody, string subject);
    }
}
