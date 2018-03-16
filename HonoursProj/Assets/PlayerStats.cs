using UnityEngine;

public class PlayerStats : MonoBehaviour {

	public static PlayerStats instance;

	public int maxHealth = 100;

	private int _curHealth;
	public int curHealth {
		get { return _curHealth; }
		set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
	}

	void Awake() {
		if (instance == null) {
			instance = this;
		}

		curHealth = maxHealth;
	}
}
