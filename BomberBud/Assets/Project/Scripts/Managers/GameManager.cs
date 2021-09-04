using UnityEngine;
using xOrfe.Utilities;

namespace Project.Scripts.Managers
{
    public abstract class GameManager : SingletonUtility<GameManager>, IGameManagement
    {
        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public event OnGameStartDelegate OnGameStart;
        public event OnLevelStartDelegate OnLevelStart;
        public event OnLevelEndDelegate OnLevelEnd;
        public event OnGameExitDelegate OnGameEnd;
        public event OnProgressDelegate OnProgress;

        public void GameStart()
        {
            OnGameStart?.Invoke();
        }

        public void GameEnd()
        {
            OnGameEnd?.Invoke();

        }
    }
}