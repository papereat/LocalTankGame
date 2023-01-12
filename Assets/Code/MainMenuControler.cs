using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuControler : MonoBehaviour
{

    #region UI refrences
        public GameObject OptionsUI;
        public GameObject CreditsUI;
    #endregion

    #region Build in Func
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        void Start()
        {
            SceneTransferScript.sceneTransferScript=gameObject.AddComponent<SceneTransferScript>();
        }
    #endregion

    #region Button Inputs Func
        public void StartButton()
        {
            SceneTransferScript.sceneTransferScript.LoadSaveGame=false;
            SceneControler.current.SwichScene(1);
        }
        public void ContinueButton()
        {
            SceneTransferScript.sceneTransferScript.LoadSaveGame=true;
            SceneTransferScript.sceneTransferScript.SaveName="Test";
            SceneControler.current.SwichScene(1);
        }
        public void OptionButton(){}
        public void CreditButton(){}
        public void QuitButton()
        {
            Application.Quit();
            Debug.Log("Game Closed");
            UnityEditor.EditorApplication.isPlaying = false;
        }
    #endregion
}
