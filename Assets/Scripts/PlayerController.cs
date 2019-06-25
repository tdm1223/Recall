using System;
using System.Collections;
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
        public int jumpCountMax;  
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
            {
                Debug.Log("flash");
                flash();
            }

            if (CrossPlatformInputManager.GetButtonDown("Recall"))
            {
                recallFlag = true;
                if (recallAmount > 0)
                {
                    FindObjectOfType<RecallPanel>().UpdateRecallIcon(recallAmount - 1);
                }
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
            Vector3 velocity;
            if (Input.GetAxis("Horizontal") < 0)
            {
                runningSound.Play();
                velocity = Vector3.left;
                Flip(-1);
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                runningSound.Play();
                velocity = Vector3.right;
                Flip(1);
            }
            else
            {
                velocity = Vector3.zero;
                runningSound.Stop();
            }
            transform.position += velocity * maxSpeed * Time.deltaTime;
            animator.SetFloat("Speed", Mathf.Abs(velocity.x));
        }

        void Jump()
        {
            if (isJumping && jumpCount > 0)
            {
                jumpCount--;
                rigidBody.velocity = new Vector2(0f, 0f);
                isGround = false;
                animator.SetBool("Ground", isGround);
                rigidBody.AddForce(new Vector2(0f, jumpForce));
            }
        }

        void Flip(int flag)
        {
            Vector3 theScale = transform.localScale;
            theScale.x = flag;
            transform.localScale = theScale;
        }

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

        void flash()
        {
            float horizontalDirection = 0f; 
            if (CrossPlatformInputManager.GetButton("Left"))
            { 
                horizontalDirection = -1f; 
            }
            if (CrossPlatformInputManager.GetButton("Right"))
            { 
                horizontalDirection = 1f; 
            }

            if (flashCount <= 0 || horizontalDirection == 0)
            { 
                return;
            }

            flashSound.Play();
            if (horizontalDirection > 0)
            {
                Vector2 rvt = new Vector2(flashDistance, 0);
                RaycastHit2D hit = Physics2D.Raycast(new Vector2((rigidBody.transform.position.x + rayControl), rigidBody.transform.position.y), rvt, flashDistance);
                Debug.Log(hit.collider);
                if (hit.collider == null)
                {
                    transform.position += new Vector3(flashDistance, 0, 0);
                }
                else if (hit.collider != null)
                {
                    transform.position = hit.point;
                }

            }
            else if (horizontalDirection < 0)
            {
                Vector2 rvt = new Vector2(-flashDistance, 0);
                RaycastHit2D hit = Physics2D.Raycast(new Vector2((rigidBody.transform.position.x - rayControl), rigidBody.transform.position.y), rvt, flashDistance);
                if (hit.collider == null)
                {
                    transform.position += new Vector3(-flashDistance, 0, 0);
                }
                else if (hit.collider != null)
                {
                    transform.position = hit.point;
                }
            }

            flashCount--;
            GameObject.FindObjectOfType<FlashPanel>().UpdateFlashIcon(flashCount);
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
                jumpCount = jumpCountMax;   // ���� Ƚ�� �ʱ�ȭ

            animator.SetBool("Ground", isGround);

            // Set the vertical animation
            animator.SetFloat("vSpeed", rigidBody.velocity.y);

        }
    }
}
