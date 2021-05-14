using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bullet;
    public Transform tr;
    public LayerMask isLayer;


    public decimal coolTime;
    private int agi;
    private decimal crip;
    private bool isShooting;
    private void Start()
    {
        //coolTime=DemoDataManager.characterDatasList[0].itemspeed-DemoDataManager.characterDatasList[0].allagi*0.005m;
        coolTime = 0.2m; //총속
        agi = 15;
        isShooting = false;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Z) && isShooting == false)
        {
            StartCoroutine(shoot());
        }
    }
    IEnumerator shoot()
    {
        isShooting = true;
        GameObject bulletclone = Instantiate(bullet, tr.position, tr.rotation);
        Rigidbody2D rigid = bulletclone.GetComponent<Rigidbody2D>();
        if (this.gameObject.transform.localScale.x==-0.5f|| Input.GetAxisRaw("Horizontal") == 1)
        {
            bullet.transform.localScale = new Vector3(0.7f, 0.7f, 1);
            rigid.AddForce(Vector2.right * 6, ForceMode2D.Impulse);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1|| this.gameObject.transform.localScale.x == 0.5f)
        {
            bullet.transform.localScale = new Vector3(-0.7f, 0.7f, 1);
            rigid.AddForce(Vector2.left * 6, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(decimal.ToSingle(coolTime));
        isShooting = false;
        Destroy(bulletclone, 0.8f);    
    }

}
