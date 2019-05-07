//Ethan Quandt
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoadBlock : MonoBehaviour {
	public Sprite[] levelEnds;
	public int rayLength;
	public Color danger;
	public Sprite hasObstacle;
	// Use this for initialization
	void Start () {
		int rand = Random.Range (0, levelEnds.Length -1);
		GetComponentInChildren<SpriteRenderer>().sprite = levelEnds [rand];
		//Debug.DrawRay (transform.position, Vector3.forward, danger, 10);

	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.forward, out hit)) {
			//print (hit.collider.gameObject);
			hit.collider.gameObject.GetComponent<TileSpace> ().hasPipe = true;
			hit.collider.gameObject.GetComponent<TileSpace> ().main.sprite = hasObstacle;
			hit.collider.gameObject.GetComponent<TileSpace> ().tileBlue = danger;
		}
	}
}
