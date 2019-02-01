using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapShield : MonoBehaviour {

    private const string EFFECT_PATH = "WallEffect";

    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーか弾かボスが壁に触れたときエフェクトを出す
        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Boss") ||
            collision.gameObject.layer.ToString() == ("EnemyAttack"))
        {

            Debug.Log("HitWall");

            ContactPoint contact = collision.contacts[0];
            GameObject effect = Instantiate(Resources.Load(EFFECT_PATH)) as GameObject;

            effect.transform.position = contact.point;

            // 向き
            effect.transform.LookAt(effect.transform.position + new Vector3(0,0,-1));
        }
    }
}
