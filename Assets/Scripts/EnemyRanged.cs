using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class EnemyRanged : Enemy
{
    [Inject] private Player player { get; set; }

	[SerializeField] private int counter;

	[SerializeField] AudioSource shootAudio;
	private void Start()
	{
		var enemyType = Resources.LoadAll<EnemyTypes>("");
		for (int i = 0; i < enemyType.Length; i++)
		{
			name = enemyType[counter].Name;
		}

	}

	void Update()
	{
		attackTimer += Time.deltaTime;

		if (target != null)
		{
			Vector3 dir = transform.position - target.transform.position;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			if (Vector3.Distance(transform.position, target.transform.position) > attackRange)
			{
				ChaseTarget();
			}
			else
			{
				if (attackTimer >= attackRate)
				{
					attackTimer = 0.0f;
					Attack();
				}
			}
		}
		else
		{
			if (player)
			{
				target = player.gameObject;
			}
			else
			{
				rig.simulated = false;
			}
		}
	}

	void ChaseTarget()
	{
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
	}

	public override void Attack()
    {
        GameObject proj = Instantiate(projectilePrefab, transform.position + (transform.up * 0.7f), transform.rotation);
		shootAudio.Play();
        Projectile projScript = proj.GetComponent<Projectile>();

		projScript.damage = damage;

		if (name != "Mage")
		{
			projScript.rig.velocity = (target.transform.position - transform.position).normalized * projectileSpeed;
		}
		else
		{
			projScript.followSpeed = projectileSpeed;
		}
	}

	public override void TakeDamage(int dmg)
	{
		if (health - dmg <= 0)
		{
			Die();
		}
		else
		{
			health -= dmg;
			StartCoroutine(DamageFlash());
		}
	}

	public override IEnumerator DamageFlash()
	{
		sr.color = Color.red;
		yield return new WaitForSeconds(0.03f);
		sr.color = Color.white;
	}

	public override void Die()
	{
		Game.game.curEnemies.Remove(gameObject);
		GameObject pe = Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
		Destroy(pe, 2);
		Destroy(gameObject);
	}
}
