using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.XR.MagicLeap;

public class scr_room : MonoBehaviour
{
    public bool seen = false;
    public scr_MLSpatialMapper sm;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        sm = scr_MLSpatialMapper.instance;
        anim = transform.root.GetComponentInChildren<Animator>();
        //anim.SetBool("shut", true);
/*

        foreach (GameObject o in sm.meshList)
        {
            MeshRenderer mr = o.GetComponent<MeshRenderer>();
            mr.enabled = false;
        }*/
    }

    public void Update()
    {

    }

    public void OnSeen()
    {
        anim.SetBool("shut", false);
        //HideRealWorld();
        seen = true;
    }


    public void OnHidden()
    {
        anim.SetBool("shut", true);
        //ShowRealWorld();
        seen = false;
    }

    public void HideRealWorld()
    {

        //Debug.Log("MeshRenderer Hidden");
        foreach (GameObject o in sm.meshList)
        {
            MeshRenderer mr = o.GetComponent<MeshRenderer>();
            mr.enabled = false;
        }
    }

    public void ShowRealWorld()
    {
        //Debug.Log("MeshRenderer Show");
        foreach (GameObject o in sm.meshList)
        {
            MeshRenderer mr = o.GetComponent<MeshRenderer>();
            mr.enabled = true;
        }
    }
}
