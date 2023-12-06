using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] Animator _animator;

    [field: SerializeField] public int Id { get; private set; }
    
    public Chest ChestToOpen { get; set; }
    public bool IsOpened { get; private set; }
    
    private void Start()
    {

    }

    private void OnMouseDown()
    {
        if (ChestToOpen != null && !ChestToOpen.IsOpened)
            return;
        
        IsOpened = true;
        _animator.Play("open");
    }
}