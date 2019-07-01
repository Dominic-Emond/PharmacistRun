using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int SafeZone = 9;
    public int Columns = 91; //Outside
    public int Rows = 11;

    public GameObject EnemyTile;
    public GameObject FirstGrid;
    public GameObject LeftGrid;
    public GameObject RightGrid;
    public GameObject[] Plants;

    static private List<GameObject> boardHolder;
    private int lastIndex;

    private int positionX = 0;
    private int positionY = 0;
    private int line = 1;
    private int insideColumn = 71;
    private int boardName = 1;

    void BoardSetup() //Set up the Tileset
    {
        boardHolder.Add(new GameObject(boardName.ToString()));
        lastIndex = boardHolder.Count - 1;

        if (line == 1)
        {
            GameObject toInstantiate = FirstGrid;
            GameObject instance = Instantiate(toInstantiate, new Vector3(positionX, positionY, 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(boardHolder[lastIndex].transform);
        }
        else
        {
            GameObject toInstantiateL = LeftGrid;
            GameObject instanceL = Instantiate(toInstantiateL, new Vector3(positionX, positionY, 0f), Quaternion.identity) as GameObject;
            instanceL.transform.SetParent(boardHolder[lastIndex].transform);
        }

        GameObject toInstantiateR = RightGrid;
        GameObject instanceR = Instantiate(toInstantiateR, new Vector3(positionX, positionY + Rows, 0f), Quaternion.identity) as GameObject;
        instanceR.transform.SetParent(boardHolder[lastIndex].transform);
    }

    void LayoutEnemies(int level)
    {

        //calculating Enemies
        int amountL = Mathf.FloorToInt(5 * (Mathf.Log(line) + 2));
        int amountR = Mathf.FloorToInt(5 * (Mathf.Log(line + 1) + 2));

        //Positions
        List<Vector3> enemyPositionsL = new List<Vector3>();
        List<Vector3> enemyPositionsR = new List<Vector3>();

        // x = positionX + SafeZone + 2
        for (float x = 6; x < (6 + insideColumn); x++)
        {
            for (float y = positionY - 4; y < positionY + 3; y++)
            {
                enemyPositionsL.Add(new Vector3((x + 0.5f), (y + 0.5f), 0));
                enemyPositionsR.Add(new Vector3((x + 0.5f), (y + 0.5f + Rows), 0));
            }
        }

        //Debug.Log(enemyPositionsL.Count + ", "  + enemyPositionsR.Count);

        GameObject toInstantiate = EnemyTile;
        var enemyProperties = toInstantiate.GetComponent<TurtleController>();

        for (int el = 0; el < amountL; el++)
        {
            int randomIndex = Random.Range(0, enemyPositionsL.Count);
            Vector3 randomPosition = enemyPositionsL[randomIndex];
            enemyPositionsL.RemoveAt(randomIndex);

            Instantiate(toInstantiate, randomPosition, Quaternion.identity).transform.SetParent(boardHolder[lastIndex].transform);
        }
        //Vector3 position = new Vector3(positionX + (Columns / 2), positionY, 0); //Place them all in the middle

        for (int er = 0; er < amountR; er++)
        {
            int randomIndex = Random.Range(0, enemyPositionsR.Count);
            Vector3 randomPosition = enemyPositionsR[randomIndex];
            enemyPositionsR.RemoveAt(randomIndex);

            Instantiate(toInstantiate, randomPosition, Quaternion.identity).transform.SetParent(boardHolder[lastIndex].transform);
        }
    }

    void LayoutPlants()
    {
        int plantsL = Random.Range(1, 3);
        int plantsR = Random.Range(1, 3);

        for (int i = 1; i <= plantsL; i++)
        {
            int posX = Random.Range(6, (6 + insideColumn));
            int posY = Random.Range(positionY - 4, positionY + 3);
            int plant = Random.Range(0, Plants.Length);

            GameObject toInstantiate = Plants[plant];
            Instantiate(toInstantiate, new Vector2(posX, posY), Quaternion.identity).transform.SetParent(boardHolder[lastIndex].transform);
        }

        for (int i = 0; i <= plantsR; i++)
        {
            int posX = Random.Range(6, (6 + insideColumn));
            int posY = Random.Range(positionY - 4 + Rows, positionY + 3 + Rows);
            int plant = Random.Range(0, Plants.Length);

            GameObject toInstantiate = Plants[plant];
            Instantiate(toInstantiate, new Vector2(posX + 0.5f, posY + 0.5f), Quaternion.identity).transform.SetParent(boardHolder[lastIndex].transform);
        }
    }

    public void SetupScene(int level)
    {
        boardHolder = new List<GameObject>();
        BoardSetup();
        LayoutEnemies(level);
        LayoutPlants();

        line += 2;
        //positionX += (Rows * 2);
        positionY += (Rows * 2);
        boardName += 1;
    }

    public void Load(int level, int board)
    {
        if (board + 1 > boardHolder.Count)
        {
            BoardSetup();
            LayoutEnemies(level);
            LayoutPlants();

            UIText.instance.ChangeText(line - 1);

            line += 2;
            //positionX += (Rows * 2);
            positionY += (Rows * 2);
            boardName += 1;

            

            if (boardHolder.Count > 3)
            {
                GameObject.Destroy(boardHolder[boardHolder.Count - 4]);
            }
        }
        
    }

    public void Reset()
    {
        //if (boardHolder[0].ac)
        foreach (GameObject board in boardHolder)
        {
            GameObject.Destroy(board);
        }

        positionX = 0;
        positionY = 0;
        line = 1;

        UIText.instance.ChangeText(1);
        SetupScene(1);
    }

    public void UpdateTextLevel()
    {
        UIText.instance.ChangeText(line - 2);
    }
}
