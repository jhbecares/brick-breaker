using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Picker : MonoBehaviour {

	public GameObject paddlePrefab;
	public GameObject ballPrefab;
	public GameObject attachedBall = null;

	static int times = 0;

	public static int numBaskets = 3;
	public float paddleBottomY = -14f;
	public float paddleSpacingY = 2f;
	public float ballBottomY = -6f;
	public List<GameObject> paddleList;

	void Start () {
		DontDestroyOnLoad (GameObject.FindGameObjectWithTag ("Highscore"));
		DontDestroyOnLoad (GameObject.FindGameObjectWithTag ("Score"));
		DontDestroyOnLoad (GameObject.FindGameObjectWithTag ("Lives")); 

		if (times == 0) {
			PlayerPrefs.SetInt ("Score", 0);
			PlayerPrefs.SetInt ("Lives", numBaskets);
			times++;

			// Instanciar la nueva bola
			Lives.lives = numBaskets;
			
			Vector3 pos = Vector3.zero;
			paddleList = new List<GameObject>();
			for(int i= 0; i<numBaskets; i++) {
				GameObject tBasketGO = Instantiate ( paddlePrefab ) as GameObject;
				pos = Vector3.zero;
				pos.y = paddleBottomY + ( paddleSpacingY * i );
				tBasketGO.transform.position = pos;
				paddleList.Add(tBasketGO);
			}
			SpawnBall ();

		} else {
			Lives.lives = PlayerPrefs.GetInt("Lives");

			Vector3 pos = Vector3.zero;
			paddleList = new List<GameObject>();
			for(int i= 0; i<Lives.lives; i++) {
				GameObject tBasketGO = Instantiate ( paddlePrefab ) as GameObject;
				pos = Vector3.zero;
				pos.y = paddleBottomY + ( paddleSpacingY * i );
				tBasketGO.transform.position = pos;
				paddleList.Add(tBasketGO);
			}
			SpawnBall ();
		}


	}

	void SpawnBall() {
		if (ballPrefab == null) {
			Debug.Log ("El prefab es null");
			return;
		}
		// Posición de la bola: lo colocamos una unidad más arriba que el paddle
		//Vector3 ballPos = paddleList[paddleList.Count-1].transform.position + new Vector3(0, 0.75f, 0);

		// Rotación inicial de la bola - dummy
		//Quaternion ballRot = Quaternion.identity;

		//attachedBall = Instantiate ( ballPrefab, ballPos, ballRot ) as GameObject;
		attachedBall = Instantiate ( ballPrefab ) as GameObject; 

	}

	// Usamos fixed update en lugar de update para que no haya lag en nuestra bola al moverla con el paddle
	void FixedUpdate() {

		// Comprobamos si estamos pulsando espacio para lanzar la bola
		if (attachedBall) {
			Rigidbody ballRB = attachedBall.GetComponent<Rigidbody> ();
			ballRB.position = paddleList[paddleList.Count-1].transform.position + new Vector3(0, 0.75f, 0);

			if (Input.GetButtonDown ("Jump")) {
				ballRB.isKinematic = false;
				//this.GetComponent<Rigidbody> ().AddForce (80, 800f, 0);
				ballRB.AddForce(300f*Input.GetAxis("Horizontal"), 500f, 0);
				attachedBall = null;
			}
		}
	}

	public void DestroyPaddles() {
		while (paddleList.Count > Lives.lives) {
			int paddleIndex = paddleList.Count - 1;
			Lives.lives = paddleIndex;
		
			// cogemos la referencia al paddle
			GameObject tPaddleGO = paddleList [paddleIndex];
			// y borramos el objeto
			paddleList.RemoveAt (paddleIndex);
			Destroy (tPaddleGO);
		}
	}

	public void BallDestroyed() {
		//// Destruimos uno de los paddles
		// cogemos el índice de nuestra lista de paddles
		int paddleIndex = paddleList.Count-1;
		Lives.lives = paddleIndex;

		// cogemos la referencia al paddle
		GameObject tPaddleGO = paddleList[paddleIndex];
		// y borramos el objeto
		paddleList.RemoveAt(paddleIndex);
		Destroy (tPaddleGO);

		// Si se nos acaban las vidas, volvemos a empezar el juego
		if (paddleList.Count == 0) {
			BrickScript.score = 0; // reseteamos la puntuación
			Application.LoadLevel ("Level0");
			times = 0;
		} else {
			// en caso contrario simplemente lanzamos de nuevo el nivel
			SpawnBall();
<<<<<<< HEAD
			PlayerPrefs.SetInt("Lives", Lives.lives);
=======
			Debug.Log ("Nuevo num de vidas: " + Lives.lives);
			Debug.Log ("Nuevo 222num de vidas: " + PlayerPrefs.GetInt("Lives"));
			PlayerPrefs.SetInt("Lives", Lives.lives);
			Debug.Log ("Nuevo 333num de vidas: " + PlayerPrefs.GetInt("Lives"));
>>>>>>> 7cc3cb07c16579b18c5f705b2f62100bd8f7abcc
		}
	}

}
