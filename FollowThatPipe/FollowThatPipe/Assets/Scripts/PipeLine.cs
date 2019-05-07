using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeLine : MonoBehaviour {
	LineRenderer oilFlow;
	public float time;
	public bool reverse;
	public bool activate;
	public bool filling = false;
	//bool filled = false;
	public int facing;
	int cornerCon;
	PipeLine hitLine;
	PipeCorner hitCorner;
	public SpriteRenderer main;
	public SpriteRenderer clear;
	public Sprite[] mains;
	public Sprite[] clears;
	public GameObject front;
	// Use this for initialization
	void Awake(){
		facing = 1;
	}
	void Start () {
		oilFlow = gameObject.GetComponent<LineRenderer> ();
		oilFlow.enabled = false;
		//front = GetComponentInChildren<GameObject> ();
		clear = front.GetComponent<SpriteRenderer> ();

	}

	// Update is called once per frame
	void Update () {
		if (!filling) {
			if (activate) {
				oilFlow.enabled = true;
				StartCoroutine (LineDraw());
			}
		}
		switch (facing) {
		case 1:
			gameObject.transform.eulerAngles = new Vector3 (0, 0, 0);
			main.sprite = mains [0];
			clear.sprite = clears [0];
			front.transform.localScale = new Vector3 (1.06f, 1.02f, 1f);
			break;
		case 2:
			gameObject.transform.eulerAngles = new Vector3 (0, 0, 90);
			main.sprite = mains [1];
			clear.sprite = clears [1];
			front.transform.localScale = new Vector3 (1.15f, .93f, 1f);
			break;
		case 3:
			gameObject.transform.eulerAngles = new Vector3 (0, 0, 180);
			main.sprite = mains [2];
			clear.sprite = clears [2];
			front.transform.localScale = new Vector3 (1.06f, 1.02f, 1f);
			break;
		case 4: 
			gameObject.transform.eulerAngles = new Vector3 (0, 0, 270);
			main.sprite = mains [3];
			clear.sprite = clears [3];
			front.transform.localScale = new Vector3 (1.11f, .93f, 1f);
			break;
		}	

	}

	IEnumerator LineDraw(){
		filling = true;
		float t = 0;
		Vector3 origin = oilFlow.GetPosition (0);
		Vector3 origin2 = oilFlow.GetPosition (1);
		Vector3 newpos;
		if (reverse) {
			oilFlow.SetPosition (1, origin);
			for (; t < time; t += Time.deltaTime) {
				newpos = Vector3.Lerp (origin, origin2, t / time);
				oilFlow.SetPosition (1, newpos);
				yield return null;
			}
			oilFlow.SetPosition (1, origin2);
			CheckForNextPipe ();
		} else {
			oilFlow.SetPosition (0, origin2);
			for(;t<time; t += Time.deltaTime){
				newpos = Vector3.Lerp (origin2, origin, t / time);
				oilFlow.SetPosition (0, newpos);
				yield return null;
			}
			oilFlow.SetPosition (0, origin);
			CheckForNextPipe ();
		}
	}

	void CheckForNextPipe(){
		Vector3 rayDir = new Vector3 ();
		int opposite;
		if (facing == 1) {
			opposite = 3;
			if (reverse) {
				rayDir = Vector3.down;
				cornerCon = 2;
			} else { 
				rayDir = Vector3.up;
				cornerCon = 4;
			}
			ActivatePipe (rayDir, opposite);
		} else if (facing == 2) {
			opposite = 4;
			if (reverse) { 
				rayDir = Vector3.right;
				cornerCon = 3;
			} else { 
				rayDir = Vector3.left;
				cornerCon = 1;
			}
			ActivatePipe (rayDir, opposite);
		} else if (facing == 3) {
			opposite = 1;
			if (reverse) {
				rayDir = Vector3.up;
				cornerCon = 4;
			} else {
				rayDir = Vector3.down;
				cornerCon = 2;
			}
			ActivatePipe (rayDir, opposite);
		} else if (facing == 4) {
			opposite = 2;
			if (reverse) {
				rayDir = Vector3.left;
				cornerCon = 1;
			} else {
				rayDir = Vector3.right;
				cornerCon = 3;
			}
			ActivatePipe (rayDir, opposite);
		}
	}

	void ActivatePipe (Vector3 dir, int opposite)
	{
		RaycastHit hit;
		if (Physics.Raycast (transform.position, dir, out hit,2.5f)) {
			if (hit.collider.gameObject.GetComponent<PipeLine> ()) {
				hitLine = hit.collider.gameObject.GetComponent<PipeLine> ();
				if (!hitLine.filling) {
					if (reverse) {
						if (hitLine.facing == opposite) {
							hitLine.activate = true;
						} else if (hitLine.facing == facing) {
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
			}else {
				FindObjectOfType<GameManager1> ().Lose ();
			}
		} else if (hit.collider.gameObject.GetComponentInChildren<PipeCorner> ()) {
			hitCorner = hit.collider.gameObject.GetComponentInChildren<PipeCorner> ();
			if (!hitCorner.filling) {
				if (reverse) {
					if (hitCorner.facing == cornerCon) {
						hitCorner.activate = true;
					} else if (hitCorner.facing == facing) {
						hitCorner.reverse = true;
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
		}else {
			Debug.Log ("Lose");
			FindObjectOfType<GameManager1> ().Lose();
		}
	}
}