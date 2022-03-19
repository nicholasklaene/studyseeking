namespace api.Models;

public class Post
{
    public long Id { get; set; }
    
    public int CategoryId { get; set; }
    
    public Category Category { get; set; }
    
    public string UserId { get; set; }
    
    public User User { get; set; }

    public string Title { get; set; }

    public string Preview { get; set; }
    
    public bool IsActive { get; set; }
    
    public virtual ICollection<PostHasTag> PostHasTags { get; set; }
}
