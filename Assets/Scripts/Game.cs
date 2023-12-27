using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Game : MonoBehaviour 
{
	public List<Wave> waves = new List<Wave>();
	public List<GameObject> curEnemies = new List<GameObject>();
	public bool waveActive;
	public int waveCount;
	public float waveTimer;
	public float timeEachWave;
	public bool canUpgrade; //
	public bool gameDone;

	public Transform[] spawnPoints;

	public Transform cam;

	//Enemy Prefabs
	public GameObject kingPrefab;

	public static Game game;
	[SerializeField] private UI ui;
	public Player player;

	[SerializeField] GameObject Buttons;

    [Inject] private EnemySpawn enemySpawn { get; set; }

	void Awake ()
	{
		game = this;
	}

	void Start ()
	{
		StartCoroutine(StartGameTimer());
	}

	//Called at the start of the game for a countdown on the screen.
	IEnumerator StartGameTimer ()
	{
		ui.startText.gameObject.active = true;
		yield return new WaitForSeconds(1);
		ui.startText.text = "BEGINS IN\n<size=150>4</size>"; ui.startText.rectTransform.localScale += new Vector3(0.15f, 0.15f, 0);
		Game.game.Shake(0.1f, 0.1f, 30.0f); Camera.main.orthographicSize -= 0.2f;
		yield return new WaitForSeconds(1);
		ui.startText.text = "BEGINS IN\n<size=150>3</size>"; ui.startText.rectTransform.localScale += new Vector3(0.15f, 0.15f, 0);
		Game.game.Shake(0.1f, 0.1f, 30.0f); Camera.main.orthographicSize -= 0.2f;
		yield return new WaitForSeconds(1);
		ui.startText.text = "BEGINS IN\n<size=150>2</size>"; ui.startText.rectTransform.localScale += new Vector3(0.15f, 0.15f, 0);
		Game.game.Shake(0.1f, 0.1f, 30.0f); Camera.main.orthographicSize -= 0.2f;
		yield return new WaitForSeconds(1);
		ui.startText.text = "BEGINS IN\n<size=150>1</size>"; ui.startText.rectTransform.localScale += new Vector3(0.15f, 0.15f, 0);
		Game.game.Shake(0.1f, 0.1f, 30.0f); Camera.main.orthographicSize -= 0.2f;
		yield return new WaitForSeconds(1);
		ui.startText.gameObject.active = false;
		Camera.main.orthographicSize = 7;

		NextWave();
	}

	void Update ()
	{
		//Если волна в данный момент идет, но врагов нет, то мы завершаем волну.
		if (waveActive)
		{
			if(curEnemies.Count == 0)
			{
				waveActive = false;
				StartCoroutine(WaveEndTimer());
			}
		}

		//Если проигрыватель может обновиться, то разрешите необходимый ввод с клавиатуры.
		if (canUpgrade)
		{
			Buttons.SetActive(true);
		}
        else
        {
			Buttons.SetActive(false);
        }

		if (Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
	}

	public void QButton()
    {
		player.damage += 5;
		canUpgrade = false;
		ui.upgradeText.gameObject.active = false;
		Game.game.Shake(0.15f, 0.15f, 30.0f);
	}

	public void EButton()
    {
		player.attackRate = 0.05f;
		canUpgrade = false;
		ui.upgradeText.gameObject.active = false;
		Game.game.Shake(0.15f, 0.15f, 30.0f);
	}

	void NextWave ()
	{
		curEnemies.Clear();
		waveCount++;
		Wave wave = waves[waveCount - 1];
		StartCoroutine(EnemySpawnLoop(wave));
		ui.StartCoroutine("NextWaveAnim");
	}

	//Порождает врагов на случайных позициях.
	IEnumerator EnemySpawnLoop (Wave wave)
	{
	
		if (!wave.spawnBoss)
		{

			for (int x = 0; x < wave.enemies.Length; x++)
			{
				yield return new WaitForSeconds(wave.spawnRates[x]);

				GameObject enemyToSpawn = enemySpawn.GetEnemyToSpawn(wave.enemies[x]);
				Vector3 randomOffset = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
				GameObject enemy = Instantiate(enemyToSpawn, spawnPoints[Random.Range(0, spawnPoints.Length)].position + randomOffset, Quaternion.identity);

				enemy.GetComponent<Enemy>().target = player.gameObject;
				curEnemies.Add(enemy);
			}
		}
		else
		{
			GameObject enemy = Instantiate(kingPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
			enemy.GetComponent<King>().target = player.gameObject;
			curEnemies.Add(enemy);
		}

		waveActive = true;
	}

	IEnumerator WaveEndTimer ()
	{
		if(!gameDone)
		{
			yield return new WaitForSeconds(2);

			
			if (waveCount == 4)
			{
				ui.upgradeText.gameObject.active = true;
				canUpgrade = true;
			}

			
			while (canUpgrade)
			{
				yield return null;
			}

			NextWave();
		}
	}

	public void EndGame ()
	{
		StartCoroutine(EndGameTimer());
	}

	IEnumerator EndGameTimer ()
	{
		yield return new WaitForSeconds(2);
		ui.endGameScreen.active = true;
		Game.game.Shake(0.1f, 0.1f, 50.0f);
	}


	public void WinGame ()
	{
		gameDone = true;
		player.GetComponent<CircleCollider2D>().enabled = false;
		player.rig.simulated = false;
		StartCoroutine(WinGameTimer());
	}


	IEnumerator WinGameTimer ()
	{
		gameDone = true;
		yield return new WaitForSeconds(2);
		ui.winScreen.active = true;
	}


	public void Shake (float duration, float amount, float intensity)
	{
		StartCoroutine(ShakeCam(duration, amount, intensity));
	}

	IEnumerator ShakeCam (float dur, float amount, float intensity)
	{
		float t = dur;
		Vector3 originalPos = cam.position;

		while (t > 0.0f)
		{
			Vector3 targetPos = originalPos + Random.insideUnitSphere * amount;
			cam.position = Vector3.Lerp(cam.position, targetPos, intensity * Time.deltaTime);
			t -= Time.deltaTime;
			yield return null;
		}

		cam.position = originalPos;
	}
}
