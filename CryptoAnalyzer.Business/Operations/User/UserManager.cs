using System;
using System.Linq;
using System.Security.Claims;
using CryptoAnalyzer.Business.DataProtection;
using CryptoAnalyzer.Business.Operations.User;
using CryptoAnalyzer.Business.Operations.User.Dtos;
using CryptoAnalyzer.Business.Types;
using CryptoAnalyzer.Data.Entities;
using CryptoAnalyzer.Data.Repositories;
using CryptoAnalyzer.Data.UnitOfWork;

namespace CryptoAnalyzer.Business.Operations.User
{
	public class UserManager:IUserService
	{

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IDataProtection _protector;
		public UserManager(IUnitOfWork unitOfWork, IRepository<UserEntity> userRepository,IDataProtection protector)
		{
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _protector = protector;
		}

        public async Task<ServiceMessage> AddUser(AddUserDto user)
        {
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == user.Email.ToLower());

            if (hasMail.Any())
            {
                return new ServiceMessage
                {
                    IsSucceeded = false,
                    Message = "Email adress already exists!"
                };

            }

            var userEntity = new UserEntity()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = _protector.Protect(user.Password),
                BirthDate = user.BirthDate,
                UserType=Data.Enums.UserType.User,
                Deposit=user.Deposit,
                
            };

            _userRepository.Add(userEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw new Exception("Couldn't save the changes");
            }

            return new ServiceMessage
            {
                IsSucceeded = true,
            };
        }

        public ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user)
        {
            var userEntity = _userRepository.Get(x => x.Email.ToLower() == user.Email.ToLower());

            if (userEntity is null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceeded = false,
                    Message = "Login failed!",
                };
            }
            var unprotectedPassword = _protector.UnProtect(userEntity.Password);

            if (unprotectedPassword == user.Password)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceeded = true,
                    Data = new UserInfoDto
                    {
                        Id = userEntity.Id,
                        Email = userEntity.Email,
                        FirstName = userEntity.FirstName,
                        LastName = userEntity.LastName,
                        UserType = userEntity.UserType


                    }
                };
            }
            else
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceeded = false,
                    Message = "Username of Password failed!"

                };
            }
        }

        public async Task<DepositDto> GetUserDepositAsync(ClaimsPrincipal user)
        {
            var userIdString = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null)
            {
                throw new UnauthorizedAccessException("User not authenticated.");
            }

            int userId = Convert.ToInt32(userIdString);

            // Fetch the user entity
            var userEntity = await _userRepository.GetByIdAsync(userId);
            if (userEntity == null)
            {
                throw new Exception("User not found.");
            }

            // Map to DTO
            return new DepositDto
            {
                UserId = userEntity.Id,
                DepositAmount = userEntity.Deposit
            };
        }
    }
}

