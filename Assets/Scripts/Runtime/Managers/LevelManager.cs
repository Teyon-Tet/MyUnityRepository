using System;
using Runtime.Data.ValueObjects;
using Runtime.Commands.Level;
using Runtime.Data.UnityObjects;
using Runtime.Signals;
using UnityEngine;


namespace Runtime.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]private Transform levelHolder;
        [SerializeField]private int totalLevelCount;
        
        private OnLevelLoaderCommend _levelLoaderCommend;
        private OnLevelDestroyerCommand _levelDestroyerCommand;
        private int _currentLevel;
        private LevelData _levelData;


        private void Awake()
        {
            _levelData = GetLevelData();
            _currentLevel = GetActiveLevel();

            Init();
        }

        private void Init()
        {
            _levelLoaderCommend = new OnLevelLoaderCommend(levelHolder);
            _levelDestroyerCommand = new OnLevelDestroyerCommand(levelHolder);
        }

        private int GetActiveLevel()
        {
            return _currentLevel;
        }

        private LevelData GetLevelData()
        {
            return Resources.Load<CD_Level>("Data/CD_Level").levels[_currentLevel];
        }

        private void OnEnable()
        {
            SubscribeEvent();
        }

        private void SubscribeEvent()
        {
            CoreGameSignals.Instance.onLevelInitialize += _levelLoaderCommend.Execute;
            CoreGameSignals.Instance.onClearActiveLevel += _levelDestroyerCommand.Execute;
            CoreGameSignals.Instance.onGetLevelValue += OnGetLevelValue;
            CoreGameSignals.Instance.onNextLevel += OnNextLevel;
            CoreGameSignals.Instance.onRestartLevel += OnRestartLevel;
        }

        private int OnGetLevelValue()
        {
            return _currentLevel;
        }

        private void UnSubscribeEvent()
        {
            CoreGameSignals.Instance.onLevelInitialize -= _levelLoaderCommend.Execute;
            CoreGameSignals.Instance.onClearActiveLevel -= _levelDestroyerCommand.Execute;
            CoreGameSignals.Instance.onGetLevelValue -= OnGetLevelValue;
            CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
            CoreGameSignals.Instance.onRestartLevel -= OnRestartLevel;
        }
        

        private void OnDisable()
        {
            UnSubscribeEvent();
        }

        private void Start()
        {
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((int) (_currentLevel % totalLevelCount));
        }

        private void OnNextLevel()
        {
            _currentLevel++;
            CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((int) (_currentLevel % totalLevelCount));
        }

        private void OnRestartLevel()
        {
            CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((int) (_currentLevel % totalLevelCount));
        }
    }
}