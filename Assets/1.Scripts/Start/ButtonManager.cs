using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Recipe = null;

    private bool _isOpen = false;

    public bool IsOpen => _isOpen;

    public void GoMainScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void OpenRecipe()
    {
        Recipe.SetActive(true);
        _isOpen = true;
    }
    public void ExitRecipe()
    {
        Recipe.SetActive(false);
        _isOpen = false;
    }
}
