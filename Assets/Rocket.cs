using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rb;
    AudioSource sound;
    [SerializeField] AudioClip yay;
    [SerializeField] AudioClip bakoom;
    [SerializeField] AudioClip thrust;
    [SerializeField] float thrusterPower = 1000f;
    [SerializeField] float rcsPower = 100f;
    [SerializeField] ParticleSystem pThrust;
    [SerializeField] ParticleSystem pWin;
    [SerializeField] ParticleSystem pDeath;

    enum State { Alive, Dying, Transcending}
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        //get ref to rigid body
        rb = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                state = State.Transcending;
                pWin.Play();
                sound.Stop();
                sound.PlayOneShot(yay);
                print("Hit finish!");
                Invoke("LoadNextScene", 1f);
                break;
            default:
                state = State.Dying;
                sound.Stop();
                sound.PlayOneShot(bakoom);
                pDeath.Play();
                print("You died!");
                Invoke("StartOver", 2.5f);
                break;
        }
    }

    private void StartOver()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            sound.Stop();
            pThrust.Stop();
        }
    }

    private void ApplyThrust()
    {
        if (!sound.isPlaying)
        {
            sound.PlayOneShot(thrust);
        }
        rb.AddRelativeForce(Vector3.up * thrusterPower * Time.deltaTime);
        pThrust.Play();
    }

    private void RespondToRotateInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.freezeRotation = true; //manual control of rotation
            transform.Rotate(Vector3.forward * rcsPower * Time.deltaTime);
            rb.freezeRotation = false; //release rotation
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.freezeRotation = true; //manual control of rotation
            transform.Rotate(-Vector3.forward * rcsPower * Time.deltaTime);
            rb.freezeRotation = false; //release rotation
        }
    }
}
