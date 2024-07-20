using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    public GameObject battery;
    [SerializeField] private float spawnRate;
    private float timer;
    int numBattery;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            spawnBattery();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            spawnBattery();
            timer = 0;
        }
    }
    void spawnBattery()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-50, 50), (float)0.3, Random.Range(-50, 50));
        Instantiate(battery, randomSpawnPosition, Quaternion.identity);
    }
}
