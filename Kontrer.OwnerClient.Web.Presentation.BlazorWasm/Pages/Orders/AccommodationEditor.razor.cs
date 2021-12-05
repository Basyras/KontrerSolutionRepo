using Basyc.Shared.Models.Pricing;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder.ValueObjects.Requirements;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.Web.Presentation.BlazorWasm.Pages.Orders
{
    public partial class AccommodationEditor
    {
        public string Text { get; set; }
        public float Number { get; set; }
        [Parameter] public AccommodationRequirement Accommodation { get; set; } = new AccommodationRequirement();

        private HashSet<TreeItemBase> TreeItems { get; set; } = new HashSet<TreeItemBase>();

        protected override void OnInitialized()
        {
            Accommodation.Rooms.Add(new RoomRequirement()
            {
                People = new List<PersonRequirement>()
                {
                    new PersonRequirement()
                    {
                        PersonItems = new List<ItemRequirement>()
                        {
                            new ItemRequirement()
                            {
                                ItemName="Lunch", Count = 3
                            }
                        }
                    }
                }
            });
            foreach (var room in Accommodation.Rooms)
            {
                TreeItems.Add(new TreeItemBase(room, Accommodation.Rooms.IndexOf(room)));
            }

            //TreeItems.Add(new TreeItemBase("src", Icons.Custom.FileFormats.FileCode)
            //{
            //    TreeItems = new HashSet<TreeItemBase>()
            //    {
            //        new TreeItemBase("MudBlazor.Docs", Icons.Custom.FileFormats.FileDocument)
            //        {
            //            TreeItems = new HashSet<TreeItemBase>()
            //            {
            //            new TreeItemBase( "compilerconfig.json", Icons.Custom.FileFormats.FileImage),
            //            }
            //        },
            //        new TreeItemBase("MudBlazor.Docs.Client", Icons.Filled.Folder)
            //    }
            //});
            //TreeItems.Add(new TreeItemBase("History", Icons.Filled.Folder));
        }
    }

    public class TreeItemBase
    {
        public string Text { get; init; }
        public object Item { get; init; }
        public Cash Cost { get; init; }
        public string Icon { get; init; }
        public bool IsExpanded { get; set; }
        public bool HasChild => TreeItems != null && TreeItems.Count > 0;
        public HashSet<TreeItemBase> TreeItems { get; init; } = new HashSet<TreeItemBase>();

        //Item
        public TreeItemBase(ItemRequirement item)
        {
            Icon = Icons.Filled.Fastfood;
            Item = item;
            Text = $"{item.ItemName} {item.Count}x";
            Cost = new Cash(Currencies.CZK, 500);
        }

        public TreeItemBase(RoomRequirement room, int index)
        {
            Icon = Icons.Filled.House;
            Item = room;
            Text = $"Room {index + 1}";
            foreach (var roomItem in room.RoomItems)
            {
                TreeItems.Add(new TreeItemBase(roomItem));
            }
            foreach (var person in room.People)
            {
                TreeItems.Add(new TreeItemBase(person, room.People.IndexOf(person)));
            }
        }

        public TreeItemBase(PersonRequirement person, int index)
        {
            Icon = Icons.Filled.Person;
            Item = person;
            Text = $"Person {index + 1}";
            foreach (var personItem in person.PersonItems)
            {
                TreeItems.Add(new TreeItemBase(personItem));
            }
        }
    }
}