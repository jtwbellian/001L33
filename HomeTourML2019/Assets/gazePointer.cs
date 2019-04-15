using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gazePointer : MonoBehaviour
{

    private GameObject hitObj;
    public MLSpatialMapper sm;
    private bool realWorldOpaque = true;

    // Start is called before the first frame update
    void Start()
    {
        if (sm ==null)
            sm = MLSpatialMapper.instance;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        int layerMask = 1 << 9;

        // Does the ray intersect any objects excluding the player layer
        if (realWorldOpaque && Physics.Raycast(transform.position, (transform.rotation * Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, (transform.rotation * Vector3.forward) * hit.distance, Color.yellow);

            scr_room room = hit.transform.GetComponentInChildren<scr_room>();

            hitObj = room.transform.root.gameObject;

            HideRealWorld();
            //Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, (transform.rotation * Vector3.forward) * 100f, Color.red);
            if (! realWorldOpaque)
            {
                ShowRealWorld();
            }
        }

    }
    

    public void OnSeen()
    {
        //anim.SetBool("shut", false);
        HideRealWorld();
    }


    public void OnHidden()
    {
        //anim.SetBool("shut", true);
        ShowRealWorld();
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
