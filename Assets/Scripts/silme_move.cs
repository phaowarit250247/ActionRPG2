using UnityEngine;
using UnityEngine.UI;

public class silme_move : MonoBehaviour
{
    public Image hpbar;
    public float Hp = 100f;

    public Transform target;
    public float moveSpeed = 2f;
    public float attackDistance = 2f; // ระยะโจมตี
    public float detectDistance = 10f; // ระยะเริ่มสนใจ Player

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        UpdateUI(); // เรียกครั้งแรกให้ hpbar อัปเดตตั้งแต่เริ่ม
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > detectDistance)
        {
            anim.SetBool("isAttacking", false);
        }
        else if (distance > attackDistance)
        {
            Vector3 direction = (target.position - transform.position);
            direction.y = 0;
            direction = direction.normalized;

            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(direction);

            anim.SetBool("isAttacking", false);
        }
        else
        {
            anim.SetBool("isAttacking", true);
        }

        // ✅ ให้ UI อัปเดตทุกเฟรม
        UpdateUI();
    }

    void UpdateUI()
    {
        hpbar.fillAmount = Mathf.Clamp01(Hp / 100f);
    }

    // 🗡 ถ้าถูก Sword ชน ให้ลด HP
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword") && CompareTag("Enemy"))
        {
            TakeDamage(20f); // ลด HP ลง 20 (กำหนดได้เอง)
        }
    }

    void TakeDamage(float damage)
    {
        Hp -= damage;
        Hp = Mathf.Clamp(Hp, 0, 100); // กันไม่ให้ติดลบ
        UpdateUI();

        if (Hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // ทำอนิเมชันตายหรือ Destroy ศัตรู
        Debug.Log("Enemy Died!");
        Destroy(gameObject);
    }
}
