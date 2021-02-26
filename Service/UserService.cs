using System;
using System.Collections.Generic;
using System.Linq;
using AddressBookApi.Data;
using AddressBookApi.Data.Entity;
using AddressBookApi.Enum;
using AddressBookApi.ViewModel;
using AddressBookApi.ViewModel.RequestModel;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AddressBookApi.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork uow, ILogger<UserService> logger, IMapper mapper)
        {
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
        }

        public ServiceResult<List<UserModel>> GetAll()
        {
            var serviceResult = new ServiceResult<List<UserModel>>();

            try
            {
                //using var uow = new UnitOfWork(_context);
                var response = _uow.UserRepository
                    .AllIncludingAsQueryable(u => u.EmailAddresses, u => u.PhoneNumbers)
                    .Where(u => u.IsActive && !u.IsDeleted)
                    .Select(u => _mapper.Map<UserModel>(u))
                    .ToList();

                serviceResult.Data = response;
                serviceResult.ServiceResultType = ServiceResultType.Success;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                serviceResult.ErrorModel = new ErrorModel()
                {
                    ErrorCode = 1,
                    ErrorMessage = e.Message
                };
            }

            return serviceResult;
        }

        public ServiceResult<UserViewModel> AddUser(UserModel model)
        {
            var serviceResult = new ServiceResult<UserViewModel>();
            int? errorCode = null;

            try
            {
                var user = new User()
                {
                    IsActive = true,
                    IsDeleted = false,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname
                };

                _uow.UserRepository.Add(user);

                model.PhoneNumbers.ForEach(p =>
                {
                    var phoneNumber = new PhoneNumber()
                    {
                        IsActive = true,
                        IsDeleted = false,
                        PhoneNumberType = p.PhoneNumberType,
                        Value = p.Value,
                        User = user
                    };

                    _uow.PhoneNumberRepository.Add(phoneNumber);
                });

                model.EmailAddresses.ForEach(e =>
                {
                    var emailAddress = new EmailAddress()
                    {
                        IsActive = true,
                        IsDeleted = false,
                        EmailTypeId = e.EmailTypeId,
                        Value = e.Value,
                        User = user
                    };

                    _uow.EmailAddressRepository.Add(emailAddress);
                });

                var saveResult = _uow.SaveChanges();

                if (saveResult == -1)
                {
                    errorCode = (int)ErrorCodes.DbError;
                    throw new Exception("Db save error");
                }

                serviceResult.Data = _mapper.Map<UserViewModel>(user);
                serviceResult.ServiceResultType = ServiceResultType.Success;
            }
            catch (Exception e)
            {
                errorCode ??= (int)ErrorCodes.UnknownError;

                serviceResult.ServiceResultType = ServiceResultType.Fail;
                serviceResult.ErrorModel = new ErrorModel()
                {
                    ErrorCode = errorCode,
                    ErrorMessage = e.Message
                };
            }

            return serviceResult;
        }

    }
}
