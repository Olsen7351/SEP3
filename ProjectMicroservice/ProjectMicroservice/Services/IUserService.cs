using ClassLibrary_SEP3;

namespace ProjectMicroservice.Services;

public interface IUserService
{
    User CreateUser(User user);
    String Login(User user);
}