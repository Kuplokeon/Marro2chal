using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    float baseLifetime;
    public float lifetime = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        baseLifetime = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            Destroy(gameObject);
        }
    }

    public void ResetTimer()
    {
        lifetime = baseLifetime;
    }
}
