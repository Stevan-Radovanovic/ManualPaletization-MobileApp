using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spherecl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
        {
            Spawn();
        }

    }

    private void Spawn()
    {
        float randomx = UnityEngine.Random.Range(-10, 10);
        float randomy = UnityEngine.Random.Range(10, 20);

        transform.position = new Vector3(randomx, randomy);
        var rigidBody = transform.GetComponent<Rigidbody>();
        rigidBody.velocity = Vector3.zero;
    }
}
