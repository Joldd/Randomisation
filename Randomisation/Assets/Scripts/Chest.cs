using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Sprite _closeSprite;
    [SerializeField] GameObject key;
    [SerializeField] GameObject locked;
    [SerializeField] SpriteRenderer lockedSpriteRenderer;
    [SerializeField] GameObject treasure;
    Color _color;

    [field: SerializeField] public int Id { get; private set; }

    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            lockedSpriteRenderer.color = _color;
        }
    }

    public List<Color> Keys { get; set; } = new List<Color>();
    public bool IsLast { get; set; }
    public bool IsOpened { get; private set; }

    void OnMouseDown()
    {
        if (GameManager.Instance.Key != Color)
            return;
        
        IsOpened = true;
        _animator.Play("open");
        
        if (IsLast)
        {
            GameObject myTreasure = Instantiate(treasure, transform);
            myTreasure.transform.position = transform.position;
        }
        else
        {
            GameManager.Instance.Key = Keys[0];
            
            GameObject myKey = Instantiate(key, transform);
            myKey.transform.position = transform.position;
            myKey.GetComponent<SpriteRenderer>().color = Keys[0];
        }
        
        locked.SetActive(false);
    }

    public void Clear()
    {
        for (int i = 2; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        IsLast = false;
        IsOpened = false;
        Color = Color.clear;
        Keys.Clear();
        _spriteRenderer.sprite = _closeSprite;
        locked.SetActive(true);
    }
}