using System.Collections.Generic;
using UnityEngine;

namespace Scripts.CutScene
{
    [CreateAssetMenu(fileName = "CutSceneConfig", menuName = "CutScene/Create CutScene Config")]
    public class CutSceneConfig : ScriptableObject
    {
        public List<CutSceneStep> steps = new();

        public List<CutSceneStep> GetSteps()
        {
            return steps;
        }
    }
}