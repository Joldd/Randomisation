using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    Chest chest;
    public Color color;

    private void Start()
    {
        chest = transform.parent.GetComponent<Chest>(); 
        color = GetComponent<SpriteRenderer>().color;
    }

    private void OnMouseDown()
    {
        GameManager.Instance.changeColor(new Color(0, 0, 0, 0), color);  
        chest.Keys.Remove(this);
        Destroy(gameObject);
        chest.test = false;
        chest.IsOpened = false;
        foreach (var k in chest.Keys)
        {
            k.gameObject.SetActive(false);
        }
        if (chest.Keys.Count != 0)
        {
            chest._animator.Play("close");
        }
    }
}
