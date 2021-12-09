using Core.Entities.Concrete;
using Core.Utilities.Result;
using Core.Utilities.Security.JWT;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserForRegisterDto register);
        IDataResult<User> Login(UserForLoginDto login);
        IResult ChangePassword(ChangePasswordDto changePasswordDto);
        IDataResult<AccessToken> CreateAccessToken(User user);
    }
}
