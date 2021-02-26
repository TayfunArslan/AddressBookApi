using System.Collections.Generic;

namespace AddressBookApi.Data.Entity
{
    public class User : BaseEntity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public ICollection<EmailAddress> EmailAddresses { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}
