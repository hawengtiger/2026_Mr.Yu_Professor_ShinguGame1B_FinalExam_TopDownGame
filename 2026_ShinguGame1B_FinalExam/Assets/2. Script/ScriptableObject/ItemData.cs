using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Scriptable Objects/ItemDataSO")]
public class ItemDataSo : ScriptableObject
{
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