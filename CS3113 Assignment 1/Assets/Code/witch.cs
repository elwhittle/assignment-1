using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class witch : MonoBehaviour{
    public int dir = 1;
    public float health = 50;
    public GameObject Player;
    public Transform fire_pos;
    public GameObject Bullet1;
    public Transform bulletpos;
    public int castingForce = 50;
    public float Timebetween = 0.5f;
    Animator _animator;
    Rigidbody2D _rigidbody;
    public float Speed = 0.7f;

    private int direction = -1; 
    float _Timer;

    enum State{
        stay,
        move,
        attack,
        stage2,
        stay2,
        move2,
        attack2,
        attack3
    }
    State currentState;
    void lookplayer(){
        if (gameObject.transform.position.x < Player.transform.position.x){
            transform.eulerAngles = new Vector3(0,180,0);
            direction = 1;
        }
        else{
            transform.eulerAngles = new Vector3(0,0,0);
            direction = -1;
        }
    }
    private void Start(){
        //Vector2 currPos = transform.position;
        //float moveRight = transform.position.x;
        //float moveLeft = -transform.position.x;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        StartCoroutine(NewState());
    }
    IEnumerator NewState(){
        while(health>25){
            yield return new WaitForSeconds(1.5f);
            currentState = (State)Random.Range(0,3);
        }
        while (health<25 & health>0){
            yield return new WaitForSeconds(3);
            currentState = (State)Random.Range(4,8);
        }
    }
    void Attack1(){
        _animator.SetTrigger("Attack1");
    }
    void Attack2(){
        _animator.SetTrigger("Attack2");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Spell"))
        {
            //health -= 1;
        }
    }

    // Update is called once per frame
    private void FixedUpdate(){
        _animator.SetFloat("health", health);
        lookplayer();
        switch(currentState){
            case State.move:
                _rigidbody.velocity = new Vector2(Speed*direction, 0);
                _animator.SetFloat("speed", Mathf.Abs(_rigidbody.velocity.x));
                break;
            case State.stay:
                _rigidbody.velocity = new Vector2(0.0f,0.0f);
                _animator.SetFloat("speed", Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.x));
                break;
            case State.attack:
                _rigidbody.velocity = new Vector2(0.0f,0.0f);
                _Timer+=Time.deltaTime;
                if (_Timer>Timebetween){
                    GameObject newBlast = Instantiate(Bullet1, bulletpos.position, Quaternion.identity);
                    newBlast.GetComponent<Rigidbody2D>().AddForce(new Vector2(castingForce * transform.localScale.x*direction, 0));
                    _Timer = 0;
                }
                Attack1();
                break;
            case State.move2:
                _rigidbody.velocity = new Vector2(Speed*direction, 0);
                _animator.SetFloat("speed", Mathf.Abs(_rigidbody.velocity.x));
                break;
            case State.stay2:
                _rigidbody.velocity = new Vector2(0.0f,0.0f);
                _animator.SetFloat("speed", Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.x));
                break;
            case State.attack2:
                _rigidbody.velocity = new Vector2(0.0f,0.0f);
                Attack2();
                break;
            case State.attack3:
                _rigidbody.velocity = new Vector2(0.0f,0.0f);
                Attack2();
                break;
            default:
                break;
        }
    }
    IEnumerator attackmotion(){
        while (true){
            if(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime>1.0f){
                    break;
            }
            else{
                _rigidbody.velocity = new Vector2(0.0f,0.0f);
            }
            yield return new WaitForSeconds(0);
        }
    }
}