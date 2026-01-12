using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [Header("Setting")]
    public int expAmount = 10;

    private Transform player;
    private bool isMagnet = false;
    private void OnEnable()
    {
        isMagnet = false;
        player = null;
        GetComponent<Collider>().enabled = true;
    }
    private void Update()
    {
        if(player == null)
        {
            var p = FindAnyObjectByType<PlayerController>();
            if (p != null) player = p.transform;
            return;
        }
        if (isMagnet)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, 15f * Time.deltaTime);
        }
        else if (Vector3.Distance(transform.position, player.position) < 3f) { isMagnet = true; }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameManager.Instance != null) GameManager.Instance.GetExp(expAmount);
            gameObject.SetActive(false);
        }
    }
}
