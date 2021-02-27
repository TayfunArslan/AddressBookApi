using System;
using System.Collections;
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
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<ContactService> _logger;
        private readonly IMapper _mapper;

        public ContactService(IUnitOfWork uow, ILogger<ContactService> logger, IMapper mapper)
        {
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
        }

        public ServiceResult<List<ContactModel>> GetAllContacts()
        {
            var serviceResult = new ServiceResult<List<ContactModel>>();

            try
            {
                var response = _uow.ContactRepository
                    .AllIncludingAsQueryable(u => u.EmailAddresses, u => u.PhoneNumbers)
                    .Where(u => u.IsActive && !u.IsDeleted)
                    .Select(u => _mapper.Map<ContactModel>(u))
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

        public ServiceResult<ContactViewModel> AddContact(ContactModel model)
        {
            var serviceResult = new ServiceResult<ContactViewModel>();
            int? errorCode = null;

            try
            {
                var contactIsExist = _uow.ContactRepository.GetSingle(c =>
                                         c.IsActive && !c.IsDeleted && c.Firstname == model.Firstname &&
                                         c.Lastname == model.Lastname) !=
                                     null;

                if (contactIsExist)
                {
                    errorCode = (int)ErrorCodes.ContactIsExist;
                    throw new Exception("Contact is exist");
                }

                var user = new Contact()
                {
                    IsActive = true,
                    IsDeleted = false,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname
                };

                _uow.ContactRepository.Add(user);

                model.PhoneNumbers.ForEach(p =>
                {
                    var phoneNumber = new PhoneNumber()
                    {
                        IsActive = true,
                        IsDeleted = false,
                        PhoneNumberType = p.PhoneNumberType,
                        Value = p.Value,
                        Contact = user
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
                        Contact = user
                    };

                    _uow.EmailAddressRepository.Add(emailAddress);
                });

                var saveResult = _uow.SaveChanges();

                if (saveResult == -1)
                {
                    errorCode = (int)ErrorCodes.DbError;
                    throw new Exception("Db save error");
                }

                serviceResult.Data = _mapper.Map<ContactViewModel>(user);
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

        public ServiceResult<ContactViewModel> UpdateContact(ContactModel model)
        {
            var serviceResult = new ServiceResult<ContactViewModel>();
            int? errorCode = null;

            try
            {
                var contact = _uow.ContactRepository.GetSingle(c => c.IsActive && !c.IsDeleted && c.Id == model.Id);

                if (contact == null)
                {
                    errorCode = (int)ErrorCodes.ContactNotFound;
                    throw new Exception("Contact not found");
                }

                if (!string.IsNullOrEmpty(model.Firstname))
                    contact.Firstname = model.Firstname;

                if (!string.IsNullOrEmpty(model.Lastname))
                    contact.Lastname = model.Lastname;

                _uow.ContactRepository.Update(contact);

                //TODO Email ve telefon no kontrolü yapılacak.

                var emails = _uow.EmailAddressRepository
                    .FindByAsQueryable(e => e.ContactId == model.Id && e.IsActive && !e.IsDeleted).ToList();

                emails.ForEach(e =>
                {
                    e.IsActive = false;
                    e.IsDeleted = true;

                    _uow.EmailAddressRepository.Update(e);
                });

                model.EmailAddresses.ForEach(e =>
                {
                    _uow.EmailAddressRepository.Add(new EmailAddress()
                    {
                        Contact = contact,
                        EmailTypeId = e.EmailTypeId,
                        Value = e.Value,
                        IsActive = true,
                        IsDeleted = false
                    });
                });

                var phones = _uow.PhoneNumberRepository
                    .FindByAsQueryable(e => e.ContactId == model.Id && e.IsActive && !e.IsDeleted).ToList();

                phones.ForEach(e =>
                {
                    e.IsActive = false;
                    e.IsDeleted = true;

                    _uow.PhoneNumberRepository.Update(e);
                });

                model.PhoneNumbers.ForEach(e =>
                {
                    _uow.PhoneNumberRepository.Add(new PhoneNumber()
                    {
                        Contact = contact,
                        PhoneNumberType = e.PhoneNumberType,
                        Value = e.Value,
                        IsActive = true,
                        IsDeleted = false
                    });
                });

                var isSaved = _uow.SaveChanges();

                if (isSaved == -1)
                {
                    errorCode = (int)ErrorCodes.DbError;
                    throw new Exception("Db error");
                }

                serviceResult.Data = _mapper.Map<ContactViewModel>(contact);
                serviceResult.ServiceResultType = ServiceResultType.Success;
            }
            catch (Exception e)
            {
                errorCode ??= (int)ErrorCodes.UnknownError;

                serviceResult.ErrorModel = new ErrorModel()
                {
                    ErrorCode = errorCode,
                    ErrorMessage = e.Message
                };
                serviceResult.ServiceResultType = ServiceResultType.Fail;
            }

            return serviceResult;
        }

        public ServiceResult<bool> DeleteContact(int contactId)
        {
            var serviceResult = new ServiceResult<bool>();
            int? errorCode = null;

            try
            {
                var contact =
                    _uow.ContactRepository.AllIncludingAsQueryable(c => c.EmailAddresses, c => c.PhoneNumbers).FirstOrDefault(c =>
                         c.Id == contactId && c.IsActive && !c.IsDeleted);

                if (contact == null)
                {
                    errorCode = (int)ErrorCodes.ContactNotFound;
                    throw new Exception("Contact not found");
                }

                contact.IsDeleted = true;
                contact.IsActive = false;

                contact.EmailAddresses.ToList().ForEach(e =>
                {
                    e.IsDeleted = true;
                    e.IsActive = false;
                });

                contact.PhoneNumbers.ToList().ForEach(p =>
                {
                    p.IsDeleted = true;
                    p.IsActive = false;
                });

                _uow.ContactRepository.Update(contact);

                var result = _uow.SaveChanges();

                if (result == -1)
                {
                    errorCode = (int)ErrorCodes.DbError;
                    throw new Exception("Db error");
                }

                serviceResult.Data = true;
                serviceResult.ServiceResultType = ServiceResultType.Success;
            }
            catch (Exception e)
            {
                serviceResult.Data = false;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                serviceResult.ErrorModel = new ErrorModel()
                {
                    ErrorCode = errorCode,
                    ErrorMessage = e.Message
                };
            }

            return serviceResult;
        }

        public ServiceResult<ContactModel> GetContactById(int id)
        {
            var serviceResult = new ServiceResult<ContactModel>();
            int? errorCode = null;

            try
            {
                var contact = _uow.ContactRepository.AllIncludingAsQueryable(c => c.EmailAddresses, c => c.PhoneNumbers)
                    .FirstOrDefault(c => c.Id == id);

                if (contact == null)
                {
                    errorCode = (int)ErrorCodes.ContactNotFound;
                    throw new Exception("Contact not found");
                }

                serviceResult.Data = _mapper.Map<ContactModel>(contact);
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

        public ServiceResult<List<ContactModel>> GetFilteredContacts(ContactFilterModel model)
        {
            var serviceResult = new ServiceResult<List<ContactModel>>();

            try
            {
                var contacts = _uow.ContactRepository.AllIncludingAsQueryable(c => c.EmailAddresses, c => c.PhoneNumbers).Where(c => c.IsActive && !c.IsDeleted);

                if (!string.IsNullOrEmpty(model.Firstname))
                    contacts = contacts.Where(c => c.Firstname == model.Firstname);

                if (!string.IsNullOrEmpty(model.Lastname))
                    contacts = contacts.Where(c => c.Lastname == model.Lastname);

                if (!string.IsNullOrEmpty(model.EmailAddress))
                    contacts = contacts.Where(c => c.EmailAddresses.Select(e => e.Value).Contains(model.EmailAddress));

                if (!string.IsNullOrEmpty(model.PhoneNumber))
                    contacts = contacts.Where(c => c.PhoneNumbers.Select(p => p.Value).Contains(model.PhoneNumber));

                serviceResult.Data = _mapper.Map<List<ContactModel>>(contacts.ToList());
                serviceResult.ServiceResultType = ServiceResultType.Success;
            }
            catch (Exception e)
            {
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                serviceResult.ErrorModel = new ErrorModel()
                {
                    ErrorCode = (int)ErrorCodes.UnknownError,
                    ErrorMessage = e.Message
                };
            }

            return serviceResult;
        }
    }
}
