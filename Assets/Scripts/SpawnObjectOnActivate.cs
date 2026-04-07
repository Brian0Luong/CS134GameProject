using UnityEngine;

public class SpawnObjectOnActivate : MonoBehaviour, IButtonActivated
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool spawnOnlyOnce = true;
    [Header("Sound")]
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private float volume = 1f;

    private GameObject spawnedObject;
    private bool hasSpawned;

    public void Activate()
    {
        if (spawnOnlyOnce && hasSpawned)
            return;

        if (objectToSpawn == null || spawnPoint == null)
            return;

        if (spawnSound != null)
        {
            AudioSource.PlayClipAtPoint(
                spawnSound,
                spawnPoint.position,
                volume
            );
        }

        spawnedObject = Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
        hasSpawned = true;
    }
}