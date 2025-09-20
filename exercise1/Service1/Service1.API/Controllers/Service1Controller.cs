using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Service1.BLL.Repositories;

namespace Service1.API.Controllers
{
    [ApiController]
    [Route("/[action]")]
    public class Service1Controller : ControllerBase
    {
        private readonly IService1Repository _service1;

        public Service1Controller(IService1Repository service1)
        {
            this._service1 = service1;
        }

        [HttpGet]
        public IActionResult status()
        {
            try
            {
                var result = _service1.getStatus();
                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult log()
        {
            try
            {
                var result = _service1.getLogs();
                return Content(result, "text/plain");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult deleteLog()
        {
            try
            {
                var result = _service1.deleteLogs();
                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
