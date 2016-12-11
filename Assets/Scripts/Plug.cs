using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour {
	public Sprite normalSprite;
	public Sprite fearSprite;
	public Sprite loveSprite;
	public Sprite berserkerSprite;

	private string type;

	public void SetType (string myType) {
		type = myType;
		if (type == "Berserker") {
			GetComponent<SpriteRenderer> ().sprite = berserkerSprite;
		} else if (type == "Fear") {
			GetComponent<SpriteRenderer> ().sprite = fearSprite;
		} else if (type == "Love") {
			GetComponent<SpriteRenderer> ().sprite = loveSprite;
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Hero") {
			if (type == "Berserker") {
				other.GetComponent<Hero> ().GoBerseker ();
				Destroy (gameObject);
			} else if (type == "Love") {
				other.GetComponent<Hero> ().FallInLove ();
				Destroy(gameObject);
			}
		}
	}
}
