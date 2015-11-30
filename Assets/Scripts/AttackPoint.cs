using UnityEngine;
using System.Collections;

public class AttackPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col) {
		if(col.name == "Alien(Clone)") {
			col.GetComponent<Alien>().notThere = false;
		} else if(col.name == "Demon(Clone)") {
			col.GetComponent<Demon>().notThere = false;
		} else if(col.name == "Robot(Clone)") {
			col.GetComponent<Robot>().notThere = false;
		} else if(col.name == "Spider(Clone)") {
			col.GetComponent<Spider>().notThere = false;
		} else if(col.name == "Troll(Clone)") {
			col.GetComponent<Troll>().notThere = false;
		}
	}
}
