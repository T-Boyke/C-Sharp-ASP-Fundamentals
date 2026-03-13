using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Erweiterte Benutzerklasse für das IAM-System.
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    /// <summary>
    /// Profilbild als Byte-Array (direkte Speicherung in DB).
    /// </summary>
    public byte[]? ProfilePicture { get; set; }
    public string? ProfilePictureContentType { get; set; }

    /// <summary>
    /// Status-Flag für die Deaktivierung durch den Admin.
    /// </summary>
    public bool IsDisabled { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    // Adressdaten
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }

    // Navigation Properties
    public virtual ICollection<GroupMember> GroupMemberships { get; set; } = new List<GroupMember>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<DiscussionThread> Threads { get; set; } = new List<DiscussionThread>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
