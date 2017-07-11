using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; //NON TE LO SCORDARE QUESTO !!!!
using UnityEngine;

public class FasoloTimer : MonoBehaviour {

	//durata del timer in millisecondi
	public float duration = 10;
	public GameObject fasolo;

	//stato del timer (quando arriva a 0 finisce)
	private float timer = 10;
	private Text timerText;

	// Use this for initialization
	void Start () {
		
		timerText = transform.GetChild (0).GetComponent<Text> ();
		//faccio partire il timer
		StartCoroutine ("timerRoutine");
	}
	
	// Update is called once per frame
	void Update () {
	}


	//aggiorna lo stato del timer
	public void updateTimer(){
		timerText.text = ""+Mathf.RoundToInt(timer);
	}

	public void fasolcalypse (){
		Debug.Log ("FASOLCALYPSE");
		fasolo.GetComponent<FasolCalypse> ().inizioDellaFine();
	}

	//coroutine per gestire il timer
	IEnumerator timerRoutine(){

		//inizializzo il timer alla quantità di tempo che deve passare
		timer = duration;
		
		while (timer > 0) {
			//sottraggo al timer il tempo trascorso in questo frame
			timer -= Time.deltaTime;

			//azzero perche potrebbe essere passato troppo tempo
			if (timer < 0)	timer = 0;
			updateTimer ();

			yield return null;
		}

		//fine timer, lancio gli eventi
		updateTimer ();
		fasolcalypse ();
	}
}
