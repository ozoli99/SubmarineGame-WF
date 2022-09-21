namespace Persistence
{
    public interface IPersistence
    {
        List<Shape> Load(String path, ref Int32 gameTime, ref Int32 destroyedMineCount);
        void Save(String path, List<Shape> mines, Shape submarine, Int32 gameTime, Int32 destroyedMineCount);
    }
}