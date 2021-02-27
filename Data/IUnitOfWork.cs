using System;
using AddressBookApi.Data.Entity;
using AddressBookApi.Data.Repository;

namespace AddressBookApi.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Contact> ContactRepository { get; }
        IRepository<PhoneNumber> PhoneNumberRepository { get; }
        IRepository<EmailAddress> EmailAddressRepository { get; }
        int SaveChanges();
    }
}
