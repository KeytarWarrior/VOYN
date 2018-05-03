using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dGun))]
	public class D2dGun_Editor : D2dEditor<D2dGun>
	{
		protected override void OnInspector()
		{
			BeginError(Any(t => t.ShootDelay < 0.0f));
				DrawDefault("ShootDelay");
			EndError();
			DrawDefault("BulletPrefab");
			DrawDefault("MuzzleFlashPrefab");
		}
	}
}
#endif

namespace Destructible2D
{
	public class D2dGun : MonoBehaviour
	{
		[Tooltip("Minimum time between each shot in seconds")]
		public float ShootDelay = 0.05f;

		[Tooltip("The bullet prefab spawned when shooting")]
		public GameObject BulletPrefab;

		[Tooltip("The muzzle prefab spawned on the gun when shooting")]
		public GameObject MuzzleFlashPrefab;
		
		// Seconds until next shot is available
		[SerializeField]
		private float cooldown;

		public bool CanShoot
		{
			get
			{
				return cooldown <= 0.0f;
			}
		}

		public void Shoot()
		{
			if (cooldown <= 0.0f)
			{
				cooldown = ShootDelay;

				if (BulletPrefab != null)
				{
					//Vector3 offset = new Vector3(0, 0.5f, 0);
					Vector3 offset = new Vector3(Random.Range(-0.5F, 0.5F), Random.Range(-0.5F, 0.5F), 0);
					//Quaternion angleOffset = new Quaternion(0,0,0,0);
					//Quaternion rotation = Quaternion.Euler(0, 30, 0);
					//Instantiate(BulletPrefab, transform.position, transform.rotation + angleOffset);
					Instantiate(BulletPrefab, transform.position + offset, transform.rotation);
					//GameObject beam = Instantiate(projectile, transform.position + offset, Quaternion.identity) as GameObject;
					//beam.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-3.0F, 3.0F), projectileSpeed, 0);
				}

				if (MuzzleFlashPrefab != null)
				{
					Instantiate(MuzzleFlashPrefab, transform.position, transform.rotation);
				}
			}
		}

		protected virtual void Update()
		{
			cooldown -= Time.deltaTime;
		}
	}
}