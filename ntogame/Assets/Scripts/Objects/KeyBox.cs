using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBox : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private BoxCollider coll;
    [SerializeField] private AudioSource Source;
    [SerializeField] private AudioClip BoxOpenSound;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenTheBox()
    {
        anim.SetTrigger("isOpening");
        coll.enabled = false;
    }

    public void PlayOpenSound()
    {
        Source.clip = BoxOpenSound;
        Source.Play();
    }
}
