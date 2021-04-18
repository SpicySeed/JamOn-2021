using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Key : Collectable
{
    [SerializeField] GameObject boss;
    [SerializeField] CinemachineShake shake;
    [SerializeField] SpriteRenderer tutorial;
    [SerializeField] SpriteRenderer keyRenderer;
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeTime;

    [SerializeField] private GameObject lightMed;
    [SerializeField] private StudioEventEmitter music;
    [SerializeField] private GameObject lightFoc;

    int picked = 0;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && picked == 0 && collision.gameObject.GetComponent<Health>().IsAlive())
        {
            picked = 1;
            tutorial.enabled = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && picked == 1)
        {
            picked = 0;
            tutorial.enabled = false;
        }
    }

    private void Update()
    {
        if (picked == 1 && Input.GetKeyDown(KeyCode.E))
        {
            shake.ShakeCamera(shakeIntensity, shakeTime);
            RuntimeManager.PlayOneShotAttached("event:/Llave", this.gameObject);
            RuntimeManager.PlayOneShotAttached("event:/Terremoto", this.gameObject);
            music.EventInstance.setParameterByName("Boss", 1);
            keyRenderer.enabled = false;
            GameManager.Instance.SetInputFreeze(true);
            GameManager.Instance.StopTimer(true);
            tutorial.enabled = false;
            picked = 2;
        }

        if (picked == 2 && !shake.isShaking())
        {
            GameManager.Instance.SetInputFreeze(false);
            GameManager.Instance.StopTimer(false);
            picked = 3;
            boss.SetActive(true);
            boss.GetComponent<EnemyFollow>().ResetHair();
            lightMed.SetActive(true);
            lightFoc.SetActive(true);
        }
    }

    public bool HasBeenPicked()
    {
        return picked == 3;
    }
}
