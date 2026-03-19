using UnityEngine;

public class SpawnObjectOnActivate : MonoBehaviour, IButtonActivated
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool spawnOnlyOnce = true;

    private GameObject spawnedObject;
    private bool hasSpawned;

    public void Activate()
    {
        if (spawnOnlyOnce && hasSpawned)
            return;

        if (objectToSpawn == null || spawnPoint == null)
            return;

        spawnedObject = Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
        hasSpawned = true;
    }
}