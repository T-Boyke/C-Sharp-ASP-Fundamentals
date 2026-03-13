using System;
using System.Collections.Generic;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert eine Fan-Gruppe in der Community.
/// Unterstützt hierarchische Strukturen (Sub-Gruppen) und detaillierte Berechtigungen.
/// </summary>
public class FanGroup
{
    public int FanGroupID { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    /// <summary>
    /// Gruppenbild (direkt in DB).
    /// </summary>
    public byte[]? GroupImage { get; set; }
    public string? GroupImageContentType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Einstellungen für Sichtbarkeit und Beitritt
    public bool IsPrivate { get; set; }
    public bool RequiresApproval { get; set; }

    // Hierarchie
    public int? ParentGroupID { get; set; }
    public virtual FanGroup? ParentGroup { get; set; }
    public virtual ICollection<FanGroup> SubGroups { get; set; } = new List<FanGroup>();

    // Navigation Properties
    public virtual ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
    public virtual ICollection<DiscussionThread> Threads { get; set; } = new List<DiscussionThread>();
    public virtual ICollection<MembershipRequest> JoinRequests { get; set; } = new List<MembershipRequest>();
    public virtual ICollection<GroupBan> BannedUsers { get; set; } = new List<GroupBan>();
}

/// <summary>
/// Verknüpfungstabelle zwischen User und FanGroup inklusive Rollenverteilung.
/// </summary>
public class GroupMember
{
    public int GroupMemberID { get; set; }
    
    public string UserID { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;

    public int FanGroupID { get; set; }
    public virtual FanGroup FanGroup { get; set; } = null!;

    public GroupRole Role { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Zusätzliche Flag für Editier-Rechte (falls Rolle kleiner als Moderator).
    /// </summary>
    public bool CanEditGroupContent { get; set; }
}

/// <summary>
/// Antrag auf Mitgliedschaft in einer privaten Gruppe.
/// </summary>
public class MembershipRequest
{
    public int MembershipRequestID { get; set; }
    public string UserID { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
    public int FanGroupID { get; set; }
    public virtual FanGroup FanGroup { get; set; } = null!;
    public string? Message { get; set; }
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public RequestStatus Status { get; set; } = RequestStatus.Pending;
}

/// <summary>
/// Sperre eines Benutzers innerhalb einer Gruppe.
/// </summary>
public class GroupBan
{
    public int GroupBanID { get; set; }
    public string UserID { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
    public int FanGroupID { get; set; }
    public virtual FanGroup FanGroup { get; set; } = null!;
    public string? Reason { get; set; }
    public DateTime BannedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }
}

public enum RequestStatus
{
    Pending,
    Approved,
    Declined
}

public enum GroupRole
{
    Member,
    Editor,     // Kann Beiträge bearbeiten, aber keine Mitglieder verwalten
    Moderator,  // Kann Beiträge und Mitglieder verwalten
    Owner       // Volle Kontrolle über die Gruppe
}
