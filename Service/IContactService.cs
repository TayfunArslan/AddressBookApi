using System.Collections.Generic;
using AddressBookApi.ViewModel;
using AddressBookApi.ViewModel.RequestModel;

namespace AddressBookApi.Service
{
    public interface IContactService
    {
        ServiceResult<List<ContactModel>> GetAllContacts();
        ServiceResult<ContactViewModel> AddContact(ContactModel model);
        ServiceResult<ContactViewModel> UpdateContact(ContactModel model);
        ServiceResult<bool> DeleteContact(int contactId);
        ServiceResult<ContactModel> GetContactById(int id);
        ServiceResult<List<ContactModel>> GetFilteredContacts(ContactFilterModel model);
    }
}
