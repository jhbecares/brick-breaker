using UnityEngine;
using System.Collections;

public class BlockerScript : MonoBehaviour {

	public GameObject blockerGO;
	public float blockerTime = 10000f;
	public float[] blockerTimes;

	float startTime = 0;
	float currentTime = 0;

	// Use this for initialization
	void Start () {
		// Elegimos uno de los tiempos del bloqueador. Tenemos un array que contiene
		// varios float y elegimos uno de esos float aleatoriamente. Esto 
		// es útil porque así el jugador no puede "contar" cuanto queda para que
		// el bloqueador se elimine, sino que será un tiempo distinto cada vez. 
		// También podríamos haber hecho directamente un random entre
		// 1000 y 10000 o algo así, pero he decidido hacerlo de esta forma para
		// tener más control sobre los tiempos. Llegado el caso se podría cambiar
		int rand = Random.Range (0, blockerTimes.Length-1);
		blockerTime = blockerTimes [rand];

		startTime = Time.deltaTime * 1000;
		currentTime = Time.deltaTime * 1000;
	}
	
	// Update is called once per frame
	void Update () {
		// Contamos cuánto tiempo lleva el bloqueador puesto.
		// Si supera el tiempo que le hemos asignado, lo eliminamos
		currentTime += Time.deltaTime * 1000;
		if (currentTime - startTime > blockerTime) {
			Destroy (gameObject);
		}
	}
}
