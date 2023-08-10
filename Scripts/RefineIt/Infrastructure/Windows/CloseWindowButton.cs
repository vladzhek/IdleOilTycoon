using Gameplay.Region;
using UnityEngine;
using Zenject;

namespace Infrastructure.Windows
{
    public class CloseWindowButton : ButtonBase
    {
        [SerializeField] private WindowType _windowType;
        
        private Window _window;

        private IWindowService _windowService;

        [Inject]
        private void Construct(IWindowService windowService)
        {
            _windowService = windowService;
        }
        
        protected override void Awake()
        {
            base.Awake();
            _window = GetComponentInParent<Window>();
        }

        public override void OnClick()
        {
            _windowService.Close(_windowType);
        }
    }
}