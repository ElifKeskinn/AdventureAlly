namespace CleanArchitecture.Core.Entities
{
    //// NotificationPreferences sýnýfý, kullanýcýlarýn bildirim tercihlerini temsil eder ve kullanýcýlarýn hangi tür bildirimleri almayý tercih ettiklerini belirtir.

    public class NotificationPreferences : AuditableBaseEntity
    {
    
        public bool ReceiveDealNotifications { get; set; }
        public bool ReceiveEventNotifications { get; set; }
    }
}
