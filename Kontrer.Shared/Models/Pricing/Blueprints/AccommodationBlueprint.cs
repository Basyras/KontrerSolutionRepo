using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Kontrer.Shared.Models.Pricing.Blueprints
{
    public class AccommodationBlueprint
    {    


        public AccommodationBlueprint(Currencies currency,DateTime start, DateTime end, List<RoomBlueprint> rooms = null, List<ItemBlueprint> contractItems = null, string customersNotes = null) 
        {
            Currency = currency;
            Start = start;
            End = end;
            Rooms = rooms ?? new List<RoomBlueprint>();
            AccommodationItems = contractItems ?? new List<ItemBlueprint>();
            CustomersNotes = customersNotes;
        }

        public AccommodationBlueprint(Currencies currency,DateTime start, DateTime end, Cash deposit, DateTime depositDeadline, List<RoomBlueprint> rooms = null, List<ItemBlueprint> contractItems = null, string customersNotes = null)
        {
            Currency = currency;
            Start = start;
            End = end;
            Deposit = deposit;
            DepositDeadline = depositDeadline;
            Rooms = rooms ?? new List<RoomBlueprint>();
            AccommodationItems = contractItems ?? new List<ItemBlueprint>();
            CustomersNotes = customersNotes;
        }

        public string CustomersNotes { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
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


    }
}
