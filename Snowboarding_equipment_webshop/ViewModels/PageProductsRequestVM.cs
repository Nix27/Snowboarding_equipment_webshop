using NuGet.Protocol.Core.Types;

namespace Snowboarding_equipment_webshop.ViewModels
{
    public class PageProductsRequestVM
    {
        public float Size { get; set; }
        public int Page { get; set; }
        public string SortBy { get; set; } = "Name";
        public string? SearchBy { get; set; }
        public string? SearchTerm { get; set; }
    }
}
