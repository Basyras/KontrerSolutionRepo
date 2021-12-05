﻿using Kontrer.OwnerServer.IdGeneratorService.Domain;
using Basyc.DomainDrivenDesign.Domain;
using Basyc.MessageBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandBox.AspApiApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMessageBusClient messageBus;

        public List<Type> Messages { get; set; } = new List<Type>();

        public IndexModel(ILogger<IndexModel> logger, IMessageBusClient messageBus)
        {
            _logger = logger;
            this.messageBus = messageBus;
            Messages.Add(typeof(CreateNewIdCommand));
            Messages.Add(typeof(CreateNewIdCommand));
        }

        public void OnGet()
        {
        }

        public void OnPostSend(string assemblyName)
        {
            var type = Type.GetType(assemblyName);
            //this.Request.Form.
            var command = Activator.CreateInstance(type, "mygroup");
            messageBus.SendAsync(type, command);
        }
    }
}