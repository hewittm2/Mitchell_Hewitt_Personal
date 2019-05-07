/*Ethan Quandt
 * Mitchell Hewitt
 * Pipe Rotation
 * Pipe Dream 
 * 2/8/2018*/

using UnityEngine;
using UnityEngine.UI;

//This script handle the rotation of pipe segments using a RayCast from the camera and incementing the pipe rotation.

public class PipeRotation : MonoBehaviour {
    
    [HideInInspector] public Camera mainCam;
    private int pipeRotate;
	public int rayLength;
	[HideInInspector] public bool holdingPipe;
	[HideInInspector] public GameObject pipe;
	GameObject tileSpace;
	public GameObject line;
	public GameObject corner;
	public bool start;
	public bool delete = false;
	public GameObject lineHUD;
	public GameObject cornerHUD;
	public GameObject deleteHUD;
	public GameObject fastHUD;
	GameObject selected = null;
	Transform startScale;
	Transform largeScale;
	public Material highlight;
	//Vector3 mousePos = new Vector3 ();
    //Allows access to the pipe rotation
    public int PipeRotate{
        get{
            return pipeRotate;
        }
    }

    //Sets the rotation once placed
    void Awake(){
        pipeRotate = 1;
		mainCam = transform.GetComponent<Camera>();
    }

    void Update(){
		if (Input.GetKeyDown (KeyCode.Mouse0) && start) {
			Click();
		}
    }

