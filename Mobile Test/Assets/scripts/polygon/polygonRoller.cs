using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class polygonRoller : MonoBehaviour {

	private Light spotLight;
	private GameObject cylinder;

	public float damping = 0.3f;
	public int numOfSymbols = 6;
	public float angleOffset = 15;

	private float velocity;
	private float degreesPerSymbol;
	private Vector3 rotation;

	private bool moving = false;
	private bool blur = false;

	private polygonStart startScript;
	private int symbol;

	public bool blurred{
		get{
			return blur;
		}
		set{

			float offset;

			if (value) {
				offset = 0.5f;
			} else {
				offset = 0;	
			}

			Renderer[] r = cylinder.GetComponentsInChildren<Renderer> ();

			for(int i=0;i<r.Length;i++)
				r[i].material.mainTextureOffset = new Vector2(offset,r[i].material.mainTextureOffset.y);

			blur = value;
		}
	}

	// Use this for initialization
	void Start () {
		spotLight = GetComponentInChildren<Light> ();
		cylinder = gameObject.transform.GetChild (0).gameObject;

		degreesPerSymbol = 360f / numOfSymbols;

		rotation = cylinder.transform.eulerAngles;
	}

	// Update is called once per frame
	void Update () {

	}

	public void setStartScript(polygonStart s){
		startScript = s;
	}

	void OnGUI(){
		if(moving)
			GUI.Label(new Rect(10, 10, 100, 20), ""+velocity);
	}

	IEnumerator coroutine_fire(float startSpeed){

		if (!moving && !float.IsNaN(startSpeed) && !float.IsInfinity(startSpeed) && startSpeed > 0) {

			moving = true;
			startScript.unlitAll ();

			velocity = startSpeed;

			if (velocity >= 3.0f)
				blurred = true;

			while (velocity > 1.0f) {

				rotation.x -= 90 * velocity * Time.deltaTime;
				if (rotation.x < 0)
					rotation.x += 360;

				cylinder.transform.eulerAngles = rotation;
				//cylinder.transform.Rotate (-90 * velocity * Time.deltaTime,0,0,Space.World);
				velocity -= startSpeed * damping * Time.deltaTime;

				if (velocity < 3.0f && blurred) {
					blurred = false;
				}

				yield return null;
			}

			//clamp to nearest
			blurred = false;


			float elapsedTime = 0;
			float time = 0.1f;

			Quaternion startingRot = cylinder.transform.rotation;

			//figures appear every 60 degrees;
			int s = (int)((rotation.x - angleOffset ) / degreesPerSymbol);

			//target orientation
			rotation.x = s * degreesPerSymbol;
			Quaternion targetRot = Quaternion.Euler (rotation);

			while (elapsedTime < time) {
				cylinder.transform.rotation = Quaternion.Slerp(startingRot, targetRot, (elapsedTime / time));
				elapsedTime += Time.deltaTime;
				yield return null;
			}

			cylinder.transform.eulerAngles = rotation;

			moving = false;
			startScript.checkRow ();
		}
	}

	public void lit(bool state){


		Renderer[] r = cylinder.GetComponentsInChildren<Renderer> ();
		Color c;

		if (state == true) {
			spotLight.color = new Color (0, 1, 0);
			c = new Color(0.02f,0.18f,0.04f);
		} else {
			spotLight.color = new Color (1, 1, 1);
			c = new Color(0,0,0);
		}

		for(int i=0;i<r.Length;i++)
			r[i].material.SetColor("_EmissionColor", c);
	}

	public void setSymbolValue(int s){
		symbol = s;
	}

	public int getSymbolValue(){

		return symbol;
	}

	public bool isRunning ()
	{
		return moving;
	}
}