using UnityEngine;

namespace Inference
{
    [CreateAssetMenu(menuName = "CombinedClueData")]
    public class CombinedClueDataStroage : ScriptableObject
    {
        public TextAsset data;
        public ClueRecipe[] recipes;

        [ContextMenu("")]
        public void Convert()
        {
            // Todo: TextAsset to ClueRecipes
        }
    }
}
