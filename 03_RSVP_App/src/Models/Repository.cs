namespace Unit_03_RSVP_App.Models;

/// <summary>
/// A simple static repository to store guest responses in-memory.
/// This demonstrates data persistence within a single application session.
/// </summary>
public static class Repository
{
    private static List<GuestResponse> responses = new();

    /// <summary>
    /// Gets all the responses that have been submitted.
    /// </summary>
    public static IEnumerable<GuestResponse> Responses => responses;

    /// <summary>
    /// Adds a new guest response to the collection.
    /// </summary>
    /// <param name="response">The response to add.</param>
    public static void AddResponse(GuestResponse response)
    {
        responses.Add(response);
    }
}
