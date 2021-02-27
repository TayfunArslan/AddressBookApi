using System;
using System.Transactions;
using AddressBookApi.Data.Entity;
using AddressBookApi.Data.Repository;

namespace AddressBookApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IRepository<Contact> _contactRepository;
        private IRepository<PhoneNumber> _phoneNumberRepository;
        private IRepository<EmailAddress> _emailAddressRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<Contact> ContactRepository
        {
            get { return _contactRepository ??= new Repository<Contact>(_context); }
        }

        public IRepository<PhoneNumber> PhoneNumberRepository
        {
            get { return _phoneNumberRepository ??= new Repository<PhoneNumber>(_context); }
        }

        public IRepository<EmailAddress> EmailAddressRepository
        {
            get { return _emailAddressRepository ??= new Repository<EmailAddress>(_context); }
        }

        public int SaveChanges()
        {
            try
            {
                using var tScope = new TransactionScope();

                _context.SaveChanges();
                tScope.Complete();
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
