using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter() {
		GameObject.Find ("Player(Clone)").GetComponent<Player> ().qtyGrenades += 5;
		GameObject.Find("Game Manager").GetComponent<GameManager>().SetGrenadesQty(5);

		Destroy (gameObject);
	}
}
