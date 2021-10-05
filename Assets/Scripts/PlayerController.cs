using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{

    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D rigidBody;
        private float rayControl;

        const float groundedRadius = .15f;
        public LayerMask whatIsGround;

        public bool airControl = true;
        private bool isGround;
        public float maxSpeed;
        private float xMove;
        private bool isJumping;
        private int jumpCount;
        public int jumpCountMax;    // 점프 횟수
        [Header("Sound variables")]
        public AudioSource recallSound;
        public AudioSource flashSound;
        public AudioSource runningSound;

        [Header("Jump variables")]
        public float jumpForce;

        [Header("Recall variables")]
        public int recallAmount;
        public int recallFrame;
        public int recallSpeed;
        private bool recallFlag;
        [HideInInspector] private LinkedList<Vector3> positionVal = new LinkedList<Vector3>();
        [HideInInspector] private LinkedList<Vector3> scaleVal = new LinkedList<Vector3>();
        [HideInInspector] private LinkedList<Vector2> velocityVal = new LinkedList<Vector2>();
        [HideInInspector] bool isRecallFirst = true;

        [Header("Flash variables")]
        public float flashDistance;
        public int flashCount;

        public Transform ground;
        private Transform groundCheck;
        private Animator animator;
        void Start()
        {
            rayControl = 0.5f;
            isJumping = false;
        }
        void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            groundCheck = transform.Find("GroundCheck");
            jumpCount = jumpCountMax;

            GameObject.FindObjectOfType<RecallPanel>().UpdateRecallIcon(recallAmount);
            GameObject.FindObjectOfType<FlashPanel>().UpdateFlashIcon(flashCount);
        }
        void Update()
        {
            if (!isJumping)
            {
                isJumping = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (CrossPlatformInputManager.GetButtonDown("Flash"))
                flash();

            if (CrossPlatformInputManager.GetButtonDown("Recall"))
            {
                recallFlag = true;
                if (recallAmount > 0)
                    FindObjectOfType<RecallPanel>().UpdateRecallIcon(recallAmount - 1);
            }
        }
        void FixedUpdate()
        {
            CheckGrounded();

            Jump();
            isJumping = false;

            if (!recallFlag)
            {
                Move();
                animator.SetFloat("vSpeed", rigidBody.velocity.y);
            }
            recall();
        }
        void Move()
        {
            float xMove = 0f;
            if (CrossPlatformInputManager.GetButton("Left"))
                xMove = -1f;
            if (CrossPlatformInputManager.GetButton("Right"))
                xMove = 1f;

            animator.SetFloat("Speed", Mathf.Abs(xMove));
            rigidBody.velocity = new Vector2(xMove * maxSpeed, rigidBody.velocity.y);

            if (xMove > 0 && GetComponent<Transform>().localScale.x < 0 || xMove < 0 && GetComponent<Transform>().localScale.x > 0)
                Flip();

            if (xMove != 0.0f && !runningSound.isPlaying && !isJumping)
                runningSound.Play();
            if ((isGround && xMove == 0.0f) || (!isGround && isJumping))
                runningSound.Stop();

            if (Mathf.Abs(rigidBody.velocity.x) > maxSpeed)
            {
                Vector2 newVelocity;
                newVelocity.x = Mathf.Sign(rigidBody.velocity.x) * maxSpeed;
                newVelocity.y = rigidBody.velocity.y;

                rigidBody.velocity = newVelocity;
            }
            else
            {
                Vector2 newVelocity = rigidBody.velocity;

                newVelocity.x *= 0.9f;
                rigidBody.velocity = newVelocity;
            }
        }

        void Jump()
        {
            if (isJumping && jumpCount > 0)
            {
                jumpCount--;
                rigidBody.velocity = new Vector2(0f, 0f);  // 점프 보정
                isGround = false;
                animator.SetBool("Ground", isGround);
                rigidBody.AddForce(new Vector2(0f, jumpForce));
            }
        }

        void Flip()
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        //역행 구현 부분
        void recall()
        {
            if (!recallFlag)
            {
                positionVal.AddLast(transform.position);
                scaleVal.AddLast(transform.localScale);
                velocityVal.AddLast(rigidBody.velocity * -1);

                if (positionVal.Count >= recallFrame)
                {
                    positionVal.RemoveFirst();
                    scaleVal.RemoveFirst();
                    velocityVal.RemoveFirst();
                }
            }
            else
            {
                if (recallAmount <= 0)
                {
                    recallFlag = false;
                    return;
                }

                if (!GetRecallAnim())
                    SetRecallAnim();

                if (isRecallFirst)
                {
                    recallSound.Play();
                    rigidBody.isKinematic = true;
                    isRecallFirst = false;
                }

                transform.position = positionVal.Last.Value;
                transform.localScale = scaleVal.Last.Value;
                rigidBody.velocity = velocityVal.Last.Value;

                for (int i = 0; i < recallSpeed; i++)
                {
                    positionVal.RemoveLast();
                    scaleVal.RemoveLast();
                    velocityVal.RemoveLast();
                }

                if (positionVal.Count == 0)
                {
                    isRecallFirst = true;
                    rigidBody.isKinematic = false;
                    recallFlag = false;
                    recallAmount--;
                    SetRecallAnim();
                }
            }
        }

        //점멸 구현 부분
        void flash()
        {
            float horizontalDirection = 0f; //점멸 좌우방향 설정. 양수면 우측, 음수면 좌측
            if (CrossPlatformInputManager.GetButton("Left"))
                horizontalDirection = -1f;
            if (CrossPlatformInputManager.GetButton("Right"))
                horizontalDirection = 1f;


            if (flashCount <= 0 || horizontalDirection == 0) return;

            flashSound.Play();

            if (horizontalDirection > 0)
            {
                Vector2 rvt = new Vector2(flashDistance, 0);
                RaycastHit2D hit = Physics2D.Raycast(new Vector2((rigidBody.transform.position.x + rayControl), rigidBody.transform.position.y), rvt, flashDistance);
                if (hit.collider == null)
                    rigidBody.MovePosition((Vector2)transform.position + new Vector2(flashDistance, 0));
                else if (hit.collider != null)
                    rigidBody.MovePosition(hit.point);

            }
            else if (horizontalDirection < 0)
            {
                Vector2 rvt = new Vector2(-flashDistance, 0);
                RaycastHit2D hit = Physics2D.Raycast(new Vector2((rigidBody.transform.position.x - rayControl), rigidBody.transform.position.y), rvt, flashDistance);
                if (hit.collider == null)
                    rigidBody.MovePosition((Vector2)transform.position + new Vector2(-flashDistance, 0));
                else if (hit.collider != null)
                    rigidBody.MovePosition(hit.point);
            }

            flashCount--;
            GameObject.FindObjectOfType<FlashPanel>().UpdateFlashIcon(flashCount);
        }

        void UseFlash(float flashDistance, float rayControl)
        {
            Vector2 distance = new Vector2(flashDistance, 0);
            Vector2 rayDistance = new Vector2(rigidBody.transform.position.x + rayControl, rigidBody.transform.position.y);
            RaycastHit2D hit = Physics2D.Raycast(rayDistance, distance, flashDistance);
            if (hit.collider == null)
                rigidBody.MovePosition((Vector2)transform.position + new Vector2(flashDistance, 0));
            else if (hit.collider != null)
                rigidBody.MovePosition(hit.point);

        }
        void SetRecallAnim()
        {
            animator.SetBool("Recall", !animator.GetBool("Recall"));
        }
        bool GetRecallAnim()
        {
            return animator.GetBool("Recall");
        }

        void CheckGrounded()
        {
            isGround = false;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    isGround = true;
            }

            if (isGround)
                jumpCount = jumpCountMax;   // 점프 횟수 초기화

            animator.SetBool("Ground", isGround);

            // Set the vertical animation
            animator.SetFloat("vSpeed", rigidBody.velocity.y);

        }
    }
}
