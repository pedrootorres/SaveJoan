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
			grenadeInstance.GetComponent<Rigidbody>().AddForce(GameObject.Find("soldierCamera").transform.forward * 1000);

			qtyGrenades--;
			GameObject.Find("Game Manager").GetComponent<GameManager>().SetGrenadesQty(qtyGrenades);

			StartCoroutine(GrenadeExplosion(grenadeInstance));
		}

		if(Input.GetMouseButtonDown(0)) {
			gunSoundInstance.GetComponent<AudioSource>().time = 0.13f;
			gunSoundInstance.GetComponent<AudioSource>().Play();
			gunSoundInstance.GetComponent<AudioSource>().loop = true;
		}

		if(gunSoundInstance.GetComponent<AudioSource>().time > 0.240f) {
			gunSoundInstance.GetComponent<AudioSource>().Stop();
			gunSoundInstance.GetComponent<AudioSource>().time = 0.1f;
			gunSoundInstance.GetComponent<AudioSource>().Play();
		}

		if(Input.GetMouseButtonUp(0)) {
			gunSoundInstance.GetComponent<AudioSource>().Stop();
		}
	}

	IEnumerator GrenadeExplosion(GameObject thisGrenade) {
		yield return new WaitForSeconds (2);

		GameObject explosionInstance = Instantiate (explosion, thisGrenade.transform.position, Quaternion.identity) as GameObject;
		thisGrenade.GetComponent<Renderer>().enabled = false;
		thisGrenade.transform.FindChild ("Explosion").GetComponent<AudioSource>().Play ();

		Vector3 rayPosition = explosionInstance.transform.position;
		
		Collider[] hits = Physics.OverlapSphere(rayPosition, 5);

		foreach(Collider hit in hits) {
			if(hit.GetComponent<Collider>().gameObject.name == "Spider(Clone)") {
				hit.GetComponent<Collider>().GetComponent<Spider>().GetHit(20);
			} else if(hit.GetComponent<Collider>().gameObject.name == "Troll(Clone)") {
				hit.GetComponent<Collider>().GetComponent<Troll>().GetHit(20);
			} else if (hit.GetComponent<Collider>().gameObject.name == "Robot(Clone)") {
				hit.GetComponent<Collider>().GetComponent<Robot>().GetHit(20);
			} else if (hit.GetComponent<Collider>().gameObject.name == "Demon(Clone)") {
				hit.GetComponent<Collider>().GetComponent<Demon>().GetHit(20);
			} else if (hit.GetComponent<Collider>().gameObject.name == "Alien(Clone)") {
				hit.GetComponent<Collider>().GetComponent<Alien>().GetHit(20);
			} else if(hit.GetComponent<Collider>().gameObject.name == "Barrel") {
				hit.GetComponent<Collider>().GetComponentInParent<Barrel>().GetHit(20);
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
		gunSoundInstance.GetComponent<AudioSource>().Stop ();
		Destroy (gunSoundInstance);
	}
}
