using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthBar;

    public Health pHealth;
     

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
     void Update()
    {
      health = pHealth.health;
      maxHealth = pHealth.maxHealth;

       healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);

        /*    if (health <= 0)
            {
                Destroy(gameObject);
            }*/
    }


}
