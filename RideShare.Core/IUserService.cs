using RideShare.Domain;

namespace RideShare.Core
{
    public interface IUserService
    {
        bool Create(User user);
        User Authenticate(string username, string password);
        User Find(string phone);
    }
}