    void Click(){
		Vector3 point = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.z));
		//print (point);
        RaycastHit hit;
		Debug.DrawRay(point, transform.forward, Color.red, 10);
		if (Physics.Raycast(point, mainCam.transform.forward, out hit, rayLength)){
			//print (hit.collider.gameObject.name);
			if (hit.collider.gameObject.name == "StraightFinal(Clone)") {
				if (hit.collider.GetComponent<PipeLine> ().filling == false) {
					if (pipe == line) {
						ClickLine (hit);
					} else if (pipe == corner) {
						tileSpace = hit.collider.GetComponentInParent<TileSpace> ().gameObject;
						tileSpace.GetComponent<TileSpace> ().hasPipe = false;
						Destroy (hit.collider.gameObject);
						Placement (hit,tileSpace);
					}
				}
			}
			else if (hit.collider.gameObject.name == "CornerFinal(Clone)") {
				if(hit.collider.GetComponentInChildren<PipeCorner> ().filling == false){
					if (pipe == corner) {
						ClickCorner (hit);
					} else if (pipe == line) {
						tileSpace = hit.collider.GetComponentInParent<TileSpace> ().gameObject;
						tileSpace.GetComponent<TileSpace>().hasPipe = false;
						Destroy(hit.collider.gameObject);
						Placement (hit,tileSpace);
					}
				}
			}
			else if (hit.collider.gameObject.GetComponent<TileSpace> ()) {
				tileSpace = hit.collider.gameObject;
				Placement (hit,tileSpace);
			}
    	}
	}
		

	void ClickLine(RaycastHit hit){
		if (!hit.collider.gameObject.GetComponent<PipeLine> ().filling) {
			if (hit.collider.gameObject.GetComponent<PipeLine> ().facing <= 3) {
				hit.collider.gameObject.GetComponent<PipeLine> ().facing++;
			} else {
				hit.collider.gameObject.GetComponent<PipeLine> ().facing = 1;
			}
		}
	}

	void ClickCorner(RaycastHit hit){
		if (!hit.collider.gameObject.GetComponentInChildren<PipeCorner> ().filling) {
			if (hit.collider.gameObject.GetComponentInChildren<PipeCorner> ().facing <= 3) {
				hit.collider.gameObject.GetComponentInChildren<PipeCorner> ().facing++;
			} else {
				hit.collider.gameObject.GetComponentInChildren<PipeCorner> ().facing = 1;
			}
		}
	}


	void Placement(RaycastHit hit,GameObject tileSpace){
		
		TileSpace tile = tileSpace.GetComponent<TileSpace> ();
		if (holdingPipe) {
			if (!tile.hasPipe) {
				GameObject temp = Instantiate (pipe, tile.transform.position + new Vector3(0,0,-.5f), Quaternion.identity) as GameObject;
				temp.transform.SetParent (tile.transform);
				temp.tag = "Playable";
				tile.hasPipe = true;
				//holdingPipe = false;
				//pipe = null;
				temp = null;
			}
		}
	}
	public void SelectLine(){
		delete = false;
		if (selected != lineHUD) {
			selected = lineHUD;
			lineHUD.transform.localScale += new Vector3 (.3f, .3f, .3f);
			lineHUD.transform.GetComponent<Image> ().material = highlight;
		}
		cornerHUD.transform.localScale = new Vector3(1.25f, 1.25f, 1);
		fastHUD.transform.localScale = new Vector3(1,1,1);

		deleteHUD.transform.GetComponent<Image> ().material = null;
		cornerHUD.transform.GetComponent<Image> ().material = null;
		fastHUD.transform.GetComponent<Image> ().material = null;

		pipe = line;
		holdingPipe = true;
	}
	public void SelectCorner(){
		
		delete = false;
		//selected = cornerHUD;
		if (selected != cornerHUD) {
			selected = cornerHUD;
			cornerHUD.transform.localScale += new Vector3 (.3f, .3f, .3f);
			cornerHUD.transform.GetComponent<Image> ().material = highlight;
		}
		lineHUD.transform.localScale = new Vector3(1.5f, 1.25f, 1);
		fastHUD.transform.localScale = new Vector3(1,1,1);

		deleteHUD.transform.GetComponent<Image> ().material = null;
		lineHUD.transform.GetComponent<Image> ().material = null;
		fastHUD.transform.GetComponent<Image> ().material = null;
		pipe = corner;
		holdingPipe = true;
	}
	public void SelectDelete(){
		holdingPipe = false;
		//selected = deleteHUD;
		if (selected != deleteHUD) {
			selected = deleteHUD;
			deleteHUD.transform.localScale += new Vector3 (.3f, .3f, .3f);
			deleteHUD.transform.GetComponent<Image> ().material = highlight;
		}
		cornerHUD.transform.localScale = new Vector3(1.25f, 1.25f, 1);
		lineHUD.transform.localScale = new Vector3(1.5f, 1.25f, 1);
		fastHUD.transform.localScale = new Vector3(1,1,1);
		cornerHUD.transform.GetComponent<Image> ().material = null;
		lineHUD.transform.GetComponent<Image> ().material = null;
		fastHUD.transform.GetComponent<Image> ().material = null;
		delete = true;
	}
	public void FastFill (){
		PipeLine[] pipesOnGrid = FindObjectsOfType<PipeLine> ();
		PipeCorner[] cornersOnGrid = FindObjectsOfType<PipeCorner> ();
		foreach (PipeLine p in pipesOnGrid) {
			p.time = .5f;		
		}
		foreach (PipeCorner p in cornersOnGrid) {
			p.time = .5f;
		}
		if (selected != fastHUD) {
			selected = fastHUD;
			fastHUD.transform.localScale += new Vector3 (.3f, .3f, .3f);
			fastHUD.transform.GetComponent<Image> ().material = highlight;
		}
		lineHUD.transform.localScale = new Vector3(1.5f, 1.25f, 1);
		cornerHUD.transform.localScale = new Vector3(1.25f, 1.25f, 1);

		lineHUD.transform.GetComponent<Image> ().material = null;
		cornerHUD.transform.GetComponent<Image> ().material = null;
		deleteHUD.transform.GetComponent<Image> ().material = null;
	}
	public void Deselect(){
		holdingPipe = false;
		delete = false;
		selected = null;
		lineHUD.transform.localScale = new Vector3(1.5f, 1.25f, 1);
		cornerHUD.transform.localScale = new Vector3(1.25f, 1.25f, 1);
		fastHUD.transform.localScale = new Vector3(1,1,1);
		lineHUD.transform.GetComponent<Image> ().material = null;
		cornerHUD.transform.GetComponent<Image> ().material = null;
		deleteHUD.transform.GetComponent<Image> ().material = null;
		fastHUD.transform.GetComponent<Image> ().material = null;
	}
}
