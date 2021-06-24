﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Dtos.Models
{
    public class GenericOrderModel<T>
    {
        public GenericOrderModel(int orderId, int customerId, T blueprint, DateTime issueDate, OrderStates state, CultureInfo culture, string customerNotes = null, string ownerPrivateNotes = null)
        {
            OrderId = orderId;
            CustomerId = customerId;
            Blueprint = blueprint;
            CreationDate = issueDate;
            CustomerNotes = customerNotes;
            OwnerPrivateNotes = ownerPrivateNotes;
            State = state;
            Culture = culture;
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public T Blueprint { get; set; }
        public DateTime CreationDate { get; set; }
        public OrderStates State { get; set; }
        public string CustomerNotes { get; set; }
        public string OwnerPrivateNotes { get; set; }
        public CultureInfo Culture { get; set; }
    }
}