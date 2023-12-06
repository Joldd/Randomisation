using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] Button _playButton;
    [SerializeField] CanvasGroup _playButtonGroup;
    [SerializeField] GameObject _stopButton;
    
    public void Play()
    {
        _playButton.interactable = false;
        
        DOTween.Sequence()
            .Append(_titleText.DOFade(0f, 1.5f))
            .Insert(0, _playButtonGroup.DOFade(0f, 1.5f))
            .OnComplete(() =>
            {
                _gameManager.Play();
                _gameManager.ShowChests();
                _stopButton.SetActive(true);
            });
    }

    public void Stop()
    {
        _playButton.interactable = true;
        
        _gameManager.HideChests();
        _stopButton.SetActive(false);
        _titleText.DOFade(1f, 0.1f);
        _playButtonGroup.DOFade(1f, 0.1f);
    }
}