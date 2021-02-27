using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressBookApi.Enum;
using AddressBookApi.Service;

namespace AddressBookApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult ReturnActionResult<T>(ServiceResult<T> serviceResult)
        {
            if (serviceResult.ServiceResultType == ServiceResultType.Fail)
                return BadRequest(serviceResult.ErrorModel);

            return Ok(serviceResult.Data);
        }
    }
}
