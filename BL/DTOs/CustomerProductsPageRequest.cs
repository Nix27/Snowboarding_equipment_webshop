namespace BL.DTOs
{
    public class CustomerProductsPageRequest
    {
        public int Page { get; set; }
        public float Size { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string SortBy { get; set; }
        public string? SearchTerm { get; set; }
    }
}
