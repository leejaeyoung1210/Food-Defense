//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.Rendering;

//public class TouchHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
//{
//    private float holdStartTime;
//    private bool isHolding;

//    public LineRenderer lr; // 드래그 사용시 사용

//    private float speed = 1f; // 포탑 이동속도 \
//    private float maxLineRange = 3f; // 라인 최대 사정거리

//    private Vector2 startPos; // 드래그 시작 위치  
//    private Vector2 endPos; // 드래그 끝 위치 

//    private TowerSpot prevOwner; // 이전소유자 
//    private TowerSpot currOwner; // 현재소유자   
//    private int prevIndex = -1; // 이전 인덱스   
//    private int currIndex = -1; // 현재 인덱스   

//    private void Start()
//    {
//        startPos = transform.position;
//    }

//    public void OnPointerDown(PointerEventData eventData)
//    {
//        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
//        if (Physics.Raycast(ray, out RaycastHit hitInfo))
//        {
//            Debug.Log($"{hitInfo.collider.gameObject.name}");
//            Debug.Log("찍힘");

//        }

//        holdStartTime = Time.time;
//        isHolding = true;
//        lr.positionCount = 0;
//        lr.SetPosition(0,startPos);
//        Debug.Log($"{eventData.pointerCurrentRaycast}선택됨");
//    }
//    public void OnDrag(PointerEventData eventData)
//    {
//        if (isHolding)
//        {
//            float holdDuration = Time.time - holdStartTime;
//            if (holdDuration > 0.5f)   // 0.5초 이상 → 홀드로 간주
//            {

//                lr.positionCount = 2;
//                lr.SetPosition(0, eventData.position);


//                var maxLineRange = Vector2.Distance(startPos, eventData.position);
//                if (maxLineRange > maxLineRange)//선 사정거리 이상이면 라인 클리어
//                {
//                    Vector2 dir = (eventData.position - startPos).normalized;
//                    Vector2 capped = startPos + dir * maxLineRange;
//                    lr.SetPosition(1, capped);
//                    //endPos = capped;
//                    Debug.Log("그리는중");
//                    return;
//                }
//                //endPos = eventData.position;
//            }
//        }
//    }

//    public void OnPointerUp(PointerEventData eventData)
//    {
//        isHolding = false;
//        lr.positionCount = 0; // 라인 클리어 

//        float holdDuration = Time.time - holdStartTime;

//        if (holdDuration > 0.5f)   // 0.5초 이상 → 홀드로 간주
//        {
//            Debug.Log("홀드 완료 (길게 누름)");
//            //if(eventData)

//        }
//        else
//        {
//            Debug.Log("탭 완료 (짧게 누름)");
//            OpenTab(); // UI 띄우기                
//        }
//    }

//    private void Update()
//    {
//        if (Vector2.Distance(transform.position, endPos) > 0.1f)
//        {
//            transform.position = Vector2.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
//        }
//    }


//    private void OpenTab()
//    {
//        // UI 띄우기
//        Debug.Log("UI 띄우기");
//    }
//}
