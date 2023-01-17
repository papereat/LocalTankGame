using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainMenuControler : MonoBehaviour
{
    public static MainMenuControler current;
    #region Refrences
        public GameObject SaveChooserPrefab;
    #endregion
    #region UI refrences
        public GameObject OptionsUI;
        public GameObject ContinueUI;
        public Transform ContinueContent;
        public GameObject CreditsUI;
    #endregion

    #region Build in Func
        void Awake()
        {
            current=this;
            DontDestroyOnLoad(gameObject);
        }
        void Start()
        {
            SceneTransferScript.sceneTransferScript=gameObject.AddComponent<SceneTransferScript>();
            FillOptionUI();
        }
        void Update()
        {
            Inputs();
        }
    #endregion
        

            
    #region Other Fuctnions
        void Inputs()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {

                OptionsUI.SetActive(false);
                ContinueUI.SetActive(false);
                //CreditsUI.SetActive(false);
            }
        }
        void FillOptionUI()
        {
            foreach (var item in new DirectoryInfo(Application.persistentDataPath+"/Game Save").GetFiles())
            {
                GameObject SaveChooser=Instantiate(SaveChooserPrefab,ContinueContent);
                SaveChooser.GetComponent<SaveChooserObject>().fileName=item.Name;
            }
        }
        public void LoadGame(string SaveName)
        {
            SceneTransferScript.sceneTransferScript.LoadSaveGame=true;
            SceneTransferScript.sceneTransferScript.SaveName=SaveName;
            SceneControler.current.SwichScene(1);
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
            ContinueUI.SetActive(true);
        }
        public void OptionButton()
        {
            OptionsUI.SetActive(true);
        }
        public void CreditButton(){}
        public void QuitButton()
        {
            Application.Quit();
            Debug.Log("Game Closed");
            UnityEditor.EditorApplication.isPlaying = false;
        }
    #endregion
}
