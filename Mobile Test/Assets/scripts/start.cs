using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class start : MonoBehaviour {

	public GameObject[] rollers;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < rollers.Length; i++) {
			rollers [i].GetComponent<roller> ().setStartScript (this);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		Touch[] myTouches = Input.touches;
		for (int i = 0; i < Input.touchCount; i++) {

			if (Input.touches [i].phase == TouchPhase.Moved) {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.touches [i].position);

				if (Physics.Raycast (ray, out hit, 100)) {
					if (hit.collider.gameObject.CompareTag ("rullo")) {
						roller r = hit.collider.gameObject.GetComponentInParent<roller> ();
						r.StartCoroutine ("coroutine_fire", -(Input.touches [i].deltaPosition.y/Input.touches[i].deltaTime)*4/Screen.height);
					}
				}
			}
		}


		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit, 100)) {
				if (hit.collider.gameObject.CompareTag ("rullo")) {
					roller r = hit.collider.gameObject.GetComponentInParent<roller> ();
					r.StartCoroutine ("coroutine_fire", 10);
				}
			}
		}
	}

	public void unlitAll(){
		for (int i = 0; i < rollers.Length; i++) {
			rollers [i].GetComponent<roller> ().lit (false);
		}
	}

	public void checkRow(){
		unlitAll ();

		for (int i = 0; i < rollers.Length; i++) {
			if (rollers [i].GetComponent<roller> ().isRunning())
				return;
		}

		int lastIndex = 0;
		int firstValue = rollers [0].GetComponent<roller> ().getSymbolValue ();
		Debug.Log (firstValue);
		for (int i = 1; i < rollers.Length; i++) {
			int val = rollers [i].GetComponent<roller> ().getSymbolValue();
			Debug.Log (val);
			if (val == firstValue) {
				rollers [lastIndex].GetComponent<roller> ().lit (true);
				rollers [i].GetComponent<roller> ().lit (true);
			} else {
				firstValue = val;
				lastIndex = i;
			}
		}
	}
}
