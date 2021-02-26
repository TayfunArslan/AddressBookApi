using AddressBookApi.Data.Entity;
using AddressBookApi.ViewModel;
using AddressBookApi.ViewModel.RequestModel;
using AutoMapper;

namespace AddressBookApi.Util
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
            CreateMap<EmailAddress, EmailAddressViewModel>();
            CreateMap<EmailAddressViewModel, EmailAddress>();
            CreateMap<PhoneNumber, PhoneNumberViewModel>();
            CreateMap<PhoneNumberViewModel, PhoneNumber>();
            CreateMap<User, UserModel>();
        }
    }
}
