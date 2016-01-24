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
	public static int maxLives {get;set;}

	// Constantes para marcar el tamaño del paddle
	public static bool sizePaddleSmall { get; set; }
	public static bool sizePaddleBig { get; set; }
	public static bool sizePaddleNormal { get; set; }
	public static int countSizePaddle = 0;
	int maxCount = 500;


	void Start () {
		DontDestroyOnLoad (GameObject.FindGameObjectWithTag ("Highscore"));
		DontDestroyOnLoad (GameObject.FindGameObjectWithTag ("Score"));
		DontDestroyOnLoad (GameObject.FindGameObjectWithTag ("Lives")); 

		maxLives = 3;

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
			for(int i= 0; i<Lives.lives && i < maxLives; i++) {
				GameObject tBasketGO = Instantiate ( paddlePrefab ) as GameObject;
				pos = Vector3.zero;
				pos.y = paddleBottomY + ( paddleSpacingY * i );
				tBasketGO.transform.position = pos;
				paddleList.Add(tBasketGO);
			}
			SpawnBall ();
		}

		sizePaddleSmall = false;
		sizePaddleBig = false;
		sizePaddleNormal = true;
	}

	void SpawnBall() {
		if (ballPrefab == null) {
			Debug.Log ("El prefab es null");
			return;
		}
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

	void LateUpdate() {
		if (sizePaddleBig == true || sizePaddleSmall == true) {
			if (countSizePaddle < maxCount) {
				countSizePaddle++;
			}
			else if (countSizePaddle == maxCount) {
				NormalisePaddle();
				countSizePaddle = 0;
			}
		}
	}

	// Eliminamos uno de los paddles y actualizamos las vidas
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


	// Crea un nuevo paddle cuando nos dan una vida más
	// y actualiza los atributos correspondientes
	public void AddLife() {
		if (Lives.lives+1 <= maxLives) {
			int numB = paddleList.Count;
			GameObject tBasketGO = Instantiate (paddlePrefab) as GameObject;
			Vector3 pos = new Vector3 (paddleList [numB - 1].transform.position.x, paddleBottomY + (paddleSpacingY * numB), 0);
			tBasketGO.transform.position = pos;
			paddleList.Add (tBasketGO);
		}

		Lives.lives++;
		PlayerPrefs.SetInt ("Lives", Lives.lives);
	}

	public void InstantiateBalls() {
		BallScript currentBall = FindObjectOfType(typeof(BallScript)) as BallScript;
		//foreach (BallScript ball in ballsc) {
			GameObject go = Instantiate(ballPrefab) as GameObject;
			go.GetComponent<Rigidbody>().isKinematic = false;
			go.transform.position = currentBall.transform.position;
			go.GetComponent<Rigidbody> ().AddForce(300f*Input.GetAxis("Horizontal"), 300f, 0);
			go.GetComponent<Rigidbody>().velocity = currentBall.GetComponent<Rigidbody>().velocity;
	}

	public void BallDestroyed() {

		int livesOld = Lives.lives;

		if (Lives.lives > paddleList.Count) {
			// estamos jugando con más de una bola, y por tanto lo único que tenemos que hacer es restar
			Lives.lives--;
			PlayerPrefs.SetInt ("Lives", Lives.lives);
			// Si hay mas de una bola, no hacemos el spawn. 
			BallScript [] ballsc = FindObjectsOfType(typeof(BallScript)) as BallScript[];
			if (ballsc.Length <= 1)
				SpawnBall();
		} else if (Lives.lives <= maxLives && Lives.lives > 1 && Lives.lives == paddleList.Count) {
			// Las vidas y los paddles son iguales, por tanto hay que destruir uno de los paddle
			int paddleIndex = paddleList.Count - 1;
			// cogemos la referencia al paddle
			GameObject tPaddleGO = paddleList [paddleIndex];
			// y borramos el objeto
			paddleList.RemoveAt (paddleIndex);
			Destroy (tPaddleGO);
			Lives.lives--;
			PlayerPrefs.SetInt ("Lives", Lives.lives);

			// Comprobamos que a pesar de eliminar un paddle no tenemos sólo una bola, en cuyo caso
			// la volvemos a lanzar
			BallScript [] ballsc = FindObjectsOfType(typeof(BallScript)) as BallScript[];
			if (ballsc.Length < 2) {
				SpawnBall ();
			}
		} 
		// Si se nos acaban las vidas, volvemos a empezar el juego
		else if (Lives.lives == 1) {
			BrickScript.score = 0; // reseteamos la puntuación

			times = 0;
			Destroy (GameObject.FindGameObjectWithTag ("Highscore"));
			Destroy (GameObject.FindGameObjectWithTag ("Score"));
			Destroy (GameObject.FindGameObjectWithTag ("Lives")); 
			Application.LoadLevel ("GameOver");

			Destroy (gameObject);
		} else {
			// En cualquier otro caso (no debería llegar aquí) simplemente restamos
			// y volvemos a lanzar la bola
			Lives.lives--;
			PlayerPrefs.SetInt ("Lives", Lives.lives);
			SpawnBall ();
		}
	}



	// Cuando el paddle choca con un powerup de cambiar el tamaño del paddle, 
	// llamamos a esta función, que aumenta o disminuye el tamaño con una
	// probabilidad de 0.5
	public void ChangePaddleSize() {
		int val = Random.Range (1, 1000);
		if (val % 2 == 0) {
			MaximisePaddle ();
		} else {
			MinimisePaddle ();
		}
	}

	public void MinimisePaddle() {
		// volvemos a empezar de 0 para el máximo tiempo del powerup
		countSizePaddle = 0;
		if (!sizePaddleSmall) {
			for (int i = 0; i < paddleList.Count; i++) {
				float prevx, prevy, prevz;
				prevx = paddleList [i].transform.localScale.x;
				prevy = paddleList [i].transform.localScale.y;
				prevz = paddleList [i].transform.localScale.z;
				// cambiamos el paddle a una escala más pequeña
				if (paddleList.Count != 1) {
					float aux = prevx - (prevx / 2) / (paddleList.Count - 1) * i;
					paddleList [i].transform.localScale = new Vector3 (aux, prevy, prevz);
				} else {
					paddleList [i].transform.localScale = new Vector3 (prevx / 2, prevy, prevz);
				}
			}

			// Guardamos con qué tamaño de bola estamos jugando
			sizePaddleSmall = true;
			sizePaddleNormal = false;
			sizePaddleBig = false;
		}
	}

	public void MaximisePaddle() {
		countSizePaddle = 0;

		if (!sizePaddleBig) {

			for (int i = 0; i < paddleList.Count; i++) {
				float prevx, prevy, prevz;
				prevx = paddleList [i].transform.localScale.x;
				prevy = paddleList [i].transform.localScale.y;
				prevz = paddleList [i].transform.localScale.z;
				// cambiamos el paddle a una escala más grande
				if (paddleList.Count != 1) {
					float aux = prevx + (prevx / 2) / (paddleList.Count - 1) * i;
					paddleList [i].transform.localScale = new Vector3 (aux, prevy, prevz);
				} else {
					paddleList [i].transform.localScale = new Vector3 (prevx * 2, prevy, prevz);
				}
			}

			sizePaddleBig = true;
			sizePaddleNormal = false;
			sizePaddleSmall = false;
		}
	}

	public void NormalisePaddle() {

		foreach (GameObject paddle in paddleList) {
			float prevx, prevy, prevz;
			prevx = paddle.transform.localScale.x;
			prevy = paddle.transform.localScale.y;
			prevz = paddle.transform.localScale.z;
			paddle.transform.localScale = new Vector3(4f, prevy, prevz);
		}
		sizePaddleNormal = true;
		sizePaddleBig = false;
		sizePaddleSmall = false;
	}
}
