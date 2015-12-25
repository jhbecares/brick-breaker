using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Picker : MonoBehaviour {

	public GameObject paddlePrefab;
	public int numBaskets = 3;
	public float paddleBottomY = -14f;
	public float paddleSpacingY = 2f;
	public List<GameObject> paddleList;

	void Start () {
		paddleList = new List<GameObject>();
		for(int i= 0; i<numBaskets; i++) {
			GameObject tBasketGO = Instantiate ( paddlePrefab ) as GameObject;
			Vector3 pos = Vector3.zero;
			pos.y = paddleBottomY + ( paddleSpacingY * i );
			tBasketGO.transform.position = pos;
			paddleList.Add(tBasketGO);
		}
	}

	public void BallDestroyed() {
		//// Destruimos uno de los paddles
		// cogemos el índice de nuestra lista de paddles
		int paddleIndex = paddleList.Count-1;

		// cogemos la referencia al paddle
		GameObject tPaddleGO = paddleList[paddleIndex];
		// y borramos el objeto
		paddleList.RemoveAt(paddleIndex);
		Destroy (tPaddleGO);
	}
}
