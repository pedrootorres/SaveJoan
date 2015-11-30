using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

	private bool goingUp = false;
	private bool goingDown = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(goingUp) {
			transform.Translate(Vector3.up * 2 * Time.deltaTime);

			if(transform.position.y > 10) {
				goingUp = false;
			}
		}

		if(goingDown) {
			transform.Translate(Vector3.down * 2 * Time.deltaTime);

			if(transform.position.y < 0) {
				goingDown = false;
			}
		}

		if(!goingUp && !goingDown && transform.position.y > 10) {
			StartCoroutine(Wait());
		}
	}

	void OnTriggerStay(Collider col) {
		if(Input.GetKeyDown(KeyCode.E)) {
			if(!goingUp && !goingDown) {
				if(transform.position.y > 10) {
					goingDown = true;
				} else {
					goingUp = true;
				}
			}
		}
	}

	IEnumerator Wait() {
		yield return new WaitForSeconds (5);
		goingDown = true;
	}
}
