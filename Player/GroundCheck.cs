using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] Image blocking;
    [SerializeField] PlayerActionBase playerAction;
    [SerializeField] Animator animator;
    Rigidbody rig;
    PlayerTag playerTag;
    
    public bool isJump = false;
    public bool JumpingMotion { get; private set; } = false;
    public bool UpClick {  get; set; }

    private void Awake()
    {
        playerAction = GetComponentInParent<PlayerActionBase>();
        animator = GetComponentInParent<Animator>();
        rig = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        playerTag = GameObject.FindObjectOfType<PlayerTag>();
    }

    private void FixedUpdate()
    {
        if(isJump)
        {
            isJump = false;
            JumpingMotion = true;
            playerTag.Jumping = true;
            playerAction.JumpMotion();
        }
    }

    private void Update()
    {
        if (blocking.enabled)
            return;

        if (playerTag.Tagging)
            return;

        Jump_DownJump_Check();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }

    void Jump_DownJump_Check()
    {
        int mask1 = 1 << 6 | 1 << 8;
        int mask2 = 1 << 9;
        Collider[] groundData = Physics.OverlapSphere(transform.position, 0.1f, mask1);
        Collider[] floorData = Physics.OverlapSphere(transform.position, 0.1f, mask2);

        if ((Input.GetKeyDown(KeyCode.UpArrow) || UpClick) && (groundData.Length >= 1 || floorData.Length >= 1) && rig.velocity == Vector3.zero)
        {
            UpClick = false;
            isJump = true;
        }

        if (groundData.Length >= 1 || floorData.Length >= 1)
        {
            JumpingMotion = false;
            playerTag.Jumping = false;
        }

        if (JumpingMotion)
        {
            animator.SetBool("Jumping", true);
        }
        else
        {
            animator.SetBool("Jumping", false);
        }

        /// floorData에 데이터가 1개이상 들어있고
        /// 윗 방향키를 눌렀다면 밟고있는 바닥콜라이더를 끄고 하강점프한다.
        /// 0.6초후 바닥콜라이더는 다시 켜진다.
        if (Input.GetKeyDown(KeyCode.DownArrow) && floorData.Length >= 1)
        {
            Collider floorCollider = floorData[0];
            floorCollider.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(FloorColliderReset(floorCollider));
        }

        IEnumerator FloorColliderReset(Collider floorCollider)
        {
            yield return new WaitForSeconds(0.6f);
            floorCollider.enabled = true;
        }
    }
}
