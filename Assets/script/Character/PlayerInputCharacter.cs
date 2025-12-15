using CharacterStateMachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerInputCharacter : Character
{
    private CharacterController controller;
    private Vector3 direction;
    private Transform cameraTransform;
    public float speed = 3f;

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<CharacterController>();
    }

    protected override void Start()
    {
        base.Start();
        cameraTransform = Camera.main.transform;
    }

    protected override void Update()
    {
        base.Update();

        if (direction.magnitude >= 0.01f)
        {
            // 카메라 기준 이동 방향 변환
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 lookDir = camForward * direction.z + camRight * direction.x;
            Vector3 moveDir = lookDir + Vector3.up * direction.y;

            controller.Move(moveDir * speed * Time.deltaTime);

            // 캐릭터 회전 (이동 방향 바라보기)
            if (lookDir != Vector3.zero)
            {
                LookDirection(lookDir);
            }
        }
    }

    private void LookDirection(Vector3 lookDir)
    {
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

    }

    public override void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
}
