//Ethan Quandt
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpace : MonoBehaviour {
	public bool hasPipe = false;
	public Color tileBlue;
	Color tileStart;
	public SpriteRenderer main;
	// Use this for initialization
	void Start () {
		tileStart = gameObject.GetComponent<Renderer> ().material.color;
		main = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (hasPipe) {
			gameObject.GetComponent<Renderer> ().material.color = tileBlue;
		} else {
			gameObject.GetComponent<Renderer> ().material.color = tileStart;
		}

	}
}
