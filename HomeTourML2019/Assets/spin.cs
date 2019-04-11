using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddTorque(new Vector3(0f, 10f, 0f));
        Destroy(gameObject, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
