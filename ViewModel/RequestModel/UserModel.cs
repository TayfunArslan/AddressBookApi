using System.Collections.Generic;

namespace AddressBookApi.ViewModel.RequestModel
{
    public class UserModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public List<EmailAddressViewModel> EmailAddresses { get; set; }
        public List<PhoneNumberViewModel> PhoneNumbers { get; set; }
    }
}
