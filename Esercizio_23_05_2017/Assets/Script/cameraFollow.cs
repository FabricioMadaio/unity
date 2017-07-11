using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour {

	public GameObject player1;
	public GameObject player2;
	public float speed = 1;
	public float turnFollowSpeed = 10;

	public Vector3 cameraOffset = new Vector3(0,1,0);
	public float minDistance = 1;

	private fighterController f1;
	private fighterController f2;

	private Vector3 right;

	// Use this for initialization
	void Start () {
		
		f1 = player1.GetComponent<fighterController> ();
		f2 = player2.GetComponent<fighterController> ();

		right = player1.transform.right;
	}

	void Update (){
	}

	/*codice per muovere la camera in modo che ci siano sempre i due personaggi in primo piano*/

	// Update is called once per frame
	void LateUpdate () {

		Vector3 p1 = f1.getPosition();
		Vector3 p2 = f2.getPosition();

		Vector3 midPoint = (p1+p2) / 2f;
		float distance = Vector3.Distance (p1,p2);
		if (distance < minDistance)
			distance = minDistance;

		if (f2.isRagdoll)
			right = player1.transform.right;
		else
			right = Quaternion.Euler(0, -90, 0)*(player1.transform.position- player2.transform.position).normalized;

		//il vettore right rappresenta la direzione del vettore uscente dalla camera, quindi perpendicolare al vettore p1-p2

		Vector3 cameraFocus = midPoint 
								+ right*cameraOffset.z 
								+ player1.transform.forward*cameraOffset.x
								+ player1.transform.up*cameraOffset.y;

		/*calcolo della posizione della camera

			p1 ---- mid ---- p2
			 \       I       /
			   \     I     /		<- altezza cos(fov)*distance(p1,p2)
			     \   I   /				fov è l'angolo di apertura
			       \ I /

                  Camera
			
			camera focus è il punto dove la camera deve guardare (midpoint + offset a piacere)
			
			la nuova posizione della camera sarà camera focus spostato di qualche unità 
			in direzione del vettore right (uscente dalla camera: vettore tra camera e mid)

			per garantire che p1 e p2 siano visibili bisogna traslare in direzione right
			per quanto è la distanza tra p1 e p2 moltiplicata per il foV
		*/
		Vector3 cameraNewPos = cameraFocus - right*(distance)*
			Mathf.Cos(Mathf.Deg2Rad*Camera.main.fieldOfView);

		transform.position = Vector3.Lerp(transform.position,cameraNewPos,speed*Time.deltaTime);
		transform.LookAt (cameraFocus);
	}
}
