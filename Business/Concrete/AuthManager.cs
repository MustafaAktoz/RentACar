using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Result;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        IUserService _userService;
        ITokenHelper _tokenHelper;
        public AuthManager(IUserService userService,ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var operationClaims=_userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, operationClaims.Data);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.TokenCreated);
        }

        public IDataResult<User> Register(UserForRegisterDto register)
        {
            var result = BusinessRules.Run(UserAlreadyExist(register.Email));
            if (result != null) return new ErrorDataResult<User>(result.Message);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(register.Password, out passwordHash, out passwordSalt);
            var user = new User
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user, Messages.RegistrationSuccessful);
        }

        public IDataResult<User> Login(UserForLoginDto login)
        {
            var user = _userService.GetByEmail(login.Email);
            if (user.Data == null) return new ErrorDataResult<User>(Messages.UserNotFound);

            bool result=HashingHelper.VerifyPasswordHash(login.Password, user.Data.PasswordHash, user.Data.PasswordSalt);
            if (!result) return new ErrorDataResult<User>(Messages.PasswordIsWrong);

            return new SuccessDataResult<User>(user.Data, Messages.LoginSuccessful);

        }

        private IResult UserAlreadyExist(string email)
        {
            var result = _userService.GetByEmail(email);
            if (result.Data != null) return new ErrorResult(Messages.UserAlreadyExist);

            return new SuccessResult();
        }

        public IResult ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var user = _userService.GetById(changePasswordDto.UserId);
            if (user.Data == null) return new ErrorDataResult<User>(Messages.UserNotFound);

            bool result = HashingHelper.VerifyPasswordHash(changePasswordDto.OldPassword, user.Data.PasswordHash, user.Data.PasswordSalt);
            if (!result) return new ErrorDataResult<User>(Messages.OldPasswordIsWrong);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(changePasswordDto.NewPassword, out passwordHash, out passwordSalt);
            user.Data.PasswordHash = passwordHash;
            user.Data.PasswordSalt = passwordSalt;

            _userService.Update(user.Data);
            return new SuccessResult(Messages.PasswordChangedSuccessfully);
        }
    }
}
