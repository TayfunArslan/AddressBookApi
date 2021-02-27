using System;
using Microsoft.AspNetCore.Mvc;
using AddressBookApi.Enum;
using AddressBookApi.Service;
using AddressBookApi.ViewModel.RequestModel;

namespace AddressBookApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly IContactService _contactService;

        public UserController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost("[action]")]
        public IActionResult Save([FromBody] ContactModel model)
        {
            var serviceResult = _contactService.AddContact(model);

            return ReturnActionResult(serviceResult);
        }

        [HttpPost("[action]")]
        public IActionResult UpdateContact([FromBody] ContactModel model)
        {
            var serviceResult = _contactService.UpdateContact(model);

            return ReturnActionResult(serviceResult);
        }

        [HttpPost("[action]")]
        public IActionResult DeleteContact(int id)
        {
            var serviceResult = _contactService.DeleteContact(id);

            return ReturnActionResult(serviceResult);
        }

        [HttpGet("[action]")]
        public IActionResult GetAllContacts()
        {
            var serviceResult = _contactService.GetAllContacts();

            return ReturnActionResult(serviceResult);
        }

        [HttpGet("[action]")]
        public IActionResult GetContactById(int id)
        {
            var serviceResult = _contactService.GetContactById(id);

            return ReturnActionResult(serviceResult);
        }

        [HttpGet("[action]")]
        public IActionResult GetFilteredContacts([FromBody] ContactFilterModel model)
        {
            var serviceResult = new ServiceResult<bool>();

            return ReturnActionResult(serviceResult);
        }
    }
}
