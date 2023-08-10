using Gameplay.Region.Storage;
using TMPro;
using UnityEngine;

namespace Gameplay.Quests.UI
{
    public class WeeklyQuestView : MonoBehaviour
    {
        [field:SerializeField] public WeeklyQuestContainer WeeklyQuestContainer{ get; private set; }
        [SerializeField] public TMP_Text TimerText;
    }
}