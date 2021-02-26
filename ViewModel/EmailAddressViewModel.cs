using System;

namespace AddressBookApi.ViewModel
{
    public class EmailAddressViewModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int UserId { get; set; }
        public string Value { get; set; }
        public int EmailTypeId { get; set; }
    }
}
