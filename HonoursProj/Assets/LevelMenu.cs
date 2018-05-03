using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour {
	[SerializeField]
	string mouseHoverSoundName = "ButtonHover";

	[SerializeField]
	string buttonPressSoundName = "ButtonPress";

	AudioManager audioManager;

	private void Start() {
		audioManager = AudioManager.audioManInstance;
		if (audioManager == null) {
			Debug.LogError("No AudioManager in scene.");
		}
	}

	public void LoadSpaceLevel() {
		audioManager.PlaySound(buttonPressSoundName);
		SceneManager.LoadScene("SpaceShooterLevel");
	}

	public void LoadMainLevel() {
		audioManager.PlaySound(buttonPressSoundName);
		SceneManager.LoadScene("Forest");
	}

	public void LoadEnvironementShowcaseLevel() {
		audioManager.PlaySound(buttonPressSoundName);
		SceneManager.LoadScene("SpaceShooterLevel");
	}

	public void LoadLightingLevel() {
		audioManager.PlaySound(buttonPressSoundName);
		SceneManager.LoadScene("SpaceShooterLevel");
	}

	public void OnMouseOver() {
		audioManager.PlaySound(mouseHoverSoundName);
	}
}
