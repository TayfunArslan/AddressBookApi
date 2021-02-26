namespace AddressBookApi.Data.Entity
{
    public class PhoneNumber : BaseEntity
    {
        public int UserId { get; set; }
        public string Value { get; set; }
        public int PhoneNumberType { get; set; }

        public User User { get; set; }
    }
}
