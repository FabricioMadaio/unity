using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class fighterController : MonoBehaviour {

	public float startPoints = 30;
	public float lifePoints = 30;
	public float duration = 1;

	public float moveSpeed = 1;
	public float lookAtSpeed = 1;

	public float minDistance = 0.8f;

	public bool isRagdoll = false;

	public GameObject lookAt;
	public GameObject lifeProgress;

	private Animator animator;
	private int punchHash;
	private int damageHash;
	private int forwardHash;
	private int backwardHash;

	private bool turnEnabled;
	private float distance;

	private fighterController enemyController;
	private GameObject spine;

	private float recoverTimer;

	// Use this for initialization
	void Start () {

		turnEnabled = true;
		animator = GetComponent<Animator> ();
		punchHash = Animator.StringToHash("punch");
		damageHash = Animator.StringToHash ("Damage");

		forwardHash = Animator.StringToHash ("Forward");
		backwardHash = Animator.StringToHash ("Backward");

		spine = transform.Find ("CG/Pelvis/Spine").gameObject;

		lifePoints = startPoints;

		enemyController = lookAt.GetComponent<fighterController>();

		updateBar ();
	}
		
	public void punch(){
		distance = Vector3.Distance (lookAt.transform.position,transform.position);
		animator.SetTrigger (punchHash);
	}
		
	public void move(Vector3 deltaPos){

		if (animator.GetCurrentAnimatorStateInfo (0).IsTag ("motionLocked"))
			return;

		//turn around enemy
		if (turnEnabled && Mathf.Abs(deltaPos.y)>0.4f) {
			turnEnabled = false;
			StartCoroutine ("turnAround",deltaPos);
			StartCoroutine ("turnEnabledCooldown");
		}

		//forward - backward movement
		Vector3 dt = -moveSpeed*Vector3.forward * deltaPos.x*Time.deltaTime;
		transform.Translate (dt);

		if (deltaPos.x > 0.1f)
			animator.SetBool (backwardHash,true);
		else
			animator.SetBool (backwardHash,false);

		if (deltaPos.x < -0.1f)
			animator.SetBool (forwardHash,true); 
		else
			animator.SetBool (forwardHash,false);

		//questo controllo sulla posizione serve a evitare che un giocatore sbatta contro l'altro (collider fittizio)
		distance = Vector3.Distance (enemyController.getPosition(),transform.position+dt);
		Vector3 dir =  - transform.position + enemyController.getPosition ();

		if(distance<minDistance){
			transform.position = enemyController.getPosition() - dir.normalized * minDistance;
		}
	}


	// Update is called once per frame
	void Update () {

		if (!isRagdoll) {

			if (lifePoints > 0) {
				recoverTimer += Time.deltaTime;
				if (recoverTimer > 1) {
					lifePoints += 5;
					recoverTimer -= 1;

					if (lifePoints > startPoints)
						lifePoints = startPoints;
					else
						updateBar ();
				}
			}
		}

		//look at rotation
		if (!animator.GetCurrentAnimatorStateInfo (0).IsTag ("motionLocked") && !isRagdoll) {
			Vector3 pos = enemyController.getPosition() - transform.position;
			Quaternion newRot = Quaternion.LookRotation (pos);
			transform.rotation = Quaternion.Lerp (transform.rotation, newRot, lookAtSpeed * Time.deltaTime);
		}
	}

	public void updateBar(){

		Image life = lifeProgress.transform.GetChild (1).GetComponent<Image>();
		float value = lifePoints / ((float)startPoints);
		life.fillAmount = value;
		lifeProgress.GetComponentInChildren<Text>().text = " "+Mathf.CeilToInt(value*100)+"%";
	}

	IEnumerator damageRoutine(){

		if (lifePoints > 0) {

			lifePoints-=10;
			updateBar ();

			if (lifePoints <= 0) {
					animator.SetBool ("Death", true);
			} else {
							
				animator.SetTrigger (damageHash);

				float timer = 0;
				while (timer < duration) {
					timer += Time.deltaTime;
					yield return null;
				}
			}
		}
	}

	public void punchTrigger(Collider col,GameObject caller){
		if (col.transform.root.name != gameObject.transform.name && !col.CompareTag("fisto")) {
			fighterController enemy = col.transform.root.GetComponent<fighterController> ();
			if (enemy != null) {
				enemy.StopCoroutine ("damageRoutine");
				enemy.StartCoroutine ("damageRoutine");
			}
		}
	}

	//abilita disabilità la ragdoll
	public void setRagdoll(bool enabled){

		isRagdoll = enabled;
		animator.enabled = !enabled;

		Rigidbody[] parts = GetComponentsInChildren<Rigidbody> ();
		for (int i = 0; i < parts.Length; i++) {
			parts [i].isKinematic = !enabled;
		}

		Collider[] cparts = GetComponentsInChildren<Collider> ();
		for (int i = 0; i < cparts.Length; i++) {
			cparts [i].isTrigger = !enabled;
		}
	}

	public void setGravity(bool enabled){
		Rigidbody[] parts = GetComponentsInChildren<Rigidbody> ();
		for (int i = 0; i < parts.Length; i++) {
			parts [i].useGravity = enabled;
		}
	}

	public Vector3 getPosition(){
		if (isRagdoll) {
			Vector3 pos = spine.transform.position;
			pos.y = transform.position.y;
			return pos;
		}else
			return transform.position;
	}

	public GameObject getSpine(){
		return spine;
	}

	public fighterController getEnemy(){
		return enemyController;
	}

	public bool isPunching(){
		return animator.GetCurrentAnimatorStateInfo (0).IsName ("Punch");
	}

	public bool motionLocked(){
		return animator.GetCurrentAnimatorStateInfo (0).IsTag ("motionLocked");
	}

	public bool isTurnEnabled(){
		return turnEnabled;
	}
}
