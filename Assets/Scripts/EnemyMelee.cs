using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Enemy
{

    private void Start()
    {
		var enemyType = Resources.LoadAll<EnemyTypes>("");
		for (int i = 0; i < enemyType.Length; i++)
        {
			name=enemyType[2].Name;
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
			if (Game.game.player)
			{
				target = Game.game.player.gameObject;
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
		Game.game.player.TakeDamage(damage);
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
