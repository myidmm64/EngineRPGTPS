using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulMove : MonoBehaviour
{

    //스컬 상태
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
    [Header("기본속성")]
    //해골 초기상태
    public SkullState skullState = SkullState.NONE;
    //해골 이동속도
    public float spdMove = 2f;
    //해골이 본 타겟
    public GameObject targetCharacter = null;
    public Transform targetTransform = null;
    public Vector3 posTarget = Vector3.zero;
    private Animation skullAnimation = null;
    private Transform skullTransform = null;
    [Header("애니메이션 클립")]
    public AnimationClip IdleAnimationClip = null;
    public AnimationClip MoveAnimaitonClip = null;
    public AnimationClip AttackAnimaitonClip = null;
    public AnimationClip DamageAnimationClip = null;
    public AnimationClip DieAnimationClip = null;
    [Header("전투속성")]
    //체력
    public int hp = 100;
    //공격 거리
    public float AtkRange = 1.5f;
    //해골 피격 이펙트
    public GameObject effectDamage = null;
    public GameObject effectDie = null;
    //해골 이동 반경
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
        //이벤트 만들어줌
        AnimationEvent newAnimationEvent = new AnimationEvent();
        //이벤트 함수 연결
        newAnimationEvent.functionName = funcNmae;
        //끝나기 직전에 호출
        newAnimationEvent.time = clip.length - 0.5f;
        //이벤트 넣어줌
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
        //블랜딩 우선순서
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
    /// 해골 상태에 따라 동작을 제어하는 함수
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
    /// 임의의 지역 이동 후 대기모드
    /// </summary>
    /// <returns></returns>
    IEnumerator SetWait()
    {
        //해골 상태를 대기상태로 변경
        skullState = SkullState.WAIT;
        //대기하는 시간
        float timeWait = Random.Range(1f, 3f);
        //대기 시간을 넣어줘야함
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
    /// 목표물 설정
    /// </summary>
    /// <param name="target"></param>
    private void OnCkTarget(GameObject target)
    {
        targetCharacter = target;

        targetTransform = targetCharacter.transform;

        skullState = SkullState.GOTARGET;
    }
    /// <summary>
    /// 해골 상태가 무브일 때 동작
    /// </summary>
    void SetMove()
    {
        //출발점 도착점 두 벡터 차이
        Vector3 distance = Vector3.zero;
        //방향
        Vector3 posLookAt = Vector3.zero;
        switch (skullState)
        {
            case SkullState.MOVE:
                //타겟 없을 때
                if (posTarget != Vector3.zero)
                {
                    //차이 구하기
                    distance = posTarget - skullTransform.position;
                    //만약 distance의 길이가 range보다 작으면
                    if (distance.magnitude < AtkRange)
                    {
                        StartCoroutine(SetWait());
                        return;
                    }
                    //방향 구하기
                    posLookAt = new Vector3(posTarget.x, skullTransform.position.y, posTarget.z);
                }
                break;
            case SkullState.GOTARGET:
                //타겟 있을 때
                if (targetCharacter != null)
                {
                    //타겟과 차이
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

        //해골이 이동할 방향은 크기가 없고 방향만
        Vector3 direction = distance.normalized;

        direction = new Vector3(direction.x, 0f, direction.z);

        //이동량 방향
        Vector3 amount = direction * spdMove * Time.deltaTime;

        //월드좌표 이동
        skullTransform.Translate(amount, Space.World);

        //캐릭터 방향
        skullTransform.LookAt(posLookAt);
    }
    /// <summary>
    /// 해골 상태가 대기일 때 동작
    /// </summary>
    void SetIdle()
    {
        if (targetCharacter == null)
        {
            posTarget = new Vector3(skullTransform.position.x + Random.Range(-moveRadius, moveRadius),
                skullTransform.position.y + 1000f,
                skullTransform.position.z + Random.Range(-moveRadius, moveRadius)
                );
            //레이캐스트 시작점 목표 방향
            Ray ray = new Ray(posTarget, Vector3.down);
            //충돌체 정보 변수
            RaycastHit infoRaycast = new RaycastHit();
            //만약 충돌체가 있냐면
            if (Physics.Raycast(ray, out infoRaycast, Mathf.Infinity))
            {
                //임의의 목표 벡터에 높이값을 추가
                posTarget.y = infoRaycast.point.y;
            }
            //해골 상태를 무브로
            skullState = SkullState.MOVE;
        }
        else
        {

            //해골 상태를 고타겟으로
            skullState = SkullState.GOTARGET;
        }
    }
    /// <summary>
    /// 애니메이션 재생 함수
    /// </summary>
    void AnimationCtrl()
    {
        //해골 상태에 따라 애니메이션 적용
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
    /// 피격
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
