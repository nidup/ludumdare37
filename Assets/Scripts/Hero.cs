using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MovingObject {

	public int playerDamage ;

	public Vector3 orientation;

	private Transform target;

	public Sprite normalSprite;
	public Sprite fearSprite;
	public Sprite loveSprite;
	public Sprite berserkerSprite;

	public bool isBerserker;
	public bool isKilled;

	protected override void Start ()
	{
		GameManager.instance.AddHeroToList(this);

		base.Start();
	}

	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		base.AttemptMove<T>(xDir, yDir);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Exit" && GameManager.instance.doorOpen) {
			GameManager.instance.RemoveHeroFromList(this);
		}
	}

	public void MoveHero()
	{
		int xDir = 0;
		int yDir = 0;

		if (target != null) {

			if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon) {
				yDir = target.position.y > transform.position.y ? 1 : -1;
			} else {
				xDir = target.position.x > transform.position.x ? 1 : -1;
			}

		} else {
			
			if (orientation.x != 0) {
				xDir = (int)orientation.x;
			} else {
				yDir = (int)orientation.y;
			}
		}

		AttemptMove<Component>(xDir, yDir);
	}

	/*
	public void Attract(Vector3 spellPosition)
	{
		float xDistance = Mathf.Abs (spellPosition.x - transform.position.x);
		float yDistance = Mathf.Abs (spellPosition.y - transform.position.y);
		if (xDistance > yDistance) {
			if (transform.position.x > spellPosition.x) {
				orientation.x = -1;
				orientation.y = 0;
			} else {
				orientation.x = 1;
				orientation.y = 0;
			}
		} else {
			if (transform.position.y > spellPosition.y) {
				orientation.y = -1;
				orientation.x = 0;
			} else {
				orientation.y = 1;
				orientation.x = 0;
			}
		}
	}
	*/

	public void Repulse(Vector3 spellPosition)
	{
		if (isBerserker) {
			return;
		}

		GameObject text = transform.Find ("Text").gameObject;
		SpriteText spriteText = text.GetComponent<SpriteText>();
		spriteText.TempText("!", 2);
		StartCoroutine(TempSprite (fearSprite, 2));
		
		float xDistance = Mathf.Abs (spellPosition.x - transform.position.x);
		float yDistance = Mathf.Abs (spellPosition.y - transform.position.y);
		if (xDistance > yDistance) {
			if (transform.position.x > spellPosition.x) {
				orientation.x = 1;
				orientation.y = 0;
			} else {
				orientation.x = -1;
				orientation.y = 0;
			}
		} else {
			if (transform.position.y > spellPosition.y) {
				orientation.y = 1;
				orientation.x = 0;
			} else {
				orientation.y = -1;
				orientation.x = 0;
			}
		}
	}

	protected IEnumerator TempSprite(Sprite sprite, int duration)
	{
		SpriteRenderer renderer = GetComponent<SpriteRenderer> ();
		renderer.sprite = sprite;

		yield return new WaitForSeconds(duration);

		if (!isBerserker) {
			renderer.sprite = normalSprite;
		}
	}
		
	protected override void OnCantMove <T> (T component)
	{
		Component hitObject = component as Component;

		if (target != null && hitObject.gameObject == target.gameObject) {

			//Debug.Log (hitObject);

			if (isBerserker) {
				target.gameObject.GetComponent<Hero> ().MarkAsKilled();
				StopBerseker ();
			}

		} else {

			// random choose dest
			if (orientation.x != 0) {
				orientation.x = 0;
				orientation.y = Random.Range (0, 2) == 0 ? -1 : 1;
			} else {
				orientation.y = 0;
				orientation.x = Random.Range (0, 2) == 0 ? -1 : 1;
			}
		}
	}

	public void GoBerseker() {

		isBerserker = true;
		GetComponent<SpriteRenderer> ().sprite = berserkerSprite;

		GameObject enemy = FindClosestEnemy ();
		if (enemy == null) {

			StartCoroutine (AutoStopBerserkerEffect(5));

			return;
		}

		target = enemy.transform;

	}

	public void MarkAsKilled() {
		isKilled = true;
	}

	public void StopBerseker() {
		isBerserker = false;
		GetComponent<SpriteRenderer> ().sprite = normalSprite;
		target = null;
	}

	private IEnumerator AutoStopBerserkerEffect(int time){
		yield return new WaitForSeconds(time);
		StopBerseker ();
	}

	public void FallInLove() {
		Debug.Log ("go go in love");
	}

	private GameObject FindClosestEnemy() {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Hero");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			if (go == gameObject) {
				continue;
			}
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}
}
