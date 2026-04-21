using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text BlocksCountText;
    public TMP_Text PlayerAttemptsText;
    public VerticalLayoutGroup _textsGroup;

    [SerializeField] private RectTransform _blockingPanel;

    private int _playerAttempts;
    private FixedJoystick _joystick;

    private void Awake()
    {
        _joystick = FindFirstObjectByType<FixedJoystick>();

        PanelsManager.OnPanelActivated += DeactivateUIGroup;
        LoadingPanelsManager.OnLoadingPanelActivated += ActivateBlockingPanel;
        ButtonsManager.OnButtonPressed += ActivateBlockingPanel;

        GameManager.OnBlocksCountUpdated += UpdateBlocksCount;
        GameManager.OnPlayerAttemptsUpdated += UpdatePlayerAttepmts;
        PlayerAttemptsText.text = $"Attempts Left: {_playerAttempts}";
    }

    private void ActivateBlockingPanel()
    {
        _blockingPanel.gameObject.SetActive(true);
    }

    private void DeactivateUIGroup()
    {
        _joystick.gameObject.SetActive(false);
        _textsGroup.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.OnBlocksCountUpdated -= UpdateBlocksCount;
        GameManager.OnPlayerAttemptsUpdated -= UpdatePlayerAttepmts;
        PanelsManager.OnPanelActivated -= DeactivateUIGroup;
        LoadingPanelsManager.OnLoadingPanelActivated -= ActivateBlockingPanel;
        ButtonsManager.OnButtonPressed -= ActivateBlockingPanel;
    }

    private void UpdateBlocksCount(int value)
    {
        BlocksCountText.text = $"Blocks Left: {value}";
    }

    private void UpdatePlayerAttepmts(int value)
    {
        _playerAttempts = value;
        PlayerAttemptsText.text = $"Attempts Left: {_playerAttempts}";
    }
}
