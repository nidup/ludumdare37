using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MovingObject {

	public int playerDamage ;

	private Animator animator;
	private Vector3 orientation;

	protected override void Start ()
	{
		GameManager.instance.AddHeroToList(this);
		animator = GetComponent<Animator>();
		orientation = new Vector3 (0, -1, 0f);
		base.Start();
	}

	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		base.AttemptMove<T>(xDir, yDir);
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

	protected override void OnCantMove <T> (T component)
	{
		Component hitObject = component as Component;

		if (hitObject.tag == "OuterWall" || hitObject.tag == "Hero") {
			if (orientation.x != 0) {
				orientation.x = 0;
				orientation.y = Random.Range (0, 2) == 0 ? -1 : 1;
			} else {
				orientation.y = 0;
				orientation.x = Random.Range (0, 2) == 0 ? -1 : 1;
			}
		}
	}
}
