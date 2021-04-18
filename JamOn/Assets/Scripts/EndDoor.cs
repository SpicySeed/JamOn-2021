using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class EndDoor : MonoBehaviour
{
    [SerializeField] private CinemachineShake shake;
    [SerializeField] private EnemyFollow boss;
    [SerializeField] private SpriteRenderer tutorial;
    [SerializeField] private Animator anim;
    [SerializeField] private Key key;
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeTime;
    private int opened = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null && playerHealth.IsAlive() && key.HasBeenPicked())
            {
                opened = 1;
                tutorial.enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (opened == 1 && playerHealth != null && playerHealth.IsAlive())
            {
                opened = 0;
                tutorial.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (opened == 1 && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.StopTimer(true);
            GameManager.Instance.SetInputFreeze(true);
            opened = 2;
            shake.ShakeCamera(shakeIntensity, shakeTime);
            tutorial.enabled = false;
            boss.StopChase();
            RuntimeManager.PlayOneShotAttached("event:/Puerta abriendose", this.gameObject);
            anim.Play("Open");
        }
        else if (opened == 2 && !shake.isShaking())
        {
            GameManager.Instance.LoadScene("Cinematic2");
        }
    }
}
