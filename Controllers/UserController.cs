using Microsoft.AspNetCore.Mvc;
using AddressBookApi.Enum;
using AddressBookApi.Service;
using AddressBookApi.ViewModel.RequestModel;

namespace AddressBookApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("[action]")]
        public IActionResult Save([FromBody] UserModel model)
        {
            var serviceResult = _userService.AddUser(model);

            if (serviceResult.ServiceResultType == ServiceResultType.Fail)
                return BadRequest(serviceResult.ErrorModel);
            
            return Ok(serviceResult.Data);
        }

        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var serviceResult = _userService.GetAll();

            if (serviceResult.ServiceResultType == ServiceResultType.Fail)
                return BadRequest(serviceResult.ErrorModel);

            return Ok(serviceResult.Data);
        }
    }
}
