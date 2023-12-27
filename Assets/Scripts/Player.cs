using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour 
{
	public int curHealth;
	public int maxHealth;
	public int damage;
	public float moveSpeed;
	public float attackRate;
	private float attackTimer;
	public float bulletSpeed;

	private Vector3 mousePos;

	//Prefabs
	public GameObject bulletPrefab;
	public GameObject deathEffect;

	//Components
	public Rigidbody2D rig;
	public SpriteRenderer sr;

	[SerializeField] FloatingJoystick joystick;
    [SerializeField] float shootingRange = 10;
	[SerializeField] LayerMask enemyLayer;
	[SerializeField] AudioSource shootAudio;

	public UI ui; 

    public Player(int curHealth, int maxHealth, int damage, float moveSpeed, float attackRate, float bulletSpeed, GameObject bulletPrefab, GameObject deathEffect, Rigidbody2D rig, SpriteRenderer sr, FloatingJoystick joystick, float shootingRange, LayerMask enemyLayer, AudioSource shootAudio, UI ui)
    {
        this.curHealth = curHealth;
        this.maxHealth = maxHealth;
        this.damage = damage;
        this.moveSpeed = moveSpeed;
        this.attackRate = attackRate;
        this.bulletSpeed = bulletSpeed;
        this.bulletPrefab = bulletPrefab;
        this.deathEffect = deathEffect;
        this.rig = rig;
        this.sr = sr;
        this.joystick = joystick;
        this.shootingRange = shootingRange;
        this.enemyLayer = enemyLayer;
        this.shootAudio = shootAudio;
        this.ui = ui;
    }

    void Update ()
	{
		attackTimer += Time.deltaTime;
		if (attackTimer >= attackRate)
		{
			attackTimer = 0.0f;
			Shoot();
		}
		Camera cam = Camera.main;
		Inputs();
	}

	void Inputs ()
	{
		float moveHorizontal = joystick.Horizontal;
		float moveVertical = joystick.Vertical;

		Vector2 movement = new Vector2(moveHorizontal, moveVertical);

		if (movement.magnitude > 0.1f)
		{
			// Вычисляем угол между направлением движения и осью x
			float angle = Mathf.Atan2( - movement.x, movement.y) * Mathf.Rad2Deg;

			// Поворачиваем персонаж на округленный угол
			transform.rotation = Quaternion.Euler(0f, 0f, angle);
		}
		movement = movement.normalized;
		rig.velocity = movement * moveSpeed;

	}

	
	void Move (float x, float y)
	{
		rig.velocity = new Vector2(x, y) * moveSpeed;
	}

	void Shoot ()
	{
	
			Collider2D closestEnemy = Physics2D.OverlapCircle(transform.position, shootingRange, enemyLayer);

			if (closestEnemy != null)
			{
				Transform enemyTransform = closestEnemy.transform;
				Vector2 direction = enemyTransform.position - transform.position;
			
				// Проверяем, находится ли враг в пределах стрельбы
				if (direction.magnitude <= shootingRange)
				{
					// Стреляем в направлении ближайшего врага
					GameObject proj = Instantiate(bulletPrefab, transform.position + (transform.up * 0.7f), transform.rotation);
					shootAudio.Play();
					Projectile projScript = proj.GetComponent<Projectile>();

					projScript.damage = damage;
					projScript.rig.velocity = direction.normalized * bulletSpeed;
				}
			}
		

	}


	public void TakeDamage (int dmg)
	{
		if(curHealth - dmg <= 0)
		{
			Die();
		}
		else
		{
			curHealth -= dmg;
			Game.game.Shake(0.01f, 0.01f, 50.0f);
			ui.ShakeSlider(0.02f, 0.05f, 30.0f);
			ui.StartCoroutine("HealthDown", curHealth);
			StartCoroutine(DamageFlash());
		}
	}

	
	IEnumerator DamageFlash ()
	{
		sr.color = Color.red;
		yield return new WaitForSeconds(0.05f);
		sr.color = Color.white;
	}


	void Die ()
	{
		SceneManager.LoadScene("Menu");
	}
}
