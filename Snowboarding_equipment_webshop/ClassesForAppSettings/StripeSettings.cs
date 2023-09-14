namespace Snowboarding_equipment_webshop.ClassesForAppSettings
{
    internal class StripeSettings
    {
        public static readonly string sectionName = "Stripe"; 
        public string PublishableKey { get; set; }
        public string SecretKey { get; set; }
    }
}
