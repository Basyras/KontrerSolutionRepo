using Basyc.DomainDrivenDesign.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Domain.Customer
{
    public record CreateCustomerCommand(string FirstName, string LastName, string Email) : ICommand<CreateCustomerCommandResponse>;
}