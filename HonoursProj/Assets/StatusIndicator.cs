using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

	[SerializeField] // Makes it show up in editor, even though it is private
	private RectTransform healthBarRect;
	[SerializeField]
	private Text healthText;

	private void Start() {
		if (healthBarRect == null) {
			Debug.LogError("STATUS INDICATOR: No health bar object referenced!");
		}
		if (healthText == null) {
			Debug.LogError("STATUS INDICATOR: No health text object referenced!");
		}
	}

	// Underscore makes it epxlicit via naming that a variable is private
	public void SetHealth(int _cur, int _max) {
		float _value = (float)_cur / _max;

		healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
		healthText.text = _cur + "/" + _max + " HP";
		//TODO: Grab image component and use a gradient value that you have created and lerp through it
	}
}
