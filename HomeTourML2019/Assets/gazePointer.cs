 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gazePointer : MonoBehaviour
{

    private GameObject hitObj;
    public scr_MLSpatialMapper sm;
    private bool realWorldOpaque = true;
    private scr_room lastRoom = null;

    // Start is called before the first frame update
    void Start()
    {
        if (sm ==null)
            sm = scr_MLSpatialMapper.instance;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        int layerMask = 1 << 9;

        bool rayBlocked = Physics.Raycast(transform.position, (transform.rotation * Vector3.forward), out hit, Mathf.Infinity, layerMask);

        // Does the ray intersect any objects excluding the player layer
        if (realWorldOpaque && rayBlocked)//Physics.Raycast(transform.position, (transform.rotation * Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, (transform.rotation * Vector3.forward) * hit.distance, Color.yellow);

            lastRoom = hit.transform.GetComponentInChildren<scr_room>();

            lastRoom.OnSeen();

            hitObj = lastRoom.transform.root.gameObject;

            HideRealWorld();
            //Debug.Log("Did Hit");
        }
        else if (!rayBlocked)
        {
            Debug.DrawRay(transform.position, (transform.rotation * Vector3.forward) * 100f, Color.red);

            if (! realWorldOpaque)
            {
                ShowRealWorld();
                if (lastRoom != null)
                    lastRoom.OnHidden();
            }
        }

    }


    public void HideRealWorld()
    {
        realWorldOpaque = false;
        Debug.Log("MeshRenderer Hidden");

        foreach (GameObject o in sm.meshList)
        {
            MeshRenderer mr = o.GetComponent<MeshRenderer>();
            mr.enabled = false;
        }
    }

    public void ShowRealWorld()
    {
        realWorldOpaque = true;
        Debug.Log("MeshRenderer Shown");
        foreach (GameObject o in sm.meshList)
        {
            MeshRenderer mr = o.GetComponent<MeshRenderer>();
            mr.enabled = true;
        }
    }
}
