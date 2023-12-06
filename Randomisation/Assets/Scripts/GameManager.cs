using System.Linq;
using UnityEngine;
using Utils.ExtensionMethods;

public class GameManager : MonoBehaviour
{
    [SerializeField] int _seed = -1;

    [ContextMenu("Play")]
    public void Play()
    {
        var seed = Init(_seed == -1 ? null : _seed);

        Debug.Log(seed);
        
        var chests = Enumerable.Range(1, 10).OrderBy(_ => Random.value);

        Debug.Log(chests.ToDisplayString());
    }

    int Init(int? seed = null)
    {
        // Generates a random seed if not given
        if (seed == null)
            seed = System.Environment.TickCount;
        
        Random.InitState(seed.Value);
        return seed.Value;
    }
}