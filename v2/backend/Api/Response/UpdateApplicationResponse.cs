namespace Api.Response;

public class UpdateApplicationResponse
{
    public short Id { get; set; }

    public string Name { get; set; }

    public string Subdomain { get; set; }

    public List<string> Errors { get; } = new();
}
