using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Microservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerProductController : ControllerBase
    {
        private readonly IBus _busService;
        public CustomerProductController(IBus busService)
        {
            _busService = busService;
        }

        [HttpPost]
        public async Task<string> CreateProduct(Shared.Models.Models.CustomerProduct user)
        {
            if (user != null)
            {
                user.Date = DateTime.Now;
                user.Age = 0;
                Uri uri = new Uri("rabbitmq://localhost/productQueue");
                var endPoint = await _busService.GetSendEndpoint(uri);
                await endPoint.Send(user);
                return "true";
            }
            return "false";
        }


    }
}
