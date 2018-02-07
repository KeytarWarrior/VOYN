using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour {

    public int rotationOffset = 90;

	// Update is called once per frame
	void Update () {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;  //Substracting the player pos from the mouse pos
        difference.Normalize();     // Normalizing the vector. Meaning thatthe sum of the vector will be equal to 1.

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; // Find the angle in degrees
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
	}
}
