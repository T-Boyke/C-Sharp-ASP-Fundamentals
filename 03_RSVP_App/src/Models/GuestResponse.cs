using System.ComponentModel.DataAnnotations;

namespace Unit_03_RSVP_App.Models;

/// <summary>
/// Model representing a guest's response to the RSVP invitation.
/// This model demonstrates the use of Data Annotations for validation.
/// </summary>
public class GuestResponse
{
    [Required(ErrorMessage = "Please enter your name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your email address")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your phone number")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please specify whether you will attend")]
    public bool? WillAttend { get; set; }
}
