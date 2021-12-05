using Basyc.DomainDrivenDesign.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Domain.Orders
{
    public abstract class OrderEntityBase<TRequirment> : IEntity where TRequirment : new()
    {
        public OrderEntityBase()
        {
            Requirment = new();
        }

        public OrderEntityBase(int id, int customerId, TRequirment requirment, DateTime issueDate, string customerNotes, string ownerPrivateNotes, OrderStates state = default)
        {
            Id = id;
            CustomerId = customerId;
            Requirment = requirment;
            IssueDate = issueDate;
            CustomerNotes = customerNotes;
            OwnerPrivateNotes = ownerPrivateNotes;
            State = state;
        }

        public int Id { get; set; } //Public setter for repository library
        public int CustomerId { get; set; }
        public TRequirment Requirment { get; set; }
        public DateTime IssueDate { get; set; }
        public string CustomerNotes { get; set; }
        public string OwnerPrivateNotes { get; set; }
        public OrderStates State { get; set; }
    }
}