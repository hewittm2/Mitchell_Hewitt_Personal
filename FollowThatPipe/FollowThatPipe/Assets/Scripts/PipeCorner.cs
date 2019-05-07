//Ethan Quandt
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCorner : MonoBehaviour {
	LineRenderer oilFlow1;
	public LineRenderer oilFlow2;
	public bool filling = false;
	public bool reverse;
	public bool activate;
	public float time;
	public int facing;
	int cornerCon;
	int lineCon;
	PipeLine hitLine;
	PipeCorner hitCorner;
	public SpriteRenderer main;
	public SpriteRenderer clear;
	public Sprite[] mains;
	public Sprite[] clears;

	// Use this for initialization
	void Start () {
		oilFlow1 = gameObject.GetComponent<LineRenderer> ();
		//oilFlow2 = GetComponentInChildren<LineRenderer> ();
		oilFlow1.enabled = false;
		oilFlow2.enabled = false;
		facing = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (!filling) {
			if (activate) {
				//oilFlow1.enabled = true;
				StartCoroutine (LineDraw());
			}
		}
		switch (facing) {
		case 1:
			gameObject.transform.eulerAngles = new Vector3 (0, 0, 0);
			//gameObject.transform.position += new Vector3 (1, 1, 0);f
			gameObject.transform.localPosition = new Vector3 (.27f, .24f, 0);
			gameObject.transform.localScale = new Vector3 (.78f, .8f, 1f);
			main.sprite = mains [0];
			clear.sprite = clears [0];
			break;
		case 2:
			gameObject.transform.eulerAngles = new Vector3 (0, 0, 90);
			gameObject.transform.localPosition = new Vector3 (-.36f,.21f, 0);
			gameObject.transform.localScale = new Vector3 (.74f, .84f, 1f);
			main.sprite = mains [1];
			clear.sprite = clears [1];
			break;
		case 3:
			gameObject.transform.eulerAngles = new Vector3 (0, 0, 180);
			gameObject.transform.localPosition = new Vector3 (-.18f, -.17f, 0);
			gameObject.transform.localScale = new Vector3 (.74f, .86f, 1f);
			main.sprite = mains [2];
			clear.sprite = clears [2];
			break;
		case 4: 
			gameObject.transform.eulerAngles = new Vector3 (0, 0, 270);
			gameObject.transform.localPosition = new Vector3 (.2f, -.17f, 0);
			gameObject.transform.localScale = new Vector3 (.78f, .86f, 1f);
			main.sprite = mains [3];
			clear.sprite = clears [3];
			break;
		}	

	}
	IEnumerator LineDraw(){
		filling = true;
		float t = 0;
		Vector3 origin = oilFlow1.GetPosition (0);
		Vector3 origin2 = oilFlow1.GetPosition (1);
		Vector3 origin3 = oilFlow2.GetPosition (0);
		Vector3 origin4 = oilFlow2.GetPosition (1);
		Vector3 newpos;
		if (!reverse) {
			oilFlow1.enabled = true;
			oilFlow1.SetPosition (1, origin);
			//oilFlow.SetPosition (2, origin);
			for (; t < time/2; t += Time.deltaTime) {
				newpos = Vector3.Lerp (origin, origin2, t / (time/2));
				oilFlow1.SetPosition (1, newpos);
				yield return null;
			}
			t = 0;

			oilFlow2.SetPosition (1, origin3);
			oilFlow2.enabled = true;
			for (; t < time / 2; t += Time.deltaTime) {
				newpos = Vector3.Lerp (origin3, origin4, t / (time / 2));
				oilFlow2.SetPosition (1, newpos);
				yield return null;
			}
			oilFlow1.SetPosition (1, origin2);
			oilFlow2.SetPosition (1, origin4);
			CheckForNextPipe ();
		} else {
			oilFlow2.enabled = true;
			oilFlow2.SetPosition (0, origin4);
			for (; t < time/2; t += Time.deltaTime) {
				newpos = Vector3.Lerp (origin4, origin3, t / (time/2));
				oilFlow2.SetPosition (0, newpos);
				yield return null;
			}
			t = 0;
			oilFlow1.SetPosition (0, origin2);
			oilFlow1.enabled = true;
			for (; t < time / 2; t += Time.deltaTime) {
				newpos = Vector3.Lerp (origin2, origin, t / (time / 2));
				oilFlow1.SetPosition (0, newpos);
				yield return null;
			}
			oilFlow1.SetPosition (0, origin);
			CheckForNextPipe ();
		}
	}
	void CheckForNextPipe(){
		Vector3 rayDir = new Vector3 ();
		int opposite;
		if (facing == 1) {
			if (reverse) {
				rayDir = Vector3.right;
				opposite = 2;
				cornerCon = 3;
				lineCon = 4;
			} else {
				rayDir = Vector3.up;
				opposite = 3;
				cornerCon = 4;
				lineCon = 1;
			}
			ActivatePipe (rayDir, opposite);
		} else if (facing == 2) {
			if (reverse){ 
				rayDir = Vector3.up;
				opposite = 3;
				cornerCon = 4;
				lineCon = 1;
			}else{ 
				rayDir = Vector3.left;
				opposite = 4;
				cornerCon = 1;
				lineCon = 2;
			}
			ActivatePipe (rayDir, opposite);
		} else if (facing == 3) {
			if (reverse){
				rayDir = Vector3.left;
				opposite = 4;
				cornerCon = 1;
				lineCon = 2;
			}else{
				rayDir = Vector3.down;
				opposite = 1;
				cornerCon = 2;
				lineCon = 3;
			}
			ActivatePipe (rayDir, opposite);
		} else if (facing == 4) {
			if (reverse){
				rayDir = Vector3.down;
				opposite = 1;
				cornerCon = 2;
				lineCon = 3;
			}else{
				rayDir = Vector3.right;
				opposite = 2;
				cornerCon = 3;
				lineCon = 4;
			}
			ActivatePipe (rayDir, opposite);
		}
	}
	void ActivatePipe(Vector3 dir, int opposite){
		RaycastHit hit;
		if(Physics.Raycast (transform.position, dir, out hit,2.5f)){
			if (hit.collider.gameObject.GetComponent<PipeLine> ()) {
				hitLine = hit.collider.gameObject.GetComponent<PipeLine> ();
				//double check directions
				if (!hitLine.filling) {
					if (reverse) {
						if (hitLine.facing == lineCon) {
							hitLine.activate = true;
						} else if (hitLine.facing == opposite) {
							hitLine.reverse = true;
							hitLine.activate = true;
						} else {
							FindObjectOfType<GameManager1> ().Lose ();
							print ("Lose");
						}
					} else {
						if (hitLine.facing == facing) {
							hitLine.activate = true;
						} else if (hitLine.facing == opposite) {
							hitLine.reverse = true;
							hitLine.activate = true;
						} else {
							FindObjectOfType<GameManager1> ().Lose ();
							print ("Lose");
						}
					}
				} else {
					FindObjectOfType<GameManager1> ().Lose ();
				}
			} else if (hit.collider.gameObject.GetComponentInChildren<PipeCorner> ()) {
				hitCorner = hit.collider.gameObject.GetComponentInChildren<PipeCorner> ();
				if (!hitCorner.filling) {
					if (reverse) {
						if (hitCorner.facing == opposite) {
							hitCorner.reverse = true;
							hitCorner.activate = true;
						} else if (hitCorner.facing == cornerCon) {
							hitCorner.activate = true;
						} else {
							FindObjectOfType<GameManager1> ().Lose ();
							print ("Lose");
						}
					} else {
						if (hitCorner.facing == cornerCon) {
							hitCorner.activate = true;
						} else if (hitCorner.facing == opposite) {
							hitCorner.reverse = true;
							hitCorner.activate = true;
						} else {
							FindObjectOfType<GameManager1> ().Lose ();
							print ("Lose");
						}
					}
				} else {
					FindObjectOfType<GameManager1> ().Lose ();
				}
			} else if (hit.collider.gameObject.GetComponent<LevelEnd>()) {
				FindObjectOfType<GameManager1> ().LevelInc();
				Debug.Log ("TestWin");
			} else {
				FindObjectOfType<GameManager1> ().Lose();
				Debug.Log ("Lose");
			}
		} else {
			FindObjectOfType<GameManager1> ().Lose();
			Debug.Log ("Lose");
		}
	}

}
