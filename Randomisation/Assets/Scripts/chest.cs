using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chest : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    private void OnMouseDown()
    {
        animator.Play("open");
    }
}
