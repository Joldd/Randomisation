using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chest : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    public int chestToOpen = 0;

    private void Start()
    {

    }

    private void OnMouseDown()
    {
        _animator.Play("open");
    }
}
