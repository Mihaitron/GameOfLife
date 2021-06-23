using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Camera camera;

    private GridCell[,] grid;
    private GridCell[,] futureGrid;
    private Vector2 center;
    private Vector2 offset;
    private bool start;

    private void Awake()
    {
        this.Initialize();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screen_mouse_pos = Input.mousePosition;
            Vector2 world_mouse_pos = this.camera.ScreenToWorldPoint(screen_mouse_pos);
            Vector2 cell_point = this.GameToGridCoordinates(world_mouse_pos);

            Debug.Log(world_mouse_pos);
            Debug.Log(cell_point);

            if (this.IsWithinGridBounds(world_mouse_pos))
            {
                GridCell cell = this.grid[(int) cell_point.x, (int) cell_point.y];

                cell.SetStatus(!cell.GetStatus());

                this.grid[(int) cell_point.x, (int) cell_point.y] = cell;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.start = !this.start;
            print("Started/Stopped");
        }

        if (this.start)
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
    }

    private void Initialize()
    {
        this.center = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        this.offset = new Vector2(center.x - this.width / 2f, center.y - this.height / 2f);
        this.grid = new GridCell[this.width, this.height];
        this.futureGrid = new GridCell[this.width, this.height];
        this.start = false;

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
    
    private Vector2 GameToGridCoordinates(Vector2 point)
    {
        point -= this.offset + Vector2.one/2f;

        return new Vector2((int) point.x, (int) point.y);
    }

    private Vector2 GridToGameCoordinates(Vector2 point)
    {
        point += this.offset + Vector2.one/2f;

        return point;
    }

    private bool IsWithinGridBounds(Vector2 point)
    {
        float left_bound = this.center.x + this.offset.x;
        float right_bound = this.center.x - this.offset.x;
        float lower_bound = this.center.y + this.offset.y;
        float upper_bound = this.center.y - this.offset.y;
        
        print(left_bound);
        print(right_bound);
        print(lower_bound);
        print(upper_bound);
        
        
        if (point.x >= left_bound && point.x <= right_bound &&
            point.y >= lower_bound && point.y <= upper_bound)
            return true;
        return false;
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

    private void OnDrawGizmos()
    {
        if (this.grid != null)
        {
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    Vector2 game_coordinates = this.GridToGameCoordinates(new Vector2(i, j));
                    
                    Gizmos.color = this.grid[i, j].GetStatus() ? Color.green : Color.black;
                    Gizmos.DrawCube(new Vector3(game_coordinates.x, game_coordinates.y, 0), Vector3.one);
                }
            }
        }
    }
}
