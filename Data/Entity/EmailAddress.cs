namespace AddressBookApi.Data.Entity
{
    public class EmailAddress : BaseEntity
    {
        public int ContactId { get; set; }
        public string Value { get; set; }
        public int EmailTypeId { get; set; }

        public Contact Contact { get; set; }
    }
}
