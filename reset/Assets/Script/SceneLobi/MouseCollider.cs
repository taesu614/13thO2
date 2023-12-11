using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseCollider : MonoBehaviour
{
    GameObject deckui;
    DeckUIManager deckuimanager;
    bool isclick = false;
    private void Start()
    {
    }
    void Update()
    {
        if (!isclick && Input.GetMouseButtonDown(0))
        {
            isclick = true;
        }
        else if(isclick && Input.GetMouseButtonUp(0))
        {
            isclick = false;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("ButtonNotInUI"));

            if (hit.collider != null && hit.collider.CompareTag("Button1"))
            {
                // ButtonNotInUI 레이어에 적용된 Collider와 충돌하고, Door 태그를 가진 게임 오브젝트가 클릭되었을 때 실행할 코드
                PresstoStart("BattleScene");
            }
            if (hit.collider != null && hit.collider.CompareTag("Button2"))
            {
                // ButtonNotInUI 레이어에 적용된 Collider와 충돌하고, Door 태그를 가진 게임 오브젝트가 클릭되었을 때 실행할 코드
                PresstoStart("DeckBuildScene");
            }
        }
    }

    public void PresstoStart(string name)
    {
        SceneManager.LoadScene(name);
    }
}
