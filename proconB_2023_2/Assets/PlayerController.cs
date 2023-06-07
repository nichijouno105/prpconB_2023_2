using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
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

    /// <summary>
    /// 移動Action(PlayerInput側から呼ばれる)
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // 入力値を保持しておく
        _inputMove = context.ReadValue<Vector2>();
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
    }

    public void OnRightRotate(InputAction.CallbackContext context)
    {
        _inputRRotate = context.ReadValue<float>();
    }

    public void OnLeftRotate(InputAction.CallbackContext context)
    {
        _inputLRotate = context.ReadValue<float>();
    }

    private void Awake()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();

        _camera = Camera.main; // メインカメラを取得
    }

    private void Update()
    {
        var isGrounded = _characterController.isGrounded;

        if (isGrounded && !_isGroundedPrev)
        {
            // 着地する瞬間に落下の初速を指定しておく
            _verticalVelocity = -_initFallSpeed;
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

        // 移動速度を計算
        var moveVelocity = moveDirection * _speed;

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
    }
}