using UnityEngine;

[System.Serializable]
public class Sound {

	public string name;
	public AudioClip clip;

	// We've setup a range clamp, because volume/pitch will never be under 0 or over 1
	// Makes a neat slider too
	[Range(0f, 1f)]
	public float volume = 0.7f;
	[Range(0f, 1f)]
	public float pitch = 1f;

	// Randomness multiplier for variety
	// Again, clamped to avoid errors
	[Range(0f, 0.5f)]
	public float randomVolume = 0.1f;
	[Range(0f, 0.5f)]
	public float randomPitch = 0.1f;

	private AudioSource source;

	public void SetSource(AudioSource _source) {
		source = _source;
		source.clip = clip;
	}

	public void Play() {
		// Plays associated sound with some variance
		source.volume = volume * (1 + Random.Range(-randomVolume/2f, randomVolume/2f));
		source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
		source.Play();
	}
}

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	[SerializeField]
	Sound[] sounds;

	private void Awake() {
		if (instance != null) {
			Debug.LogError("More than one AudioManager in the scene.");
		} else {
			instance = this;
		}
	}

	private void Start() {
		// Loop through all sounds & spawn an object for each one
		for (int i = 0; i < sounds.Length; i++) {
			GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
			_go.transform.SetParent(this.transform); // Used to clean up the Hierarchy on runtime (nests it under parent)
			sounds[i].SetSource(_go.AddComponent<AudioSource>());
		}
	}

	public void PlaySound (string _name) {
		for (int i = 0; i < sounds.Length; i++) {
			// Check if it's the correct sound
			if (sounds[i].name == _name) {
				sounds[i].Play();
				return;
			}
		}

		//	No sound  found with that _name
		Debug.LogWarning("AudioManager: Sound not found in list: " + _name);
	}
}
