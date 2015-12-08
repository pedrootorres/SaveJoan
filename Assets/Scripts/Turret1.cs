using UnityEngine;
using System.Collections;

public class Turret1 : MonoBehaviour {

	// FOR AI
	public string[] targetRootName;
	public float range;
	private Transform targetTransform;
	float lastTargetfoundTime;
	GameObject[] targets;
	bool foundTarget;

	Animation anim;

	Transform flashGenerator;
	Transform flashGenerator2;
	Transform traceGenerator;
	Transform traceGenerator2;

	public int damage;
	public int value;

	bool onPause;
	bool alreadyShooting;

	// Use this for initialization
	void Start () {
		foundTarget = false;
		alreadyShooting = false;

		flashGenerator = this.transform.FindChild ("Head").FindChild ("Cannon_1").FindChild ("flashGenerator");
		flashGenerator2 = this.transform.FindChild ("Head").FindChild ("Cannon_2").FindChild ("flashGenerator");
		traceGenerator = this.transform.FindChild ("Head").FindChild ("Cannon_2").FindChild ("traceGenerator");
		traceGenerator2 = this.transform.FindChild ("Head").FindChild ("Cannon_2").FindChild ("traceGenerator");
	}
	
	// Update is called once per frame
	void Update () {
		turretAI ();
	}

	void turretAI() {
		Vector3 rayPosition = this.transform.position - new Vector3(0, this.transform.position.y, 0);

		Collider[] hits = Physics.OverlapSphere(rayPosition, range);

		bool isTarget = false;
		bool stop = false;
		int shootMe = 0;
		
		//Check if what's ahead correspond to the target list.
		for (int j = 0; j < hits.Length && !stop; j++) {
			for (var i = 0; i < targetRootName.Length && !stop; i++){
				if (hits[j].GetComponent<Collider>().transform.root.name == targetRootName[i]){
					isTarget = true;
					stop = true;
					shootMe = j;
				}
			}
		}
		
		if (isTarget) {
			foundTarget = true; //If what's ahead is a target.
			
			if(targetTransform == null){
				targetTransform = hits[shootMe].transform; //This is the target.
			}
		} else {
			targets = GetTargets();
			foundTarget = false;
		}


		//If target has been destroyed or got away.
		if(targetTransform == null){
			foundTarget = false;
		}

		// Look at the target
		if(foundTarget){
			this.transform.FindChild("Head").LookAt(targetTransform);
			if(!alreadyShooting) {
				StartCoroutine( Shoot(hits[shootMe]));
			}
		} else {
			this.transform.FindChild("Head").LookAt(null);

			flashGenerator.GetComponent<FlashGenerator> ().on = false;
			flashGenerator2.GetComponent<FlashGenerator> ().on = false;
			traceGenerator.GetComponent<TraceGenerator> ().on = false;
			traceGenerator2.GetComponent<TraceGenerator> ().on = false;
		}
	}

	GameObject[] GetTargets(){
		GameObject[] returnList = new GameObject[targetRootName.Length];

		for (var i = 0; i < targetRootName.Length; i++){
			returnList[i] = GameObject.Find(targetRootName[i]);
		}

		return returnList;
	}

	IEnumerator Shoot(Collider hit) {
		if(!onPause) {
			alreadyShooting = true;

			this.transform.FindChild("Head").LookAt(hit.transform.position);
			flashGenerator.GetComponent<FlashGenerator> ().on = true;
			flashGenerator2.GetComponent<FlashGenerator> ().on = true;

			if(hit.GetComponent<Collider>().gameObject.name == "Spider(Clone)") {
				hit.GetComponent<Collider>().GetComponent<Spider>().GetHit(damage);
			} else if(hit.GetComponent<Collider>().gameObject.name == "Troll(Clone)") {
				hit.GetComponent<Collider>().GetComponent<Troll>().GetHit(damage);
			} else if (hit.GetComponent<Collider>().gameObject.name == "Robot(Clone)") {
				hit.GetComponent<Collider>().GetComponent<Robot>().GetHit(damage);
			} else if (hit.GetComponent<Collider>().gameObject.name == "Demon(Clone)") {
				hit.GetComponent<Collider>().GetComponent<Demon>().GetHit(damage);
			} else if (hit.GetComponent<Collider>().gameObject.name == "Alien(Clone)") {
				hit.GetComponent<Collider>().GetComponent<Alien>().GetHit(damage);
			}
			
			yield return new WaitForSeconds(1f);
			alreadyShooting = false;
		} else {
			flashGenerator.GetComponent<FlashGenerator> ().on = false;
			flashGenerator2.GetComponent<FlashGenerator> ().on = false;
			traceGenerator.GetComponent<TraceGenerator> ().on = false;
			traceGenerator2.GetComponent<TraceGenerator> ().on = false;
		}
	}

	public void PauseGame() {
		this.onPause = true;
	}

	public void ResumeGame() {
		this.onPause = false;
	}
}
