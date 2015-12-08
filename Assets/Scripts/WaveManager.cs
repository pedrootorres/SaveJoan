using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {

	public GameObject alien;
	public GameObject demon;
	public GameObject robot;
	public GameObject spider;
	public GameObject troll;
	private GameObject alienInstance;
	private GameObject demonInstance;
	private GameObject robotInstance;
	private GameObject spiderInstance;
	private GameObject trollInstance;

	public GameObject wave1;
	public GameObject wave2;
	public GameObject wave3;
	public GameObject wave4;
	public GameObject wave5;
	private GameObject wave1Instance;
	private GameObject wave2Instance;
	private GameObject wave3Instance;
	private GameObject wave4Instance;
	private GameObject wave5Instance;

	private GameObject currentMusic;
	
	int totalEnemies = 0;
	int enemiesToProduce = 0;

	bool onPause;

	// Use this for initialization
	void Start () {
		onPause = false;

		demon.GetComponent<Demon> ().damage = 100;
		troll.GetComponent<Troll> ().damage = 30;
		alien.GetComponent<Alien> ().damage = 15;
		robot.GetComponent<Robot> ().damage = 5;
		spider.GetComponent<Spider> ().damage = 1;

		demon.GetComponent<Demon> ().health = 100;
		troll.GetComponent<Troll> ().health = 30;
		alien.GetComponent<Alien> ().health = 5;
		robot.GetComponent<Robot> ().health = 3;
		spider.GetComponent<Spider> ().health = 1;

		demon.GetComponent<Demon> ().value = 180;
		troll.GetComponent<Troll> ().value = 60;
		alien.GetComponent<Alien> ().value = 10;
		robot.GetComponent<Robot> ().value = 5;
		spider.GetComponent<Spider> ().value = 1;

		StartCoroutine (Next5Waves (1));
	}
	
	// Update is called once per frame
	void Update () {
		totalEnemies = GameObject.FindGameObjectsWithTag ("Enemy").Length;
	}

	IEnumerator Next5Waves(int waveNumber) {
		// wave 1, 6, 11, 16...
//		StartCoroutine(StartWave (0, 0, 0, 0, 50));
		ChooseEnemiesQty (waveNumber);
		wave1Instance = Instantiate (wave1) as GameObject;
		currentMusic = wave1Instance;
		StartCoroutine(GameManager.DisplayWaveNumber (waveNumber));
		while (totalEnemies != 0 || enemiesToProduce != 0) {
			yield return new WaitForSeconds(1);
		}
		// break between waves
		wave1Instance.GetComponent<AudioSource>().Stop ();
		Destroy (wave1Instance);
		yield return new WaitForSeconds (6);

		// wave 2, 7, 12, 17...
//		StartCoroutine(StartWave (3, 0, 0, 10, 1));
		ChooseEnemiesQty (++waveNumber);
		wave2Instance = Instantiate (wave2) as GameObject;
		currentMusic = wave2Instance;
		StartCoroutine(GameManager.DisplayWaveNumber (waveNumber));
		while (totalEnemies != 0 || enemiesToProduce != 0) {
			yield return new WaitForSeconds(1);
		}
		// break between waves
		wave2Instance.GetComponent<AudioSource>().Stop ();
		Destroy (wave2Instance);
		yield return new WaitForSeconds (6);

		// wave 3, 8, 13, 18...
//		StartCoroutine(StartWave (5, 0, 4, 6, 3));
		ChooseEnemiesQty (++waveNumber);
		wave3Instance = Instantiate (wave3) as GameObject;
		currentMusic = wave3Instance;
		StartCoroutine(GameManager.DisplayWaveNumber (waveNumber));
		while (totalEnemies != 0 || enemiesToProduce != 0) {
			yield return new WaitForSeconds(1);
		}
		// break between waves
		wave3Instance.GetComponent<AudioSource>().Stop ();
		Destroy (wave3Instance);
		yield return new WaitForSeconds (6);

		// wave 4, 9, 14, 19...
//		StartCoroutine(StartWave (3, 0, 5, 5, 4));
		ChooseEnemiesQty (++waveNumber);
		wave4Instance = Instantiate (wave4) as GameObject;
		currentMusic = wave4Instance;
		StartCoroutine(GameManager.DisplayWaveNumber (waveNumber));
		while (totalEnemies != 0 || enemiesToProduce != 0) {
			yield return new WaitForSeconds(1);
		}
		wave4Instance.GetComponent<AudioSource>().Stop ();
		Destroy (wave4Instance);
		yield return new WaitForSeconds (6);

		// wave 5, 10, 15, 20...
//		StartCoroutine(StartWave (0, 1, 0, 0, 0));
		ChooseEnemiesQty (++waveNumber);
		wave5Instance = Instantiate (wave5) as GameObject;
		currentMusic = wave5Instance;
		StartCoroutine(GameManager.DisplayWaveNumber (waveNumber));
		while (totalEnemies != 0 || enemiesToProduce != 0) {
			yield return new WaitForSeconds(1);
		}
		// break between waves
		wave5Instance.GetComponent<AudioSource>().Stop ();
		Destroy (wave5Instance);
		yield return new WaitForSeconds (6);

		// start again until player dies
		StartCoroutine (Next5Waves (++waveNumber));
	}

	private void ChooseEnemiesQty(int waveNumber) {
		int waveType = waveNumber % 5;

		int numberOfDemons = 0, numberOfTrolls = 0, numberOfAliens = 0, numberOfRobots = 0, numberOfSpiders = 0;

		if (waveType == 1) {
			numberOfSpiders = (int) (50 + Mathf.Pow(waveNumber, 2)/100f * 50); //50, 68, 110
		} else if(waveType == 2) {
			numberOfSpiders = (int) ((3*waveNumber + 50) * 1.5f); // 84, 106
			numberOfRobots = (int) ((3*waveNumber + 20) * 1.3f); // 33, 53, 
			numberOfAliens = (int) ((3*waveNumber + 10) * 1.1f); // 17, 34
		} else if(waveType == 3) {
			numberOfSpiders = (int) ((3*waveNumber + 60) * 1.5f);
			numberOfRobots = (int) ((3*waveNumber + 25) * 1.3f);
			numberOfAliens = (int) ((3*waveNumber + 10) * 1.3f);
			if(waveNumber == 3) {
				numberOfTrolls = 1;
			} else {
				numberOfTrolls = waveNumber / 4;
			}
		} else if(waveType == 4) {
			numberOfSpiders = (int) ((3*waveNumber + 80) * 1.5f);
			numberOfRobots = (int) ((3*waveNumber + 30) * 1.4f);
			numberOfAliens = (int) ((3*waveNumber + 15) * 1.35f);
			if(waveNumber == 3) {
				numberOfTrolls = 2;
			} else {
				numberOfTrolls = waveNumber / 4 + 1;
			}
		} else {
			numberOfDemons = waveNumber / 5;
			numberOfTrolls = numberOfDemons + 1;
		}

		StartCoroutine (StartWave (numberOfDemons, numberOfTrolls, numberOfAliens, numberOfRobots, numberOfSpiders));
	}

	IEnumerator StartWave(int numberOfDemons, int numberOfTrolls, int numberOfAliens, int numberOfRobots, int numberOfSpiders) {
		enemiesToProduce = numberOfAliens + numberOfDemons + numberOfRobots + numberOfSpiders + numberOfTrolls;

		StartCoroutine (ProduceEnemies(alien, numberOfAliens, 4, 6));

		StartCoroutine (ProduceEnemies(demon, numberOfDemons, 0, 1));

		StartCoroutine (ProduceEnemies(robot, numberOfRobots, 2, 4));

		StartCoroutine (ProduceEnemies(spider, numberOfSpiders, 0, 2));

		StartCoroutine (ProduceEnemies(troll, numberOfTrolls, 6, 8));

		yield return null;
	}

	IEnumerator ProduceEnemies(GameObject enemyType, int qty, int minWait, int maxWait) {
		int x;
		int z;
		
		float wait;

		int zone;

		GameObject instance;

		for (int i = 0; i < qty; i++) {
			zone = Random.Range(0, 3);

			if(zone == 0) {
				x = Random.Range (2, 65);
				z = Random.Range (88, 98);
			} else if (zone == 1) {
				x = Random.Range (90, 98);
				z = Random.Range (90, 98);
			} else {
				x = Random.Range (90, 98);
				z = Random.Range (2, 10);
			}
		
			wait = Random.Range (minWait, maxWait);

			yield return new WaitForSeconds(wait);

			while(onPause) {
				yield return new WaitForSeconds(wait);
			}

			instance = Instantiate(enemyType, new Vector3(x, 0, z), Quaternion.identity) as GameObject;
			enemiesToProduce--;
		}
	}

	public void PauseGame() {
		if(currentMusic != null) {
			currentMusic.GetComponent<AudioSource>().Pause ();
		}
		onPause = true;
	}

	public void ResumeGame() {
		if(currentMusic != null) {
			currentMusic.GetComponent<AudioSource>().Play();
		}
		onPause = false;
	}

	public void GameOver() {
		if(currentMusic != null) {
			currentMusic.GetComponent<AudioSource>().Stop ();
			Destroy (currentMusic);
		}
	}
}
