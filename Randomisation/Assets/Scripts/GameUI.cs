using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] CanvasGroup _titleButton;
    [SerializeField] GameObject _playButton;
    [SerializeField] GameObject _stopButton;
    
    public void Play()
    {
        //_titleButton.DOFade();
        _gameManager.Play();
    }
}