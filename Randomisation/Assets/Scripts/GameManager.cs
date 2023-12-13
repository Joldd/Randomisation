using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] public HorizontalLayoutGroup _currentKeys;
    [SerializeField] private Image keyImage;
    public List<Image> _currentKeysImages  = new List<Image>();

    [SerializeField] public HorizontalLayoutGroup _btnsKeys;
    [SerializeField] public Button btnKey;

    [SerializeField] List<Chest> _chests;
    [SerializeField] GameObject _container;
    List<Color> _colors;

    public List<Color> _keys = new List<Color>();
    
    //Color _key;
    //public Color Key
    //{
    //    get => _key;
    //    set
    //    {
    //        _key = value;
    //        KeyChanged?.Invoke(_key);
    //    } 
    //}

    public Action<Color> KeyChanged { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void ShowChests()
    {
        _container.SetActive(true);
    }
    
    public void HideChests()
    {
        _container.SetActive(false);
    }
    
    public int Play(int seed)
    {
        ResetAllChests();

        _colors = new List<Color>() {
            new Color(230f/255f, 25f/255f, 75f/255f, 1),
            new Color(60f/255f, 180f/255f, 75f/255f, 1),
            new Color(255f/255f, 225f/255f, 25f/255f, 1),
            new Color(0f/255f, 130f/255f, 200f/255f, 1),
            new Color(245f/255f, 130f/255f, 48f/255f, 1),
            new Color(70f/255f, 240f/255f, 240f/255f, 1),
            new Color(240f/255f, 50f/255f, 230f/255f, 1),
            new Color(250f/255f, 190f/255f, 212f/255f, 1),
            new Color(0f/255f, 128f/255f, 128f/255f, 1),
            new Color(220f/255f, 190f/255f, 255f/255f, 1),
            new Color(170f/255f, 110f/255f, 40f/255f, 1),
            new Color(255f/255f, 250f/255f, 200f/255f, 1),
            new Color(128f/255f, 0f/255f, 0f/255f, 1),
            new Color(170f/255f, 255f/255f, 195f/255f, 1),
            new Color(0f/255f, 0f/255f, 128f/255f, 1),
            new Color(128f/255f, 128f/255f, 128f/255f, 1),
            new Color(255f/255f, 255f/255f, 255f/255f, 1),
            new Color(0f/255f, 0f/255f, 0f/255f, 1)
        };

        var finalSeed = Init(seed);

        _chests = _chests.OrderBy(_ => Random.value).ToList();

        foreach (var chest in _chests)
        {
            int randomIndex = Random.Range(0, _colors.Count - 1);
            Color color = _colors[randomIndex];
            _colors.RemoveAt(randomIndex);
            chest.Color = color;
        }
        
        for (var i = 0; i < _chests.Count; i++)
        {
            var chest = _chests.Single(chest => chest == _chests[i]);
            
            // Première clé
            if (i != _chests.Count - 1)
            {
                chest.Keys.Add(_chests[i + 1].Color);
            }
            
            // % de chance d'une 2eme clé
            if (Random.value >= 0.5)
            {
                var futureChests = _chests.Skip(i).ToList();
                chest.Keys.Add(futureChests[Random.Range(0, futureChests.Count)].Color);
            }
        }

        _chests.Last().IsLast = true;

        // Ajout de 3 clés
        for (int i = 0; i < 3; i++)
        {
            _keys.Add(_chests[Random.Range(0, _chests.Count - 1)].Color);
            Image img = Instantiate(keyImage, _currentKeys.transform);
            img.color = _keys[i];
            _currentKeysImages.Add(img);
        }

        return finalSeed;
    }

    int Init(int seed = -1)
    {
        // Generates a random seed if not given
        if (seed == -1)
            seed = Environment.TickCount;
        
        Random.InitState(seed);
        return seed;
    }

    void ResetAllChests()
    {
        foreach (var chest in _chests)
        {
            chest.Clear();
        }
    }

    public void changeColor(Color key, Color newColor)
    {
        var i = _keys.IndexOf(key);
        _keys[i] = newColor;
        foreach (var keyImage in _currentKeysImages)
        {
            Color color = keyImage.color;
            if (color == key)
            {
                keyImage.color = newColor;
                break;
            }   
        }
    }
}