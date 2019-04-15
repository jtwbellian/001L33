using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_anim : MonoBehaviour
{
    [SerializeField]
    private string stateName = "NONE";

    // Start is called before the first frame update
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        anim.Play(stateName, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
