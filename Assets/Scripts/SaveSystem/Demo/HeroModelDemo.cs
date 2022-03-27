using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroModelDemo : MonoBehaviour
{
    [SerializeField]  List<GameData> gameDataSlots; // Se pueden hacer ranuras para guardar diferentes partidas
    [SerializeField]  GameData gameData;

    GameModel gameModel;
    public bool save;
    public bool load;

    // Start is called before the first frame update
    void Start()
    {
        //gameModel = new GameModel();
    }

    // Update is called once per frame
    void Update()
    {
        if (save)
        {
            save = false;
            gameModel.Save(gameData);
        }

        if (load)
        {
            gameData = gameModel.Load(gameData);
            load = false;
        }
    }
}
