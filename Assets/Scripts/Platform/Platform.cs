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

            switch (other.tag)
            {
                case Tags.player:
                    Player.instance.FallDamage();
                    break;

                case Tags.enemy:
                    // Enemyden miras almış class FallDamage fonksiyonunu çalıştırması için mesaj atıyoz
                    other.SendMessage(EnemyFunctions.fallDamage);
                    break;
            }
        }
    }
}