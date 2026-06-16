using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Scriptable Objects/ItemDataSO")]
public class ItemDataSo : ScriptableObject
{
    [Header("아이템 가격")]
    public int ItemPrice = 1;        // 아이템에 닿았을 경우 증가할 값

    [Header("UI 표시")]
    public string itemName;

    [Header("효과 내용")]
    [TextArea(3, 10)]
    public List<string> dialogueLines = new List<string>();


    public enum PassiveItemType {Ink_Hp, White_DMG, Tape_AR, Water_CT, Dry_ATR, Shoes_SPD} //잉크 최대체력 증가, 수정액 데미지 증가, 수정테이프 사거리증가, 물 공속증가, 선풍기 공격지속시간 증가, 신발 이속 증가 

    [Header("패시브 아이템 타입")]
    public PassiveItemType passiveType;

    [Header("패시브로 증가될 스텟 수")]
    public float hp = 1;
    public float dmg = 2f;
    public float speed = 0.1f;
    public float cooltime = 0.1f;
    public float range = 0.1f;
    public float attackTime = 0.1f;
}