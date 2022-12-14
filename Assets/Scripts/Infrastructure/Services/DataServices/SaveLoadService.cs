using Infrastructure.Services;
using UnityEngine;

public class SaveLoadService : ISaveLoadService
{
   private  IGameFactory _gameFactory;
   private readonly IPersistantProgressService _persistantProgressService;

   private readonly string _progressKey = "PlayerProgress";

   public SaveLoadService(IGameFactory gameFactory, IPersistantProgressService persistantProgressService)
   {
      _gameFactory = gameFactory;
      _persistantProgressService = persistantProgressService;
   }

   public void SaveProgress()
   {
      if (_gameFactory == null)
         _gameFactory = AllServices.Container.Single<IGameFactory>();
      
      foreach (IProgressWatcher watcher in _gameFactory.ProgressWatchers)
      {
         watcher.SaveProgress(_persistantProgressService.PlayerProgress);
      }

      PlayerPrefs.SetString(_progressKey, _persistantProgressService.PlayerProgress.Serialize());
   }

   public PlayerProgress LoadProgress() => 
      PlayerPrefs.GetString(_progressKey).Deserialize<PlayerProgress>();
}
