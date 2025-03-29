using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFramework.Filters;

namespace WebFramework.Api
{

    [ApiController]
    [AllowAnonymous]
    [ApiResultFilter]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[ApiVersion("1.0")]
    public class BaseController : ControllerBase
    {
        public bool UserIsAutheticated => User.Identity.IsAuthenticated;
    }
}
