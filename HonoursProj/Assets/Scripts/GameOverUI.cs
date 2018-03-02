using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {
	[SerializeField]
	string mouseHoverSoundName = "ButtonHover";

	[SerializeField]
	string buttonPressSoundName = "ButtonPress";

	AudioManager audioManager;

	private void Start() {
		audioManager = AudioManager.audioManInstance;
		if(audioManager == null) {
			Debug.LogError("No AudioManager in scene.");
		}
	}

	public void Quit() {
		audioManager.PlaySound(buttonPressSoundName);

		Debug.Log("Application Terminated");
		Application.Quit();
	}

	public void Retry() {
		audioManager.PlaySound(buttonPressSoundName);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void OnMouseOver() {
		audioManager.PlaySound(mouseHoverSoundName);
	}
}
