using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyText : MonoBehaviour {
	public Text fore;
	public Text back;
	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update () {
		back.text = fore.text;
	}
}
