namespace src;
public interface IRepository<T>
{
    void Save();
    List<T> Load();
}
