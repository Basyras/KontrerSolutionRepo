using Basyc.DomainDrivenDesign.Domain;
using System;

namespace Kontrer.OwnerServer.IdGeneratorService.Domain
{
    public record CreateNewIdCommand(string groupName) : ICommand<GetNewIdResponse>;
}