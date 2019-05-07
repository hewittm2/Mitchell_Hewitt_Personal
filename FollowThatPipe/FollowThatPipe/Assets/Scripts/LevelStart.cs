//Ethan Quandt
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStart : MonoBehaviour {
	LineRenderer oilFlow;
	float startDelay;
	public float time;
	public bool start;
	public bool activate;
	public bool filling = false;
	//bool filled = false;
	int cornerCon;
	PipeLine hitLine;
	PipeCorner hitCorner;
	public Sprite[] levelStarts;
	public Text label;
	public string[] labels;
	GameManager1 gm;
	PipeRotation pr;
	public GameObject station;
	int level;
	//public GameObject facts;
	// Use this for initialization
	void Awake() {
		gm = FindObjectOfType<GameManager1> ();
		pr = FindObjectOfType<PipeRotation> ();
        gm.countDown.enabled = true;
		//facts = gm.factsRun;
		oilFlow = gameObject.GetComponentInChildren<LineRenderer> ();
		oilFlow.enabled = false;
		//gm.countDown.enabled = true;
		gm.countDown.sprite = gm.count [0];
		pr.start = false;
		level = gm.level;
		startDelay = 0;
		GetComponentInChildren<SpriteRenderer>().sprite = levelStarts [FindObjectOfType<GameManager1> ().level -1];
		label.text = labels [FindObjectOfType<GameManager1> ().level - 1];
	}
	void Start(){
		//facts = GameObject.FindGameObjectWithTag ("Facts");
		switch (level) {
		case 1:
			station.transform.localPosition = new Vector3 (-2, .33f, 0);
			break;
		case 2:
			station.transform.localPosition = new Vector3 (-2.58f, .02f, 0);
			break;
		case 3:
			station.transform.localPosition = new Vector3 (-2.45f, .51f, 0);
			break;
		case 4:
			station.transform.localPosition = new Vector3 (-2.43f, -.03f, 0);
			break;
		case 5:
			station.transform.localPosition = new Vector3 (-2.36f, .24f, 0);
			break;
		}
	}

	void Update () {
		startDelay += Time.deltaTime;
		if (!start) {
			if (startDelay <= 1) {
				gm.factsRun.SetActive (true);
				gm.countDown.gameObject.SetActive(false);
			}else if (startDelay <= 2 && startDelay > 1) {
				gm.countDown.gameObject.SetActive (true);
				gm.countDown.gameObject.transform.localPosition = new Vector3 (880, -464, 0);
				gm.countDown.gameObject.transform.localScale = new Vector3 (1.25f,1.25f , 1);
				gm.countDown.sprite = gm.count [0];
			} else if (startDelay <= 3 && startDelay > 2) {
				//print ("2");
				//gm.start.image.sprite = gm.count [1];
				gm.countDown.sprite = gm.count [1];
			} else if (startDelay <= 4 && startDelay > 3) {
				//print ("1");
				//gm.start.image.sprite = gm.count [2];
				gm.countDown.sprite = gm.count [2];
			}else if(startDelay <= 5 && startDelay > 4){
				gm.factsRun.SetActive(false);
				gm.countDown.gameObject.transform.localPosition = new Vector3 (0, 0, 0);
				gm.countDown.gameObject.transform.localScale = new Vector3 (5, 5, 1);
				gm.countDown.sprite = gm.count [3];
			} else {
				activate = true;
				pr.start = true;
				//gm.start.enabled = false;
				gm.countDown.gameObject.SetActive(false);
				//gm.factsRun.SetActive (false);
			}
		}
		if (!filling) {
			if (activate) {
				oilFlow.enabled = true;
				StartCoroutine (LineDraw());
			}
		}
	}

	IEnumerator LineDraw(){
		filling = true;
		float t = 0;
		Vector3 origin = oilFlow.GetPosition (0);
		Vector3 origin2 = oilFlow.GetPosition (1);
		Vector3 newpos;
		oilFlow.SetPosition (1, origin);
		for(;t<time; t += Time.deltaTime){
			newpos = Vector3.Lerp (origin, origin2, t / time);
			oilFlow.SetPosition (1, newpos);
			yield return null;
		}
		oilFlow.SetPosition (1, origin2);
		CheckForNextPipe ();

	}

	void CheckForNextPipe(){
		Vector3 rayDir = Vector3.right;
		RaycastHit hit;
		if (Physics.Raycast (transform.position, rayDir, out hit, 2.5f)) {
			if (hit.collider.gameObject.GetComponent<PipeLine> ()) {
				hitLine = hit.collider.gameObject.GetComponent<PipeLine> ();
				if (hitLine.facing == 2) {
					hitLine.reverse = true;
					hitLine.activate = true;
				} else if (hitLine.facing == 4) {
					hitLine.activate = true;
				} else {
					FindObjectOfType<GameManager1> ().Lose();
				} 
			} else if (hit.collider.gameObject.GetComponentInChildren<PipeCorner> ()) {
				hitCorner = hit.collider.gameObject.GetComponentInChildren<PipeCorner> ();
				if (hitCorner.facing == 2) {
					hitCorner.reverse = true;
					hitCorner.activate = true;
				} else if (hitCorner.facing == 3) {
					hitCorner.activate = true;
				}else {
					FindObjectOfType<GameManager1> ().Lose();
				}
			} else {
				FindObjectOfType<GameManager1> ().Lose();
			}
		} else {
			FindObjectOfType<GameManager1> ().Lose();
		}
	}
}
