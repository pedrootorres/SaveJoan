using UnityEngine;
using System.Collections;

public class Troll : MonoBehaviour {

	public Vector3 castle;
	private GameObject spaceship;
	
	public bool notThere = true;
	public int health;
	public int damage;
	public int value;

	NavMeshAgent navMesh;

	bool onPause;

	// Use this for initialization
	void Update () {

	}
	
	IEnumerator Start () {
		onPause = false;
		spaceship = GameObject.Find ("SpaceShip(Clone)");

		navMesh = GetComponent<NavMeshAgent> ();

		GameObject[] attackPoints = spaceship.GetComponent<Spaceship> ().attackPoints;
		int random = Random.Range (0, attackPoints.Length);
		navMesh.SetDestination (attackPoints[random].transform.position);
		
		while (notThere) {
			yield return StartCoroutine(GoThere());
		}
		
		StartCoroutine (Attack ());
	}

	IEnumerator GoThere() {
		this.animation.wrapMode = WrapMode.Loop;
		this.animation.Play ("Walk");
		
		if(transform.position == navMesh.destination) {
			notThere = false;
		}
		
		yield return null;
	}
	
	IEnumerator MoveObject (Transform thisTransform, Vector3 startPos, Vector3 endPos, float time) {
		float i = 0.0f;
		float rate = 1.0f / time;
		
		this.animation.wrapMode = WrapMode.Loop;
		this.animation.Play ("Walk");
		
		while (i < 1.0f && health > 0) {
			this.transform.LookAt(castle);
			
			i += Time.deltaTime * rate;
			
			thisTransform.position = Vector3.Lerp(startPos, endPos, i);
			yield return null; 
		}
		
		notThere = false;
		StartCoroutine (Attack ());
	}
	
	IEnumerator Attack() {
		while (health > 0 && !onPause) {
			this.animation.wrapMode = WrapMode.Once;
			this.gameObject.animation.CrossFade ("Attack_01");
			transform.LookAt(spaceship.transform.position);
			
			spaceship.GetComponent<Spaceship> ().GetHit (this.damage);
			yield return new WaitForSeconds (2);
		}
	}
	
	IEnumerator WaitAndDisappear() {
		navMesh.Stop ();
		notThere = false;
		this.animation.Stop ();

		this.animation.wrapMode = WrapMode.ClampForever;
		this.gameObject.animation.CrossFade("Die");
		yield return new WaitForSeconds(2);
		StopAllCoroutines ();
		Destroy (gameObject);
	}
	
	public void GetHit (int demage) {
		if(health > 0) {
			this.health -= demage;
			
			if(health <= 0) {
				this.GetComponent<BoxCollider>().enabled = false;
				StartCoroutine(WaitAndDisappear());
				
				float distance = Vector3.Distance(castle, this.transform.position);
				
				int coins = Mathf.FloorToInt(distance/2 + value);
				
				GameObject.Find("Player(Clone)").GetComponent<Player>().coins += coins;
			}
		}
	}

	public void PauseGame() {
		if(notThere) {
			navMesh.Stop();
		} else {
			StopCoroutine("Attack");
		}
		
		onPause = true;
	}
	
	public void ResumeGame() {
		if(notThere) {
			onPause = false;
			navMesh.Resume();
		} else {
			onPause = false;
			StartCoroutine(Attack());
		}
	}
}
