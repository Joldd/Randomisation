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

    [field: SerializeField] public int Id { get; private set; }
    
    public Chest ChestToOpen { get; set; }
    public Color ColorToOpen { get; set; }
    public bool IsLast { get; set; }
    public bool IsOpened { get; private set; }

    void OnMouseDown()
    {
        if (ChestToOpen != null && !ChestToOpen.IsOpened)
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
            GameObject myKey = Instantiate(key, transform);
            myKey.transform.position = transform.position;
            myKey.GetComponent<SpriteRenderer>().color = ColorToOpen;
        }
        locked.SetActive(false);
    }

    public void SetColor(Color color)
    {
        lockedSpriteRenderer.color = color;
    }

    public void Clear()
    {
        for (int i = 2; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        IsLast = false;
        IsOpened = false;
        ChestToOpen = null;
        ColorToOpen = Color.clear;
        _spriteRenderer.sprite = _closeSprite;
        locked.SetActive(true);
    }
}