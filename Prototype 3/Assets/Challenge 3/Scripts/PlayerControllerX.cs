using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    public float bounceForce = 10.0f; // 바닥에서 튕겨지는 힘

    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip boingSound;

    // 추가: 플레이어가 충분히 낮은지 확인하는 변수
    private bool isLowEnough = true;
    public float upperLimit = 9.0f; // 플레이어의 최대 높이 제한

    void Start()
    {
        playerRb = GetComponent<Rigidbody>(); // 초기화
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    
    void Update()
    {
        isLowEnough = transform.position.y < upperLimit;

        // 스페이스바가 눌리고 게임이 끝나지 않았으며, 플레이어가 충분히 낮은 경우에만 위로 힘을 더함
        if (Input.GetKey(KeyCode.Space) && !gameOver && isLowEnough)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }
        else if (transform.position.y >= upperLimit)
        {
            // 플레이어가 upperLimit에 도달했을 때, 더 이상 힘을 추가하지 않음
            playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
        }

        if (Input.GetKey(KeyCode.Space) && !gameOver && isLowEnough)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        }


        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            playerAudio.PlayOneShot(boingSound, 1.0f);
            playerRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse); // 바닥에서 튀어 오름
        }

    }
}
