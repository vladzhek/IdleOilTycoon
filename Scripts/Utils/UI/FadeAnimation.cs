using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class FadeAnimation : MonoBehaviour
    {
        [SerializeField] private float _duration;

        private Image _image;

        public void OnEnable()
        {
            _image = gameObject.GetComponent<Image>();

            var color = _image.color;

            DOVirtual.Float(1, 0.25f, _duration, value =>
            {
                color.a = value;
                _image.color = color;
            }).SetLoops(-1, LoopType.Yoyo);
        }
    }
}