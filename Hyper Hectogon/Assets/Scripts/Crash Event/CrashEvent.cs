using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashEvent : MonoBehaviour {
    [SerializeField]
    private GameObject _Crash_Power, _Crash_Enemy, _Player;
    private Rigidbody2D _Power_Rigid, _Enemy_Rigid;
    private bool _Power_Active = false, _Enemy_Active = false;
    private Vector2 _Pos;
    private readonly Vector2 _Delta = new Vector2(0f, 2f);
    private readonly float _Gap = 3f;

    private GameObject _Object = null;
    private CircleCollider2D _Object_Collider;

    // Start is called before the first frame update
    void Start() {
        _Power_Rigid = _Crash_Power.GetComponent<Rigidbody2D>();
        _Enemy_Rigid = _Crash_Enemy.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if(_Power_Active) {
            if(_Player.transform.position.y >= _Pos.y + _Gap) {
                _Crash_Power.SetActive(false);
                _Power_Active = false;
            }
        }
        if(_Enemy_Active) {
            if(_Player.transform.position.y >= _Pos.y + _Gap * 0.5) {
                _Crash_Enemy.SetActive(false);
                _Enemy_Active = false;
            }
        }
        if(_Object != null) {
            _Object.SetActive(true);
            _Object.transform.Rotate(0f, 0f, Random.Range(2f, 3f));
            if(_Object.transform.localScale.x > 0f) {
                _Object.transform.localScale = new Vector2(_Object.transform.localScale.x - 0.05f, _Object.transform.localScale.y - 0.05f);
            } else {
                _Object.SetActive(false);
                _Object_Collider.enabled = true;
            }
        }
    }

    public void Crash(string __type__) {
        _Pos = _Player.transform.position;
        _Crash_Enemy.SetActive(false);
        _Crash_Power.SetActive(false);
        _Power_Active = _Enemy_Active = false;
        if (__type__.Equals("Enemy")) {
            _Crash_Enemy.SetActive(true);
            _Enemy_Rigid.velocity = _Delta;
            _Crash_Enemy.transform.position = _Pos;
            _Enemy_Active = true;
        } else if (__type__.Equals("Power")) {
            _Crash_Power.SetActive(true);
            _Power_Rigid.velocity = _Delta;
            _Crash_Power.transform.position = _Pos;
            _Power_Active = true;
        }
    }

    public void CrashPower(GameObject __obj__) {
        _Object = __obj__;
        _Object_Collider = _Object.GetComponent<CircleCollider2D>();
        _Object_Collider.enabled = false;
    }

    public void CrashEnemy() {
        //
    }
}
