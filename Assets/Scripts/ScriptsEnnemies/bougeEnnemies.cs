using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class bougeEnnemies : MonoBehaviour
{

    public GameObject joueur;
    public float speed = 2f;
    public Animator animator;
    public SpriteRenderer sr;
    public string animationName = "Move";
    private vieEnnemies vieEnnemiesScript;

    void Start()
    {
        vieEnnemiesScript = GetComponent<vieEnnemies>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private void move()
    {
        // direction vers le joueur
        Vector2 direction = (joueur.transform.position - transform.position).normalized;

        // déplacement
        Vector3 move = (Vector3)(direction * speed * Time.deltaTime);
        transform.position += move;

        // mise à jour du flip
        if (direction.x != 0)
        {
            // on récupère le scale du transform de l'ennemie
            Vector3 scale = transform.localScale;
            // si jamais il va vers la gauche, on le multiplie par 1 (change rien), sinon -1 (inverse l'axe)
            scale.x = Mathf.Abs(scale.x) * (direction.x < 0 ? 1 : -1);
            // on applique le flip
            transform.localScale = scale;
        }
        animator.Play(animationName);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Joueur")
        {
            Singleton.Instance.TakeDamage(vieEnnemiesScript.degats);
            //Debug.Log("le joueur prend 1 degat");
        }
    }
}