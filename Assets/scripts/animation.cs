using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation : MonoBehaviour
{
    public Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0))
        {
            animator.SetBool("condiiton", false);
        }

        if (Input.GetMouseButton(0))
        {
            animator.SetBool("condiiton", false);
        }

        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("condiiton", true);
        }
    }
}
