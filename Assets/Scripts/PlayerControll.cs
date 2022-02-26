using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [SerializeField] bool isAxisRaw = true;

    [SerializeField] float _moveSpeed;
    [SerializeField] float inputX;

    Vector2 _speed;
    Vector3 newPosition;

    Transform _transform;


    float Gravity = -15;
    public bool isGravity = true;
    public float floatingRate = 0;
    bool isRight;
    public BoxCollider2D _boxCollider;
    Rect rayCheckBox;
    bool isFalling = false; // 下降
    float rayOffset = 0.15f;
    public LayerMask layerMask = 0;
    GameObject standingOn;
    bool isCollDown;
    Vector2 moveHelpParam;
    float smallValue = 0.0001f;

    public bool isGrounded
    {
        get { return isCollDown; }
    }

    public bool canJump = true;
    float jumpPower = 2;

    RaycastHit2D rayCast; // 射线


    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO:初始化参数 - input - 重力/漂浮计算 - boformove - 射线检测 - move - 重置数据
        setParam();
        input();
        doGravity();
        beforMove();
        rayCheck();
        move();
        resetParam();
    }

    void input()
    {
        if (isAxisRaw)
        {
            inputX = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            inputX = Input.GetAxis("Horizontal");
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void beforMove()
    {
        _speed.x = inputX * _moveSpeed;
        newPosition = _speed * Time.deltaTime;
    }

    void move()
    {
        _transform.Translate(newPosition, Space.World);
    }

    //初始化参数-检测盒
    void setParam()
    {
        isFalling = true;
    }

    // 射线检测
    void rayCheck()
    {
        setRayParam();
        rayCheckFoot();
    }

    // 重置数据
    void resetParam()
    {
        if (Time.deltaTime > 0)
        {
            _speed = newPosition / Time.deltaTime;
        }

        moveHelpParam.x = 0;
        moveHelpParam.y = 0;
    }

    void Jump()
    {
        canJump = isGrounded;
        if (!canJump)
            return;

        moveHelpParam.y = _speed.y = Mathf.Sqrt(2f * jumpPower * Mathf.Abs(Gravity));
    }

    void setRayParam()
    {
        rayCheckBox = new Rect(_boxCollider.bounds.min.x, _boxCollider.bounds.min.y, _boxCollider.bounds.size.x,
            _boxCollider.bounds.size.y);
    }

    void rayCheckFoot()
    {
        isFalling = newPosition.y < 0;
        if (Gravity > 0 && !isFalling) return; // 如果没有下坠 return
        float rayLength = rayCheckBox.height / 2 + rayOffset;
        if (newPosition.y < 0)
        {
            rayLength += Mathf.Abs(newPosition.y);
        }

        Vector2 rayCenter = rayCheckBox.center;
        rayCenter.y += rayOffset;

        RaycastHit2D[] rayInfo = new RaycastHit2D[1];
        rayInfo[0] = getRayCast(rayCenter, -Vector2.up, rayLength, Color.red, layerMask, true);

        if (rayInfo[0])
        {
            standingOn = rayInfo[0].collider.gameObject;
            isFalling = false;
            isCollDown = true;

            if (moveHelpParam.y > 0)
            {
                newPosition.y = _speed.y * Time.deltaTime;
                isCollDown = false;
            }
            else
            {
                newPosition.y = -Mathf.Abs(rayInfo[0].point.y - rayCenter.y) + rayCheckBox.height / 2 + rayOffset;
            }

            if (Mathf.Abs(newPosition.y) < smallValue)
            {
                newPosition.y = 0;
            }
        }
        else
        {
            isCollDown = false;
        }
    }

    RaycastHit2D getRayCast(Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, Color color,
        LayerMask mask,
        bool isShowGizmo = false)
    {
        if (isShowGizmo)
        {
            Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
        }

        return Physics2D.Raycast(rayOriginPoint, rayDirection, rayDistance, mask);
    }

    // 设置重力 / 漂浮
    void doGravity()
    {
        if (!isGravity) return;
        _speed.y += (Gravity * Time.deltaTime);
        if (floatingRate != 0)
        {
            _speed.y *= floatingRate;
        }
    }
}