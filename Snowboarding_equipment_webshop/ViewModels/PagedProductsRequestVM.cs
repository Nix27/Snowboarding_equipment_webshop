using NuGet.Protocol.Core.Types;

namespace Snowboarding_equipment_webshop.ViewModels
{
    public class PagedProductsRequestVM
    {
        public int Size { get; set; }
        public int Page { get; set; }
        public string SortBy { get; set; } = "Name";
        public IList<string>? Categories { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public string? SearchTerm { get; set; }
    }
}
