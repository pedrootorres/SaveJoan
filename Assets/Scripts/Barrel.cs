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
				this.transform.FindChild("Barrel").GetComponent<Renderer>().enabled = false;

				this.transform.FindChild("Explosion").GetComponent<AudioSource>().Play();

				Vector3 rayPosition = explosionInstance.transform.position;
				
				Collider[] hits = Physics.OverlapSphere(rayPosition, 10);
				
				foreach(Collider hit in hits) {
					if(hit.GetComponent<Collider>().gameObject.name == "Spider(Clone)") {
						hit.GetComponent<Collider>().GetComponent<Spider>().GetHit(100);
					} else if(hit.GetComponent<Collider>().gameObject.name == "Troll(Clone)") {
						hit.GetComponent<Collider>().GetComponent<Troll>().GetHit(100);
					} else if (hit.GetComponent<Collider>().gameObject.name == "Robot(Clone)") {
						hit.GetComponent<Collider>().GetComponent<Robot>().GetHit(100);
					} else if (hit.GetComponent<Collider>().gameObject.name == "Demon(Clone)") {
						hit.GetComponent<Collider>().GetComponent<Demon>().GetHit(100);
					} else if (hit.GetComponent<Collider>().gameObject.name == "Alien(Clone)") {
						hit.GetComponent<Collider>().GetComponent<Alien>().GetHit(100);
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
