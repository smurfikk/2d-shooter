using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	public string name;

	public int health;
	public int damage;
	public float moveSpeed;
	public float attackRate;
	public float attackTimer;
	public float attackRange;
	public float projectileSpeed;

	public GameObject target;
	public Rigidbody2D rig;
	public SpriteRenderer sr;
	public GameObject projectilePrefab;

	public GameObject deathParticleEffect;



	//Если враг находится на расстоянии выстрела, выпустить снаряд. Если враг ведет ближний бой, поразить цель.
	public abstract void Attack();

	//Вызывается, когда игрок атакует врага.
	public abstract void TakeDamage(int dmg);

	//Вызывается при получении урона. Спрайт мигает красным.
	public abstract IEnumerator DamageFlash();

	//Вызывается, когда здоровье падает ниже 0. Уничтожает их.
	public abstract void Die();
}

public enum EnemyType {Knight, Archer, Mage}
