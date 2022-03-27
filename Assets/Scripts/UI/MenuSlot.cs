using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curso_Metroidvania
{
    public class MenuSlot : MonoBehaviour
    {
        public int slotNumber;
        protected TitleScreen titleScreen;
        // Start is called before the first frame update
        void Start()
        {
            titleScreen = FindObjectOfType<TitleScreen>();
        }

        public virtual void NewGameSlot()
        {
            titleScreen.NewGame(slotNumber);
        }

        public virtual void LoadGameSlot()
        {
            titleScreen.LoadGame(slotNumber);
        }

        public virtual void GoBackToMainMenu()
        {
            titleScreen.MainMenu();
        }
    }
}

