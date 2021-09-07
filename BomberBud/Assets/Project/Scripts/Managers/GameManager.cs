using System.Collections;
using Project.Scripts.Scriptables;
using UnityEngine;
using xOrfe.Utilities;

namespace Project.Scripts.Managers
{
    public sealed class GameManager : SingletonUtility<GameManager>, IGameManagement
    {
        
        [SerializeField]private int _currentLevel;
        public int CurrentLevel
        {
            get => _currentLevel;
            set => _currentLevel = value;
        }
        
        [SerializeField]private LevelDefinitionScriptable[] _levelDefinitions;
        public LevelDefinitionScriptable[] LevelDefinitions
        {
            get => _levelDefinitions;
            set => _levelDefinitions = value;
        }

        private void Start()
        {
            GameStart();
            LevelManager.Instance.StartLevel(LevelDefinitions[_currentLevel]);
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }
        
        

        public event OnGameStartDelegate OnGameStart;
        public event OnLevelStartDelegate OnLevelStart;
        public event OnLevelEndDelegate OnLevelEnd;
        public event OnGameExitDelegate OnGameEnd;
        public event OnProgressDelegate OnProgress;
        
        private bool _isGameplayRunning;

        public bool IsGameplayRunning
        {
            get => _isGameplayRunning;
            set => _isGameplayRunning = value;
        }

        public void GameStart()
        {
            OnGameStart?.Invoke();
        }
        public void Progress(int progressAmount)
        {
            throw new System.NotImplementedException();
        }

        public void LevelStart(LevelManager levelManager)
        {
            OnLevelStart?.Invoke(levelManager);

        }

        public void LevelEnd(bool succeed)
        {
            GameManager.Instance.IsGameplayRunning = false;
            
            if (succeed) CurrentLevel++;

            PhysicsProcessor.Instance.Reset();
            StartCoroutine(LoadLevelCall(2f));
            
            OnGameEnd?.Invoke();
        }

        private IEnumerator LoadLevelCall(float time)
        {
            yield return new WaitForSeconds(time);
            LevelManager.Instance.StartLevel(LevelDefinitions[_currentLevel]);

        }
        
        public void GameEnd()
        {
            OnGameEnd?.Invoke();

        }

    }
}