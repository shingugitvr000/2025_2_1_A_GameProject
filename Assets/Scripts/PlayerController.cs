using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�̵� ����")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10;

    [Header("���� ����")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;                          //�߷� �ӵ� �߰�
    public float landingDuration = 0.3f;                    //���� �� ���� ���� �ð�

    [Header("���� ����")]
    public float attackDuration = 0.8f;                     //���� ���� �ð� 
    public bool canMoveWhileAttacking = false;              //������ �̵� ���� ���� 

    [Header("������Ʈ")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;

    //���� ����
    private float currentSpeed;
    private bool isAttacking = false;                           //���������� üũ 
    private bool isLanding = false;                             //���� ������ Ȯ��
    private float landingTimer;                                 //���� Ÿ�̸�

    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;                                   //���� �����ӿ� �� �̾����� 
    private float attackTimer;                                   


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        HandleLanding();
        HandleMovement();       
        HandleJump();
        HandleAttack();
        UpdateAnimator();
    }

    void CheckGrounded()
    {
        //���� ���� ����
        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;                             //ĳ���� ��Ʈ�ѷ����� �޾ƿ´�. 

        if (!isGrounded && wasGrounded)                                 //������ ���������� (���� �������� ���� �ƴϰ�, ���� �������� ��)
        {
            Debug.Log("�������� ����");
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            //���� ��� Ʈ���� �� ���� ���� ����
            if (!wasGrounded && animator != null)
            {
                //animator.SetTrigger("landTrigger");
                isLanding = true;
                landingTimer = landingDuration;
                Debug.Log("����");
            }
        }
    }

    void HandleLanding()
    {
        if(isLanding)
        {
            landingTimer -= Time.deltaTime;                 //���� Ÿ�̸� �ð� ��ŭ �� ������
            if(landingTimer <= 0)
            {
                isLanding = false;                      //���� �Ϸ�
            }
        }
    }

    void HandleAttack()
    {
        if(isAttacking)                                             //���� ���϶�
        {
            attackTimer -= Time.deltaTime;                          //Ÿ�̸Ӹ� ���� ��Ų��.
            if(attackTimer <= 0)
            {
                isAttacking = false;
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking)        //�������� �ƴҶ� Ű�� ������ ����
        {
            isAttacking = true;                                     //������ ǥ��
            attackTimer = attackDuration;                           //Ÿ�̸� ����

            if(animator != null)
            {
                animator.SetTrigger("attackTrigger");
            }
        }                
    }


    void HandleJump()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)       //�� ���� �������� ������ �� �� �ִ�.
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator != null)
            {
                animator.SetTrigger("jumpTrigger");
            }
        }

        if(!isGrounded)                                 //���� ���� ���� ��� �߷� ����
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    
    }


    void HandleMovement()                   //�̵� �Լ� ����
    {
        if((isAttacking && !canMoveWhileAttacking) || isLanding)                //���� ���̰ų� ���� ���� �� ������ ����
        {
            currentSpeed = 0;
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float verical = Input.GetAxis("Vertical");

        if (horizontal != 0 || verical != 0)                    //������ �ϳ��� �Է��� ������ 
        {
            //ī�޶� ���� ������ �������� �ǰ� ���� 
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * verical + cameraRight * horizontal;         //�̵� ���� ����

            if (Input.GetKey(KeyCode.LeftShift))                        //���� Shift �� ������ �� ���� ���� 
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            controller.Move(moveDirection * currentSpeed * Time.deltaTime);     //ĳ���� ��Ʈ�ѷ��� �̵� �Է�

            //�̵� ���� ������ �ٶ󺸸鼭 �̵�
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed = 0;                                               //�̵��� �ƴ� ��� ����Ʈ 0
        }
    }

    void UpdateAnimator()
    {
        //��ü ��� �ӵ�(runSpeed) �������� 0 ~ 1 ���
        float animatorSpeed = Mathf.Clamp01(currentSpeed / runSpeed);
        animator.SetFloat("speed", animatorSpeed);
        animator.SetBool("isGrounded", isGrounded);

        bool isFalling = !isGrounded && velocity.y < -0.1f;                 //ĳ������ Y �� �ӵ��� ������ �Ѿ�� �������� �ִٰ� �Ǵ�
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isLanding", isLanding);
    }
}
