using System.Collections.Generic;

namespace AddressBookApi.ViewModel.RequestModel
{
    public class ContactModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public List<EmailAddressViewModel> EmailAddresses { get; set; }
        public List<PhoneNumberViewModel> PhoneNumbers { get; set; }
    }
}
