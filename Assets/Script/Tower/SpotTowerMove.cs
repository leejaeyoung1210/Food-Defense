using UnityEngine.UI;
using UnityEngine;
using System;

public class SpotTowerMove : MonoBehaviour
{
    Touch touch;  // 터치 정보  
    public LineRenderer lr; // 드래그 사용시 사용    

    private float startTime; // 이동 시작 시간
    private float tapThreshold = 0.2f; //터치 
    private float speed = 2f; // 타워 이동 속도    

    private Vector2 startPos; // 드래그 시작 위치     
    private Vector2 endPos; // 드래그 끝 위치 

    //기존값    
    public TowerSpot ownerSpot;//                 

    private int spotLay;
    private Collider2D col;
    //private GameObject tower;
    Spot current;
    Spot targetSpot;
    private bool swaping;

    bool moving = false;

    private bool isSelected = false; // 선택된 상태인지    

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        spotLay = LayerMask.GetMask("Spot");
    }

    private void Update()
    {
        if (Input.touchCount > 0)

        {
            touch = Input.GetTouch(0);
            if (!moving)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    startTime = Time.time;
                    Began();
                }

                if (!isSelected) return;

                if (touch.phase == TouchPhase.Moved)
                {
                    Dragging();
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    Move();
                }
            }
        }

        if (moving)
        {
            if (current.tower != null)
            {
                current.tower.transform.position = Vector2.MoveTowards((Vector2)current.tower.transform.position, endPos, speed * Time.deltaTime);

                if (swaping)
                {
                    targetSpot.tower.transform.position = Vector2.MoveTowards((Vector2)targetSpot.tower.transform.position, startPos, speed * Time.deltaTime);
                    if (Vector2.Distance(current.tower.transform.position, endPos) < 0.001f&&
                        Vector2.Distance(targetSpot.tower.transform.position, startPos) < 0.001f)
                    {
                        var temp = targetSpot.tower;   
                        targetSpot.tower = current.tower;//타워정보 넘겨주고 
                        current.tower = temp;

                        ResetState();
                        return;
                    }


                }

                if (Vector2.Distance(current.tower.transform.position, endPos) < 0.001f)
                {
                    current.isSpawning = false; //기존위치비우고
                    targetSpot.isSpawning = true;// 이동위치 활서오하
                    targetSpot.tower = current.tower;//타워정보 넘겨주고 
                    //current.tower = null; // 정보초기화 

                    ResetState(); return;   
                }
            }
        }
    }


    private void Began()
    {
        Vector2 startpoint = Camera.main.ScreenToWorldPoint(touch.position); //이게 찍으니거고

        RaycastHit2D hit = Physics2D.Raycast(startpoint, Vector2.zero, 0f, spotLay);


        if (hit.collider == null) // 땅바닥 찍으면 리턴
        {
            lr.positionCount = 0; //라인클리어
            return;
        }


        if (hit.collider == col && !moving) // 어딘가 찍엇다 그럼 자신인지 검사 필요 
        {
            current = ownerSpot.FindAvailableSpot(hit.point); //그 위치 저장 

            if (current != null)
            {
                startPos = current.point;
                isSelected = true;
                lr.positionCount = 1;
                lr.SetPosition(0, current.point);
            }
        }
    }

    private void Dragging()
    {
        if (lr.positionCount == 0) //라인이 없으면 리턴 
        {
            isSelected = false;
            return;
        }

        if (Time.time - startTime > tapThreshold && isSelected) // 드래그로 판단되면 선 이어주고 아니면 탭으로 인식할거임
        {
            Vector2 dragPos = Camera.main.ScreenToWorldPoint(touch.position);

            lr.positionCount = 2;
            lr.SetPosition(1, dragPos);
        }

    }

    private void Move()
    {
        lr.positionCount = 0; //라인클리어 

        Vector2 endpoint = Camera.main.ScreenToWorldPoint(touch.position);

        RaycastHit2D hit = Physics2D.Raycast(endpoint, Vector2.zero, 0f, spotLay); // 타워 레이어에 닿았는지 확인


        if (hit.collider == null) // 스폿말고 딴데 찍히면 취소 
        {
            isSelected = false;
            return;
        }


        if (hit.collider == col && isSelected) // 어딘가 찍엇다 그럼 자신인지 검사 필요 
        {
            //텝인지 검사 필요 
            targetSpot = ownerSpot.FindAvailableSpot(hit.point); //그 위치 저장 
            if (targetSpot == null)
            {
                isSelected = false;
                return;
            }

            if(targetSpot.point == current.point) // 이동하는거 막고나서 텝인지 검사하자.
            { 
                if(Time.time - startTime <= tapThreshold)
                {
                    OnenTab(targetSpot);
                    return;
                }
                else
                {
                    Debug.Log("같은위치 드래그");
                    isSelected = false;
                    return;
                }
                
            }

            endPos = targetSpot.point;

            //드래그 이동시 
            if (targetSpot.isSpawning == false) //포탑 없는거 바로 이동 
            {
                moving = true;
            }
            else //타워가 있음        targetSpot          
            {
                moving = true;
                swaping = true;
            }

        }


    }

    private void OnenTab(Spot target) //UI 띄우기 타워에 캔버스 설정 (강화)
    {
        Debug.Log($"터치함{target.tower}");

        return;
    }

    private void ResetState()
    {
        startPos = Vector2.zero;
        endPos = Vector2.zero;
        current = null;
        targetSpot = null;
        isSelected = false;
        moving = false;
        swaping =false;
    }
}

