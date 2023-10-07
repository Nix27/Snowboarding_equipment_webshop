namespace BL.DTOs
{
    public class PageProductsRequest
    {
        public float Size { get; set; }
        public int Page { get; set; }
        public string? SearchBy { get; set; }
        public string? SearchTerm { get; set; }
    }
}
