using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Populate : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform grid;
    public Slider speedSlider;

    private int size;
    private GameObject[,] cells;

    // Start is called before the first frame update
    private void Start()
    {
        this.size = 5;
        PopulateScene();
    }

    public void StartLife()
    {
        StartCoroutine(Iterate());
    }

    public void RestartLife()
    {
        StopAllCoroutines();

        foreach (GameObject cell in this.cells)
        {
            Destroy(cell);
        }

        PopulateScene();
    }

    public void HandleSize(int value)
    {
        if (value == 0)
            this.size = 5;
        if (value == 1)
            this.size = 10;
        if (value == 2)
            this.size = 15;
        if (value == 3)
            this.size = 20;

        this.RestartLife();
    }

    private void PopulateScene()
    {
        this.cells = new GameObject[2 * this.size, 2 * this.size];
        for (int i = 0; i < 2 * size; i++)
            for (int j = 0; j < 2 * size; j++)
                this.cells[i, j] = Instantiate(this.cellPrefab, new Vector3(i - size, j - size, 0f), Quaternion.identity, this.grid);
    }

    private IEnumerator Iterate()
    {
        foreach (GameObject cell in this.cells)
            cell.GetComponent<Cell>().UpdateCell(cells, 2.0f);

        foreach (GameObject cell in this.cells)
            cell.GetComponent<Cell>().setStatus(cell.GetComponent<Cell>().getWillBe());

        yield return new WaitForSeconds(this.speedSlider.value);

        StartCoroutine(Iterate());
    }
}
