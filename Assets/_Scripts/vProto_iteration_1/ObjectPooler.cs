using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance { get; private set; }



    Dictionary<string, Queue<GameObject>> poolDict;
    public List<Pool> pools;

    private void Awake()
    {

        instance = this;

        instance.poolDict = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in instance.pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab, instance.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            instance.poolDict.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag) //Vector3 position
    {
        if (!instance.poolDict.ContainsKey(tag))
        {
            Debug.LogError("Pool w tag " + tag + " doesnt exist");
            return null;
        }

        var objectToSpawn = instance.poolDict[tag].Dequeue();
        objectToSpawn.SetActive(true);
        //objectToSpawn.transform.position = position;

        instance.poolDict[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}
