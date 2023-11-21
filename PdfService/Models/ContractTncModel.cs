namespace PdfService.Models
{
    public class ContractTncModel
    {
        private static string MISSING = "-";
        public string TncLink { get; set; } = MISSING;
        public string TncHash { get; set; } = MISSING;
    }
}
