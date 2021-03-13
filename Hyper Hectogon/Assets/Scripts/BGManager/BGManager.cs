using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGManager : MonoBehaviour {

    // private variable
    [SerializeField]
    private Sprite[] _BGSprites;
    [SerializeField]
    private GameObject[] _BGs;
    [SerializeField]
    private GameObject _Player;
    private PlayerStat _playerStat;


    private GameObject _cam;
    private int _BGs_Size;
    private SpriteRenderer[] _BG_Sprite;
    private Rigidbody2D[] _BG_delta;
    
    // Start is called before the first frame update
    void Start() {
        //
        _playerStat = _Player.GetComponent<PlayerStat>();
        _BGs_Size = _BGs.Length;
        _cam = _BGs[0].transform.parent.gameObject.transform.parent.gameObject;
        //
        _BG_delta = new Rigidbody2D[_BGs_Size];
        for(int i = 0; i < _BGs_Size; i++) {
            _BG_delta[i] = _BGs[i].gameObject.GetComponent<Rigidbody2D>();
            _BG_delta[i].velocity = new Vector2(Random.Range(-1.6f, 1.6f), Random.Range(-1.6f, 1.6f));
        }
        //
        _BG_Sprite = new SpriteRenderer[_BGs_Size];
        for (int i = 0; i < _BGs_Size; i++)
        {
            _BG_Sprite[i] = _BGs[i].gameObject.GetComponent<SpriteRenderer>();
        }
        //_set_Sprite_BG();
    }

    // Update is called once per frame
    void Update() {
        _rotating_BG();
        _moving_BG();
        //_set_Sprite_BG();
    }

    private void _set_Sprite_BG() {
        for(int i = 0; i < _BGs_Size; i++) {
            _BG_Sprite[i].sprite = _BGSprites[_playerStat.Stage];
        }
    }

    private void _rotating_BG() {
        for(int i = 0; i < _BGs_Size; i++) {
            _BGs[i].transform.Rotate(0f, 0f, Random.Range(.3f, 1f));
        }
    }

    private void _moving_BG() {
        for(int i = 0; i < _BGs_Size; i++) {
            Vector2 tmp = _BGs[i].transform.position, delta = _BG_delta[i].velocity;
            if(_cam.transform.position.x - tmp.x <= -4.5f){
                delta.x = delta.x > 0f ? -delta.x : delta.x;
            }
            if(4.5f <= _cam.transform.position.x - tmp.x) {
                delta.x = delta.x < 0f ? -delta.x : delta.x;
            }
            if(_cam.transform.position.y - tmp.y <= -8f) {
                delta.y = delta.y > 0f ? -delta.y : delta.y;
            }
            if(8f <= _cam.transform.position.y - tmp.y) {
                delta.y = delta.y < 0f ? -delta.y : delta.y;
            }
            _BG_delta[i].velocity = delta;
        }
    }
}
