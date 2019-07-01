using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager BoardScript;

    private int level = 1;

    // Start is called before the first frame update
    void Awake()
    {
        BoardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        BoardScript.SetupScene(level);
    }

    public void Loader(GameObject board)
    {
        BoardScript.Load(level, int.Parse(board.name));
    }

    // Update is called once per frame
    public void Reset()
    {
        BoardScript.Reset();
    }

    public void UpdateText()
    {
        BoardScript.UpdateTextLevel();
    }
}
