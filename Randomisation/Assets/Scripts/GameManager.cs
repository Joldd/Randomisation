using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils.ExtensionMethods;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    static readonly List<Color> Colors = new List<Color> {
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
        new Color(0f/255f, 0f/255f, 0f/255f, 1),
    };
    
    public static GameManager Instance { get; private set; }

    [SerializeField] public HorizontalLayoutGroup _currentKeys;
    [SerializeField] private Image keyImage;
    public List<Image> _currentKeysImages  = new List<Image>();

    [SerializeField] public HorizontalLayoutGroup _btnsKeys;
    [SerializeField] public Button btnKey;

    [SerializeField] List<Chest> _chests;
    [SerializeField] GameObject _container;

    public List<Color> _keys = new List<Color>();

    List<List<Chest>> _layout;

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

    void Awake()
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

    void OnDrawGizmosSelected()
    {
        if (_layout == null)
            return;
        
        for (var i = 0; i < _layout.Count; i++)
        {
            for (var j = 0; j < _layout[i].Count; j++)
            {
                var position = new Vector3(2 * j, -2 * i);
                Gizmos.DrawCube(position, Vector3.one);
                Gizmos.DrawSphere(position + Vector3.down / 2, 0.125f);
                Gizmos.DrawCube(position + Vector3.up / 2, new Vector3(0.25f, 0.25f));

                for (var k = 0; k < _layout[i][j].Keys.Count; k++)
                {
                    var targetColor = _layout[i][j].Keys[k];
                    var targetChest = _chests.Single(chest => chest.Color == targetColor);
                    var columnIndex = _layout.FindIndex(row => row.Contains(targetChest));
                    var rowIndex = _layout[columnIndex].IndexOf(targetChest);

                    if (k >= 1)
                        Gizmos.color = Color.red;

                    Gizmos.DrawLine(position + new Vector3(0.1f * k, -0.5f), new Vector3(2 * rowIndex, -2 * columnIndex) + Vector3.up / 2);
                    Gizmos.color = Color.white;
                }
            }
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
        
        seed = Init(seed);

        AssignColors();
        CreateLayout();
        DefineRequiredPath();
        AddRandomEdges();
        AddShortcutEdges();
        
        return seed;

        void AssignColors()
        {
            var colors = new List<Color>(Colors);
            
            foreach (var chest in _chests)
            {
                int randomIndex = Random.Range(0, colors.Count - 1);
                Color color = colors[randomIndex];
                colors.RemoveAt(randomIndex);
                chest.Color = color;
            }    
        }

        void CreateLayout()
        {
            _layout = new List<List<Chest>>
            {
                new List<Chest> { _chests[0], _chests[1], _chests[2] },
                new List<Chest> { _chests[3], _chests[4], _chests[5], _chests[6] },
                new List<Chest> { _chests[7], _chests[8] },
                new List<Chest> { _chests[9] },
            };
        }

        void DefineRequiredPath()
        {
            var chests = new Chest[_layout.Count];

            for (var i = 0; i < _layout.Count; i++)
            {
                var randomChest = _layout[i].GetRandom();
                chests[i] = randomChest;
                randomChest.OnRequiredPath = true;
            }

            for (var i = 0; i < chests.Length - 1; i++)
            {
                chests[i].Keys.Add(chests[i + 1].Color);
            }
        }

        void AddRandomEdges()
        {
            for (var i = 0; i < _layout.Count - 1; i++)
            {
                for (var j = 0; j < _layout[i].Count; j++)
                {
                    // Don't add more keys to chest already on required path
                    if (_layout[i][j].OnRequiredPath)
                        continue;

                    // 5% chance no key inside this chest
                    if (Random.value < 0.05f)
                        continue;

                    var possibleTargets = _layout[i + 1].Where(chest => !chest.OnRequiredPath).ToList();
                    
                    if (possibleTargets.Count == 0)
                        continue;

                    var target = possibleTargets.GetRandom();
                    
                    _layout[i][j].Keys.Add(target.Color);
                }
            }
        }

        void AddShortcutEdges()
        {
            for (var i = 0; i < _layout.Count - 1; i++)
            {
                for (var j = 0; j < _layout[i].Count; j++)
                {
                    // 50% chance of double key
                    if (_layout[i][j].Keys.Count != 0 && Random.value >= 0.5f)
                    {
                        var possibleTargets = _layout.Skip(Math.Min(i + 2, _layout.Count - 1))
                            .SelectMany(row => row)
                            .Where(chest => !_layout[i][j].Keys.Contains(chest.Color))
                            .ToList();
                        
                        if (possibleTargets.Count == 0)
                            continue;
                        
                        var target = possibleTargets.GetRandom();
                        _layout[i][j].Keys.Add(target.Color);
                    }
                }
            }
        }
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
        _layout = null;
        
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