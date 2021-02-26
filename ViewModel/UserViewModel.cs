using System;
using System.Collections.Generic;

namespace AddressBookApi.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public ICollection<EmailAddressViewModel> EmailAddresses { get; set; }
        public ICollection<PhoneNumberViewModel> PhoneNumbers { get; set; }
    }
}
