using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBox : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private BoxCollider coll;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenTheBox()
    {
        anim.SetTrigger("isOpening");
        coll.enabled = false;
    }
}
