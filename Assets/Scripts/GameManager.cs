using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public Terrain map;
	private Terrain mapInstance;

	public Camera menuCamera;
	private Camera menuCameraInstance;

	public GUIText gameTitle;
	private GUIText gameTitleInstance;
	public GUIText guiGrenadesQty;
	private GUIText guiGrenadesQtyInstance;
	public GUITexture guiGrenadesImage;
	private GUITexture guiGrenadesImageInstance;
	public GUITexture spaceshipImage;
	private GUITexture spaceshipImageInstance;
	public GUITexture spaceshipLife;
	private GUITexture spaceshipLifeInstance;
	public GUITexture spaceshipLifeBox;
	private GUITexture spaceshipLifeBoxInstance;

	public GameObject player;
	private GameObject playerInstance;

	public GameObject ammo;
	private GameObject ammoInstance;
	public GameObject chopper;
	private GameObject chopperInstance;
	private bool moveChopper;
	private bool ammoReady;

	public GUIText wavesNumber;
	static GUIText wavesNumberInstance;

	public GameObject justSoldier;
	private GameObject justSoldierInstance;

	public GameObject spaceship;
	private GameObject spaceshipInstance;

	public GameObject waveManager;
	private GameObject waveManagerInstance;

	private string startGameButton;

	private Camera mapCamera;

	public Texture turret1;
	public Texture turret3;
	public Texture turret4;
	public Texture turret6;
	public GameObject objectTurret1;
	public GameObject objectTurret3;
	public GameObject objectTurret4;
	public GameObject objectTurret6;
	
	public GUIStyle style;
	public GUIStyle styleCoins;

	bool gameHasStarted;
	bool onPause;
	bool menuIsOpen;
	bool alreadySpawning;

	// buy menu
	private Vector3 mouseCoord;
	private GameObject newTurret;
	private GameObject turretNotPurchased;
	private int turretValue;
	private List<GameObject> allTurrets;

	// Use this for initialization
	void Start () {
		allTurrets = new List<GameObject> ();
		menuCameraInstance = Instantiate (menuCamera) as Camera;
		menuCamera.camera.enabled = true;

		gameHasStarted = false;
		onPause = false;
		menuIsOpen = false;
		moveChopper = false;
		alreadySpawning = false;

		gameTitleInstance = Instantiate (gameTitle) as GUIText;
		gameTitleInstance.enabled = true;

		guiGrenadesQtyInstance = Instantiate (guiGrenadesQty) as GUIText;
		guiGrenadesQtyInstance.enabled = false;
		guiGrenadesImageInstance = Instantiate (guiGrenadesImage) as GUITexture;
		guiGrenadesImageInstance.enabled = false;
		spaceshipImageInstance = Instantiate (spaceshipImage) as GUITexture;
		spaceshipImageInstance.enabled = false;
		spaceshipLifeInstance = Instantiate (spaceshipLife) as GUITexture;
		spaceshipLifeInstance.enabled = false;
		spaceshipLifeBoxInstance = Instantiate (spaceshipLifeBox) as GUITexture;

		wavesNumberInstance = Instantiate (wavesNumber) as GUIText;
		wavesNumberInstance.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameHasStarted  && !alreadySpawning) {
			StartCoroutine (SpawnGrenades ());
		}

		if(moveChopper && !onPause && !menuIsOpen) {
			chopperInstance.transform.Translate(Vector3.forward * -25 * Time.deltaTime);
			chopperInstance.transform.FindChild("Rotor_Control").FindChild("Rotor").Rotate(new Vector3(0, 0, 800 * Time.deltaTime));

			if(chopperInstance.transform.position.z < 80 && !ammoReady) {
				ammoInstance = Instantiate(ammo) as GameObject;
				playerInstance.GetComponent<Player>().PlayChopperSound();
				ammoReady = true;
			} else if(chopperInstance.transform.position.z < -52) {
				Destroy(chopperInstance);
				moveChopper = false;
				ammoReady = false;
			}
		}

		if (Input.GetKeyDown ("escape")) {
			if(menuIsOpen) {
				this.Unpaused();
				
				mapCamera.camera.enabled = false;
				mapCamera.orthographic = false;
				Screen.lockCursor = true;
				Screen.showCursor = false;
				
				Destroy (justSoldierInstance);
				playerInstance.gameObject.SetActive(true);
				
				menuIsOpen = false;

				foreach (GameObject t in allTurrets) {
					t.transform.FindChild("Sphere").gameObject.SetActive(false);
				}
			} else if(gameHasStarted) {
				// pause game
				onPause = true;
				
				menuCamera.camera.enabled = true;
				playerInstance.gameObject.SetActive(false);
				
				Screen.lockCursor = false;
				Screen.showCursor = true;
				
				this.OnPause();
				
				//			Application.Quit();
			}
		}

		if(Input.GetKeyDown(KeyCode.B)) {
			if(!onPause) {
				if(menuIsOpen) {
					this.Unpaused();

					mapCamera.camera.enabled = false;
					mapCamera.orthographic = false;
					Screen.lockCursor = true;
					Screen.showCursor = false;

					Destroy (justSoldierInstance);
					playerInstance.gameObject.SetActive(true);

					menuIsOpen = false;

					foreach (GameObject t in allTurrets) {
						t.transform.FindChild("Sphere").gameObject.SetActive(false);
					}
				} else {
					this.OnPause();

					mapCamera.camera.enabled = true;
					mapCamera.orthographic = true;
					mapCamera.orthographicSize = 50;
					Screen.lockCursor = false;
					Screen.showCursor = true;

					justSoldierInstance = Instantiate(justSoldier, playerInstance.transform.position, Quaternion.identity) as GameObject;
					playerInstance.gameObject.SetActive(false);
					
					menuIsOpen = true;

					foreach (GameObject t in allTurrets) {
						if(t != null) {
							t.transform.FindChild("Sphere").gameObject.SetActive(true);

							if(t.gameObject.name == "Turret 1(Clone)") {
								t.transform.FindChild("Sphere").transform.localScale  = new Vector3(t.GetComponent<Turret1>().range, 0, t.GetComponent<Turret1>().range);
							} else if(t.gameObject.name == "Turret 3(Clone)") {
								t.transform.FindChild("Sphere").transform.localScale  = new Vector3(t.GetComponent<Turret3>().range, 0, t.GetComponent<Turret3>().range);
							} else if(t.gameObject.name == "Turret 4(Clone)") {
								t.transform.FindChild("Sphere").transform.localScale  = new Vector3(t.GetComponent<Turret4>().range, 0, t.GetComponent<Turret4>().range);
							} else if(t.gameObject.name == "Turret 6(Clone)") { 
								t.transform.FindChild("Sphere").transform.localScale  = new Vector3(t.GetComponent<Turret6>().range, 0, t.GetComponent<Turret6>().range);
							}
						}
					}
				}
			}
		}

		if(menuIsOpen) {
			mouseCoord = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
			Vector3 aux = mapCamera.ScreenToWorldPoint(mouseCoord);

			if(newTurret != null) {
				newTurret.transform.position = aux - new Vector3(0, aux.y, 0);
				newTurret.transform.position += new Vector3(0, Terrain.activeTerrain.SampleHeight(aux), 0);

				if(Input.GetMouseButtonDown(0) && aux.x >= 1 && aux.x <= 99 && aux.z >= 1 && aux.z <= 99) {
					Ray ray = mapCamera.ScreenPointToRay(Input.mousePosition);
					RaycastHit[] hitsMouse = Physics.RaycastAll(ray);

					bool thatsOk = true;
					int qtyTurret = 0;

					for(int i = 0; i < hitsMouse.Length; i++) {
						if(hitsMouse[i].collider.name != "Map(Clone)" && !hitsMouse[i].collider.name.Contains("Turret")) {
							thatsOk = false;
						}

						if(hitsMouse[i].collider.name.Contains("Turret")) {
							qtyTurret++;
						}
					}

					if(thatsOk && qtyTurret == 1) {
						playerInstance.GetComponent<Player>().coins -= turretValue;
						newTurret.GetComponent<NavMeshObstacle>().enabled = true;
						allTurrets.Add(newTurret);

						if(newTurret.name == "Turret 1(Clone)") {
							objectTurret1.GetComponent<Turret1>().value = (int) (objectTurret1.GetComponent<Turret1>().value * 2f);
						} else if(newTurret.name == "Turret 3(Clone)") {
							objectTurret3.GetComponent<Turret3>().value = (int) (objectTurret3.GetComponent<Turret3>().value * 2.3f);
						} else if(newTurret.name == "Turret 4(Clone)") {
							objectTurret4.GetComponent<Turret4>().value = (int) (objectTurret4.GetComponent<Turret4>().value * 2.7f);
						} else if(newTurret.name == "Turret 6(Clone)") { 
							objectTurret6.GetComponent<Turret6>().value = (int) (objectTurret6.GetComponent<Turret6>().value * 3);
						}

						newTurret = null;
						turretNotPurchased = null;
					}
				}
			}
		} else {
			Destroy(turretNotPurchased);
		}
	}

	void OnGUI() {
		// Beginning of the game
		if (!gameHasStarted) {
			if(GUI.Button(new Rect(Screen.width/2 - 50, Screen.height/2 - 17.5f, 100, 25), "Start Game")) {
				GameStart();
			}
		}

		// when paused
		if (onPause) {
			if(GUI.Button(new Rect(Screen.width/2 - 50, Screen.height/2 - 17.5f, 100, 25), "Resume Game")) {
				Time.timeScale = 1;
				playerInstance.gameObject.SetActive(true);
				menuCamera.camera.enabled = false;

				Screen.lockCursor = true;
				Screen.showCursor = false;

				this.Unpaused();

				onPause = false;
			}
		}

		// buy menu
		if (menuIsOpen) {
			// Turret 1
			GUI.BeginGroup(new Rect(0, 0, Screen.width/4.7f, Screen.height/2));
			bool buyTurret1 = GUI.Button(new Rect(0, 0, Screen.width/4.7f, Screen.height/2), new GUIContent(turret1));
			GUI.Box(new Rect(10, Screen.height/2.3f, Screen.width/4.7f/2, 20), "Turret 1", style);
			GUI.Box(new Rect(Screen.width/4.7f - 90, Screen.height/2.3f, Screen.width/4.7f/2, 20), "Range: " + objectTurret1.GetComponent<Turret1>().range, style);
			GUI.Box(new Rect(10, Screen.height/2.3f + 20, Screen.width/4.7f/2, 20), "Damage: " + objectTurret1.GetComponent<Turret1>().damage, style);
			GUI.Box(new Rect(Screen.width/4.7f - 90, Screen.height/2.3f + 20, Screen.width/4.7f/2, 20), "Price: " + objectTurret1.GetComponent<Turret1>().value, style);
			GUI.EndGroup();

			// Turret 3
			GUI.BeginGroup(new Rect(0, Screen.height/2, Screen.width/4.7f, Screen.height/2));
			bool buyTurret3 = GUI.Button(new Rect(0, 0, Screen.width/4.7f, Screen.height/2), new GUIContent(turret3));
			GUI.Box(new Rect(10, Screen.height/2.3f, Screen.width/4.7f/2, 20), "Turret 2", style);
			GUI.Box(new Rect(Screen.width/4.7f - 90, Screen.height/2.3f, Screen.width/4.7f/2, 20), "Range: " + objectTurret3.GetComponent<Turret3>().range, style);
			GUI.Box(new Rect(10, Screen.height/2.3f + 20, Screen.width/4.7f/2, 20), "Damage: " + objectTurret3.GetComponent<Turret3>().damage, style);
			GUI.Box(new Rect(Screen.width/4.7f - 90, Screen.height/2.3f + 20, Screen.width/4.7f/2, 20), "Price: " + objectTurret3.GetComponent<Turret3>().value, style);
			GUI.EndGroup();

			// Turret 4
			GUI.BeginGroup(new Rect(Screen.width/1.27f, 0, Screen.width/4.7f, Screen.height/2));
			bool buyTurret4 = GUI.Button(new Rect(0, 0, Screen.width/4.7f, Screen.height/2), new GUIContent(turret4));
			GUI.Box(new Rect(10, Screen.height/2.3f, Screen.width/4.7f/2, 20), "Turret 3", style);
			GUI.Box(new Rect(Screen.width/4.7f - 90, Screen.height/2.3f, Screen.width/4.7f/2, 20), "Range: " + objectTurret4.GetComponent<Turret4>().range, style);
			GUI.Box(new Rect(10, Screen.height/2.3f + 20, Screen.width/4.7f/2, 20), "Damage: " + objectTurret4.GetComponent<Turret4>().damage, style);
			GUI.Box(new Rect(Screen.width/4.7f - 90, Screen.height/2.3f + 20, Screen.width/4.7f/2, 20), "Price: " + objectTurret4.GetComponent<Turret4>().value, style);
			GUI.EndGroup();

			// Turret 6
			GUI.BeginGroup(new Rect(Screen.width/1.27f, Screen.height/2, Screen.width/4.7f, Screen.height/2));
			bool buyTurret6 = GUI.Button(new Rect(0, 0, Screen.width/4.7f, Screen.height/2), new GUIContent(turret6));
			GUI.Box(new Rect(10, Screen.height/2.3f, Screen.width/4.7f/2, 20), "Turret 4", style);
			GUI.Box(new Rect(Screen.width/4.7f - 90, Screen.height/2.3f, Screen.width/4.7f/2, 20), "Range: " + objectTurret6.GetComponent<Turret6>().range, style);
			GUI.Box(new Rect(10, Screen.height/2.3f + 20, Screen.width/4.7f/2, 20), "Damage: " + objectTurret6.GetComponent<Turret6>().damage, style);
			GUI.Box(new Rect(Screen.width/4.7f - 90, Screen.height/2.3f + 20, Screen.width/4.7f/2, 20), "Price: " + objectTurret6.GetComponent<Turret6>().value, style);
			GUI.EndGroup();

			// coins
			GUI.Box(new Rect(Screen.width/4.7f + 30, 0, 100, 20), "Coins: " + playerInstance.GetComponent<Player>().coins);

			if(buyTurret1 && playerInstance.GetComponent<Player>().coins >= objectTurret1.GetComponent<Turret1>().value) {
				Destroy(newTurret);
				newTurret = Instantiate(objectTurret1) as GameObject;
				newTurret.GetComponent<NavMeshObstacle>().enabled = false;
				newTurret.GetComponent<Turret1>().PauseGame();
				turretNotPurchased = newTurret;
				turretValue = objectTurret1.GetComponent<Turret1>().value;
			} else if(buyTurret3 && playerInstance.GetComponent<Player>().coins >= objectTurret3.GetComponent<Turret3>().value) {
				Destroy(newTurret);
				newTurret = Instantiate(objectTurret3) as GameObject;
				newTurret.GetComponent<NavMeshObstacle>().enabled = false;
				newTurret.GetComponent<Turret3>().PauseGame();
				turretNotPurchased = newTurret;
				turretValue = objectTurret3.GetComponent<Turret3>().value;
			} else if(buyTurret4 && playerInstance.GetComponent<Player>().coins >= objectTurret4.GetComponent<Turret4>().value) {
				Destroy(newTurret);
				newTurret = Instantiate(objectTurret4) as GameObject;
				newTurret.GetComponent<NavMeshObstacle>().enabled = false;
				newTurret.GetComponent<Turret4>().PauseGame();
				turretNotPurchased = newTurret;
				turretValue = objectTurret4.GetComponent<Turret4>().value;
			} else if(buyTurret6 && playerInstance.GetComponent<Player>().coins >= objectTurret6.GetComponent<Turret6>().value) {
				Destroy(newTurret);
				newTurret = Instantiate(objectTurret6) as GameObject;
				newTurret.GetComponent<NavMeshObstacle>().enabled = false;
				newTurret.GetComponent<Turret6>().PauseGame();
				turretNotPurchased = newTurret;
				turretValue = objectTurret6.GetComponent<Turret6>().value;
			}
		}
	}

	public void GameStart() {
		if(spaceshipInstance != null) {
			Destroy(spaceshipInstance);
			
			mapCamera.transform.position = new Vector3(50, 57.5f, 50);
			mapCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

			foreach(GameObject turret in allTurrets) {
				Destroy(turret);
			}
		} else {
			mapInstance = Instantiate (map) as Terrain;
		}

		mapCamera = mapInstance.transform.FindChild ("Camera").camera;
		mapCamera.camera.enabled = false;
		menuCamera.camera.enabled = false;
		
		gameTitleInstance.enabled = false;

		guiGrenadesQtyInstance.enabled = true;
		guiGrenadesImageInstance.enabled = true;

		spaceshipInstance = Instantiate (spaceship) as GameObject;
		spaceshipImageInstance.enabled = true;
		spaceshipLifeInstance.enabled = true;
		this.spaceshipLifeInstance.pixelInset = new Rect(this.spaceshipLifeInstance.pixelInset.x, this.spaceshipLifeInstance.pixelInset.y, 1 * this.spaceshipLifeBoxInstance.pixelInset.width, this.spaceshipLifeInstance.pixelInset.height);
		spaceshipLifeBoxInstance.enabled = true;

		objectTurret1.GetComponent<Turret1> ().value = 300;
		objectTurret3.GetComponent<Turret3> ().value = 1300;
		objectTurret4.GetComponent<Turret4> ().value = 2000;
		objectTurret6.GetComponent<Turret6> ().value = 4500;

		waveManagerInstance = Instantiate (waveManager) as GameObject;

		Screen.lockCursor = true;
		Screen.showCursor = false;

		playerInstance = Instantiate (player, new Vector3 (17, 8.5f, 6), Quaternion.identity) as GameObject;
		gameHasStarted = true;
	}

	public void GameOver() {
		StopAllCoroutines ();
		waveManagerInstance.GetComponent<WaveManager> ().GameOver ();
		Destroy (waveManagerInstance);

		GameObject[] allEnemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < allEnemies.Length; i++) {
			Destroy(allEnemies[i]);
		}

		playerInstance.GetComponent<Player> ().GameOver ();
		Destroy (playerInstance);

		mapInstance.transform.FindChild ("Camera").transform.position = new Vector3(29, 10, 26);
		mapInstance.transform.FindChild ("Camera").LookAt (spaceship.transform.position);
		mapInstance.transform.FindChild ("Camera").camera.enabled = true;

		Screen.lockCursor = false;
		Screen.showCursor = true;

		gameHasStarted = false;
		gameTitleInstance.enabled = true;
	}

	public void OnPause() {
		Object[] objects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("PauseGame", SendMessageOptions.DontRequireReceiver);
		}

		spaceshipImageInstance.enabled = false;
		spaceshipLifeBoxInstance.enabled = false;
		spaceshipLifeInstance.enabled = false;
		guiGrenadesImageInstance.enabled = false;
		guiGrenadesQtyInstance.enabled = false;
	}

	public void Unpaused() {
		Object[] objects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("ResumeGame", SendMessageOptions.DontRequireReceiver);
		}

		spaceshipImageInstance.enabled = true;
		spaceshipLifeBoxInstance.enabled = true;
		spaceshipLifeInstance.enabled = true;
		guiGrenadesImageInstance.enabled = true;
		guiGrenadesQtyInstance.enabled = true;
	}

	private IEnumerator SpawnGrenades() {
		if(ammoInstance == null && chopperInstance == null && !onPause) {
			alreadySpawning = true;
			yield return new WaitForSeconds(Random.Range(60, 120));
			chopperInstance = Instantiate(chopper) as GameObject;
			ammoReady = false;
			moveChopper = true;
			alreadySpawning = false;
		}
	}

	public void SetGrenadesQty(int qty) {
		this.guiGrenadesQtyInstance.text = "x " + qty;
	}

	public void SetSpaceshipLifeBar(float percentage) {
		this.spaceshipLifeInstance.pixelInset = new Rect(this.spaceshipLifeInstance.pixelInset.x, this.spaceshipLifeInstance.pixelInset.y, percentage * this.spaceshipLifeBoxInstance.pixelInset.width, this.spaceshipLifeInstance.pixelInset.height);
	}

	public static IEnumerator DisplayWaveNumber(int waveNumber) {
		GameManager.wavesNumberInstance.text = "Wave " + waveNumber;
		GameManager.wavesNumberInstance.enabled = true;

		yield return new WaitForSeconds (3);

		GameManager.wavesNumberInstance.enabled = false;
	}
}
