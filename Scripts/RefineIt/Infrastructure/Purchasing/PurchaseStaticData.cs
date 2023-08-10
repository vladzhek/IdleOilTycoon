using System.Collections.Generic;
using Constants;
using Gameplay.Currencies;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Infrastructure.Purchasing
{
    [CreateAssetMenu(fileName = FILE, menuName = MENU)]
    public class PurchaseStaticData : ScriptableObject
    {
        private const string CATEGORY = "StaticData";
        private const string TITLE = "Purchase";
        private const string FILE = TITLE + CATEGORY;
        private const string MENU = Names.PATH + "/" + CATEGORY + "/" + TITLE;

        [SerializeField] private string _id;
        [SerializeField] private bool _isInApp;
        [SerializeField, HideIf(nameof(_isInApp))] private List<CurrencyData> _price;
        [SerializeField] private List<CurrencyData> _currencies;

        public string ID => _id;
        public bool IsInApp => _isInApp;
        public List<CurrencyData> Price => _price;
        public List<CurrencyData> Currencies => _currencies;
    }
}