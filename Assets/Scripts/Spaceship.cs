using UnityEngine;
using System.Collections;

public class Spaceship : MonoBehaviour {

	public int health;
	public int maxHealth;
	public GameObject[] attackPoints;
	private GameObject player;

	// Use this for initialization
	void Start () {
		this.transform.FindChild ("Explosion").gameObject.SetActive (false);
		player = GameObject.Find ("Player(Clone)");

		for(int i = 0; i < attackPoints.Length; i++) {
			Instantiate(attackPoints[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void GetHit(int damage) {
		if(health > 0) {
			this.health -= damage;

			player.transform.FindChild("smoothWorldPosition").FindChild("soldierCamera").FindChild("Right").gameObject.GetComponent<ParticleSystem>().enableEmission = true;
			player.transform.FindChild("smoothWorldPosition").FindChild("soldierCamera").FindChild("Left").gameObject.GetComponent<ParticleSystem>().enableEmission = true;
			player.transform.FindChild("smoothWorldPosition").FindChild("soldierCamera").FindChild("Up").gameObject.GetComponent<ParticleSystem>().enableEmission = true;
			player.transform.FindChild("smoothWorldPosition").FindChild("soldierCamera").FindChild("Down").gameObject.GetComponent<ParticleSystem>().enableEmission = true;

			StartCoroutine(BlinkRed());

			float percentage = (health*1f)/(maxHealth*1f);
 			GameObject.Find("Game Manager").GetComponent<GameManager>().SetSpaceshipLifeBar(percentage);

			if(health <= 0) {
				// activate game over
				this.transform.FindChild ("Explosion").gameObject.SetActive (true);
				GameObject.Find("Game Manager").gameObject.GetComponent<GameManager>().GameOver();
			}
		}
	}

	IEnumerator BlinkRed() {
		yield return new WaitForSeconds (0.1f);
		if (player != null) {
			player.transform.FindChild("smoothWorldPosition").FindChild("soldierCamera").FindChild("Right").gameObject.GetComponent<ParticleSystem>().enableEmission = false;
			player.transform.FindChild("smoothWorldPosition").FindChild("soldierCamera").FindChild("Left").gameObject.GetComponent<ParticleSystem>().enableEmission = false;
			player.transform.FindChild("smoothWorldPosition").FindChild("soldierCamera").FindChild("Up").gameObject.GetComponent<ParticleSystem>().enableEmission = false;
			player.transform.FindChild("smoothWorldPosition").FindChild("soldierCamera").FindChild("Down").gameObject.GetComponent<ParticleSystem>().enableEmission = false;
		}
	}
}
