using UnityEngine;
using System.Collections;

public class Barrel : MonoBehaviour {

	public int health;
	public GameObject explosion;

	private GameObject explosionInstance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GetHit(int damage) {
		if (health > 0) {
			health -= damage;
			
			if(health <= 0) {
				explosionInstance = Instantiate(explosion, this.transform.position, Quaternion.identity) as GameObject;
				this.transform.FindChild("Barrel").renderer.enabled = false;

				this.transform.FindChild("Explosion").audio.Play();

				Vector3 rayPosition = explosionInstance.transform.position;
				
				Collider[] hits = Physics.OverlapSphere(rayPosition, 10);
				
				foreach(Collider hit in hits) {
					if(hit.collider.gameObject.name == "Spider(Clone)") {
						hit.collider.GetComponent<Spider>().GetHit(100);
					} else if(hit.collider.gameObject.name == "Troll(Clone)") {
						hit.collider.GetComponent<Troll>().GetHit(100);
					} else if (hit.collider.gameObject.name == "Robot(Clone)") {
						hit.collider.GetComponent<Robot>().GetHit(100);
					} else if (hit.collider.gameObject.name == "Demon(Clone)") {
						hit.collider.GetComponent<Demon>().GetHit(100);
					} else if (hit.collider.gameObject.name == "Alien(Clone)") {
						hit.collider.GetComponent<Alien>().GetHit(100);
					}
				}
				
				StartCoroutine(WaitAndDisappear());
			}
		}

	}

	IEnumerator WaitAndDisappear() {
		yield return new WaitForSeconds(3);

		Destroy (explosionInstance);
		Destroy (gameObject);
	}
}
