using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RideShare.Data.Repositories;
using RideShare.Domain;

namespace RideShare.Core.Implementations
{
    public class UserService : IUserService
    {
        private UserRepository UserRepository { get; set; }
        private const string JwtSecret = "rideshare_token_2020";

        public UserService(UserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        /// <summary>
        /// User create service with user model
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Create(User user)
        {
            try
            {
                UserRepository.Create(user);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// User authentication service with JWT token mechanism
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Authenticate(string phone, string password)
        {
            var user = UserRepository.Authenticate(phone, password);

            if (user == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(type: ClaimTypes.MobilePhone, value: user.Phone),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = null;

            return user;
        }

        public User Find(string phone)
        {
            return UserRepository.Find(phone);
        }
    }
}