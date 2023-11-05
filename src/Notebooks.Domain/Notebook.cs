namespace Notebooks.Domain;

public class Notebook
{
    public Guid Id { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime UpdatedOn { get; private set; }
    public string Title { get; private set; }
    public string Path { get; private set; }

    public Notebook(Guid id, string title, string path)
        : this(id, title, path, DateTime.UtcNow, DateTime.UtcNow)
    {
    }

    private Notebook(Guid id, string title, string path, DateTime createdOn, DateTime updatedOn)
    {
        Id = id;
        Title = title;
        Path = path;
        CreatedOn = createdOn;
        UpdatedOn = updatedOn;
    }

    public void ChangeTitle(string title)
    {
        Title = title;
        UpdatedOn = DateTime.UtcNow;
    }

    public void ContentChanged()
    {
        UpdatedOn = DateTime.UtcNow;
    }
}