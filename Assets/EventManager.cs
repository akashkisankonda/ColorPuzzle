using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public GameObject undoButton;
    public GameObject undoTitle;
    public GameObject cancellUndoButtn;
    public GameObject levelFailScreen;
    public GameObject levelCompleteScreen;
    public List<Coin> coins;
    public ParticleSystem confetti;

    [NonSerialized]
    public bool undoModeActivated = false;
    [NonSerialized]
    public bool gameOver = false;

    public void ToggleUndoMode()
    {
        undoTitle.SetActive(!undoTitle.activeInHierarchy);
        undoButton.SetActive(!undoButton.activeInHierarchy);
        cancellUndoButtn.SetActive(!cancellUndoButtn.activeInHierarchy);
        undoModeActivated = !undoModeActivated;
    }

    public void TriggerLevelFailScreen()
    {
        gameOver = true;
        levelFailScreen.SetActive(true);
        levelFailScreen.GetComponent<Animator>().SetTrigger("FadeIn");
    }

    public void TriggerLevelSuccessScreen()
    {
        confetti.Play();
        gameOver = true;
        levelCompleteScreen.SetActive(true);
        levelCompleteScreen.GetComponent<Animator>().SetTrigger("FadeIn");
    }

    public void CheckLevelComplete()
    {
        bool result = true;
        foreach (var item in coins)
        {

            if (!item.ReachedDestination)
            {
                result = false;
            }
        }
        if (result)
        {
            TriggerLevelSuccessScreen();
        }
    }

    public void TabVibrate()
    {
        Handheld.Vibrate();
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
