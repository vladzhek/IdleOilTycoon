using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.TutorialStateMachine
{
    public class TutorialWindow : Window
    {
        [SerializeField] private TutorialViewInitializer _tutorialViewInitializer;

        private TutorialModel _tutorialModel;

        [Inject]
        public void Construct(TutorialModel tutorialModel)
        {
            _tutorialModel = tutorialModel;
        }

        public void OnEnable()
        {
            _tutorialViewInitializer.Initialize(_tutorialModel);
        }
    }
}