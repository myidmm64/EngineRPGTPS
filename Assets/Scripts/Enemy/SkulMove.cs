using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulMove : MonoBehaviour
{

    //���� ����
    public enum SkullState
    {
        NONE,
        IDLE,
        MOVE,
        WAIT,
        GOTARGET,
        ATK,
        DAMAGE,
        DIE
    }
    [Header("�⺻�Ӽ�")]
    //�ذ� �ʱ����
    public SkullState skullState = SkullState.NONE;
    //�ذ� �̵��ӵ�
    public float spdMove = 2f;
    //�ذ��� �� Ÿ��
    public GameObject targetCharacter = null;
    public Transform targetTransform = null;
    public Vector3 posTarget = Vector3.zero;
    private Animation skullAnimation = null;
    private Transform skullTransform = null;
    [Header("�ִϸ��̼� Ŭ��")]
    public AnimationClip IdleAnimationClip = null;
    public AnimationClip MoveAnimaitonClip = null;
    public AnimationClip AttackAnimaitonClip = null;
    public AnimationClip DamageAnimationClip = null;
    public AnimationClip DieAnimationClip = null;
    [Header("�����Ӽ�")]
    //ü��
    public int hp = 100;
    //���� �Ÿ�
    public float AtkRange = 1.5f;
    //�ذ� �ǰ� ����Ʈ
    public GameObject effectDamage = null;
    public GameObject effectDie = null;
    //�ذ� �̵� �ݰ�
    public float moveRadius = 10f;
    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer = null;
    [SerializeField]
    private float attackDistance = 0.3f;

    private bool _isAttack = false;

    private void OnAtkAnimationFinished()
    {
        Debug.Log("Atk animation Finished");
        _isAttack = false;
    }
    private void OnDamageAnimationFinished()
    {
        skinnedMeshRenderer.material.color = Color.white;
        Debug.Log("Damage animation Finished");
    }
    private void OnDieAnimationFinished()
    {
        Debug.Log("Die animation Finished");
    }
    void OnAnimationEvent(AnimationClip clip, string funcNmae)
    {
        //�̺�Ʈ �������
        AnimationEvent newAnimationEvent = new AnimationEvent();
        //�̺�Ʈ �Լ� ����
        newAnimationEvent.functionName = funcNmae;
        //������ ������ ȣ��
        newAnimationEvent.time = clip.length - 0.5f;
        //�̺�Ʈ �־���
        clip.AddEvent(newAnimationEvent);
    }
    private void Start()
    {
        skullState = SkullState.IDLE;
        skullTransform = GetComponent<Transform>();
        skullAnimation = GetComponent<Animation>();

        skullAnimation[IdleAnimationClip.name].wrapMode = WrapMode.Loop;
        skullAnimation[MoveAnimaitonClip.name].wrapMode = WrapMode.Loop;
        skullAnimation[DieAnimationClip.name].wrapMode = WrapMode.Once;
        skullAnimation[DieAnimationClip.name].layer = 10;
        skullAnimation[DamageAnimationClip.name].wrapMode = WrapMode.Once;
        //���� �켱����
        skullAnimation[DamageAnimationClip.name].layer = 10;
        skullAnimation[AttackAnimaitonClip.name].wrapMode = WrapMode.Once;
        skullAnimation[AttackAnimaitonClip.name].speed = 1.5f;

        OnAnimationEvent(AttackAnimaitonClip, "OnAtkAnimationFinished");
        OnAnimationEvent(DamageAnimationClip, "OnDamageAnimationFinished");
        OnAnimationEvent(DieAnimationClip, "OnDieAnimationFinished");

    }
    private void Update()
    {
        CheckState();
        AnimationCtrl();
    }
    /// <summary>
    /// �ذ� ���¿� ���� ������ �����ϴ� �Լ�
    /// </summary>
    void CheckState()
    {
        switch (skullState)
        {
            case SkullState.IDLE:
                _isAttack = false;
                SetIdle();
                break;
            case SkullState.GOTARGET:
            case SkullState.MOVE:
                if(_isAttack == false)
                    SetMove();
                break;
            case SkullState.WAIT:
                break;
            case SkullState.ATK:
                SetAttack();
                break;
            case SkullState.DAMAGE:
                break;
            case SkullState.DIE:
                break;
            default:
                break;

        }
    }
    /// <summary>
    /// ������ ���� �̵� �� �����
    /// </summary>
    /// <returns></returns>
    IEnumerator SetWait()
    {
        //�ذ� ���¸� �����·� ����
        skullState = SkullState.WAIT;
        //����ϴ� �ð�
        float timeWait = Random.Range(1f, 3f);
        //��� �ð��� �־������
        yield return new WaitForSeconds(timeWait);
        skullState = SkullState.IDLE;
    }
    void SetAttack()
    {
        Vector3 temp = targetTransform.position - transform.position;
        if (temp.magnitude > AtkRange * AtkRange + attackDistance)
        {
            skullState = SkullState.GOTARGET;
        }

        float distance = Vector3.Distance(targetTransform.position, skullTransform.position);

        if (distance > AtkRange + attackDistance)
        {
            skullState = SkullState.GOTARGET;
        }
        //to be continue
    }

    /// <summary>
    /// ��ǥ�� ����
    /// </summary>
    /// <param name="target"></param>
    private void OnCkTarget(GameObject target)
    {
        targetCharacter = target;

        targetTransform = targetCharacter.transform;

        skullState = SkullState.GOTARGET;
    }
    /// <summary>
    /// �ذ� ���°� ������ �� ����
    /// </summary>
    void SetMove()
    {
        //����� ������ �� ���� ����
        Vector3 distance = Vector3.zero;
        //����
        Vector3 posLookAt = Vector3.zero;
        switch (skullState)
        {
            case SkullState.MOVE:
                //Ÿ�� ���� ��
                if (posTarget != Vector3.zero)
                {
                    //���� ���ϱ�
                    distance = posTarget - skullTransform.position;
                    //���� distance�� ���̰� range���� ������
                    if (distance.magnitude < AtkRange)
                    {
                        StartCoroutine(SetWait());
                        return;
                    }
                    //���� ���ϱ�
                    posLookAt = new Vector3(posTarget.x, skullTransform.position.y, posTarget.z);
                }
                break;
            case SkullState.GOTARGET:
                //Ÿ�� ���� ��
                if (targetCharacter != null)
                {
                    //Ÿ�ٰ� ����
                    distance = targetCharacter.transform.position - skullTransform.position;

                    if (distance.magnitude < AtkRange)
                    {
                        skullState = SkullState.ATK;
                        return;
                    }

                    posLookAt = new Vector3(targetCharacter.transform.position.x,
                        skullTransform.position.y,
                        targetCharacter.transform.position.z
                        );
                }
                break;
            default:
                break;
        }

        //�ذ��� �̵��� ������ ũ�Ⱑ ���� ���⸸
        Vector3 direction = distance.normalized;

        direction = new Vector3(direction.x, 0f, direction.z);

        //�̵��� ����
        Vector3 amount = direction * spdMove * Time.deltaTime;

        //������ǥ �̵�
        skullTransform.Translate(amount, Space.World);

        //ĳ���� ����
        skullTransform.LookAt(posLookAt);
    }
    /// <summary>
    /// �ذ� ���°� ����� �� ����
    /// </summary>
    void SetIdle()
    {
        if (targetCharacter == null)
        {
            posTarget = new Vector3(skullTransform.position.x + Random.Range(-moveRadius, moveRadius),
                skullTransform.position.y + 1000f,
                skullTransform.position.z + Random.Range(-moveRadius, moveRadius)
                );
            //����ĳ��Ʈ ������ ��ǥ ����
            Ray ray = new Ray(posTarget, Vector3.down);
            //�浹ü ���� ����
            RaycastHit infoRaycast = new RaycastHit();
            //���� �浹ü�� �ֳĸ�
            if (Physics.Raycast(ray, out infoRaycast, Mathf.Infinity))
            {
                //������ ��ǥ ���Ϳ� ���̰��� �߰�
                posTarget.y = infoRaycast.point.y;
            }
            //�ذ� ���¸� �����
            skullState = SkullState.MOVE;
        }
        else
        {

            //�ذ� ���¸� ��Ÿ������
            skullState = SkullState.GOTARGET;
        }
    }
    /// <summary>
    /// �ִϸ��̼� ��� �Լ�
    /// </summary>
    void AnimationCtrl()
    {
        //�ذ� ���¿� ���� �ִϸ��̼� ����
        switch (skullState)
        {
            case SkullState.WAIT:
            case SkullState.IDLE:
                skullAnimation.CrossFade(IdleAnimationClip.name);
                break;
            case SkullState.MOVE:
            case SkullState.GOTARGET:
                if(_isAttack == false)
                    skullAnimation.CrossFade(MoveAnimaitonClip.name);
                break;
            case SkullState.ATK:
                skullAnimation.CrossFade(AttackAnimaitonClip.name);
                _isAttack = true;
                break;
            case SkullState.DIE:
                skullAnimation.CrossFade(DieAnimationClip.name);
                break;
            case SkullState.DAMAGE:
                skullAnimation.CrossFade(IdleAnimationClip.name);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// �ǰ�
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAtk"))
        {
            hp -= 10;
            if (hp > 0)
            {
                Instantiate(effectDamage, other.transform.position, Quaternion.identity);

                skullAnimation.CrossFade(DamageAnimationClip.name);

                EffectDamageTween();
            }
            else
            {
                skullState = SkullState.DIE;
            }
        }

    }
    private void EffectDamageTween()
    {
    }
}
