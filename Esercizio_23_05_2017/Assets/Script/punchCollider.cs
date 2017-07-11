using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class punchCollider : MonoBehaviour {

	private fighterController fc;

	// Use this for initialization
	void Start () {
		fc = transform.root.gameObject.GetComponent<fighterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		fc.punchTrigger (col,gameObject);
	}
}
