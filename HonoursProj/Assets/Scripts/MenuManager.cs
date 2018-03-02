using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	[SerializeField]
	string hoverOverSound = "ButtonHover";

	[SerializeField]
	string pressButtonSound = "ButtonPress";

	AudioManager audioManager;

	private void Start() {
		audioManager = AudioManager.audioManInstance;
		if(audioManager == null) {
			Debug.LogError("No AudioManager foiund!");
		}
	}

	public void StartGame() {
		audioManager.PlaySound(pressButtonSound);
		// Gets current build index and loads next scene
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void QuitGame() {
		audioManager.PlaySound(pressButtonSound);
		Debug.Log("We quit the game");
		Application.Quit();
	}

	public void OnMouseOver() {
		audioManager.PlaySound(hoverOverSound);
	}

}
