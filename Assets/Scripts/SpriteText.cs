using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteText : MonoBehaviour
{
	private bool textDisplayed = false;

	void Start()
	{
		var parent = transform.parent;

		var parentRenderer = parent.GetComponent<Renderer>();
		var renderer = GetComponent<Renderer>();
		renderer.sortingLayerID = parentRenderer.sortingLayerID;
		renderer.sortingOrder = parentRenderer.sortingOrder;

		var spriteTransform = parent.transform;
		var text = GetComponent<TextMesh>();
		var pos = spriteTransform.position;

		text.text = "";
	}

	public void TempText(string tempText, int seconds)
	{
		TextMesh text = GetComponent<TextMesh>();
		text.text = tempText;

		textDisplayed = true;
		StartCoroutine ("Countdown", seconds);
	}

	private IEnumerator Countdown(int time){
		while(time > 0) {
			time--;
			yield return new WaitForSeconds(1);
		}
			
		GetComponent<TextMesh>().text = "";

		//GetComponent<TextMesh>().material.color.a -= 0.4 * Time.deltaTime;
		//text.text = "";
		//textDisplayed = false;
	}
}
