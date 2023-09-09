namespace Snowboarding_equipment_webshop.ViewModels
{
    public class FilterCustomerProductsRequestVM
    {
        public IList<string>? Categories { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
    }
}
