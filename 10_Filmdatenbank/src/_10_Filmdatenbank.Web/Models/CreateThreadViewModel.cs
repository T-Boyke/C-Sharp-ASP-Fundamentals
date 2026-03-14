using System.ComponentModel.DataAnnotations;

namespace _10_Filmdatenbank.Web.Models;

public class CreateThreadViewModel
{
    public int FanGroupID { get; set; }

    [Required(ErrorMessage = "Titel ist erforderlich.")]
    [StringLength(200, ErrorMessage = "Der Titel darf maximal 200 Zeichen lang sein.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Inhalt ist erforderlich.")]
    public string Content { get; set; } = string.Empty;
}
