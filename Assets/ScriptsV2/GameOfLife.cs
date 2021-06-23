using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Camera camera;

    private Grid grid;
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
            int[] cell_point_raw = Grid.GameToGridCoordinates(world_mouse_pos.x, world_mouse_pos.y);
            Vector2 cell_point = new Vector2(cell_point_raw[0], cell_point_raw[1]);

            Debug.Log(world_mouse_pos);
            Debug.Log(cell_point);

            if (Grid.IsWithinGridBounds(world_mouse_pos.x, world_mouse_pos.y))
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
            this.grid.Update();
        }
    }

    private void Initialize()
    {
        Vector3 grid_position = this.transform.position;
        this.grid = new Grid(this.width, this.height, grid_position.x, grid_position.y);
        this.start = false;
    }

    private void OnDrawGizmos()
    {
        if (this.grid != null)
        {
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    int[] game_coordinates_raw = Grid.GridToGameCoordinates(i, j);
                    Vector2 game_coordinates = new Vector2(game_coordinates_raw[0], game_coordinates_raw[1]);
                    
                    Gizmos.color = this.grid[i, j].GetStatus() ? Color.green : Color.black;
                    Gizmos.DrawCube(new Vector3(game_coordinates.x, game_coordinates.y, 0), Vector3.one);
                }
            }
        }
    }
}
