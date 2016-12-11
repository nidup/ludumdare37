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
	public GameObject button;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
	public GameObject[] heroTiles;
    public GameObject[] outerWallTiles;
	public GameObject spellEffect;
	public GameObject[] floorTriggerPrefabs;

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
             {"ow-0","ow-1","ow-1","ow-1","ow-1","ow-1","ow-2","ow-0","ow-1","ow-1","ow-1","ow-1","ow-1","ow-1","ow-1","ow-1","ow-1","ow-1","ow-1","ow-2"},
             {"ow-3","ow-4","ow-4","ow-4","ow-4","ow-4","ow-5","ow-3","ow-4","ow-4","ow-4","ow-4","ow-4","ow-4","do-0","ow-4","ow-4","ow-4","ow-4","ow-5"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","ow-13","ow-14","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","ow-15","ow-16","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","ow-17","ow-18","fl-0","fl-0","fl-0","fl-0","fl-0","ow-20","fl-0","ow-20","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-21","fl-0","ow-21","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-20","fl-0","ow-20","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-21","fl-0","ow-21","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
			 {"ow-8","ow-9","ow-9","ow-9","ow-12","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-20","fl-0","ow-20","fl-0","fl-0","fl-0","ow-7"},
			 {"ow-19","ow-19","ow-19","ow-19","ow-14","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-21","fl-0","ow-21","fl-0","fl-0","fl-0","ow-7"},
			 {"ow-0","ow-1","ow-1","ow-1","ow-16","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
			 {"ow-3","ow-4","bt-0","ow-4","ow-18","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","ft-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
			 {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-11","ow-12","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
			 {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-13","ow-14","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
			 {"ow-8","ow-9","ow-9","ow-9","ow-9","ow-9","ow-9","ow-9","ow-9","ow-9","ow-9","ow-9","ow-10","ow-8","ow-9","ow-9","ow-9","ow-9","ow-9","ow-10"},
			 {"ow-19","ow-19","ow-19","ow-19","ow-19","ow-19","ow-19","ow-19","ow-19","ow-19","ow-19","ow-19","ow-19","ow-19","ow-19","ow-19","ow-19","ow-19","ow-19","ow-19"}
		};

        rows = map.GetLength(0);
        columns = map.GetLength(1);

        boardHolder = new GameObject("Board").transform;
        for (int x = 0; x < columns; x++) {
            for (int y = 0; y < rows; y++) {

                string[] tokens = map[y, x].Split('-');
                string prefabType = tokens[0];
                int prefabIndex = Int32.Parse(tokens[1]);

				GameObject floorPrefab = floorTiles[0];
				GameObject instanceFloor = Instantiate(floorPrefab, new Vector3(x, rows-y-1, 0f), Quaternion.identity) as GameObject;
				instanceFloor.transform.SetParent(boardHolder);

				GameObject prefab = null;

                if (prefabType == "ow") {
                    prefab = outerWallTiles[prefabIndex];
					GameObject instance = Instantiate(prefab, new Vector3(x, rows-y-1, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent(boardHolder);
                } else if (prefabType == "do") {
                    prefab = exit;
                    GameObject instance = Instantiate(prefab, new Vector3(x, rows-y-1, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(boardHolder);
				} else if (prefabType == "bt") {
					prefab = button;
					GameObject instance = Instantiate(prefab, new Vector3(x, rows-y-1, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent(boardHolder);
				} else if (prefabType == "ft") {
					prefab = floorTriggerPrefabs[prefabIndex];
					GameObject instance = Instantiate(prefab, new Vector3(x, rows-y-1, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent(boardHolder);
				}
			
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
        //LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        //LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        //int enemyCount = (int) Mathf.Log(level, 2f);
		int enemyCount = 0;
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
		Instantiate(heroTiles[0], new Vector3(8, 8, 0f), Quaternion.identity);
		Instantiate(heroTiles[0], new Vector3(5, 5, 0f), Quaternion.identity);
		Instantiate(heroTiles[0], new Vector3(3, 3, 0f), Quaternion.identity);
        //Instantiate(exit, new Vector3(columns - 2, rows -2, 0f), Quaternion.identity);
    }
}
