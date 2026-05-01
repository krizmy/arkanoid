using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class LoadingPanelsManager : MonoBehaviour
{
    public static event Action OnLoadingPanelActivated;
    public static float AnimationTime;

    [SerializeField] private RectTransform _leftPart;
    [SerializeField] private RectTransform _rightPart;
    [SerializeField] private RectTransform _topPart;
    [SerializeField] private RectTransform _bottomPart;
    [SerializeField] private RectTransform _topLeftPart;
    [SerializeField] private RectTransform _bottomRightPart;

    private float _animationTime = 2;
    private AnimationType _type;

    private enum AnimationType
    {
        Width,
        Height,
        Diagonal
    }

    private void Awake()
    {
        AnimationTime = _animationTime;
        ButtonsManager.OnButtonPressed += ChooseType;
    }

    private void SetType()
    {
        switch (_type)
        {
            case AnimationType.Width:
                AnimateWidthPanels();
                break;
            case AnimationType.Height:
                AnimateHeightPanels();
                break;
            case AnimationType.Diagonal:
                AnimateDiagonalPanels();
                break;
        }
    }

    private void ChooseType()
    {
        AnimationType[] type = (AnimationType[])Enum.GetValues(typeof(AnimationType));
        _type = type[Random.Range(0, type.Length)];
        SetType();
    }

    private void AnimateWidthPanels()
    {
        OnLoadingPanelActivated?.Invoke();
        Vector3 endPositionLeftPart = new Vector3(519, 0);
        Vector3 endPositionRightPart = new Vector3(-519, 0);
        Sequence seq = DOTween.Sequence();
        seq.Append(_leftPart.DOAnchorPos(endPositionLeftPart, _animationTime))
            .Join(_rightPart.DOAnchorPos(endPositionRightPart, _animationTime));
    }

    private void AnimateHeightPanels()
    {
        OnLoadingPanelActivated?.Invoke();
        Vector3 endPositionTopPart = new Vector3(0, -215);
        Vector3 endPositionBottomPart = new Vector3(0, 215);
        Sequence seq = DOTween.Sequence();
        seq.Append(_topPart.DOAnchorPos(endPositionTopPart, _animationTime))
            .Join(_bottomPart.DOAnchorPos(endPositionBottomPart, _animationTime));
    }

    private void AnimateDiagonalPanels()
    {
        OnLoadingPanelActivated?.Invoke();
        Vector3 endPositionTopLeftPart = new Vector3(772, 200);
        Vector3 endPositionBottomRightPart = new Vector3(-772, -200);
        Sequence seq = DOTween.Sequence();
        seq.Append(_topLeftPart.DOAnchorPos(endPositionTopLeftPart, _animationTime))
            .Join(_bottomRightPart.DOAnchorPos(endPositionBottomRightPart, _animationTime));
    }

    private void OnDestroy()
    {
        ButtonsManager.OnButtonPressed -= ChooseType;
    }
}