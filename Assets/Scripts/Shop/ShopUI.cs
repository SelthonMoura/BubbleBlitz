using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameEventListener<CustomEvent> _updateUI;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private TMP_Text _playerScore;
    [SerializeField] private GameObject _settingsPanel;
    public GameObject shopPanel;

    private void Start()
    {
        _updateUI.AddListener(UpdatePlayerScoreText);
        UpdatePlayerScoreText();
    }

    private void OnDestroy()
    {
        _updateUI.RemoveListener(UpdatePlayerScoreText);
    }

    public void CloseShop()
    {
        Time.timeScale = 1.0f;
        shopPanel.SetActive(false);
    }

    private void UpdatePlayerScoreText()
    {
        _playerScore.text = _playerStats.score.ToString();
    }

    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        _settingsPanel.SetActive(true);
    }
}
