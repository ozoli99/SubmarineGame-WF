namespace Persistence
{
    public class TextFilePersistence : IPersistence
    {
        public List<Shape> Load(string path, ref int gameTime, ref int destroyedMineCount)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String[] numbers = reader.ReadToEnd().Split();

                    gameTime = Int32.Parse(numbers[0]);
                    destroyedMineCount = Int32.Parse(numbers[1]);

                    List<Shape> submarineAndMines = new List<Shape>();

                    ShapeType type = (ShapeType)Int32.Parse(numbers[3]);
                    Int32 x = Int32.Parse(numbers[4]);
                    Int32 y = Int32.Parse(numbers[5]);
                    Int32 width = Int32.Parse(numbers[6]);
                    Int32 height = Int32.Parse(numbers[7]);
                    Int32 weight = Int32.Parse(numbers[8]);
                    Shape submarine = new Shape(type, x, y, width, height, weight);
                    submarineAndMines.Add(submarine);

                    for (Int32 i = 10; i < numbers.Length - 3; i += 7)
                    {
                        type = (ShapeType)Int32.Parse(numbers[i]);
                        x = Int32.Parse(numbers[i + 1]);
                        y = Int32.Parse(numbers[i + 2]);
                        width = Int32.Parse(numbers[i + 3]);
                        height = Int32.Parse(numbers[i + 4]);
                        weight = Int32.Parse(numbers[i + 5]);
                        submarineAndMines.Add(new Shape(type, x, y, weight, height, weight));
                    }
                    return submarineAndMines;
                }
            }
            catch
            {
                throw new DataException("Error occurred during reading.");
            }
        }

        public void Save(string path, List<Shape> mines, Shape submarine, int gameTime, int destroyedMineCount)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (mines == null)
                throw new ArgumentNullException("mines");
            if (submarine == null)
                throw new ArgumentNullException("submarine");

            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine((Int32)gameTime + " " + (Int32)destroyedMineCount);
                    writer.WriteLine((Int32)submarine.Type + " " + (Int32)submarine.X + " " + (Int32)submarine.Y + " " + (Int32)submarine.Width + " " + (Int32)submarine.Height + " " + (Int32)submarine.Weight);
                    for (Int32 i = 0; i < mines.Count; ++i)
                    {
                        writer.WriteLine((Int32)mines[i].Type + " " + (Int32)mines[i].X + " " + (Int32)mines[i].Y + " " + (Int32)mines[i].Width + " " + (Int32)mines[i].Height + " " + (Int32)mines[i].Weight);
                    }
                }
            }
            catch
            {
                throw new DataException("Error occurred during writing.");
            }
        }
    }
}
