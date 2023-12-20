using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] public Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Sprite _closeSprite;
    [SerializeField] key _key;
    [SerializeField] SpriteRenderer _sr;
    [SerializeField] GameObject treasure;
    [SerializeField] GameObject smoke;
    Color _color;
    public bool test = false;
    
    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            _sr.color = _color;
        }
    }

    public List<Color> KeysColors { get; set; } = new List<Color>();
    public List<key> Keys { get; set; } = new List<key>();
    public bool IsLast { get; set; }
    public bool IsOpened { get; set; }
    public bool OnRequiredPath { get; set; }

    void OnMouseDown()
    {
        test = false;
        if (IsOpened && Keys.Count == 0 && !IsLast)
        {
            _animator.Play("close");
            IsOpened = false;
        }
        foreach (var key in GameManager.Instance._keys)
        {
            if (key == Color)
            {
                test = true;
                break;
            }
        }
        if (!test)
            return;
        
        if (IsLast)
        {
            GameObject myTreasure = Instantiate(treasure, transform);
            myTreasure.transform.position = new Vector3(transform.position.x - 0.23f, transform.position.y + 1.04f, transform.position.z);
            _animator.Play("open");
            IsOpened = true;
            GameManager.Instance.changeColor(Color, new Color(0, 0, 0, 0));
        }
        else if (Keys.Count == 0 && !IsOpened)
        {
            GameObject mySmoke = Instantiate(smoke, transform);
            mySmoke.transform.position = new Vector3(transform.position.x - 0.23f, transform.position.y + 0.3f, transform.position.z);
            _animator.Play("open");
            IsOpened = true;
            GameManager.Instance.changeColor(Color, new Color(0, 0, 0, 0));
               
        }
        else
        {
            _animator.Play("open");
            IsOpened = true;
            GameManager.Instance.changeColor(Color, new Color(0, 0, 0, 0));
            foreach (var key in Keys)
            {
                key.gameObject.SetActive(true);
            }
        }
    }

    public void Setup()
    {
        float i = 0.2f;
        
        foreach (var keyColor in KeysColors)
        {
            key keyObj = Instantiate(_key, transform);
            keyObj.transform.position = new Vector3(transform.position.x + i, transform.position.y + 0.8f, transform.position.z);
            keyObj.GetComponent<SpriteRenderer>().color = keyColor;
            Keys.Add(keyObj);
            keyObj.gameObject.SetActive(false);
            i = -i;
        }
    }

    public void Clear()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        IsLast = false;
        IsOpened = false;
        OnRequiredPath = false;
        Color = Color.clear;
        KeysColors.Clear();
        Keys.Clear();
        _spriteRenderer.sprite = _closeSprite;
    }
}