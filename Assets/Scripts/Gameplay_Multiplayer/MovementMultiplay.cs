using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementMultiplay : MonoBehaviour
{
    [SerializeField] float thrustSpeed = 1500f, rotateThrust = 200;
    [SerializeField] float autoLevelSpeed = 360f; // 自動垂直化の回転速度(度/秒)
    [SerializeField] float velocityAlignSpeed = 540f; // 慣性をロケット頂部方向へ向け直す速度(度/秒。大きいほど即座に方向転換)
    [SerializeField] float thrustSpeedUpFactor = 2f, thrustSppedNormalFactor = 1f;
    [SerializeField] float maxSpeed = 50f, slowDownSpeedFactor = 1.0005f;
    [SerializeField] Image speedDownImage;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem boostParticles;

    int playerID;
    float speedFactor, currentMaxSpeed;
    Rigidbody rb;
    AudioSource audioSource;
    PlayerStatusMultiplay playerStatus;
    bool canControl, canResetRotation;
    bool isAutoLeveling;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        playerStatus = GetComponent<PlayerStatusMultiplay>();
        Initialization();
    }

    private void Initialization()
    {
        canControl = true;
        canResetRotation = false;
        isAutoLeveling = false;
        speedFactor = thrustSppedNormalFactor;
        playerID = GetComponent<PlayerStatusMultiplay>().GetPlayerID();
        currentMaxSpeed = maxSpeed;
        speedDownImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameManagerMultiplay.GameState currentGameState = GameManagerMultiplay.Instance.GetCurrentGameState();
        if(currentGameState == GameManagerMultiplay.GameState.Finish)
        {
            return;
        }

        if (canControl)
        {
            ProcessThrust();
            ProcessRotation();
            if (canResetRotation)
            {
                ProcessResetRotation();
            }
            if (isAutoLeveling)
            {
                ProcessAutoLevel();
            }
        }
    }

    private void ProcessThrust()
    {
        // Disable keyboard Input
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || 
            Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E) ||
            Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.V) ||
            Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Space) ||
            Input.GetKey(KeyCode.Escape))
        {
            return;
        }
        if (Input.GetButton(playerID + "PBoost") && playerStatus.GetCurrentBoost() > 0)
        {
            if (Input.GetButton(playerID + "PSlowDown"))
            {
                SlowDownSpeed();
            }
            else
            {
                StartThursting();
                LimitMaxmimumSpeed();
            }
            playerStatus.ReducePlayerBoostContinously(speedFactor);
        }
        else if (Input.GetButton(playerID + "PSlowDown"))
        {
            SlowDownSpeed();
            playerStatus.ReducePlayerBoostContinously(speedFactor);
        }
        else
        {
            StopThursting();
            //playerStatus.RecoverBoost();
        }
        playerStatus.UpdatePlayerBoostSlider();
    }

    private void ProcessResetRotation()
    {
        // Disable keyboard Input
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E) ||
            Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.V) ||
            Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Space) ||
            Input.GetKey(KeyCode.Escape))
        {
            return;
        }
        if (Input.GetButton(playerID + "PResetRotation"))
        {
            ResetRotation();
        }
    }

    private void ResetRotation()
    {
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        rb.linearVelocity = Vector3.zero;
    }

    private void ResponseToBoostUp()
    {
        speedFactor = thrustSpeedUpFactor;
    }

    private void ResetSpeedFactor()
    {
        speedFactor = thrustSppedNormalFactor;
    }

    private void StopThursting()
    {
        audioSource.Stop();
        if (boostParticles != null && boostParticles.isPlaying)
        {
            boostParticles.Stop();
        }
    }

    private void StartThursting()
    {
        rb.AddRelativeForce(Vector3.up * thrustSpeed * Time.deltaTime * speedFactor);
        AlignVelocityToHeading();
        if (!audioSource.isPlaying && mainEngine != null)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (boostParticles != null && boostParticles.isStopped)
        {
            boostParticles.Play();
        }
    }
    private void LimitMaxmimumSpeed()
    {
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, currentMaxSpeed);
    }

    // 慣性(velocity)の向きをロケット頂部方向(transform.up)へ素早く向け直す。
    // 速度の大きさは保ったまま方向だけ寄せるので「頂部が向く方向にすぐ進む」操作感になる。
    private void AlignVelocityToHeading()
    {
        float speed = rb.linearVelocity.magnitude;
        if (speed < 0.01f) { return; }
        Vector3 newDir = Vector3.RotateTowards(rb.linearVelocity.normalized, transform.up,
            velocityAlignSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);
        rb.linearVelocity = newDir * speed;
    }

    public void ReduceMaximumSpeed(float reduceMaximumSpeedFactor)
    {
        currentMaxSpeed = maxSpeed - reduceMaximumSpeedFactor;
        speedDownImage.enabled = true;
    }

    public void RestoreOrginalMaximumSpeed()
    {
        currentMaxSpeed = maxSpeed;
        speedDownImage.enabled = false;
    }

    public float GetLimitedMaxVelocity()
    {
        return maxSpeed;
    }

    private void SlowDownSpeed()
    {
        rb.linearVelocity = rb.linearVelocity / slowDownSpeedFactor;
        if (!audioSource.isPlaying && mainEngine != null)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (boostParticles != null && boostParticles.isStopped)
        {
            boostParticles.Play();
        }

    }

    private void ProcessRotation()
    {
        // Disable keyboard Input
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E) ||
            Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.V) ||
            Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Space) ||
            Input.GetKey(KeyCode.Escape))
        {
            return;
        }
        if (Input.GetButton(playerID + "PRotateLeft"))
        {
            RotateLeft();
        }
        else if (Input.GetButton(playerID + "PRotateRight"))
        {
            RotateRight();
        }
    }

    private void RotateLeft()
    {
        ApplyRotation(rotateThrust);
    }
    private void RotateRight()
    {
        ApplyRotation(-rotateThrust);
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        isAutoLeveling = false; // 手動回転が入ったら自動垂直化を中断
        rb.freezeRotation = true; // Freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.angularVelocity = Vector3.zero; // 物理由来の角速度が手動回転を打ち消さないよう即応化
        rb.freezeRotation = false; // Unfreezing rotation so the physics system can take over
    }

    public void StartAutoLevelToVertical()
    {
        isAutoLeveling = true;
    }

    private void ProcessAutoLevel()
    {
        Quaternion target = Quaternion.identity; // 垂直姿勢(Euler 0,0,0)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, autoLevelSpeed * Time.deltaTime);
        rb.angularVelocity = Vector3.zero;
        if (Quaternion.Angle(transform.rotation, target) < 0.1f)
        {
            transform.rotation = target;
            isAutoLeveling = false;
        }
    }

    public void DisablePlayerControl()
    {
        canControl = false;
        rb.freezeRotation = true;
    }

    public void EnablePlayerControl()
    {
        canControl = true;
        /*
        rb.constraints = RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        */
        rb.freezeRotation = false;
    }

    public bool PlayerCanControl()
    {
        return canControl;
    }

    public void StopMovement()
    {
        rb.linearVelocity = new Vector3(0f, 0f, 0f);
        rb.angularVelocity = Vector3.zero;
    }

    public void EnableResetRotation()
    {
        canResetRotation = true;
    }

    public void DisableResetRotation()
    {
        canResetRotation = false;
    }
}
