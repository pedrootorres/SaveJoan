using UnityEngine;
using System.Collections;

public class TraceGenerator : MonoBehaviour {

	public GameObject bulletTracePrefab;
	public float rate = 8.0f;
	public Vector3 velocity;
	public bool on = false;

	private float nextbulletTraceTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (on){
			if(Time.time > nextbulletTraceTime){
				rate = Mathf.Max(rate, 1.0f);
				nextbulletTraceTime = Time.time + (1.0f / rate);

				GameObject newBulletTrace = Instantiate(bulletTracePrefab,transform.position,transform.rotation) as GameObject;
			}
		}
	}
}
