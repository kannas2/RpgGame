using UnityEngine;
using System.Collections;

public class Animation_Check : MonoBehaviour
{
    public Animator _playerAnim;
    //public AudioSource _publicAudio;
    //public AudioClip _attackSnd;
    //public AudioClip _damageSnd;

    // Use this for initialization
    public PlayerCtrl player;

    void Start()
    {
        _playerAnim = transform.GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
    }

    void AttackDone()
    {
        if (_playerAnim != null)
        {
            _playerAnim.SetBool("isAttack", false);
        }
        else Debug.Log("Need Animator File");
        //if (_attackSnd != null && _publicAudio != null) _publicAudio.PlayOneShot(_attackSnd);
        //else Debug.Log("Need AudioSource File or Attack Audio Clip");
        player.attackChk = false;
    }

    void DamageDone()
    {
        if (_playerAnim != null)
        {
            _playerAnim.SetBool("isDamage", false);
        }
        else Debug.Log("Need Animator File");
        //if (_damageSnd != null && _publicAudio != null) _publicAudio.PlayOneShot(_damageSnd);
        //else Debug.Log("Need AudioSource File or Damage Audio Clip");
    }
}