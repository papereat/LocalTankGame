using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControler : MonoBehaviour
{
    public static SceneControler current;

    #region Built in Func
        void Awake()
        {
            current=this;
        }
    #endregion

    #region Swich Scene Func
        public void SwichScene(int index)
        {
            SceneManager.LoadScene(sceneBuildIndex:index);
        }
    #endregion
}
