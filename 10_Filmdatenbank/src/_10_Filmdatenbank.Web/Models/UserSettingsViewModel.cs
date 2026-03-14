using System.ComponentModel.DataAnnotations;

namespace _10_Filmdatenbank.Web.Models;

public class UserSettingsViewModel
{
    [Display(Name = "Theme")]
    public string Theme { get; set; } = "default";

    [Display(Name = "Preferred Language")]
    public string PreferredLanguage { get; set; } = "de";

    public bool EnableNotifications { get; set; } = true;
}

public class UserSettingsData
{
    public string Theme { get; set; } = "default";
    public string PreferredLanguage { get; set; } = "de";
    public bool EnableNotifications { get; set; } = true;
}
