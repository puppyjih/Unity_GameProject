using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon, IDamaging
{
    [SerializeField] private int __damage__;
    [SerializeField] private float __attackSpeed__ = 1f;
    [SerializeField] private float __spriteAngle__;
    [SerializeField] private bool isShake = false;

    private Animator weaponAnim;
    private Animator effectAnim;
    private AnimatorControllerParameter[] weaponAnimParameters;
    private AnimatorControllerParameter[] effectAnimParameters;
    private CameraShake cameraShake;

    private bool isEquiped = false;

    private int weaponAnimLength;
    private int effectAnimLength;
    private int weaponAnimFlag = 0; // If there are more than 1 animations, use flag.
    private int effectAnimFlag = 0; // If there are more than 1 animations, use flag.
    private int _weaponPrevAnimFlag = 0; // To reset trigger
    private int _effectPrevAnimFlag = 0; // To reset trigger

    public int damage { get { return __damage__; } set { __damage__ = value; } }
    public float attackSpeed { get { return __attackSpeed__; } set { __attackSpeed__ = value; } }
    public float spriteAngle { get { return __spriteAngle__; } set { __spriteAngle__ = value; } }
    public string ownerTag { get; set; }

    void Start()
    {
        ownerTag = "";
        damage = __damage__;
        attackSpeed = __attackSpeed__;
        spriteAngle = __spriteAngle__;

        #region Animation

        weaponAnim = GetComponent<Animator>();
        effectAnim = gameObject.transform.GetChild(0).GetComponent<Animator>();

        weaponAnimParameters = weaponAnim.parameters;
        weaponAnimLength = weaponAnimParameters.Length;

        effectAnimParameters = effectAnim.parameters;
        effectAnimLength = effectAnimParameters.Length;

        #endregion

        // Camera Shake
        cameraShake = GameObject.Find("Cinemachine/ControlCameraShake").GetComponent<CameraShake>();
    }

    public void Attack() 
    {
        #region Animation

        weaponAnim.ResetTrigger(weaponAnimParameters[_weaponPrevAnimFlag].name);
        effectAnim.ResetTrigger(effectAnimParameters[_effectPrevAnimFlag].name);

        weaponAnim.SetTrigger(weaponAnimParameters[weaponAnimFlag].name);
        effectAnim.SetTrigger(effectAnimParameters[effectAnimFlag].name);

        _weaponPrevAnimFlag = weaponAnimFlag;
        _effectPrevAnimFlag = effectAnimFlag;

        weaponAnimFlag = (weaponAnimFlag + 1) % weaponAnimLength;
        effectAnimFlag = (effectAnimFlag + 1) % effectAnimLength;

        #endregion

        // Camera shake
        if (isShake)
        {
            cameraShake.Shake();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isEquiped)
        {
            if (string.IsNullOrEmpty(ownerTag)) return;
            if (!other.gameObject.CompareTag(ownerTag))
            {
                IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.GetDamage(damage);
                    if (ownerTag.Equals("Player") && !other.gameObject.active) {
                    GameObject obj = GameObject.Find("Player");
                    if (obj != null) {
                        PlayerStat playerStat = obj.GetComponent<PlayerStat>();
                        playerStat.HealMP();
                    }
                }
                }
            }
        }
    }

    public void SetIsEquiped(bool isEquiped)
    {
        this.isEquiped = isEquiped;
    }
}
