using System;
using System.Collections.Generic;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Ein Diskussionsthema innerhalb einer Fan-Gruppe.
/// </summary>
public class DiscussionThread
{
    public int ThreadID { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActivity { get; set; } = DateTime.UtcNow;

    public string AuthorID { get; set; } = null!;
    public virtual ApplicationUser? Author { get; set; }

    public int FanGroupID { get; set; }
    public virtual FanGroup? FanGroup { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}

/// <summary>
/// Ein Kommentar zu einem Thread. Unterstützt verschachtelte Antworten.
/// </summary>
public class Comment
{
    public int CommentID { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string AuthorID { get; set; } = null!;
    public virtual ApplicationUser? Author { get; set; }

    public int ThreadID { get; set; }
    public virtual DiscussionThread? Thread { get; set; }

    // Hierarchische Kommentare (Antworten)
    public int? ParentCommentID { get; set; }
    public virtual Comment? ParentComment { get; set; }
    public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
}
