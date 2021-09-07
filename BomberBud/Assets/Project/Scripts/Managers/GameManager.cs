using xOrfe.Utilities;

namespace Project.Scripts.Managers
{
    public sealed class GameManager : SingletonUtility<GameManager>, IGameManagement
    {

        private void Start()
        {
            GameStart();
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
            OnGameEnd?.Invoke();
        }
        
        public void GameEnd()
        {
            OnGameEnd?.Invoke();

        }

    }
}