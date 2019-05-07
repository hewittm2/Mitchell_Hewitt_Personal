//Cale Toburen
//Ethan Quandt
//3-1-18
//Pipe Dream - CSG 119
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager1 : MonoBehaviour {
    //private int pipeNum;
	//public int pipeAmount;
	//public GameObject pipeOne;
	//private GameObject pipeTwo;
	//private GameObject pipeThree;
	//private GameObject pipeFour;

	//GameObjects HUD
    [Header("HUD")]
	public Image countDown;
	public Button start;
	public Sprite[] count;
	public GameObject gameOver;
	public GameObject credits;
	public GameObject HUD;
	public GameObject go;
	public GameObject powerOn;
	public GameObject controlStart;
	public GameObject controlRun;

    //GameObjects LevelGeneration
    [Header("Level Generation")]
    [SerializeField] private GameObject snapPoints;
    [SerializeField] private GameObject obstacle;
	[SerializeField] private GameObject levelEnd;
	[SerializeField] private GameObject levelStart;
	[SerializeField] private GameObject levelStart2;

    //Grid Variables
    [Header("Grid Variables")]
    private int gridSizeX;
	private int gridSizeY;
    private float centerX;
    private float centerY;
    private float offsetX;
	private float offsetY;
	public float vertOffset;
	public float fillRate;

    //Position Lists
    [HideInInspector] public List<int> startSpot;
	[HideInInspector] public List<int> endSpot;
	[HideInInspector] public List<int> obX;
	[HideInInspector] public List<int> obY;

    //Facts Display
    [Header("Facts Display")]
    public List<string> facts;
	List<string> factsCopy;
	int factPicker;
	public GameObject factsRun;
	Text factDisplay;
	public GameObject factStart;
	Text factStartDisplay;
	float counter;
	int factStartPicker;

    //Runtime Tracking
    [Header("Level and Win Conditions")]
    PipeRotation pr;
    private bool win;
    //private bool lose;
	bool started;
	public bool levelSwitch;
	int totalEnds;
	int endsHit;
	public int level = 1;

    void Awake(){
		HUD.SetActive (false);
		started = false;
		pr = FindObjectOfType<PipeRotation> ();
		factsCopy = new List<string>(facts);
		factStartDisplay = factStart.GetComponentInChildren<Text> ();
		factStartDisplay.text = factsCopy [0];
		factPicker = Random.Range(0, facts.Count - 1);
		factDisplay = factsRun.GetComponentInChildren<Text> ();
		factDisplay.text = facts[factPicker];
		facts.RemoveAt (factPicker);
		countDown.gameObject.SetActive (false);
		//controlRun.SetActive (false);
		//controlStart.SetActive (true);
		gameOver.SetActive (false);
		credits.SetActive (false);
    }
    
    void Start (){
		
    }
	
	void Update (){
        if (levelSwitch){
			if (level < 5) {
				level += 1;
                StartUp();
            } else {
                Win();

			}
		
			pr.Deselect ();
            
        }
		if (!started) {
			factStart.SetActive (true);
			factsRun.SetActive (false);
			StartCoroutine (FactDisplay ());
		}  
    }

	public void LevelInc(){
		endsHit++;
		if (endsHit == totalEnds) {
			levelSwitch = true;
		}
	}

	public void Launch(){
		if (!started) {
			StartUp ();
		}
	}
	IEnumerator FactDisplay(){
		counter += Time.deltaTime;
		if (counter >= 10) {
			counter = 0;
			factStartPicker++;
			if (factStartPicker >= factsCopy.Count) {
				factStartPicker = 0;
			}
			factStartDisplay.text = factsCopy [factStartPicker];
		}
		yield return null;
	}

    public void StartUp(){
		started = true;
		//controlRun.SetActive (true);
		//controlStart.SetActive (false);
		factsRun.SetActive (true);
		factStart.SetActive (false);
		go.SetActive (false);
		powerOn.SetActive (false);
		countDown.gameObject.SetActive (true);
		HUD.SetActive (true);
		pr.pipe = null;
		pr.holdingPipe = false;
		endsHit = 0;
		factPicker = Random.Range(0, facts.Count - 1);
		factDisplay.text = facts[factPicker];
		facts.RemoveAt (factPicker);
        switch (level){
		case 1:
			gridSizeX = 4;
			gridSizeY = 3;
			totalEnds = 1;
			fillRate = 6;
			ListClear ();
                break;
		case 2:
			gridSizeX = 5;
			gridSizeY = 4;
			totalEnds = 1;
			fillRate = 6;
			ListClear ();
                break;
		case 3:
			gridSizeX = 6;
			gridSizeY = 5;
			totalEnds = 2;
			fillRate = 10;
			vertOffset = 1.08f;
			ListClear ();
                break;
		case 4:
			gridSizeX = 7;
			gridSizeY = 5;
			totalEnds = 2;
			fillRate = 10;
			vertOffset = 1.08f;
			ListClear ();
                break;
		case 5:
			gridSizeX = 8;
			gridSizeY = 5;
			totalEnds = 3;
			fillRate = 14;
			vertOffset = 1.08f;
			ListClear ();
			break;
        }
        levelSwitch = false;
		offsetX = snapPoints.transform.lossyScale.x;
		offsetY = snapPoints.transform.lossyScale.y;
        if (level != 6){
			int endPlacement = Random.Range (1, endSpot.Count);
			int startPlacement = Random.Range (1, startSpot.Count);
            centerX = ((gridSizeX - 1) * offsetX) / 2;
            centerY = ((gridSizeY - 1) * offsetY) / 2;
            for (int i = 0; i < gridSizeX; i++){
				for (int j = 0; j < gridSizeY; j++) {	
					Instantiate (snapPoints, new Vector3 (((i * offsetX) - centerX), ((j * offsetY) - centerY) + vertOffset, -1), Quaternion.identity);
				}
            }
			GameObject temp = Instantiate (levelStart, new Vector3 (((- 1 * offsetX) - centerX), ((startSpot [startPlacement] * offsetY) - centerY) + vertOffset, -1.5f), Quaternion.identity);
			temp.GetComponent<LevelStart> ().time = fillRate;
			Instantiate (levelEnd, new Vector3 ((gridSizeX * offsetX) - centerX,(endSpot[endPlacement] * offsetY) - centerY + vertOffset, -1.5f),Quaternion.identity);
			startSpot.RemoveAt (startPlacement);
			endSpot.RemoveAt (endPlacement);
			if (level >= 3) {
				startPlacement = Random.Range (0, startSpot.Count);
				endPlacement = Random.Range (0, endSpot.Count);
				temp = Instantiate (levelStart2, new Vector3 (((- 1 * offsetX) - centerX), ((startSpot[startPlacement] * offsetY) - centerY)+ vertOffset, -1.5f), Quaternion.identity);
				temp.GetComponent<LevelStart2> ().time = fillRate;
				Instantiate (levelEnd, new Vector3 (((gridSizeX* offsetX) - centerX), ((endSpot[endPlacement] * offsetY) - centerY)+ vertOffset, -1.5f), Quaternion.identity);
				startSpot.RemoveAt (startPlacement);
				endSpot.RemoveAt (endPlacement);
			}if (level >= 5) {
				startPlacement = Random.Range (0, startSpot.Count);
				endPlacement = Random.Range (0, endSpot.Count);
				temp = Instantiate (levelStart2, new Vector3 (((- 1 * offsetX) - centerX), ((startSpot[startPlacement] * offsetY) - centerY)+ vertOffset, -1.5f), Quaternion.identity);
				temp.GetComponent<LevelStart2> ().time = fillRate;
				Instantiate (levelEnd, new Vector3 (((gridSizeX * offsetX) - centerX), ((endSpot[endPlacement] * offsetY) - centerY)+ vertOffset, -1.5f), Quaternion.identity);
				startSpot.RemoveAt (startPlacement);
				endSpot.RemoveAt (endPlacement);
			}
			for (int i = 1; i < gridSizeY-2; i++) {
				int numX = Random.Range(0,obX.Count);
				int numY = Random.Range(0,obY.Count-1);
				Instantiate (obstacle, new Vector3 (((obX [numX] * offsetX) - centerX), ((obY [numY] * offsetY) - centerY)+ vertOffset, -1.5f), Quaternion.identity);
				obX.RemoveAt (numX);
				obY.RemoveAt (numY);
			}
        }
    }

    public void Win(){
        print("Winner");
        //win = true;
		pr.Deselect();
		started = false;
		go.SetActive (true);
		HUD.SetActive (false);
		powerOn.SetActive (true);
        countDown.enabled = false;
        levelSwitch = false;
        level = 1;
        //controlRun.SetActive (false);
        //controlStart.SetActive (true);
        facts.Clear ();
		facts.AddRange (factsCopy);
		StartCoroutine (TimedDisplay (credits,10));
		GameObject[] things = GameObject.FindGameObjectsWithTag("Playable");
		foreach (GameObject g in things)
		{
			Destroy(g);
			//print("asdf");
		}
		ListClear ();
    }

    public void Lose(){
		pr.Deselect();
		started = false;
		go.SetActive (true);
		HUD.SetActive (false);
		powerOn.SetActive (true);
        countDown.enabled = false;
        levelSwitch = false;
        level = 1;
        //controlRun.SetActive (false);
        //controlStart.SetActive (true);
        facts.Clear ();
		facts.AddRange (factsCopy);
		StartCoroutine (TimedDisplay (gameOver,3));
		GameObject[] things = GameObject.FindGameObjectsWithTag("Playable");
		foreach (GameObject g in things)
		{
			Destroy(g);
			//print("asdf");
		}
    }

	IEnumerator TimedDisplay(GameObject display,float time){
		display.SetActive (true);
		yield return new WaitForSeconds(time);
		display.SetActive (false);
    }

	public void ListClear(){
		startSpot.Clear ();
		endSpot.Clear ();
		obX.Clear ();
		obY.Clear ();
        GameObject[] things = GameObject.FindGameObjectsWithTag("Playable");
        foreach (GameObject g in things)
        {
            Destroy(g);
        }
        for (int i = 0; i < gridSizeY; i++) {
			startSpot.Add (i);
			endSpot.Add (i);
		}for (int i = 2; i < (gridSizeX-2); i++) {
			obX.Add (i);
			obX.Add (i);
		}for (int i = 1; i < gridSizeY; i++) {
			obY.Add (i);
		}
	}
}
