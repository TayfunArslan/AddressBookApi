namespace AddressBookApi.Data.Entity
{
    public class PhoneNumber : BaseEntity
    {
        public int ContactId { get; set; }
        public string Value { get; set; }
        public int PhoneNumberType { get; set; }

        public Contact Contact { get; set; }
    }
}
