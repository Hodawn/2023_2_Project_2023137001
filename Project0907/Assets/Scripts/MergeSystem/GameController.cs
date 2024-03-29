using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Slot[] slots;

    private Vector3 _target;
    private ItemInfo carryingitem;              //이동 시키고 있는 아이템 정보

    private Dictionary<int, Slot> slotDictionary;   //슬롯 정보값 관리하는 자료구조

    // Start is called before the first frame update
    void Start()
    {
        slotDictionary = new Dictionary<int, Slot>();       //슬롯 딕셔너리 초기화

        for(int i=0; i<slots.Length; i++)               //각 슬롯의 ID를 설정하고 딕셔너리에 추가
        {
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))            //마우스 버튼을 눌렀을 때
        {
            SendRayCast();
        }
        if(Input.GetMouseButton(0)&&carryingitem)           //마우스 버튼 누름 상태에서 아이템 선택 및 이동 처리
        {
            Onitemselected();
        }
        if (Input.GetMouseButtonUp(0))          //마우스 버튼 떼기 이벤트 처리
        {
            SendRayCast();
        }
        if (Input.GetKeyDown(KeyCode.Space))    //스페이스 키를 눌렀을 때 랜덤 아이템 배치
        {
            PlaceRandomitem();
        }
    }

    void SendRayCast()      //마우스 클릭 시 Ray 발사
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);                //화면의 마우스 좌표를 통해서 월드 상의 레이케스팅
        RaycastHit hit;                                                               //hit 물리 선언

        if(Physics.Raycast(ray, out hit))                               //hit된 것이 있을 경우
        {
            var slot = hit.transform.GetComponent<Slot>();          //hit된 slot Component를 가져온다
            if (slot.state == Slot.SLOTSTATE.FULL && carryingitem == null)      //선택한 슬롯에서 아이템을 잡고 이동하는 경우
            {
                string itemPath = "Prefabs/Item_Grabbed_" + slot.itemObject.id.ToString("000");     //잡을 아이템 생성
                var itemGO = (GameObject)Instantiate(Resources.Load(itemPath));                     //해당 경로를 통해서 생성
                itemGO.transform.position = slot.transform.position;                                //slot 위치로 생성하게 설정
                itemGO.transform.localScale = Vector3.one * 2;                                      //크기는 2배로   
                carryingitem = itemGO.GetComponent<ItemInfo>();                                     //잡은 아이템을 carryingitem에 입력                
                carryingitem.initDummy(slot.id, slot.itemObject.id);                                //정보값까지 생성

                slot.itemGrabbed();                                                                   //슬롯 아이템이 잡혔다  
            }
            else if(slot.state==Slot.SLOTSTATE.EMPTY&&carryingitem != null)         //빈 슬롯에 아이템 배치
            {
                slot.Createitem(carryingitem.itemld);               //잡고 있떤 아이템 아이디를 가져와서 슬롯에 생성하고    
                Destroy(carryingitem.gameObject);                   //잡고 있던 아이템 삭제
            }
            else if(slot.state==Slot.SLOTSTATE.FULL&&carryingitem != null)          //아이템끼리 같은 슬롯 위에 있을때
            {
                if (slot.itemObject.id == carryingitem.itemld)                      //슬롯 위에 있는 아이템 id와 잡고 있는 아이템이 같을 경우
                {
                    OnitemMergedWithTarget(slot.id);                            //아이템 병합
                }
                else
                {
                    OnitemCarryFail();      //아이템 배치 실패
                }
            }

        }
        else //hit 가 없을 경우
        {
            if (!carryingitem)          //잡고 있는 아이템도 없을 경우
            {
                return;
            }
            OnitemCarryFail();      //아이템 배치 실패
        }
    }
    //아이템을 선택하고 마우스 위치로 이동
    void Onitemselected()
    {
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //월드 좌표에서 마우스 포지션 값을 가져와서 _target입력
        _target.z = 0;                                              //2D

        var delta = 10 * Time.deltaTime;        //이동속도 조절

        delta *= Vector3.Distance(transform.position, _target);
        carryingitem.transform.position = Vector3.MoveTowards(carryingitem.transform.position, _target, delta);         //함수로 이동
    }

    //아이템이 슬롯과 병합 될 때 함수
    void OnitemMergedWithTarget(int targetSlotld)       //아이템이 슬롯과 병합 될 때 함수
    {
        var slot = GetSlotByld(targetSlotld);           //기존 슬롯에 있는 오브젝트를 가져와서 파괴
        Destroy(slot.itemObject.gameObject);
        slot.Createitem(carryingitem.itemld + 1);       //병합 되었으므로 다음 오브젝트를 생성
        Destroy(carryingitem.gameObject);               //들고있던 더미 오브젝트를 파괴
    }
    void OnitemCarryFail()          //아이템 배치 실패 시 실행
    {
        var slot = GetSlotByld(carryingitem.slotld);            //잡고 있던 아이템의 원래 슬롯 위치
        slot.Createitem(carryingitem.itemld);                   //해당 슬롯에 다시 생성
        Destroy(carryingitem.gameObject);                       //잡고 있는 더미 아이템을 삭제
    }
    void PlaceRandomitem()
    {
        if (AllSlotsOccupied())
        {
            Debug.Log("슬롯이 다 차있음 => 생성 불가");
            return;
        }

        var rand = UnityEngine.Random.Range(0, slots.Length);               //슬롯 갯수에 따라서 랜덤 번호를 rand에 입력
        var slot = GetSlotByld(rand);                                       //Rand 받은 index값을 통해서 slot 객체를 가져온다

        while (slot.state == Slot.SLOTSTATE.FULL)               //슬롯 상태가 FULL이 아닐때까지 랜덤 번호를 찾아서 진행
        {
            rand = UnityEngine.Random.Range(0, slots.Length);           //슬롯 갯수에 따라서 랜덤 번호를 rand에 입력
            slot = GetSlotByld(rand);                               //Rand 받은 index값을 통해서 slot 객체를 가져온다
        }
        slot.Createitem(0);                                 //빈 슬롯을 발견하면 0번째 아이템을 생성
    }
    bool AllSlotsOccupied()                     //슬롯이 채워져 있는지 확인 하는 함수
    {
        foreach(var slot in slots)                      //슬롯 배열을 하나씩 확인하면서 foreach 문 반복
        {
            if (slot.state == Slot.SLOTSTATE.EMPTY)     //슬롯 배열에 빈 자리가 있으면
            {
                return false;           //중간에 false를 리턴
            }
        }
        return true;                //다 차있으므로 true를 리턴
    }
    //슬롯 ID로 슬롯을 검색
    Slot GetSlotByld(int id)        //슬롯 ID로 슬롯을 검색
    {
        return slotDictionary[id];      //딕셔너리에 담겨있는 Slot Class 반환 (번호를 통해서)
    }
}
