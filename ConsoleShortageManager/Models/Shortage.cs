namespace ConsoleShortageManager;

public class Shortage
{
    public string Title { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Room Room { get; set; }
    public Category Category { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}


