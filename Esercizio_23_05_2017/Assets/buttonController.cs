using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonController : MonoBehaviour {

	public fighterController fc;
	public float movingDuration = 0.4f;

	private Vector3 moveAxis;
	private float timer;

	// Use this for initialization
	void Start () {
		fc = GetComponent<fighterController> ();
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 axis = Vector3.zero;

		if (timer < movingDuration) {
			timer += Time.deltaTime;
		
			axis = moveAxis;
		}

		fc.move (axis);
	}

	public void forward(){
		moveAxis = new Vector3 (-1, 0, 0);
		timer = 0;
	}

	public void backward (){
		moveAxis = new Vector3 (1, 0, 0);
		timer = 0;
	}

	public void punch(){
		fc.punch ();
	}
}
