using UnityEngine;
using System.Collections;

public class Alien : MonoBehaviour {

	public Vector3 castle;
	private GameObject spaceship;

	public bool notThere = true;
	public int health;
	public int damage;
	public int value;

	Animation anim;
	NavMeshAgent navMesh;

	bool onPause;
	
	// Use this for initialization
	void Update () {

	}
	
	IEnumerator Start () {
		onPause = false;
		anim = GetComponent ("Animation") as Animation;
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
		anim.wrapMode = WrapMode.Loop;
		anim.Play ("flight");
		
		if(transform.position == navMesh.destination) {
			notThere = false;
		}
		
		yield return null;
	}
	
	IEnumerator Attack() {
		while (health > 0 && !onPause) {
			anim.CrossFade ("shot");
			transform.LookAt(spaceship.transform.position);
			
			spaceship.GetComponent<Spaceship> ().GetHit (this.damage);
			yield return new WaitForSeconds (2);
		}
	}
	
	IEnumerator WaitAndDisappear() {
		navMesh.Stop ();
		notThere = false;
		this.animation.Stop ();

		anim.wrapMode = WrapMode.ClampForever;
		anim ["dead"].speed = 3.0f;
		anim.CrossFade ("dead");
		yield return new WaitForSeconds(1.5f);
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
			anim.Stop();
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
