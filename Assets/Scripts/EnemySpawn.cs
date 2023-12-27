using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn
{
	//Enemy Prefabs
	private GameObject knightPrefab;
	private GameObject archerPrefab;
	private GameObject magePrefab;

	public EnemySpawn(GameObject knightPrefab, GameObject archerPrefab, GameObject magePrefab)
    {
        this.knightPrefab = knightPrefab;
        this.archerPrefab = archerPrefab;
        this.magePrefab = magePrefab;
    }

    public GameObject GetEnemyToSpawn(EnemyType e)
	{
		switch (e)
		{
			case EnemyType.Knight:
				return knightPrefab;
				break;
			case EnemyType.Archer:
				return archerPrefab;
				break;
			case EnemyType.Mage:
				return magePrefab;
				break;
		}

		return null;
	}
}
