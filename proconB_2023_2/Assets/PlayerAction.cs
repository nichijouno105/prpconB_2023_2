using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerAction : MonoBehaviour
{
    [Header("移動の速さ"), SerializeField]
    private float _speed = 3;

    [Header("回転量"), SerializeField]
    private float _rotate = 1;

    [Header("ジャンプする瞬間の速さ"), SerializeField]
    private float _jumpSpeed = 7;

    [Header("重力加速度"), SerializeField]
    private float _gravity = 15;

    [Header("落下時の速さ制限（Infinityで無制限）"), SerializeField]
    private float _fallSpeed = 10;

    [Header("落下の初速"), SerializeField]
    private float _initFallSpeed = 2;

    private Transform _transform;
    private CharacterController _characterController;
    private Camera _camera;

    private Vector2 _inputMove;
    private float _verticalVelocity;
    private bool _isGroundedPrev;
    private float _inputRRotate;
    private float _inputLRotate;


    //追加
    //右手のコライダー
    [SerializeField]
    public SphereCollider handCollider;
    private Animator _animator;
    private bool _inputAttack;
    private bool _inputDash;
    private bool _inputSquat;

    private bool AttackFlag = false;

    [Header("ダッシュの速さ倍率"), SerializeField]
    private float _dashspeed = 3;
    private float _dspeed = 1;
    [Header("しゃがんだ時の高さ"), SerializeField]
    private float squatHeight = 1f;  // しゃがんだ時の背の高さ
    [Header("通常の高さ"), SerializeField]
    private float normalHeight = 1.55f;  // 通常時の背の高さ
    


    /// <summary>
    /// 移動Action(PlayerInput側から呼ばれる)
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // 入力値を保持しておく
        _inputMove = context.ReadValue<Vector2>();
 
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                // ボタンが押された時の処理
                _animator.SetBool("walking", true);
                break;

            case InputActionPhase.Canceled:
                // ボタンが離された時の処理
                _animator.SetBool("walking", false);

                break;        
        }
    }

    /// <summary>
    /// ジャンプAction(PlayerInput側から呼ばれる)
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        // ボタンが押された瞬間かつ着地している時だけ処理
        if (!context.performed || !_characterController.isGrounded) return;

        // 鉛直上向きに速度を与える
        _verticalVelocity = _jumpSpeed;
        //_animator.SetTrigger("Jump");
        _animator.SetBool("walking", false);
        _animator.SetBool("jumping", true);

    }

    public void OnRightRotate(InputAction.CallbackContext context)
    {
        _inputRRotate = context.ReadValue<float>();
    }

    public void OnLeftRotate(InputAction.CallbackContext context)
    {
        _inputLRotate = context.ReadValue<float>();
    }




//追加 
    public void BreakEffect(Collider other)
    {
        GameObject flagment = (GameObject)Resources.Load("breakBox");
        Instantiate(flagment, other.transform.position + new Vector3(0, 1, 0), other.transform.rotation);
    }

    //オブジェクトと接触した瞬間に呼び出される
    void OnTriggerStay(Collider other)
    {
        Debug.Log("aaa");
        //攻撃した相手がEnemyの場合
        if(other.CompareTag("Enemy")&&AttackFlag==true){
            
            Destroy(other.gameObject, 0.1f);
            BreakEffect(other);
 
        }
    }

    
    
//攻撃する
    public void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("punch");
        AttackFlag=true;
        //  // 入力値を保持しておく
        _inputAttack = context.ReadValueAsButton();
        //_animator.SetTrigger("Attack");
        _animator.SetBool("walking", false);
        _animator.SetBool("attacking", true);

        


    }

//ダッシュする
    public void OnDash(InputAction.CallbackContext context)
    {
        //_animator.SetTrigger("Run");

        switch (context.phase)
        {
            case InputActionPhase.Performed:
                // ボタンが押された時の処理
                _inputDash = true;
                _animator.SetBool("walking", false);
                _animator.SetBool("running", true);
                break;

            case InputActionPhase.Canceled:
                // ボタンが離された時の処理
                _inputDash = false;
                _animator.SetBool("running", false);

                break;
        
        }
    }

//しゃがみ
    public void OnSquat(InputAction.CallbackContext context)
    {
        
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                // ボタンが押された時の処理
                _inputSquat = true;
                _animator.SetBool("walking", false);
                _animator.SetBool("squatting", true);
                break;

            case InputActionPhase.Canceled:
                // ボタンが離された時の処理
                _inputSquat = false;
                _animator.SetBool("squatting", false);
                break;
        
        }
    }

    private void ColliderReset()
    {
        handCollider.enabled = false;
        _animator.SetBool("attacking", false);

        if(AttackFlag)
        {
            AttackFlag = false;
            Debug.Log("punch-end");
        }

    }

     private void OnDashReleased()
    {
        _inputDash = false;
        // ダッシュが終了した際の処理をここに記述する
    }

        public void AttackEnd()
    {
        // AttackEndの処理を記述する
    }



    private void Awake()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();

        _camera = Camera.main; // メインカメラを取得

        //追加
         //右手のコライダーを取得
        handCollider = GetComponent<SphereCollider>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        var isGrounded = _characterController.isGrounded;

        if (isGrounded && !_isGroundedPrev)
        {
            // 着地する瞬間に落下の初速を指定しておく
            _verticalVelocity = -_initFallSpeed;
            _animator.SetBool("jumping", false);
        }
        else if (!isGrounded)
        {
            // 空中にいるときは、下向きに重力加速度を与えて落下させる
            _verticalVelocity -= _gravity * Time.deltaTime;

            // 落下する速さ以上にならないように補正
            if (_verticalVelocity < -_fallSpeed)
                _verticalVelocity = -_fallSpeed;
        }

        _isGroundedPrev = isGrounded;

        var cameraForward = _camera.transform.forward;
        var cameraRight = _camera.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // 操作入力とカメラの向きから、移動方向を計算
        var moveDirection = cameraForward * _inputMove.y + cameraRight * _inputMove.x;
        moveDirection.Normalize();

        // moveDirectionの値をコンソールに表示
//Debug.Log("Move Direction: " + moveDirection);

        //改
        // 移動速度を計算
        var moveVelocity = moveDirection * _speed * _dspeed;

        // ジャンプ速度を追加
        moveVelocity.y = _verticalVelocity;

        // 移動量を計算
        var moveDelta = moveVelocity * Time.deltaTime;

        // キャラクターを移動させる
        _characterController.Move(moveDelta);

        // 右回転の入力がある場合は、回転させる
        if (_inputRRotate != 0)
        {
            var rotation = Quaternion.Euler(0, _transform.eulerAngles.y + _inputRRotate * _rotate, 0);
            _transform.rotation = rotation;
        }

        // 左回転の入力がある場合は、回転させる
        if (_inputLRotate != 0)
        {
            var rotation = Quaternion.Euler(0, _transform.eulerAngles.y + _inputLRotate * -_rotate, 0);
            _transform.rotation = rotation;
        }

//追加
        if(_inputAttack)
        {
            handCollider.enabled = true;
            Invoke("ColliderReset", 2.0f);
        }

        //Dashボタンが押されている間はダッシュする
        if(_inputDash)
        {
                    _dspeed = _dashspeed;
        }else
        {
                    _dspeed = 1;
        }

        if(_inputSquat)
        {
            _characterController.height = squatHeight;
        }else
        {
            _characterController.height = normalHeight;
        }
    }
}