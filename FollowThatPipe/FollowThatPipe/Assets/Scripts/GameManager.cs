using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	int gridSize;
	public int level;
	public GameObject tileSpace;
	Vector3 mousePos;
	// Use this for initialization
	void Start () {
		switch (level) {
		case 1:
			gridSize = 3;
			break;
		case 2:
			gridSize = 5;
			break;
		case 3: 
			gridSize = 7;
			break;
		}

		float radius = tileSpace.transform.localScale.x;
		for (int i = 1; i <= gridSize; i++) {
			for (int j = 1; j <= gridSize; j++) {
				Instantiate (tileSpace, new Vector3 (i * radius, j * radius, 0),Quaternion.identity, gameObject.transform);
			}
		}	
	}
	
	// Update is called once per frame
	void Update () {
//		if (Input.GetKeyDown (KeyCode.Mouse0)) {
//			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//			RaycastHit hit;
//			if (Physics.Raycast (ray, out hit)) {
//			//	Debug.Log ("Test" + hit.transform.gameObject);
//				hit.transform.gameObject.GetComponent<TileSpace>().hasPipe = true;
//			}
//		}
		//mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//transform.position = new Vector3 (Mathf.Round (mousePos.x), Mathf.Round (mousePos.y), Mathf.Round (mousePos.z));
	}
}
