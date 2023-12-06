using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils.ExtensionMethods;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] List<Chest> _chests;
    [SerializeField] GameObject _container;
    List<Color> _colors;

    [SerializeField] public Image currentKey;

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

        Debug.Log(finalSeed);
        
        var chestIds = Enumerable.Range(0, 10).OrderBy(_ => Random.value).ToList();

        Debug.Log(chestIds.ToDisplayString());

        for (var i = chestIds.Count - 1; i >= 0; i--)
        {
            var chest = _chests.Single(chest => chest.Id == chestIds[i]);
            Chest prevChest = null;
            int randomIndex = Random.Range(0, _colors.Count - 1);
            Color color = _colors[randomIndex];
            _colors.RemoveAt(randomIndex);

            if (i == chestIds.Count - 1)
                chest.IsLast = true;
            else
                chest.IsLast = false;

            if (i >= 1)
            {
                prevChest = _chests.Single(chest => chest.Id == chestIds[i - 1]);
                prevChest.ColorToOpen = color;
            }
                
            chest.ChestToOpen = prevChest;
            chest.SetColor(color);

            if (i == 0)
            {
                currentKey.color = color;
            }
        }
        
        return finalSeed;
    }

    int Init(int seed = -1)
    {
        // Generates a random seed if not given
        if (seed == -1)
            seed = System.Environment.TickCount;
        
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
}