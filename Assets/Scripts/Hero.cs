using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MovingObject {

	public int playerDamage ;

	private Vector3 orientation;

	protected override void Start ()
	{
		GameManager.instance.AddHeroToList(this);
		orientation = new Vector3 (0, -1, 0f);
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

		if (orientation.x != 0) {
			xDir = (int) orientation.x;
		} else {
			yDir = (int) orientation.y;
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
		GameObject text = transform.Find ("Text").gameObject;
		SpriteText spriteText = text.GetComponent<SpriteText>();
		spriteText.TempText("!", 1);
		
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
		
	protected override void OnCantMove <T> (T component)
	{
		Component hitObject = component as Component;

		if (orientation.x != 0) {
			orientation.x = 0;
			orientation.y = Random.Range (0, 2) == 0 ? -1 : 1;
		} else {
			orientation.y = 0;
			orientation.x = Random.Range (0, 2) == 0 ? -1 : 1;
		}
	}
}
