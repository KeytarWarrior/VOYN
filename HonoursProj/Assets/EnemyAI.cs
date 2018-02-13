using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour {

	// Target for chasing
	public Transform target;

	// How many times each second the path is updated
	public float updateRate = 2f;

	// Caching
	private Seeker seeker;
	private Rigidbody2D rb;

	// The calculated path
	public Path path;

	// The AI's speed per second (framerate independent)
	public float speed = 300f;
	// Way to change between force and impulse - simple Enum
	public ForceMode2D fMode;

	[HideInInspector] // Makes sure it is public but doesn't show in inspector
	public bool pathIsEnded = false;

	// The max distance from the AI toa  waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3f;

	// The waypoint we are currently moving towards
	private int currentWaypoint = 0;

	void Start() {
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();

		if (target == null) {
			Debug.LogError("No Player found.");
			return;
		}

		// Start a new path to the target position and return the result to the OnPathComplete method
		seeker.StartPath(transform.position, target.position, OnPathComplete);

		StartCoroutine(UpdatePath ());
	}

	IEnumerator UpdatePath() {
		if (target == null) {
			//TODO: Insert a palyer search here.
			yield return false; // This is a change in the API - now you need to use yield for enumerators
		}

		// Start a new path to the target position and return the result to the OnPathComplete method
		seeker.StartPath(transform.position, target.position, OnPathComplete);

		yield return new WaitForSeconds(1f / updateRate);
		StartCoroutine(UpdatePath());
	}

	public void OnPathComplete (Path p) {
		Debug.Log("Path found. Error: " + p.error);
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}

	// Very sueful for physics calculations
	void FixedUpdate() {

		if (target == null) {
			//TODO: Insert a palyer search here.
			return;
		}

		//TODO: Always look at player.

		if (path == null)
			return;

		if (currentWaypoint >= path.vectorPath.Count) {
			if (pathIsEnded)
				return;

			Debug.Log("End of path reached.");
			pathIsEnded = true;
			return;
		}
		pathIsEnded = false;

		// Direction to next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime; // Fixed delta time instead of deltaTime because of fixed Update

		// AI Movement
		rb.AddForce(dir, fMode);

		float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

		if (dist < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
	}
}
