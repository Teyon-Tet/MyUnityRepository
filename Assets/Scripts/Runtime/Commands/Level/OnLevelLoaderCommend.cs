using UnityEngine;

namespace Runtime.Commands.Level
{
    public class OnLevelLoaderCommend
    {
        private Transform _levelHolder;
        
        internal OnLevelLoaderCommend(Transform levelHolder)
        {
            _levelHolder = levelHolder;
        }

        internal void Execute(int levelIndex)
        {
            Object.Instantiate(Resources.Load<GameObject>($"Prefabs/LevelPrefabs/Level{levelIndex}"), _levelHolder, true);
        }
    }
}