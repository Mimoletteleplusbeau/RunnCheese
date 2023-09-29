using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Button Parameters")]
    [SerializeField] private string _targetScene;
    [SerializeField] private float _originalScale = 1;
    [SerializeField] private float _hoverScale = 1.1f;
    [SerializeField] private float _hoverScaleDuration = 0.3f;
    [SerializeField] private float _notHoverScaleDuration = 0.5f;
    [SerializeField] private ButtonAction Action;

    public enum ButtonAction
    {
        Next,
        Restart,
        Quit,
        Menu,
        ByName,
    }

    private void Start()
    {
        SetAction();
    }

    private void SetAction()
    {
        UnityAction myAction;
        switch (Action)
        {
            case ButtonAction.Next:
                myAction = GoToNextLevel;
                break;
            case ButtonAction.Restart:
                myAction = RestartLevel;
                break;
            case ButtonAction.Quit:
                myAction = QuitGame;
                break;
            case ButtonAction.Menu:
                myAction = GoToMenu;
                break;
            case ButtonAction.ByName:
                myAction = GoToScene;
                break;
            default:
                myAction = RestartLevel;
                break;
        } 
        GetComponent<Button>().onClick.AddListener(myAction);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MenuManager.MenuManagerInstance.CanClickButtons)
        {
            transform.DOKill();
            transform.DOScale(new Vector3(_hoverScale, _hoverScale), _hoverScaleDuration);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(new Vector3(_originalScale, _originalScale), _notHoverScaleDuration);
    }

    public void GoToScene()
    {
        if (MenuManager.MenuManagerInstance.CanClickButtons)
        {
            MenuManager.MenuManagerInstance.SetButtonsUnclickable();
            MenuManager.MenuManagerInstance.Transition.SetTransition(() => SceneManager.LoadScene(_targetScene));
        }
    }

    public void QuitGame()
    {
        if (MenuManager.MenuManagerInstance.CanClickButtons)
        {
            MenuManager.MenuManagerInstance.SetButtonsUnclickable();
            MenuManager.MenuManagerInstance.Transition.SetTransition(() => Application.Quit());
        }
    }

    private void RestartLevel()
    {
        if (MenuManager.MenuManagerInstance.CanClickButtons)
        {
            MenuManager.MenuManagerInstance.SetButtonsUnclickable();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }

    private void GoToNextLevel()
    {
        if (MenuManager.MenuManagerInstance.CanClickButtons)
        {
            Debug.Log("OKclick");
            MenuManager.MenuManagerInstance.SetButtonsUnclickable();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }
        else
        {
            Debug.Log("NOPEclick");
        }
    }

    private void GoToMenu()
    {
        LevelsManager.Instance.GoToMenu();
    }
}
