﻿using Gameplay.Region;
using UnityEngine;
using Zenject;

namespace Infrastructure.Windows
{
    public class OpenWindowButton : ButtonBaseSFX
    {
        [SerializeField] private WindowType _windowType;
        
        private IWindowService _windowService;

        [Inject]
        private void Construct(IWindowService windowService)
        {
            _windowService = windowService;
        }
        
        public override void OnClick()
        {
            base.OnClick();
            _windowService.Open(_windowType);
        }
    }
}