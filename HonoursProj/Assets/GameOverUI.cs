using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

	public void Quit() {
		Debug.Log("Application Terminated");
		Application.Quit();
	}

	public void Retry() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
