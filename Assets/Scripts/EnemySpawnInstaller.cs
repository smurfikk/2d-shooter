using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EnemySpawnInstaller : MonoInstaller
{
	//Enemy Prefabs
	[SerializeField] private GameObject knightPrefab;
	[SerializeField] private GameObject archerPrefab;
	[SerializeField] private GameObject magePrefab;

	public override void InstallBindings()
	{
		Container.Bind<EnemySpawn>().FromInstance(new EnemySpawn(knightPrefab, archerPrefab, magePrefab));
	}

}
