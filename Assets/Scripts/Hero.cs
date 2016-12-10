using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MovingObject {

	public int playerDamage ;

	private Animator animator;


	protected override void Start ()
	{
		GameManager.instance.AddHeroToList(this);
		animator = GetComponent<Animator>();
		base.Start();
	}

	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		base.AttemptMove<T>(xDir, yDir);
	}

	public void MoveHero()
	{
		int xDir = 1;
		int yDir = 0;
		/*if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon) {
			yDir = target.position.y > transform.position.y ? 1 : -1;
		} else {
			xDir = target.position.x > transform.position.x ? 1 : -1;
		}*/
		AttemptMove<Player>(xDir, yDir);
	}

	protected override void OnCantMove <T> (T component)
	{
		Player hitPlayer = component as Player;
		animator.SetTrigger ("enemyAttack");
		hitPlayer.LoseFood(playerDamage);
	}
}
