using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testina : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		int face = Mathf.CeilToInt(other.gameObject.GetComponent<Renderer>().material.mainTextureOffset.y/0.166f);
		transform.parent.GetComponent<polygonRoller> ().setSymbolValue (face);
	}
}
