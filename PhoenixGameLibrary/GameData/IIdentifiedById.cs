namespace PhoenixGameLibrary.GameData
{
    public interface IIdentifiedById
    {
        int Id { get; }
    }

    public interface IIdentifiedByIdAndName
    {
        int Id { get; }
        string Name { get; }
    }
}