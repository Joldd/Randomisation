using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
using static log4net.Appender.ColoredConsoleAppender;

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
    [SerializeField] GameObject _solutionButton;
    [SerializeField] GameObject panelSolutions;
    [SerializeField] VerticalLayoutGroup solutionsGroup;
    [SerializeField] HorizontalLayoutGroup solutionGroup;
    [SerializeField] Image chestImg;
    [SerializeField] Image arrowImg;

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
                _solutionButton.SetActive(true);
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

    public void Solution()
    {
        panelSolutions.SetActive(true);
        List<List<Color>> L_Solutions = GameManager.Instance.GetPaths();
        foreach (List<Color> solution in L_Solutions)
        {
            HorizontalLayoutGroup sGroup = Instantiate(solutionGroup, solutionsGroup.transform);
            foreach(Color color in solution)
            {
                Image cImg = Instantiate(chestImg, sGroup.transform);
                cImg.color = color;
                if (solution.IndexOf(color) != (solution.Count - 1))
                {
                    Image aImg = Instantiate(arrowImg, sGroup.transform);
                }
            }
        }
    }

    public void closeSolutions()
    {
        panelSolutions.SetActive(false);
        foreach(Transform solution in solutionsGroup.transform)
        {
            Destroy(solution.gameObject);
        }
    }

}