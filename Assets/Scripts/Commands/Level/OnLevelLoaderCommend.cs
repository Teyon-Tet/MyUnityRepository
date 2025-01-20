using UnityEngine;

namespace Commands.Level
{
    public class OnLevelLoaderCommend
    {
        private Transform _levelHolder;
        
        public OnLevelLoaderCommend(Transform levelHolder)
        {
            _levelHolder = levelHolder;
        }

        public void Execute(int levelIndex)
        {
            Object.Instantiate(Resources.Load<GameObject>($"Prefabs/LevelPrefabs/Level{levelIndex}"), _levelHolder, true);
        }
    }
}