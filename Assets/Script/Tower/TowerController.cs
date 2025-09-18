using UnityEngine.UI;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    Touch touch;  // 터치 정보  
    public LineRenderer lr; // 드래그 사용시 사용    

    private float startTime; // 이동 시작 시간
    private float tapThreshold = 0.15f; //터치 
    private float speed = 1f; // 이동 속도    

    private Vector3 startPos; // 드래그 시작 위치     
    private Vector3 endPos; // 드래그 끝 위치 

    private Vector3 dir = Vector3.zero;
    private Vector3 targetPos = Vector3.zero;
    //기존값
    private TowerSpot prevOwner;
    private int prevIndex = -1;



    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTime = Time.time;
                Began();
            }
            if (touch.phase == TouchPhase.Moved)
            {
                Dragging();
            }
            if (touch.phase == TouchPhase.Ended)
            {
                Move();
            }
        }
        if (dir != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, endPos) < 0.001f)
            {
                dir = Vector3.zero;
            }

        }
    }


    private void Began()
    {
        //OverlapPoint 
        var startpoint = ToWorldOnZ0(touch.position);
        startpoint.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(startpoint, Vector2.zero);
        //var hit = Physics2D.OverlapPoint(startpoint); 
        if (hit.collider == null) // 땅바닥 찍으면 리턴
        {
            lr.positionCount = 0; //라인클리어
            return;
        }
        else if (hit.collider.gameObject == this.gameObject)// 어딘가 찍엇다 그럼 자신인지 검사 필요 
        {
            startPos = transform.position; //터치 시작 위치는 타워 위치    
            lr.positionCount = 1;
            lr.SetPosition(0, startPos);
        }
    }

    private void Dragging()
    {
        var dragPos = ToWorldOnZ0(touch.position);
        dragPos.z = 0;

        if (lr.positionCount == 0) //라인이 없으면 리턴 
        {
            return;
        }
        lr.positionCount = 2;
        lr.SetPosition(1, dragPos);

        var maxLineRange = Vector2.Distance(startPos, dragPos);

        if (maxLineRange > 10f)//선 사정거리 이상이면 라인 클리어
        {
            lr.positionCount = 0;
            return;
        }

    }

    private void Move()
    {
        lr.positionCount = 0; //라인클리어 
        var pos = ToWorldOnZ0(touch.position);
        pos.z = 0;

        var lineRange = Vector3.Distance(pos, startPos); // 이동거리 확인용

        if (lineRange < 0.1f || (Time.time - startTime) <= tapThreshold)// 탭으로 인식 UI 띄우기
        {
            OnenTab();// UI 띄우기
            return;
        }
        else //이동 가능한 조건이다. 드래그로 인식
        {
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, LayerMask.GetMask("Spot")); // 타워 레이어에 닿았는지 확인                         

            if (hit.collider != null) // 설치가 가능한 스폿에 닿았는지 확인    
            {// 타워가 있음
                TowerSpot targetspot = hit.collider.GetComponent<TowerSpot>();
                if (targetspot == null)
                {
                    Debug.Log("타워 스폿이 아님");
                    endPos = startPos;
                    return;
                }

                int index = targetspot.FindAvailableSpot(pos); // 비어있는 스폿 인덱스 확인

                if (index == -1) // 비어있는 스폿이 없음 
                {
                    Debug.Log("비어있는 스폿 아님");
                    endPos = startPos;
                    return;
                }

                if (prevOwner != null && prevIndex != -1)
                {
                    prevOwner.spotPoints[prevIndex].isSpawning = false;
                    prevOwner.spotPoints[prevIndex].tower = null;
                }

                Debug.Log("이동 가능");              
                targetspot.spotPoints[index].isSpawning = true; // 스폿 사용중으로 변경
                targetspot.spotPoints[index].tower = gameObject;
                prevOwner = targetspot;
                prevIndex = index;
                dir = (targetspot.GetSpotPoint(index) - startPos).normalized;
                endPos = targetspot.GetSpotPoint(index);

            }
        }
    }
    Vector3 ToWorldOnZ0(Vector2 screenPos)
    {
        float zDist = 0f - Camera.main.transform.position.z;               // 카메라가 -10이면 +10
        return Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, zDist));
    }

    private void OnenTab() //UI 띄우기 타워에 캔버스 설정 (강화)
    {
        return;
    }
}

