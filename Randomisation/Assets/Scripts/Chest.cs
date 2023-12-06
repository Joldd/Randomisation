using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            myTreasure.transform.position = new Vector3(transform.position.x - 0.23f, transform.position.y + 1.04f, transform.position.z); ;
        }
        else
        {
            if (Keys.Count >= 2)
            {
                GameManager.Instance.Key = new Color(0,0,0,0);
                List<Button> l_buttons = new List<Button>();
                float i = 0.2f;
                foreach (var keyClr in Keys)
                {
                    Button btn = Instantiate(GameManager.Instance.btnKey, GameManager.Instance._btnsKeys.transform);
                    l_buttons.Add(btn);
                    btn.GetComponent<Image>().color = keyClr;
                    btn.onClick.AddListener(() => {
                        GameManager.Instance.Key = keyClr;
                        foreach (var btn in l_buttons)
                        {
                            Destroy(btn.gameObject);
                        }
                    });
                    GameObject keyObj = Instantiate(key, transform);
                    keyObj.transform.position = new Vector3(transform.position.x + i, transform.position.y + 0.8f, transform.position.z);
                    keyObj.GetComponent<SpriteRenderer>().color = keyClr;
                    i = -i;
                }
            }
            else
            {
                GameManager.Instance.Key = Keys[0];
                GameObject myKey = Instantiate(key, transform);
                myKey.transform.position = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z); ;
                myKey.GetComponent<SpriteRenderer>().color = Keys[0];
            }
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