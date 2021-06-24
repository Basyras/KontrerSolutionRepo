using Kontrer.Shared.DomainDrivenDesign.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Domain.Orders
{
    public abstract class OrderEntityBase<TRequirment> : IEntity
    {
        public OrderEntityBase(int id, int customerId, TRequirment requirment, DateTime issueDate, string customerNotes, string ownerPrivateNotes)
        {
            Id = id;
            CustomerId = customerId;
            Requirment = requirment;
            IssueDate = issueDate;
            CustomerNotes = customerNotes;
            OwnerPrivateNotes = ownerPrivateNotes;
        }

        public int Id { get; }
        public int CustomerId { get; }
        public TRequirment Requirment { get; }
        public DateTime IssueDate { get; }
        public string CustomerNotes { get; }
        public string OwnerPrivateNotes { get; }
    }
}