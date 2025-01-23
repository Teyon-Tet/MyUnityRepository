using System.Collections.Generic;
using Runtime.Data.ValueObjects;
using UnityEngine;

namespace Runtime.Data.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_Level", menuName = "3DGame/CD_level", order = 0)]
    public class CD_Level : ScriptableObject
    {
        public List<LevelData> levels;
    }
}