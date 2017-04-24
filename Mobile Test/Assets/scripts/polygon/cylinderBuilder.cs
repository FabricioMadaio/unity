using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cylinderBuilder : MonoBehaviour {

	public int numFaces = 1;
	private GameObject face; 

	// Use this for initialization
	void Start () {
		
		face = transform.GetChild (0).gameObject;
	
		float radius = -face.transform.GetChild (0).transform.localPosition.z;
		float angle = 360.0f / numFaces;

		float size = 2*radius*Mathf.Tan(Mathf.Deg2Rad*180.0f / numFaces);
		size *= 0.1f;

		Debug.Log (size);
		face.transform.GetChild (0).transform.localScale = new Vector3 (size, size, size);

		for (int i = 1; i < numFaces; i++) {
			GameObject f = Instantiate (face,transform);
			f.transform.GetChild (0).transform.localScale = new Vector3 (size, size, size);
			f.transform.Rotate (i*angle,0,0);
			f.transform.GetChild (0).GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0,0.166f*i);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
