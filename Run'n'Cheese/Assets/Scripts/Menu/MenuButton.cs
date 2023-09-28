using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Button Parameters")]
    [SerializeField] private string _targetScene;
    [SerializeField] private float _originalScale = 1;
    [SerializeField] private float _hoverScale = 1.1f;
    [SerializeField] private float _hoverScaleDuration = 0.3f;
    [SerializeField] private float _notHoverScaleDuration = 0.5f;

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
}
