using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Any prefab object that you want to spawn (player or enemy in our game)
    public GameObject prefabToSpawn;

    // Used to spawn multiple copies at a set interval (primarily for enemy objects)
    public float repeatInterval = 25.0f;

    // Start is called before the first frame update
    void Start()
    {
        // If the object should be repeatedly spawned
        if (repeatInterval > 0)
        {
            // Call the "SpawnObject" method repeatedly
            // Wait 0.0 time before calling the first time
            // repeatInterval is how often to call the method
            InvokeRepeating("SpawnObject", 0.0f, repeatInterval);
        }
    }

    // Used to spawn a new game object
    public GameObject SpawnObject()
    {
        if (prefabToSpawn != null)
        {
            // Instantiate the prefab at the location of the current SpawnPoint object
            // Quaternion is a data structure used to represent rotations; identity = no rotation
            return Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }

        return null;
    }
}
