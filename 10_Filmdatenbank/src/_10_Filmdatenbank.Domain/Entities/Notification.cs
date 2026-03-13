using System;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Benachrichtigung für den Benutzer (z.B. Antworten auf Kommentare, Administrator-Hinweise).
/// </summary>
public class Notification
{
    public int NotificationID { get; set; }
    public string Message { get; set; } = null!;
    
    /// <summary>
    /// Ziel-Link für die Notification (z.B. zum entsprechenden Thread).
    /// </summary>
    public string? TargetUrl { get; set; }
    
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string UserID { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
    
    public NotificationType Type { get; set; }
}

public enum NotificationType
{
    General,
    CommentReply,
    GroupInvite,
    AdminAlert
}
