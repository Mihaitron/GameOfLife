using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public enum Status { Alive, Dead};

    private Status status;
    private Status willBe;

    // Start is called before the first frame update
    void Start()
    {
        this.status = Status.Dead;
        this.willBe = Status.Dead;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.status == Status.Dead)
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        else
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            if (this.status == Status.Alive)
                this.status = Status.Dead;
            else
                this.status = Status.Alive;
    }

    public void UpdateCell(GameObject[,] cells, float min_distance)
    {
        // Get number of alive surrounding cells
        int count_alive_surrounding = 0;
        foreach (GameObject cell in cells)
        {
            Vector2 difference = cell.transform.position - this.gameObject.transform.position;

            if (difference.magnitude < min_distance && difference.magnitude > 0)
                if (cell.GetComponent<Cell>().getStatus() == Status.Alive)
                    count_alive_surrounding++;
        }

        // Apply the rules
        if (this.status == Status.Alive && count_alive_surrounding < 2)
            this.willBe = Status.Dead;
        else if (this.status == Status.Alive && (count_alive_surrounding == 2 || count_alive_surrounding == 3))
            this.willBe = Status.Alive;
        else if (this.status == Status.Alive && count_alive_surrounding > 3)
            this.willBe = Status.Dead;
        else if (this.status == Status.Dead && count_alive_surrounding == 3)
            this.willBe = Status.Alive;
    }

    public Status getStatus()
    {
        return this.status;
    }

    public void setStatus(Status new_status)
    {
        this.status = new_status;
    }

    public Status getWillBe()
    {
        return this.willBe;
    }

    public void setWillBe(Status new_will_be)
    {
        this.willBe = new_will_be;
    }
}
