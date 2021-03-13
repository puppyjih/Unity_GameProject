using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Bomb : SkillParent
{
    private Player _player;
    private Transform _playerTransform;
    private Rigidbody2D _playerRigid;
    private string bombTag = "Bomb";

    ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        SetCost(10);
        SetCoolTime(0.5f);
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        _player = player.GetComponent<Player>();
        _playerRigid = player.GetComponent<Rigidbody2D>();
        _playerTransform = player.transform; // caching
    }

    public override void Activate()
    {
        Vector3 bias = new Vector3(0.5f * _playerTransform.localScale.x, 0f, 0f);
        bias = Vector3.zero;
        if(_player.IsPressingDown())
        {
            objectPooler.SpawnFromPool(bombTag, _playerTransform.position + bias, Quaternion.identity);
        }
        else
        {
            //bias.y = 0.5f;
            GameObject obj = objectPooler.SpawnFromPool(bombTag, _playerTransform.position + bias, Quaternion.identity);
            Throwable throwable = obj.GetComponent<Throwable>();
            throwable.Throw(_playerRigid.velocity, _playerTransform.localScale.x);
        }
    }
}
