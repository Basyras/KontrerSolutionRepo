using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.Web.Presentation.BlazorWasm.Shared.Labels
{
    public partial class StatusLabel
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public Statuses Status { get; set; } = Statuses.Ok;

        private Dictionary<string, object> containerAttributes { get; set; } =
        new()
        {
            { "class", "statusLabelContainer" },
        };

        protected override void OnParametersSet()
        {
            string classCode = Status switch
            {
                Statuses.Ok => "Ok",
                Statuses.Error => "Error",
                Statuses.Info => "Info",
                _ => "",
            };
            containerAttributes["class"] = $"statusLabelContainer statusLabelContainer--{classCode}";
            base.OnParametersSet();
        }
    }

    public enum Statuses
    {
        Ok,
        Error,
        Info,
    }
}