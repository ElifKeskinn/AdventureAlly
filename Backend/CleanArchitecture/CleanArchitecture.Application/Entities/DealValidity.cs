using System;
//Bu s�n�f, bir anla�man�n ge�erlilik s�resini temsil eder.
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
