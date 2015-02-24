using UnityEngine;
using System.Collections;

public class GiveDamageToEnemy: MonoBehaviour {

    public int DamageToGive = 1;

    private Vector2
        _lastPosition,
        _velocity;

    public void LateUpdate()
    {
        _velocity = (_lastPosition - (Vector2)transform.position) / Time.deltaTime;
        _lastPosition = transform.position;

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var enemyT = other.tag == "Enemy" ? other : null;
        if (enemyT == null)
            return;
        var enemy = (ITakeDamage) other.GetComponent<MonoBehaviour>().GetComponent(typeof(ITakeDamage));
        enemy.TakeDamage(1, gameObject);
        var controller = enemyT.GetComponent<CharacterController2D>();
        var totalVelocity = controller.Velocidade + _velocity;

        controller.SetForce(new Vector2(
            -1 * Mathf.Sign(totalVelocity.x) * Mathf.Clamp(Mathf.Abs(totalVelocity.x) * 5, 5, 5),
            -1 * Mathf.Sign(totalVelocity.y) * Mathf.Clamp(Mathf.Abs(totalVelocity.y) * 2, 0, 5)));

        //enemy.gameObject.SetActive(false);

    }   


}
