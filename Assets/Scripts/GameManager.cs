using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;
	[HideInInspector] public bool doorOpen = false;

    private Text levelText;
    private GameObject levelImage;
    private int level = 1;
    private List<Enemy> enemies;
	private List<Hero> heroes;
	private bool enemiesMoving;
	private bool heroesMoving;
    private bool doingSetup;

	// Use this for initialization
	void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
		heroes = new List<Hero>();
		boardScript = GetComponent<BoardManager>();
		InitGame();
	}

	private void OnLevelWasLoaded(int index)
	{
	    level++;
	    InitGame();
	}

	void InitGame()
	{
	    doingSetup = true;
	    levelImage = GameObject.Find("LevelImage");
	    levelText = GameObject.Find("LevelText").GetComponent<Text>();
	    levelText.text = "Level " + level;
	    levelImage.SetActive(true);
	    Invoke("HideLevelImage", levelStartDelay);

	    enemies.Clear();
		heroes.Clear();
	    boardScript.SetupScene(level);
	}

	private void HideLevelImage()
	{
	    levelImage.SetActive(false);
	    doingSetup = false;
	}

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			CastSpell (Input.mousePosition, "Repulse");
		}

		if (enemiesMoving || heroesMoving || doingSetup) {
            return;
        }

        StartCoroutine(MoveEnemies());
		StartCoroutine(MoveHeroes());
	}

	public void AddEnemyToList(Enemy script)
	{
        enemies.Add(script);
	}

	IEnumerator MoveEnemies()
	{
		enemiesMoving = true;
		yield return new WaitForSeconds(turnDelay);
		if (enemies.Count == 0) {
		    yield return new WaitForSeconds(turnDelay);
		}

		for (int i = 0; i < enemies.Count; i++) {
		    enemies[i].MoveEnemy();
		    yield return new WaitForSeconds(enemies[i].moveTime);
		}

		playersTurn = true;
		enemiesMoving = false;
	}

	public void AddHeroToList(Hero script)
	{
		heroes.Add(script);
	}

	public void RemoveHeroFromList(Hero script)
	{
		heroes.Remove (script);
		Destroy (script.gameObject);
	}

	IEnumerator MoveHeroes()
	{
		heroesMoving = true;
		yield return new WaitForSeconds(turnDelay);
		if (heroes.Count == 0) {
			yield return new WaitForSeconds(turnDelay);
		}

		for (int i = 0; i < heroes.Count; i++) {
			heroes[i].MoveHero();
			yield return new WaitForSeconds(heroes[i].moveTime);
		}

		playersTurn = true;
		heroesMoving = false;
	}

	public void CastSpell(Vector3 spellPosition, string spellType)
	{
		Vector3 worldSpellPosition = Camera.main.ScreenToWorldPoint (spellPosition);
		int spellDistance = 4;

		for (int i = 0; i < heroes.Count; i++) {
			bool impactedHero = false;

			if (
				Mathf.Abs(worldSpellPosition.x - heroes[i].transform.position.x) < spellDistance &&
				Mathf.Abs(worldSpellPosition.y - heroes[i].transform.position.y) < spellDistance
			) {
				impactedHero = true;
			}

			if (spellType == "Repulse" && impactedHero) {
				heroes [i].Repulse (worldSpellPosition);
			}
		}

		if (spellType == "Repulse") {
			GameObject mainCam = GameObject.Find("Main Camera");
			Bloom bloomEffect = mainCam.GetComponent<Bloom> ();
			bloomEffect.bloomIntensity = -0.7f;

			GameObject spellEffect = boardScript.spellEffect;
			Vector3 spellFXPos = new Vector3 (worldSpellPosition.x, worldSpellPosition.y, worldSpellPosition.z);
			spellFXPos.z = -5;
			GameObject fx = Instantiate(spellEffect, spellFXPos, Quaternion.identity);
			Destroy(fx, 2);

			StartCoroutine(CountdownSpellCameraEffect (0.090f));
		}
	}

	private IEnumerator CountdownSpellCameraEffect(float time){
		bool called = false;

		if (!called) {
			called = true;
			yield return new WaitForSeconds(time);
		}

		GameObject mainCam = GameObject.Find("Main Camera");
		Bloom bloomEffect = mainCam.GetComponent<Bloom> ();
		bloomEffect.bloomIntensity = 0f;
	}
}
