using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : UIWindow
{
    public Button Option;
    public Button Start;
    public Button Skins;
    public Button Level;
    public Button Close;

    public GameObject SkinsTab;

    public static bool GoToLevelWindow;

    private void Awake()
    {
        Skins.onClick.AddListener(delegate { OpenTab(SkinsTab); });
        Close.onClick.AddListener(delegate { CloseTab(SkinsTab); });
        Level.onClick.AddListener(PerformLevelButton);
        Start.onClick.AddListener(PerformStartButton);


        //una roba brutta che non sapevo come fare altrimenti, se poi scopro un metodo piu carino tolgo sta cacata :)
        if (GoToLevelWindow)
        {
            GoToLevelWindow = false;
            Level.onClick.Invoke();
        }
    }

    private void PerformLevelButton() => ChangeWindow(FindObjectOfType<LevelMenuUI>(true));

    private void PerformStartButton()
    {
        if (PlayerData.LastSelectedLevel == null)
            PlayerData.LastSelectedLevel = LevelLoader.GetLevel(Theme.Sushi, Difficulty.Beginner, 1);

        LevelLoader.LevelToLoad = PlayerData.LastSelectedLevel;
        SceneManager.LoadScene("GameScene");
    }
}