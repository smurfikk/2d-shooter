using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    public GameObject[] environmentPrefabs;  // массив префабов окружения
    public int spawnDistance = 20;  // расстояние, на котором будет спавниться окружение
    public int spawnCount = 10;  // количество объектов окружения, которые будут спавниться
    public float spawnRadius = 10f;  // радиус спавна окружения

    private Transform playerTransform;  // ссылка на трансформ игрока
    private Vector3 previousSpawnPosition;  // предыдущая позиция спавна окружения

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // находим игрока по тегу
        previousSpawnPosition = playerTransform.position;  // сохраняем начальную позицию спавна окружения
        SpawnEnvironment();  // вызываем метод спавна окружения
    }

    void SpawnEnvironment()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomDirection = playerTransform.forward * spawnDistance;  // направление относительно игрока
            randomDirection += playerTransform.position;  // смещаем направление относительно позиции игрока
            randomDirection += Random.insideUnitSphere * spawnRadius;  // добавляем случайное смещение
            randomDirection.y = 0f;  // устанавливаем Y-координату на нуль, чтобы объекты не спавнились выше уровня земли

            int randomIndex = Random.Range(0, environmentPrefabs.Length);  // выбираем случайный префаб окружения
            Instantiate(environmentPrefabs[randomIndex], randomDirection, Quaternion.identity);  // спавним объект окружения в случайном направлении
        }

        previousSpawnPosition = playerTransform.position;  // сохраняем позицию последнего спавна окружения
    }

    void Update()
    {
        if (Vector3.Distance(previousSpawnPosition, playerTransform.position) > spawnDistance)
        {
            SpawnEnvironment();  // если игрок покинул пределы последнего спавна, вызываем метод спавна окружения
        }
    }
}