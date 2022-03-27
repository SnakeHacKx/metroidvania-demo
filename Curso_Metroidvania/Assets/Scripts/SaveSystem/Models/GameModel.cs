using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameModel : MonoBehaviour
{
    public void Save(GameData gameData)
    {
        var heroModel = new HeroModel();
        var levelModel = new LevelModel();
        heroModel.Save(gameData.gameName, gameData.heroData);
        levelModel.Save(gameData.gameName, gameData.levelData);


    }
    public GameData Load(GameData gameData)
    {
        var heroModel = new HeroModel();
        var levelModel = new LevelModel();
        gameData.heroData = heroModel.Load(gameData.gameName);
        gameData.levelData = levelModel.Load(gameData.gameName);

        return gameData;
    }
}
