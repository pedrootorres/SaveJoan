using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int damage;
	public int coins;
	public int qtyGrenades;

	public GameObject grenade;
	private GameObject grenadeInstance;

	public GameObject explosion;

	public GameObject gunSound;
	private GameObject gunSoundInstance;
	
	public GameObject getToTheChopper;
	private GameObject getToTheChopperInstance;

	// Use this for initialization
	void Start () {
		GameObject.Find("Game Manager").GetComponent<GameManager>().SetGrenadesQty(qtyGrenades);
		gunSoundInstance = Instantiate (gunSound) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F) && qtyGrenades > 0) {
			grenadeInstance = Instantiate(grenade, this.gameObject.transform.position + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
			grenadeInstance.rigidbody.AddForce(GameObject.Find("soldierCamera").transform.forward * 1000);

			qtyGrenades--;
			GameObject.Find("Game Manager").GetComponent<GameManager>().SetGrenadesQty(qtyGrenades);

			StartCoroutine(GrenadeExplosion(grenadeInstance));
		}

		if(Input.GetMouseButtonDown(0)) {
			gunSoundInstance.audio.time = 0.13f;
			gunSoundInstance.audio.Play();
			gunSoundInstance.audio.loop = true;
		}

		if(gunSoundInstance.audio.time > 0.240f) {
			gunSoundInstance.audio.Stop();
			gunSoundInstance.audio.time = 0.1f;
			gunSoundInstance.audio.Play();
		}

		if(Input.GetMouseButtonUp(0)) {
			gunSoundInstance.audio.Stop();
		}
	}

	IEnumerator GrenadeExplosion(GameObject thisGrenade) {
		yield return new WaitForSeconds (2);

		GameObject explosionInstance = Instantiate (explosion, thisGrenade.transform.position, Quaternion.identity) as GameObject;
		thisGrenade.renderer.enabled = false;
		thisGrenade.transform.FindChild ("Explosion").audio.Play ();

		Vector3 rayPosition = explosionInstance.transform.position;
		
		Collider[] hits = Physics.OverlapSphere(rayPosition, 5);

		foreach(Collider hit in hits) {
			if(hit.collider.gameObject.name == "Spider(Clone)") {
				hit.collider.GetComponent<Spider>().GetHit(20);
			} else if(hit.collider.gameObject.name == "Troll(Clone)") {
				hit.collider.GetComponent<Troll>().GetHit(20);
			} else if (hit.collider.gameObject.name == "Robot(Clone)") {
				hit.collider.GetComponent<Robot>().GetHit(20);
			} else if (hit.collider.gameObject.name == "Demon(Clone)") {
				hit.collider.GetComponent<Demon>().GetHit(20);
			} else if (hit.collider.gameObject.name == "Alien(Clone)") {
				hit.collider.GetComponent<Alien>().GetHit(20);
			} else if(hit.collider.gameObject.name == "Barrel") {
				hit.collider.GetComponentInParent<Barrel>().GetHit(20);
			}
		}

		yield return new WaitForSeconds (2);

		Destroy (thisGrenade);
		Destroy (explosionInstance);
	}

	public void PlayChopperSound() {
		getToTheChopperInstance = Instantiate (getToTheChopper) as GameObject;
	}

	public void GameOver() {
		gunSoundInstance.audio.Stop ();
		Destroy (gunSoundInstance);
	}
}
