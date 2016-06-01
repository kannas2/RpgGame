using UnityEngine;
using System.Collections;

[System.Serializable]
public class BaseCharacter : MonoBehaviour
{
    public enum CharacterState
    {
        Idle,
        Walk,
        Damage,
        Attack
    }

    public string playerName;
    public float curExp;
    public float maxExp;

    public float curHP;
    public float maxHP;

    public float curMP;
    public float maxMP;

    public float baseAttackPower;
    public float curAttackPower;

    public float baseAttackSpeed;
    public float curAttackSpeed;

    public float baseMoveSpeed;
    public float curMoveSpeed;
    public float rotSpeed;

    public bool isDead;

    public BaseCharacter() { }
    public BaseCharacter(string _name, float _exp, float _speed, float _rotSpeed, float _Hp, float _Mp)
    {
        playerName = _name;
        curExp = _exp;
        baseMoveSpeed = _speed;
        rotSpeed = _rotSpeed;
        curHP = _Hp;
        curMP = _Mp;
     }

    protected virtual void Character_Move() { }
    public virtual void Character_Update_Hp() { }
    public virtual void Character_Uptate_Mp() { }
    public virtual void Character_Anim_Speed() { }
}