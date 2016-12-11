using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTriggerButton : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Hero") {
			
			// animate door
			GameObject[] doors = GameObject.FindGameObjectsWithTag ("Exit");
			GameObject door = doors [0];
			Animator doorAnimator = door.GetComponent<Animator> ();

			if (doorAnimator.GetCurrentAnimatorStateInfo (0).IsName ("DoorOpening")) {
				return;
			}
			if (doorAnimator.GetCurrentAnimatorStateInfo (0).IsName ("DoorClosing")) {
				return;
			}

			doorAnimator.SetTrigger ("Switch");

			bool oldState = GameManager.instance.doorOpen;
			bool newState = !oldState;
			GameManager.instance.doorOpen = newState;

			int blockingLayerIdx = LayerMask.NameToLayer ("BlockingLayer");
			int defaultLayerIdx = LayerMask.NameToLayer ("Default");

			door.layer = GameManager.instance.doorOpen ? defaultLayerIdx : blockingLayerIdx;

			// animate button
			GameObject[] buttons = GameObject.FindGameObjectsWithTag ("Button");
			GameObject button = buttons [0];
			Animator buttonAnimator = button.GetComponent<Animator> ();

			if (buttonAnimator.GetCurrentAnimatorStateInfo (0).IsName ("ButtonDowning")) {
				return;
			}
			if (buttonAnimator.GetCurrentAnimatorStateInfo (0).IsName ("ButtonOpening")) {
				return;
			}

			buttonAnimator.SetTrigger ("Switch");
		}
	}
}
