using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragItem : MonoBehaviour {
	float distance = 5;
	Vector3 startPos;
	bool objectPlaced = false;
	void Start () {
		startPos = gameObject.transform.position;
	}
	void OnMouseDrag(){
//		Vector3 mousePosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distance);
//		Vector3 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);
//		transform.position = objPosition;
//		StartCoroutine (CheckForTiles ());
//		} else {
//			transform.position = startPos;
//		}
	}
	// Use this for initialization
	IEnumerator CheckForTiles(){
		if (Input.GetKeyUp (KeyCode.Mouse0)) {
			Debug.Log ("test");
			objectPlaced = true;
		}

		
		yield return null;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Mouse0)) {
			Vector3 mousePosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distance);
			Vector3 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);
			transform.position = objPosition;
		}



		if (objectPlaced) {
			Ray ray = Camera.main.ScreenPointToRay (gameObject.transform.position);
			RaycastHit hit;
			Debug.Log (Physics.Raycast (ray, out hit));
			if (Physics.Raycast (ray, out hit)) {
				
				Debug.Log ("Test" + hit.transform.gameObject);
				hit.transform.gameObject.GetComponent<TileSpace> ().hasPipe = true;
			} else {
				transform.position = startPos;
			}
		}

	}
}
