using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class GameManager : MonoBehaviour {

	public bool gameStarting = true;
    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager boardScript;
	[HideInInspector] public bool doorOpen = false;

    private Text levelText;
    private GameObject levelImage;
	private GameObject paulImage;
    private int level = 1;
	private List<Hero> heroes;
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
		heroes = new List<Hero>();

		levelImage = GameObject.Find("LevelImage");
		paulImage = GameObject.Find("PaulImage");
		levelText = GameObject.Find("LevelText").GetComponent<Text>();

		boardScript = GetComponent<BoardManager>();
		InitGame();
	}

	void InitGame() {
		levelText.text = "You're Paul. Paul Terguei.\nYou were a king. Deceased for decades, \nyou're now a ghost who tries to protect \n his glory room from felony knights \nby terrifying them. \n MAKE THEM LEAVE! \n \n \n(click to start)";
		levelText.fontSize = 40;

		levelImage.SetActive(true);

	}

	private void OnLevelWasLoaded(int index)
	{
	    level++;
		InitLevel();
	}

	void InitLevel()
	{
	    doingSetup = true;
		doorOpen = false;

	    levelText.text = "Wave " + level;
		levelText.fontSize = 24;
	    levelImage.SetActive(true);
	    Invoke("HideLevelImage", levelStartDelay);

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
        levelText.text = "You are finally safe in your Glory Room. \n THE END";
        levelImage.SetActive(true);
        enabled = false;
    }

	// Update is called once per frame
	void Update ()
	{

		if (Input.GetMouseButtonDown (0)) {
			if (gameStarting) {
				gameStarting = false;
				Invoke ("HideLevelImage", levelStartDelay);
				paulImage.SetActive (false);
				InitLevel ();
 
			} else {
				CastSpell (Input.mousePosition, "Repulse");
			}
		}

		if (heroesMoving || doingSetup) {
            return;
        }
			
		StartCoroutine(MoveHeroes());
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

	private void NextLevel()
	{
		if (heroes.Count == 0 && !doingSetup && !gameStarting) {
			if (level == 5) {
				GameOver ();
				return;
			}
			level++;
			InitLevel ();
		}

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

			if (((Hero)heroes [i]).isKilled) {
				RemoveHeroFromList (heroes [i]);
			}
		}
			
		heroesMoving = false;

		NextLevel ();
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
