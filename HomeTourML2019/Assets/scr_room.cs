using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.XR.MagicLeap;

public class scr_room : MonoBehaviour
{

   public MLSpatialMapper sm;

    // Start is called before the first frame update
    void Start()
    {
        sm = MLSpatialMapper.instance;

        foreach (GameObject o in sm.meshList)
        {
            o.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
