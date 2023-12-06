using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.ExtensionMethods;

public class GameManager : MonoBehaviour
{
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
    
    public int Play(int seed)
    {
        var finalSeed = Init(seed);

        Debug.Log(finalSeed);
        
        var chestIds = Enumerable.Range(0, 10).OrderBy(_ => Random.value).ToList();

        Debug.Log(chestIds.ToDisplayString());

        for (var i = chestIds.Count - 1; i >= 0; i--)
        {
            var chest = _chests.Single(chest => chest.Id == chestIds[i]);
            Chest prevChest = null;
            
            if (i >= 1)
                prevChest = _chests.Single(chest => chest.Id == chestIds[i - 1]);

            chest.ChestToOpen = prevChest;
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
}