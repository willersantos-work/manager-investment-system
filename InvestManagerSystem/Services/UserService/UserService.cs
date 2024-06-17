using Coravel.Events.Interfaces;
using InvestManagerSystem.Enums;
using InvestManagerSystem.Global.Helpers.CustomException;
using InvestManagerSystem.Global.Helpers.Hash;
using InvestManagerSystem.Global.Mapper;
using InvestManagerSystem.Interfaces.Auth;
using InvestManagerSystem.Interfaces.User;
using InvestManagerSystem.Models;
using InvestManagerSystem.Repositories.UserRepository;
using System.Net;
using System.Text.Json;

namespace InvestManagerSystem.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _repository;

        public UserService(ILogger<UserService> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public UserSaveResponseDto VerifyCredential(CredentialDto credential, UserTypeEnum userType)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(VerifyCredential)} - Request - {credential.Email} {userType}");
                var user = _repository.FindByEmail(credential.Email);

                if (user is null)
                {
                    _logger.LogWarning($"Invalid credentials.");
                    throw new CustomException(HttpStatusCode.Unauthorized, "Invalid credentials.");
                }

                bool isValidPassword = credential.Password.Verify(user.HashPassword);

                if (!isValidPassword)
                {
                    _logger.LogWarning($"Invalid credentials.");
                    throw new CustomException(HttpStatusCode.Unauthorized, "Invalid credentials.");
                }

                if (user.Type is not UserTypeEnum.Admin && user.Type != userType)
                {
                    _logger.LogWarning($"Invalid credentials.");
                    throw new CustomException(HttpStatusCode.Forbidden, "User without permission for this login.");
                }

                var responseDto = new UserSaveResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.Fullname,
                    Type = userType
                };
                
                _logger.LogInformation($"end service {nameof(VerifyCredential)} - Response - {JsonSerializer.Serialize(responseDto)}");
                
                return responseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(VerifyCredential)} - Error - {credential.Email} {userType}");
                throw;
            }
        }

        public UserSaveResponseDto GetByEmail(string email)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(GetByEmail)} - Request - {email}");
                var user = _repository.FindByEmail(email);

                if (user is null)
                {
                    _logger.LogWarning($"User with email {email} not found.");
                    throw new CustomException(HttpStatusCode.NotFound, $"User with email {email} not found.");
                }

                var response = Mapper.Map<User, UserSaveResponseDto>(user);
                _logger.LogInformation($"end service {nameof(GetByEmail)} - Response - {JsonSerializer.Serialize(response)}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(GetByEmail)} - Error - {email}");
                throw;
            }
        }

        public IList<UserSaveResponseDto> GetByType(UserTypeEnum type)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(GetByType)} - Request - {type}");
                var users = _repository.FindByType(type);
                var response = Mapper.Map<IList<User>, IList<UserSaveResponseDto>>(users);
                _logger.LogInformation($"end service {nameof(GetByType)} - Response - {JsonSerializer.Serialize(response)}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(GetByType)} - Error - {type}");
                throw;
            }
        }

        public UserSaveResponseDto Register(UserCreateDto newUser, UserTypeEnum type)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(Register)} - Request - {JsonSerializer.Serialize(newUser)} {type}");
                var userAlreadyExist = _repository.FindByEmail(newUser.Email);

                if (userAlreadyExist is not null)
                {
                    _logger.LogWarning($"User with email {newUser.Email} already exist.");
                    throw new CustomException(HttpStatusCode.Conflict, $"User with email {newUser.Email} already exist.");
                }

                var hashedPassword = newUser.Password.Hash();

                var user = new UserSaveDto

                {
                    FullName = newUser.FullName,
                    Email = newUser.Email,
                    HashPassword = hashedPassword,
                    Type = type
                };

                User userEntity = Mapper.Map<UserSaveDto, User>(user);

                _repository.Save(userEntity);

                UserSaveResponseDto savedUser = new UserSaveResponseDto
                {
                    FullName = newUser.FullName,
                    Email = newUser.Email,
                    Id = userEntity.Id
                };
                _logger.LogInformation($"end service {nameof(Register)} - Response - {JsonSerializer.Serialize(savedUser)}");
                return savedUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(Register)} - Error - {JsonSerializer.Serialize(newUser)}");
                throw;
            }
        }
    }
}