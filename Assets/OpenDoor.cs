using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class OpenDoor : MonoBehaviour, IInterectable
{
    public Transform leftDoor;
    public Transform rightDoor;
    public Animator animator;
    public int openTime = 4000;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.StopPlayback();
    }

    public void Interect()
    {
        Open();
        Debug.Log("Open");
    }

    public void Open()
    {
        OpenRoutine();
    }

    public async void OpenRoutine()
    {
        animator.SetTrigger("Open");
        await Task.Delay(openTime);
        animator.SetTrigger("Close");
    }
}
