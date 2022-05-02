using Microsoft.AspNetCore.Components;

namespace Kontrer.OwnerClient.Web.Presentation.BlazorWasm.Shared.Containers
{
    public partial class LayoutSection
    {
        [Parameter] public string Heading { get; set; }
        [Parameter] public RenderFragment AdditionalHeading { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public bool RemoveBorder { get; set; } = false;
        private string layoutSectionMainBorderClass => $"layoutSectionMainBorder--" + (RemoveBorder ? "simple" : "bordered");
    }
}
