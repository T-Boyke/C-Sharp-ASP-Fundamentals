using System;
using System.Collections.Generic;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert ein Achievement oder Abzeichen, das ein User verdienen kann.
/// </summary>
public class Achievement
{
    public int AchievementID { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string IconClass { get; set; } = "fa-solid fa-medal";
    public string ColorHex { get; set; } = "#EAB308"; // Default Gold
    
    public virtual ICollection<UserAchievement> EarnedBy { get; set; } = new List<UserAchievement>();
}

/// <summary>
/// Verknüpfungstabelle für verliehene Achievements.
/// </summary>
public class UserAchievement
{
    public int UserAchievementID { get; set; }
    public string UserID { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
    
    public int AchievementID { get; set; }
    public virtual Achievement Achievement { get; set; } = null!;
    
    public DateTime EarnedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Verknüpfung für Lieblingsfilme eines Users.
/// </summary>
public class FavoriteFilm
{
    public int FavoriteFilmID { get; set; }
    public string UserID { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
    
    public int FilmID { get; set; }
    public virtual Film Film { get; set; } = null!;
    
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public int DisplayOrder { get; set; } // Für die Sortierung im Profil
}
