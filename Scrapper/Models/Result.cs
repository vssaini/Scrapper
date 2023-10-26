namespace Scrapper.Models;

public class Result<T>
{
    public T Data { get; set; }
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets success message.
    /// </summary>
    public string SuccessMessage { get; set; }

    /// <summary>
    /// Gets or sets error message.
    /// </summary>
    public string ErrorMessage { get; set; }

    public bool Success => string.IsNullOrEmpty(ErrorMessage);
}