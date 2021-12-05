using Basyc.Shared.Models.Pricing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Kontrer.OwnerServer.OrderService.Dtos.Models.Blueprints
{
    public class AccommodationBlueprint
    {
        public AccommodationBlueprint()
        {
        }

        public AccommodationBlueprint(Currencies currency, DateTime start, DateTime end, int customerId, List<RoomBlueprint> rooms = null, List<ItemBlueprint> contractItems = null, string customersNotes = null)
        {
            Currency = currency;
            From = start;
            To = end;
            CustomerId = customerId;
            Rooms = rooms ?? new List<RoomBlueprint>();
            AccommodationItems = contractItems ?? new List<ItemBlueprint>();
        }

        public AccommodationBlueprint(Currencies currency, DateTime start, DateTime end, int customerId, Cash deposit, DateTime depositDeadline, List<RoomBlueprint> rooms = null, List<ItemBlueprint> contractItems = null, string customersNotes = null)
        {
            Currency = currency;
            From = start;
            To = end;
            CustomerId = customerId;
            Deposit = deposit;
            DepositDeadline = depositDeadline;
            Rooms = rooms ?? new List<RoomBlueprint>();
            AccommodationItems = contractItems ?? new List<ItemBlueprint>();
        }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        /// <summary>
        /// Null when deposit is not required
        /// </summary>
        public Cash Deposit { get; set; }

        /// <summary>
        /// Null when deposit is not required
        /// </summary>
        public DateTime? DepositDeadline { get; set; }

        public List<RoomBlueprint> Rooms { get; set; } = new List<RoomBlueprint>();

        public List<ItemBlueprint> AccommodationItems { get; set; } = new List<ItemBlueprint>();

        public Currencies Currency { get; set; }
        public List<DiscountBlueprint> Discounts { get; set; } = new List<DiscountBlueprint>();

        public int CustomerId { get; set; }
    }
}