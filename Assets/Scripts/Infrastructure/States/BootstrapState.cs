﻿using Infrastructure.Services;
using Infrastructure.StateMachine;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine gameStateMachine, AllServices services, ICoroutineRunner coroutineRunner)
        {
            _gameStateMachine = gameStateMachine;
            _services = services;
            _coroutineRunner = coroutineRunner;

            RegisterServices();
        }

        public void Enter()
        {
            _services.Single<ILoadLevelService>().LoadLevel(ScenesNamePathes._initialSceneName,OnLoad);
        }

        public void Exit()
        {
        
        }

        public void OnLoad()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(ScenesNamePathes._gameSceneName);
        }

        private void RegisterServices()
        {
            RegisterEnemyStaticDataService();   
            _services.RegisterServices<IGameFactory>(new GameFactory(_services.Single<IEnemyStaticDataService>()));
            _services.RegisterServices<ILoadLevelService>(new LoadLevelService(_coroutineRunner));
            _services.RegisterServices<IPersistantProgressService>(new PersistantProgressService());
            _services.RegisterServices<ISaveLoadService>(new SaveLoadService(_services.Single<IGameFactory>(), 
                _services.Single<IPersistantProgressService>()));
        }

        private void RegisterEnemyStaticDataService()
        {
            EnemyStaticDataService enemyStaticData = new EnemyStaticDataService();
            enemyStaticData.LoadEnemies();
            _services.RegisterServices<IEnemyStaticDataService>(enemyStaticData);
        }
    }    
}
