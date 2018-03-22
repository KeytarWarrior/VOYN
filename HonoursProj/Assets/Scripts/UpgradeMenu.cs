using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour {

	[SerializeField] private Text healthText;
	[SerializeField] private Text speedText;
	[SerializeField] private float healthMultiplier = 1.3f;
	[SerializeField] private float movementSpeedMultiplier = 1.2f;
	[SerializeField] private int upgradeCost = 50;

	private PlayerStats stats;

	// Calls UpdateValues whenever menu is opened
	private void OnEnable() {
		stats = PlayerStats.instance;
		UpdateValues();
	}

	// Updates UI text elements with the correct stat values
	void UpdateValues() {
		healthText.text = "HEALTH: " + stats.maxHealth.ToString();
		speedText.text = "SPEED: " + stats.movementSpeed.ToString();
	}

	// Applies health upgrade multiplier
	public void UpgradeHealth() {
		if(GameMaster.Money < upgradeCost) {
			AudioManager.audioManInstance.PlaySound("NoMoney");
			return;
		}
		stats.maxHealth = (int)(stats.maxHealth * healthMultiplier);    // Need to cast multiplier into an int
		GameMaster.Money -= upgradeCost;    // Substracts money when upgrading
		AudioManager.audioManInstance.PlaySound("Money");
		UpdateValues();
	}

	// Applies health upgrade multiplier
	public void UpgradeSpeed() {
		if (GameMaster.Money < upgradeCost) {
			AudioManager.audioManInstance.PlaySound("NoMoney");
			return;
		}
		// Already float so no casting. However, number is rounded so it doesn't clip outside UI boundaries
		stats.movementSpeed = Mathf.Round (stats.movementSpeed * movementSpeedMultiplier);
		GameMaster.Money -= upgradeCost;    // Substracts money when upgrading
		AudioManager.audioManInstance.PlaySound("Money");
		UpdateValues();
	}
}
