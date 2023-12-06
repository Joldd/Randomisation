using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.ExtensionMethods;

public class GameManager : MonoBehaviour
{
    [SerializeField] int _seed = -1;
    [SerializeField] List<Chest> _chests;
    [SerializeField] GameObject _container;

    public void ShowChests()
    {
        _container.SetActive(true);
    }
    
    public void HideChests()
    {
        _container.SetActive(false);
    }
    
    public void Play()
    {
        var seed = Init(_seed);

        Debug.Log(seed);
        
        var chestIds = Enumerable.Range(0, 10).OrderBy(_ => Random.value).ToList();

        Debug.Log(chestIds.ToDisplayString());

        for (var i = chestIds.Count - 1; i >= 0; i--)
        {
            var chest = _chests.Single(chest => chest.Id == chestIds[i]);
            Chest prevChest = null;
            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);

            if (i == chestIds.Count - 1)
                chest.isLast = true;
            else
                chest.isLast = false;

            if (i >= 1)
            {
                prevChest = _chests.Single(chest => chest.Id == chestIds[i - 1]);
                prevChest.colorToOpen = color;
            }
                
            chest.ChestToOpen = prevChest;
            chest.locked.GetComponent<SpriteRenderer>().color = color;
        }
    }

    int Init(int seed = -1)
    {
        // Generates a random seed if not given
        if (seed == -1)
            seed = System.Environment.TickCount;
        
        Random.InitState(seed);
        return seed;
    }
}