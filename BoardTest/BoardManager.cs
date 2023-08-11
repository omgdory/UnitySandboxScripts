using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject cell_prefab;

    private static int cell_count = 0;

    public class BoardCell {
        public GameObject prefab;
        public int cell_ID;

        public BoardCell(GameObject cell_pfab) {
            cell_ID = cell_count;
            cell_count++;
            prefab = Instantiate(cell_pfab, Vector3.zero, Quaternion.identity);
            prefab.name = $"Cell {cell_ID}";
        }

        public BoardCell(GameObject cell_pfab, Vector3 position) {
            cell_ID = cell_count;
            cell_count++;
            prefab = Instantiate(cell_pfab, position, Quaternion.identity);
            prefab.name = $"Cell {cell_ID}";
        }

        ~BoardCell() {
            cell_count--;
            Destroy(prefab);
        }
    }

    private const int WIDTH = 20;
    private const int HEIGHT = 10;

    private float cell_size_horizontal = 1.0f;
    private float cell_size_vertical = 1.0f;

    private BoardCell[,] board_array = new BoardCell[HEIGHT, WIDTH];

    private Transform board_object;

    // Start is called before the first frame update
    void Awake()
    {
        cell_size_horizontal = cell_prefab.GetComponent<BoxCollider>().size.x;
        cell_size_vertical = cell_prefab.GetComponent<BoxCollider>().size.y;
        board_object = InitializeBoard(board_array).transform;
    }

    private GameObject InitializeBoard(BoardCell[,] arr) {
        GameObject ans = new GameObject("Board");
        float height_offset = 0;
        float width_offset = 0;

        int row_count = 0;
        for(int i=0; i<HEIGHT; i++) {
            Transform curr = new GameObject($"Row {row_count}").transform;
            curr.parent = ans.transform;
            row_count++;
            for(int j=0; j<WIDTH; j++) {
                arr[i,j] = new BoardCell(cell_prefab, new Vector3(width_offset, 0, height_offset));
                width_offset += cell_size_horizontal;
                arr[i,j].prefab.transform.parent = curr.transform;
            }
            height_offset += cell_size_vertical;
            width_offset = 0;
        }

        Debug.Log("Board Initialized");

        return ans;
    }
}
