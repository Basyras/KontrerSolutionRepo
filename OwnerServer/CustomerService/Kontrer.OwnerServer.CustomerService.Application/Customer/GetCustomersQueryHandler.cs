using Kontrer.OwnerServer.CustomerService.Application.Interfaces;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Kontrer.Shared.DomainDrivenDesign.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Application.Customer
{
    public class GetCustomersQueryHandler : QueryHandlerBase<GetCustomersQuery, GetCustomersQueryResponse>
    {
        private readonly ICustomerRepository repository;

        public GetCustomersQueryHandler(ICustomerRepository repository)
        {
            this.repository = repository;
        }

        public async override Task<GetCustomersQueryResponse> Handle(GetCustomersQuery command, CancellationToken cancellationToken = default)
        {
            var customers = await repository.GetByIdsAsync(command.CustomerIds);
            var response = new GetCustomersQueryResponse(customers);
            return response;
        }
    }
}