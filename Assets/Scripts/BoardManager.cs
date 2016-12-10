using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count {
        public int minimum;
        public int maximum;
        public Count (int min, int max) {
            minimum = min;
            maximum = max;

        }
    }
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    private int columns;
    private int rows;

    void InitializeList()
    {
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++) {
            for (int y = 1; y < rows - 1; y++) {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {

        string[,] map = {
             {"ow-0","ow-1","ow-1","ow-1","ow-1","ow-1","ow-1","ow-2"},
             {"ow-3","ow-4","ow-4","ow-4","ow-4","ow-4","ow-4","ow-5"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
             {"ow-8","ow-9","ow-9","ow-9","ow-9","ow-9","ow-9","ow-10"},
        };
        rows = map.GetLength(0);
        columns = map.GetLength(1);

        boardHolder = new GameObject("Board").transform;
        for (int x = 0; x < columns; x++) {
            for (int y = 0; y < rows; y++) {

                string[] tokens = map[y, x].Split('-');
                string prefabType = tokens[0];
                int prefabIndex = Int32.Parse(tokens[1]);

                GameObject prefab = null;
                if (prefabType == "ow") {
                    prefab = outerWallTiles[prefabIndex];
                } else if (prefabType == "fl") {
                    prefab = floorTiles[prefabIndex];
                }

                GameObject instance = Instantiate(prefab, new Vector3(x, rows-y-1, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);

                //Debug.logger.Log(x.ToString()+y.ToString());
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum+1);
        for (int i = 0; i < objectCount; i++) {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);

        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int) Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 2, rows -2, 0f), Quaternion.identity);
    }
}
