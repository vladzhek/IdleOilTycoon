using Gameplay.Region.Storage;
using TMPro;
using UnityEngine;

namespace Gameplay.Quests.UI
{
    public class DailyQuestView : MonoBehaviour
    {
        [field:SerializeField] public DailyQuestContainer DailyQuestContainer{ get; private set; }
        [SerializeField] public TMP_Text TimerText;
    }
}