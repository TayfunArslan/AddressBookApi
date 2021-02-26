namespace AddressBookApi.Data.Entity
{
    public class EmailAddress : BaseEntity
    {
        public int UserId { get; set; }
        public string Value { get; set; }
        public int EmailTypeId { get; set; }

        public User User { get; set; }
    }
}
