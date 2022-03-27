using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public string gameName = ""; // Nombre de la partida
    public LevelData levelData = new LevelData();
    public HeroData heroData = new HeroData();
}
