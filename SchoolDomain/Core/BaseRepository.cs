namespace SchoolDomain.Core;

public abstract class BaseRepository<T> where T : BaseEntity
{
    protected List<T> _items = new();

    public virtual T? GetById(int id) => _items.FirstOrDefault(x => x.Id == id);
    
    public virtual List<T> GetAll() => _items;
    
    public virtual void Add(T item)
    {
        item.CreatedAt = DateTime.Now;
        item.UpdatedAt = DateTime.Now;
        _items.Add(item);
    }
    
    public virtual void Update(T item)
    {
        item.UpdatedAt = DateTime.Now;
    }
    
    public virtual void Delete(int id)
    {
        var item = GetById(id);
        if (item != null)
            _items.Remove(item);
    }
}
