using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class LevelData
{
    public SceneID lastCheckpointScene;
    public float lastCheckPointPostionX;
    public float lastCheckPointPostionY;
    public List<string> defeatedBosses = new List<string>(); // Lista de jefes que ya han sido derrotados
}