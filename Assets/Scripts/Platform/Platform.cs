using UnityEngine;

public class Platform : MonoBehaviour
{
    // Oyuncu yada düşmanlar platformdan çıkarsa yer çekii ekliyip düşmeleri sağlanıyor
    private void OnTriggerExit2D(Collider2D other)
    {
        // Nesnenin tagı Player yada Enemy ise çalışıcak
        if (other.tag != Tags.cloud)
        {
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            other.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            if (other.tag == Tags.player)
            {
                Player.instance.FallDamage();
            }
            else if (other.tag == Tags.enemy)
            {
                other.GetComponent<Enemy>().FallDamage();
            }
        }
    }
}