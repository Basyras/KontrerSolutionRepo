using Kontrer.Shared.Models.Pricing.Costs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kontrer.OwnerServer.PdfCreatorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController2 : ControllerBase
    {
        private readonly IConfiguration config;

        public TestController2(IConfiguration config)
        {
            this.config = config;
        }

        // GET: api/<AccommodationController>
        [HttpGet]
        public string RabbitMqUrl()
        {
            string result = "";
            Uri tempUri = config.GetServiceUri("rabbitmq");
            
            if (tempUri != null)
            {
                result += $"GetServiceUri(\"rabbitmq\"): {tempUri} \n";
            }
            tempUri = config.GetServiceUri("rabbitmq", "amqp");
            if (tempUri != null)
            {
                result += $"GetServiceUri(\"rabbitmq\",\"amqp\"): {tempUri} \n";
            }

            result += $"config.GetConnectionString(\"rabbitmq\"): {config.GetConnectionString("rabbitmq")} \n";
            result += $"config.GetConnectionString(\"rabbitmq\",\"amqp\"): {config.GetConnectionString("rabbitmq", "amqp")} \n";

            tempUri = config.GetServiceUri("rabbitmq", "ui");
            if (tempUri != null)
            {
                result += $"GetServiceUri(\"rabbitmq\",\"ui\"): {tempUri} \n";
            }
            result += $"config.GetConnectionString(\"rabbitmq\",\"ui\"): {config.GetConnectionString("rabbitmq", "ui")} \n";

            return result;
        }



        [HttpPut]
        public string OwnerServer()
        {
            var rabbitUrl = config.GetServiceUri("kontrer-ownerserver-bootstrapper");
            if (rabbitUrl != null)
            {
                return rabbitUrl.ToString();
            }
            else
            {
                return config.GetConnectionString("kontrer-ownerserver-bootstrapper");
            }

        }


    }
}
