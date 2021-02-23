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
        if (!instance)
            instance = this;

        poolDict = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDict.Add(pool.tag, objectPool);
        }
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    poolDict = new Dictionary<string, Queue<GameObject>>();
    //    foreach (Pool pool in pools)
    //    {
    //        Queue<GameObject> objectPool = new Queue<GameObject>();
    //        for (int i = 0; i < pool.poolSize; i++)
    //        {
    //            GameObject obj = Instantiate(pool.prefab, transform);
    //            obj.SetActive(false);
    //            objectPool.Enqueue(obj);
    //        }
    //        poolDict.Add(pool.tag, objectPool);
    //    }
    //}

    public GameObject SpawnFromPool(string tag)//Vector3 position
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.LogError("Pool w tag " + tag + " doesnt exist");
            return null;
        }

        var objectToSpawn = poolDict[tag].Dequeue();
        objectToSpawn.SetActive(true);
        //objectToSpawn.transform.position = position;

        poolDict[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}
