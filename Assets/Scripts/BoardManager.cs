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
	public GameObject plugPrefab;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
	public GameObject[] heroTiles;
    public GameObject[] outerWallTiles;
	public GameObject spellEffect;
	public GameObject[] floorTriggerPrefabs;

    private Transform boardHolder;
    private int columns;
    private int rows;

	void BoardSetup(int level)
    {
		if (boardHolder != null) {
			Destroy (boardHolder.gameObject);
		}

        string[,] map = {
             {"ow-0","ow-1","ow-1","ow-22","ow-1","ow-1","ow-2","ow-0","ow-1","ow-1","ow-1","ow-1","ow-1","ow-22","ow-1","ow-22","ow-1","ow-1","ow-1","ow-2"},
             {"ow-3","ow-4","ow-4","ow-23","ow-4","ow-4","ow-5","ow-3","ow-4","ow-4","ow-4","ow-4","ow-4","ow-23","do-0","ow-23","ow-4","ow-4","ow-4","ow-5"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","ow-13","ow-14","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","ow-15","ow-16","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-1","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","ow-17","ow-18","fl-0","fl-0","fl-0","fl-0","fl-0","ow-20","fl-0","ow-20","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-21","fl-0","ow-21","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-20","fl-0","ow-20","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-2","fl-0","fl-0","fl-0","fl-0","fl-0","ow-21","fl-0","ow-21","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
			 {"ow-8","ow-9","ow-9","ow-9","ow-12","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-20","fl-0","ow-20","fl-0","fl-0","fl-0","ow-7"},
			 {"ow-19","ow-19","ow-19","ow-19","ow-14","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-21","fl-0","ow-21","fl-3","fl-0","fl-0","ow-7"},
			 {"ow-0","ow-1","ow-1","ow-1","ow-16","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-3","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
			 {"ow-3","ow-4","bt-0","ow-4","ow-18","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
             {"ow-6","fl-0","ft-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-4","fl-0","fl-0","ow-7"},
			 {"ow-6","fl-0","fl-0","fl-1","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","fl-0","ow-11","ow-12","fl-0","fl-0","fl-0","fl-0","fl-0","ow-7"},
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
				} else if (prefabType == "fl" && prefabIndex != 0) {
					prefab = floorTiles[prefabIndex];
					GameObject instance = Instantiate(prefab, new Vector3(x, rows-y-1, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent(boardHolder);
				}
            }
        }
    }

    public void SetupScene(int level)
    {
		BoardSetup(level);
		SetupItems(level);
    }

	public void SetupItems(int level)
	{
		if (level == 1) {
			SetupHero (1, 8, 0, 1);
			SetupHero (18, 4, -1, 0);
			SetupHero (12, 16, 0, -1);
		} else if (level == 2) {
			SetupHero (2, 13, 0, 1);
			SetupHero (5, 13, 0, -1);
			SetupHero (9, 16, 0, -1);
			SetupHero (17, 16, 0, -1);
			SetupHero (18, 4, -1, 0);

			SetupPlug (4, 4, "Love");
			SetupPlug (14, 6, "Love");
			SetupPlug (11, 2, "Love");
		} else if (level == 3) {
			SetupHero (2, 13, 0, 1);
			SetupHero (5, 13, 0, -1);
			SetupHero (9, 16, 0, -1);
			SetupHero (17, 16, 0, -1);
			SetupHero (18, 4, -1, 0);
			SetupHero (1, 12, 1, 0);
			SetupHero (1, 9, 1, 0);

			SetupPlug (4, 4, "Berserker");
			SetupPlug (14, 6, "Berserker");
			SetupPlug (14, 16, "Berserker");
		} else if (level == 4) {
			SetupHero (2, 13, 0, 1);
			SetupHero (5, 13, 0, -1);
			SetupHero (9, 16, 0, -1);
			SetupHero (17, 16, 0, -1);
			SetupHero (18, 4, -1, 0);
			SetupHero (1, 12, 1, 0);
			SetupHero (1, 9, 1, 0);

			SetupPlug (4, 4, "Berserker");
			SetupPlug (14, 6, "Berserker");
			SetupPlug (14, 16, "Berserker");

			SetupPlug (8, 16, "Love");
			SetupPlug (9, 8, "Love");
			SetupPlug (18, 2, "Love");
		} else if (level == 5) {
			SetupHero (2, 13, 0, 1);
			SetupHero (5, 13, 0, -1);
			SetupHero (9, 16, 0, -1);
			SetupHero (17, 16, 0, -1);
			SetupHero (18, 4, -1, 0);
			SetupHero (1, 12, 1, 0);
			SetupHero (1, 9, 1, 0);
			SetupHero (18, 12, -1, 0);
			SetupHero (18, 9, -1, 0);
			SetupHero (9, 2, 0, 1);

			SetupPlug (7, 11, "Berserker");

			SetupPlug (15, 9, "Love");
			SetupPlug (3, 16, "Love");
		}
	}

	public void SetupHero(int xPos, int yPos, int xDir, int yDir)
	{
		Vector3 position = new Vector3 (xPos, yPos, 0f);
		GameObject hero = Instantiate(heroTiles[0], position, Quaternion.identity);
		hero.GetComponent<Hero> ().orientation = new Vector3 (xDir, yDir, 0f);
	}

	public void SetupPlug(int xPos, int yPos, string type)
	{
		GameObject plug = Instantiate(plugPrefab, new Vector3(xPos, yPos, 0f), Quaternion.identity);
		plug.GetComponent<Plug>().SetType (type);
		plug.transform.SetParent(boardHolder);
	}
}
