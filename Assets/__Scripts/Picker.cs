using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Picker : MonoBehaviour {

	public GameObject paddlePrefab;
	public GameObject ballPrefab;
	public GameObject attachedBall = null;

	public int numBaskets = 3;
	public float paddleBottomY = -14f;
	public float paddleSpacingY = 2f;
	public float ballBottomY = -6f;
	public List<GameObject> paddleList;

	void Start () {
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
		} else {
			// en caso contrario simplemente lanzamos de nuevo el nivel
			SpawnBall();
		}
	}

}
