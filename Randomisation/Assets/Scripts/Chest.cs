using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] public GameObject key;
    [SerializeField] public GameObject locked;
    [SerializeField] public GameObject treasure;

    [field: SerializeField] public int Id { get; private set; }
    
    public Chest ChestToOpen { get; set; }
    public Color colorToOpen { get; set; }
    public bool IsOpened { get; private set; }

    public bool isLast { get; set; }

    private void Start()
    {

    }

    private void OnMouseDown()
    {
        if (ChestToOpen != null && !ChestToOpen.IsOpened)
            return;
        
        IsOpened = true;
        _animator.Play("open");
        if (isLast)
        {
            GameObject myTreasure = Instantiate<GameObject>(treasure);
            myTreasure.transform.position = transform.position;
        }
        else
        {
            GameObject myKey = Instantiate<GameObject>(key);
            myKey.transform.position = transform.position;
            myKey.GetComponent<SpriteRenderer>().color = colorToOpen;
        }
        locked.SetActive(false);
    }
}