using Gameplay.Workers;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Personnel
{
    public class WorkersWindow : Window
    {
       [SerializeField] private WorkersViewInitializer workersViewInitializer;
       private WorkersModel _workersModel;

       [Inject]
       private void Construct(WorkersModel workersModel)
       {
           _workersModel = workersModel;
       }

       private void OnEnable()
       {
           workersViewInitializer.Initialize(_workersModel);
       }
    }
}