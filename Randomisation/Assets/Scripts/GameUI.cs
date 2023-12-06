using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] CanvasGroup _mainMenuGroup;
    [SerializeField] Button _playButton;
    [SerializeField] GameObject _seedButton;
    [SerializeField] TextMeshProUGUI _seedText;
    [SerializeField] TMP_InputField _seedInput;
    [SerializeField] GameObject _stopButton;
    [SerializeField] GameObject _currentKeyGO;
    [SerializeField] Image _currentKey;

    void OnEnable()
    {
        _gameManager.KeyChanged += OnKeyChanged;
    }

    void OnDisable()
    {
        _gameManager.KeyChanged -= OnKeyChanged;
    }

    void OnKeyChanged(Color newColor)
    {
        _currentKey.color = newColor;
    }
    
    public void Play()
    {
        _playButton.interactable = false;

        _mainMenuGroup.DOFade(0f, 1.5f)
            .OnComplete(() =>
            {
                var hasValue = int.TryParse(_seedInput.text, out var seed);
                var finalSeed = _gameManager.Play(hasValue ? seed : -1);
                _seedText.text = $"Seed : {finalSeed}";
                _gameManager.ShowChests();
                _seedButton.SetActive(true);
                _stopButton.SetActive(true);
                _currentKeyGO.SetActive(true);
            }
        );
    }

    public void Stop()
    {
        _playButton.interactable = true;
        
        _gameManager.HideChests();
        _seedButton.SetActive(false);
        _stopButton.SetActive(false);
        _currentKeyGO.SetActive(false);
        _mainMenuGroup.DOFade(1f, 0.1f);
    }

    public void CopySeed()
    {
        GUIUtility.systemCopyBuffer = _seedText.text[7..];
    }
}