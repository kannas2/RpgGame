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
    public float characterExp;
    public float characterSpeed;
    public float characterRotSpeed;
    public float characterHp;
    public float characterMp;


    public BaseCharacter() { }
    public BaseCharacter(string _name, float _exp, float _speed, float _rotSpeed, float _Hp, float _Mp)
    {
        playerName = _name;
        characterExp = _exp;
        characterSpeed = _speed;
        characterRotSpeed = _rotSpeed;
        characterHp = _Hp;
        characterMp = _Mp;
     }

    protected virtual void Character_Move() { }
    public virtual void Character_Update_Hp(float damage) { }
    public virtual void Character_Uptate_Mp() { }
}