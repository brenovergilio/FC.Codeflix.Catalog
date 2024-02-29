namespace FC.Codeflix.Catalog.Domain.Entity;

public class Category(string name, string description)
{
  public Guid Id { get; private set; } = Guid.NewGuid();
  public string Name { get; private set; } = name;
  public string Description { get; private set; } = description;
  public bool IsActive { get; private set; } = true;
  public DateTime CreatedAt { get; private set; } = DateTime.Now;
}