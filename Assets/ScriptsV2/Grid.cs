using System.Numerics;

public class Grid
{
    private int width;
    private int height;
    private static Vector2 center;
    private static Vector2 offset;
    private GridCell[,] grid;
    private GridCell[,] futureGrid;

    public Grid(int width, int height, float x, float y)
    {
        this.width = width;
        this.height = height;
        
        center = new Vector2(x, y);
        offset = new Vector2(center.X - this.width / 2f, center.Y - this.height / 2f);
        
        this.grid = new GridCell[this.width, this.height];
        this.futureGrid = new GridCell[this.width, this.height];
        
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                this.grid[i, j] = new GridCell(i, j);
                this.grid[i, j].SetStatus(false);
            
                this.futureGrid[i, j] = new GridCell(i, j);
                this.futureGrid[i, j].SetStatus(false);
            }
        }
    }

    public void Update()
    {
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                this.futureGrid[i, j].SetStatus(this.grid[i, j].Update(this.CountAliveNeighbours(i, j)));
            }
        }
        
        GridCell[,] temp = this.grid;
        this.grid = this.futureGrid;
        this.futureGrid = temp;
    }
    
    private int CountAliveNeighbours(int x, int y)
    {
        int count = 0;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i >= 0 && i < this.width && j >= 0 && j < this.height)
                {
                    if (this.grid[i, j].GetStatus() == true)
                    {
                        if (this.grid[i, j] != this.grid[x, y])
                        {
                            count++;
                        }
                    }
                }
            }
        }
        
        return count;
    }
    
    public static int[] GameToGridCoordinates(float x, float y)
    {
        x -= offset.X + Vector2.One.X / 2f;
        y -= offset.Y + Vector2.One.Y / 2f;

        int[] rez = {(int)x, (int)y};
        return rez;
    }

    public static int[] GridToGameCoordinates(float x, float y)
    {
        x += offset.X + Vector2.One.X / 2f;
        y += offset.Y + Vector2.One.Y / 2f;
        
        int[] rez = {(int)x, (int)y};
        return rez;
    }

    public static bool IsWithinGridBounds(float x, float y)
    {
        float left_bound = center.X + offset.X;
        float right_bound = center.X - offset.X;
        float lower_bound = center.Y + offset.Y;
        float upper_bound = center.Y - offset.Y;

        if (x >= left_bound && x <= right_bound &&
            y >= lower_bound && y <= upper_bound)
            return true;
        return false;
    }

    public GridCell this[int i, int j]
    {
        get => this.grid[i, j];
        set => this.grid[i, j] = value;
    }
}
