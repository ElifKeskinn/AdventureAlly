namespace CleanArchitecture.Core.Entities
{
    public class User : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }

        //Database Modified
    }
}
