namespace Unit_02_Hello_MVC.Models;

/// <summary>
/// A simple ViewModel to demonstrate data transfer from Controller to View.
/// In MVC, the Model represents the data or state of the application.
/// </summary>
public class WelcomeViewModel
{
    /// <summary>
    /// The message to be displayed in the view.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// The timestamp of when the message was generated.
    /// </summary>
    public DateTime GeneratedAt { get; set; }

    /// <summary>
    /// A flag to demonstrate conditional rendering in Razor.
    /// </summary>
    public bool IsPriority { get; set; }
}
