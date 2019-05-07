//Ethan Quandt
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelEnd : MonoBehaviour {
	SpriteRenderer sprite;
	public Text labelDisplay;
	public Sprite[] levelEnds;
	public string[] labels;
	public GameObject station;
	GameManager1 gm;
	int level;
	// Use this for initialization
	void Start () {
		gm = FindObjectOfType<GameManager1> ();
		level = gm.level;
		labelDisplay.text = labels [level - 1];
		GetComponentInChildren<SpriteRenderer>().sprite = levelEnds [level -1];
		switch (level) {
		case 1:
			station.transform.localPosition = new Vector3 (2.5f, .15f, 0);
			break;
		case 2:
			station.transform.localPosition = new Vector3 (2.25f, .65f, 0);
			break;
		case 3:
			station.transform.localPosition = new Vector3 (2.5f, .04f, 0);
			break;
		case 4:
			station.transform.localPosition = new Vector3 (2f, .12f, 0);
			break;
		case 5:
			station.transform.localPosition = new Vector3 (2.5f, .22f, 0);
			break;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
