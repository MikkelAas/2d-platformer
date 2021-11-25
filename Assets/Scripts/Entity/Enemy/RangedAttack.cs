using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour {

    Transform _player;
    Vector3 _movementDirection;
    float _timeLeft = 2f;

    // Start is called before the first frame update
    void Start() {
        _player = PlayerEntity.INSTANCE.transform;
        _movementDirection = (_player.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update() {
        if (_timeLeft > 0) {
            transform.Translate(_movementDirection * Time.deltaTime * 5);
            _timeLeft -= Time.deltaTime;
        } else {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            collision.GetComponent<Entity>().InflictDamage(1, transform.position, 5);
        }
    }
}
