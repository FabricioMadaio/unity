using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class switchImage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void trigger(bool value){
		Debug.Log (value);
		gameObject.transform.GetChild(0).GetComponent<Image>().enabled = !value;
	}
}
