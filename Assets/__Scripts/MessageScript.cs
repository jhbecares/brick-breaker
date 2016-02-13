using UnityEngine;
using System.Collections;

public class MessageScript : MonoBehaviour {

	public int timeAlive = 5000;
	public int points { get; set; }
	public bool messagePointsBool{ get; set; }
	public bool messageLifeBool{ get; set; }
	public bool messageIman{ get; set; }

	float startTime;
	float currentTime;

	// Use this for initialization
	void Start () {
		// Llevamos la cuenta del tiempo que el mensaje ha estado activo, para saber
		// cuándo tenemos que eliminarlo
		startTime = Time.deltaTime * 1000;
		currentTime = Time.deltaTime * 1000;

		if (this.gameObject.tag == "MessagePoints") {
			messagePointsBool = true;
		} else if (this.gameObject.tag == "MessageLife") {
			messageLifeBool = true;
		} else if (this.gameObject.tag == "MessageIman") {
			messageIman = true;
		}
	}

	public void ShowMessagePoints(int points) {
		// Mostramos el texto correspondiente al número de puntos que hemos conseguido
		this.points = points;
		this.GetComponent<GUIText> ().text = "+ " + points + " extra points!";
	}

	void Update () {
		// Avanzamos el tiempo actual
		currentTime += Time.deltaTime * 1000;

		// Hacemos que el mensaje parpadee durante todo el tiempo que está activo
		// Cuando su tiempo de vida (timeAlive) ya ha acabado, destruimos el 
		// componente
		if (messagePointsBool == true || messageLifeBool == true || messageIman == true) {
			if(Time.fixedTime%.5<.2) {
				this.gameObject.GetComponent<GUIText> ().enabled = false;
			}
			else{
				this.gameObject.GetComponent<GUIText> ().enabled = true;
			}

			if (currentTime - startTime > timeAlive) {
				Destroy (this.gameObject);
				Destroy (this.GetComponent<GUIText> ());
				Destroy (this);
			}
		}
	}
}
