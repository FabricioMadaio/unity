using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasolCalypse : MonoBehaviour {

	public float duration  = 0.5f;
	private float timer;

	// Use this for initialization
	void Start () {
		timer = duration;
	}
	
	// Update is called once per frame
	void Update () {
		

		if (timer > 0) {
			//sottraggo al timer il tempo trascorso in questo frame
			timer -= Time.deltaTime;

			if (timer < 0) {
				//rendo l'oggetto invisibile
				gameObject.GetComponent<MeshRenderer> ().enabled = false;
				gameObject.GetComponent<Collider> ().enabled= false;

				newFasolo (0);
				newFasolo (0.05f);

				GameObject.Destroy (gameObject,2f);
			}
		}
	}

	public void newFasolo(float timeOffset){
		GameObject fasolo = Instantiate (gameObject, transform.position,transform.rotation,null);

		fasolo.GetComponent<MeshRenderer> ().enabled = true;
		fasolo.GetComponent<Collider> ().enabled= true;

		fasolo.transform.localScale = gameObject.transform.localScale*0.95f;

		fasolo.GetComponent<AudioSource> ().pitch = 1 + (duration+timeOffset);
		fasolo.GetComponent<FasolCalypse>().inizioDellaFine(duration*1.1f + timeOffset);
		fasolo.GetComponent<Rigidbody> ().velocity = new Vector3 (Random.value, Random.value, Random.value);
	}

	public void inizioDellaFine(){
		gameObject.SetActive (true);
	}

	public void inizioDellaFine(float nextTime){
		duration = nextTime;
		gameObject.SetActive (true);
	}
}
