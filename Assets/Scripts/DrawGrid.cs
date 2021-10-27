
using UnityEngine;

public class DrawGrid : MonoBehaviour
{
    private MazeGrid m_MazeGrid;
    void Start()
    {
        m_MazeGrid = new MazeGrid(10, 10);
        BinaryTreeAlgorithm.BuildMaze(m_MazeGrid);
        Debug.Log(m_MazeGrid.ToString());
    }

    
}
