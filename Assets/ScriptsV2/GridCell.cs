using UnityEngine;

public class GridCell
{
    private bool status;
    private Vector2 position;

    public GridCell(int x, int y)
    {
        this.position = new Vector2(x, y);
    }
    
    public void SetStatus(bool new_status)
    {
        this.status = new_status;
    }

    public bool GetStatus()
    {
        return this.status;
    }

    public bool Update(int neighbours)
    {
        if (this.status && (neighbours == 2 || neighbours == 3))
            return true;
        if (!this.status && neighbours == 3)
            return true;
        return false;
    }

    public static bool operator ==(GridCell lhs, GridCell rhs)
    {
        return lhs.position == rhs.position;
    }

    public static bool operator !=(GridCell lhs, GridCell rhs)
    {
        return lhs.position != rhs.position;
    }
}
