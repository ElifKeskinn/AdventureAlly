using System;
//Bu sýnýf, bir anlaþmanýn geçerlilik süresini temsil eder.
namespace CleanArchitecture.Core.Entities
{
    public class DealValidity : AuditableBaseEntity
    {
         public int DealId { get; set; }  
        public Deal Deal { get; set; }
        public DateTime ValidStartDate { get; set; }
        public DateTime ValidEndDate { get; set; }
        public bool IsValid(DateTime dateToCheck)
        {
            return dateToCheck >= ValidStartDate && dateToCheck <= ValidEndDate;
        }
    }
}
