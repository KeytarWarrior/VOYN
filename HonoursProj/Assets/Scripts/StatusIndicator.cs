using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

	[SerializeField] // Makes it show up in editor, even though it is private
	private RectTransform healthBarRect;
	[SerializeField]
	private Text healthText;
	// Experimental gradient code
	/*[SerializeField]
	private Image healthBarColor;*/

	// Experimental gradient code
	/*GradientColorKey[] gck = new GradientColorKey[3];
	GradientAlphaKey[] gak = new GradientAlphaKey[2];
	Gradient gradient;
	public Color startColor = new Color(0, 1, 0, 1);
	public Color midColor = new Color(1, 0.92f, 0.016f, 1);
	public Color endColor = new Color(1, 0, 0, 1);*/

	private void Start() {

		// Experimental Gradient code
		/*gck[0].color = startColor;
		gck[0].time = 0.0F;
		gck[1].color = midColor;
		gck[1].time = 0.5F;
		gck[2].color = endColor;
		gck[2].time = 1F;
		gak[0].alpha = 1.0F;
		gak[0].time = 0.0F;
		gak[1].alpha = 1.0F;
		gak[1].time = 1.0F;
		gradient.SetKeys(gck, gak);*/

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

		// Experimental gradient code
		/*healthBarColor.color = gradient.Evaluate(1 - _value);*/
	}
}
