using System;
using DG.Tweening;
using Gameplay.Tilemaps.Data;
using Infrastructure.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.TutorialStateMachine
{
    public class TutorialView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private TextMeshProUGUI _shortMessageText;
        [SerializeField] private GameObject _finger;
        [SerializeField] private GameObject _closeMiningTarget;
        [SerializeField] private GameObject _closeComplexTarget;
        [SerializeField] private GameObject _closeProcessingTarget;
        [SerializeField] private GameObject _highlightBackButton;
        [SerializeField] private GameObject _highlightUpgradeButton;
        [SerializeField] private Image _highlightOrder;
        [SerializeField] private Image _highlightQuests;
        [SerializeField] private Image _background;
        
        private void OnDisable()
        {
            DisableTutorialGameObjects();
        }

        public void DisableTutorialGameObjects()
        {
            _messageText.transform.parent.parent.gameObject.SetActive(false);
            _shortMessageText.transform.parent.parent.gameObject.SetActive(false);
            _finger.SetActive(false);
            _closeMiningTarget.SetActive(false);
            _closeComplexTarget.SetActive(false);
            _highlightUpgradeButton.SetActive(false);
            _highlightQuests.gameObject.SetActive(false);
            _highlightOrder.gameObject.SetActive(false);
            _highlightBackButton.SetActive(false);
            _closeProcessingTarget.SetActive(false);
            _background.raycastTarget = true;
            SetBackgroundAlpha(0f);
        }

        public void SetMessage(string message)
        {
            _messageText.transform.parent.parent.gameObject.SetActive(true);
            _messageText.text = message;
            SetBackgroundAlpha(0.375f);
        }

        public void SetShortMessage(string message, bool isFinger, bool isBlock)
        {
            _shortMessageText.transform.parent.parent.gameObject.SetActive(true);
            _shortMessageText.text = message;
            Finger(isFinger);
            _background.raycastTarget = isBlock;
        }

        private void Finger(bool isActive)
        {
            _finger.SetActive(isActive);
            if (isActive)
            {
                _finger.transform.GetChild(0).transform.DOMoveY(_finger.transform.position.y + 20, 0.75f)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }

        public void ShowCloseMiningTarget(bool IsActive, BuildingType type)
        {
            switch (type)
            {
                case BuildingType.Mining:
                    _closeMiningTarget.SetActive(IsActive);
                    break;
                case BuildingType.Complex:
                    _closeComplexTarget.SetActive(IsActive);
                    break;
                case BuildingType.Process:
                    _closeProcessingTarget.SetActive(IsActive);
                    break;
            }
        }

        private void SetBackgroundAlpha(float value)
        {
            var color = _background.color;
            color.a = value;
            _background.color = color;
        }

        public void HighlightHudButton(WindowType type)
        {
            SetBackgroundAlpha(0.375f);
            if (type == WindowType.OrdersWindow)
            {
                _highlightOrder.gameObject.SetActive(true);
            }

            if (type == WindowType.DailyQuest)
            {
                _highlightQuests.gameObject.SetActive(true);
            }
        }

        public void HighlightBackButton(bool isActive)
        {
            _highlightBackButton.SetActive(isActive);
            SetBackgroundAlpha(0);
        }

        public void HighlightUpgradeButton(bool isActive)
        {
            _highlightUpgradeButton.SetActive(isActive);
            SetBackgroundAlpha(0);
        }
    }
}