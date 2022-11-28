using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRTicketGenerator.Shared.ControllerBases
{
    public class CustomControllerBase: ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(ResponseDto<T> res)
        {
            return new ObjectResult(res)
            {
                StatusCode = res.StatusCode
            };
        }
    }
}
