namespace CleanArchitecture.Core.Entities
{
    //// NotificationPreferences s�n�f�, kullan�c�lar�n bildirim tercihlerini temsil eder ve kullan�c�lar�n hangi t�r bildirimleri almay� tercih ettiklerini belirtir.

    public class NotificationPreferences : AuditableBaseEntity
    {
    
        public bool ReceiveDealNotifications { get; set; }
        public bool ReceiveEventNotifications { get; set; }
    }
}
