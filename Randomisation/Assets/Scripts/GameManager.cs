using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils.ExtensionMethods;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    [SerializeField] List<Chest> _chests;
    [SerializeField] GameObject _container;

    public List<Color> _keys = new List<Color>();

    List<List<Chest>> _layout;

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

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (_layout == null)
            return;
        
        for (var i = 0; i < _layout.Count; i++)
        {
            for (var j = 0; j < _layout[i].Count; j++)
            {
                var position = new Vector3(2 * j, -2 * i);
                Gizmos.color = _layout[i][j].Color;
                Gizmos.DrawCube(position, Vector3.one);
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(position + Vector3.down / 2, 0.125f);
                Gizmos.DrawCube(position + Vector3.up / 2, new Vector3(0.25f, 0.25f));

                for (var k = 0; k < _layout[i][j].KeysColors.Count; k++)
                {
                    var targetColor = _layout[i][j].KeysColors[k];
                    var targetChest = _chests.Single(chest => chest.Color == targetColor);
                    var columnIndex = _layout.FindIndex(row => row.Contains(targetChest));
                    var rowIndex = _layout[columnIndex].IndexOf(targetChest);

                    if (k >= 1)
                    {
                        var dest = new Vector3(2 * rowIndex, -2 * columnIndex) + Vector3.up / 2;
                        var halfHeight = (position.y - dest.y) * 0.5f;
                        var heightOffset = Vector3.up * halfHeight;
                        var widthOffset = Mathf.Approximately(position.x, dest.x) ? Vector3.right : Vector3.zero;

                        Handles.DrawBezier(position + Vector3.down / 2, dest, position - heightOffset - widthOffset,
                            dest + heightOffset - widthOffset, Color.red, EditorGUIUtility.whiteTexture, 1f);   
                    }
                    else
                    {
                        Gizmos.DrawLine(position + Vector3.down / 2, new Vector3(2 * rowIndex, -2 * columnIndex) + Vector3.up / 2);
                    }

                    Gizmos.color = Color.white;
                }
            }
        }
    }
#endif

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
        ResetKeys();
        
        seed = Init(seed);

        AssignColors();
        CreateLayout();
        DefineRequiredPath();
        AddRandomEdges();
        AddShortcutEdges();
        AddInitialKeys();
        SetupChests();
        
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
            var shuffledChests = new List<Chest>(_chests).OrderBy(_ => Random.value).ToList();

            var layoutIndex = Random.Range(0, 3);

            _layout = layoutIndex switch
            {
                0 => new List<List<Chest>>
                {
                    new List<Chest> { shuffledChests[0], shuffledChests[1], shuffledChests[2] },
                    new List<Chest> { shuffledChests[3], shuffledChests[4], shuffledChests[5], shuffledChests[6] },
                    new List<Chest> { shuffledChests[7], shuffledChests[8] },
                    new List<Chest> { shuffledChests[9] },
                },
                1 => new List<List<Chest>>
                {
                    new List<Chest> { shuffledChests[0], shuffledChests[1], shuffledChests[2] },
                    new List<Chest> { shuffledChests[3], shuffledChests[4], shuffledChests[5] },
                    new List<Chest> { shuffledChests[6], shuffledChests[7], shuffledChests[8] },
                    new List<Chest> { shuffledChests[9] },
                },
                2 => new List<List<Chest>>
                {
                    new List<Chest> { shuffledChests[0], shuffledChests[1], shuffledChests[2] },
                    new List<Chest> { shuffledChests[3], shuffledChests[4] },
                    new List<Chest> { shuffledChests[5], shuffledChests[6] },
                    new List<Chest> { shuffledChests[7], shuffledChests[8] },
                    new List<Chest> { shuffledChests[9] },
                },
                _ => throw new ArgumentOutOfRangeException(),
            };

            shuffledChests[9].IsLast = true;
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
                chests[i].KeysColors.Add(chests[i + 1].Color);
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
                    
                    _layout[i][j].KeysColors.Add(target.Color);
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
                    if (_layout[i][j].KeysColors.Count != 0 && Random.value >= 0.5f)
                    {
                        var possibleTargets = _layout.Skip(Math.Min(i + 2, _layout.Count - 1))
                            .SelectMany(row => row)
                            .Where(chest => !_layout[i][j].KeysColors.Contains(chest.Color))
                            .ToList();
                        
                        if (possibleTargets.Count == 0)
                            continue;
                        
                        var target = possibleTargets.GetRandom();
                        _layout[i][j].KeysColors.Add(target.Color);
                    }
                }
            }
        }

        void AddInitialKeys()
        {
            foreach (var k in _layout[0])
            {
                _keys.Add(k.Color);
                Image img = Instantiate(keyImage, _currentKeys.transform);
                img.color = k.Color;
                _currentKeysImages.Add(img);
            }
        }

        void SetupChests()
        {
            foreach (var chest in _chests)
            {
                chest.Setup();
            }
        }
    }

    int Init(int seed = -1)
    {
        // Generates a random seed if not given
        if (seed == -1)
            seed = Math.Abs(Environment.TickCount);
        
        Random.InitState(seed);
        return seed;
    }
    
    public List<List<Color>> GetPaths()
    {
        var paths = new List<List<Color>>();
        var queue = new Queue<List<Color>>(_layout[0].Select(chest => new List<Color> { chest.Color }));
        var lastChest = _layout.Last()[0].Color;
        
        while (queue.Count > 0)
        {
            var path = queue.Dequeue();

            var node = path[^1];

            if (node == lastChest)
            {
                paths.Add(path);
                continue;
            }

            var neighbors = _chests.First(chest => chest.Color == node).KeysColors;

            foreach (var neighbor in neighbors)
            {
                queue.Enqueue(new List<Color>(path) { neighbor });
            }
        }
        
        return paths;
    }

    void ResetAllChests()
    {
        _layout = null;
        
        foreach (var chest in _chests)
        {
            chest.Clear();
        }
    }

    void ResetKeys()
    {
        _keys.Clear();
        _currentKeysImages.Clear();
        
        foreach (Transform key in _currentKeys.transform)
        {
            Destroy(key.gameObject);
        }
    }

    public void changeColor(Color key, Color newColor)
    {
        var i = _keys.IndexOf(key);
        if (i == -1)
            return;
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