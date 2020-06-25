using UnityEngine;
using UnityEngine.UI;
using SelfishCoder.Core;

[DisallowMultipleComponent]
public class UI_Manager : Singleton<UI_Manager>
{
    [SerializeField] private Text goldText;
    [SerializeField] private GameObject gameEndPanel;
    
    private void Start()
    {
        GoldManager.Instance.OnGoldChanged += UpdateGoldText;
        GameManager.Instance.OnGameEnd += OnGameEnded;
    }

    private void OnDestroy()
    {
        GoldManager.Instance.OnGoldChanged -= UpdateGoldText;
        GameManager.Instance.OnGameEnd -= OnGameEnded;
    }

    private void UpdateGoldText(int amount)
    {
        goldText.text = amount.ToString();
    }

    private void OnGameEnded()
    {
        gameEndPanel.gameObject.SetActive(true);
    }
}
