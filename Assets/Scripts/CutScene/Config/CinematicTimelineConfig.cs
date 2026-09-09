using System.Collections.Generic;
using UnityEngine;

namespace Scripts.CutScene
{
    [CreateAssetMenu(fileName = "CutSceneConfig", menuName = "CutScene/Create CutScene Config")]
    public class CinematicTimelineConfig : ScriptableObject
    {
        [SerializeField] private List<ActionConfigBase> steps = new();

        public List<ActionConfigBase> GetActions()
        {
            return steps;
        }
    }
}