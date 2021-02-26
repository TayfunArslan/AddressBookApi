using System.Collections.Generic;
using AddressBookApi.ViewModel;
using AddressBookApi.ViewModel.RequestModel;

namespace AddressBookApi.Service
{
    public interface IUserService
    {
        ServiceResult<List<UserModel>> GetAll();
        ServiceResult<UserViewModel> AddUser(UserModel model);
    }
}
