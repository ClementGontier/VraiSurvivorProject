using UnityEngine;

public class bougejoueur : MonoBehaviour
{
    public GameObject joueur;
    private Vector2 position;
    public float vitesse = 5f;
    [SerializeField] private Animator animwg, animwd, animag, animad;

   
    private void Awake()
    {

        GameObject[] obj = GameObject.FindGameObjectsWithTag("Joueur");

        if(obj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        
        DontDestroyOnLoad(this.gameObject);
        
       
    }
    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        // je configure position.x
        position.x += moveX * vitesse * Time.deltaTime;
        // je limite les valeurs de position.x entre -7 et 7
        position.x = Mathf.Clamp(position.x, -10f, 32f);
        
        // input getaxis horizontal récupère une valeur float envoyé par le clavier
        float moveY = Input.GetAxis("Vertical");
        // je configure position.y
        position.y += moveY * vitesse * Time.deltaTime;
        // je limite les valeurs de position.y entre -7 et 7
        position.y = Mathf.Clamp(position.y, -3.5f, 19f);
        // permet de déplacer l'objet banane
        transform.position = position;
        if (moveX != 0)
        {
            if (moveX > 0)
            {
                joueur.GetComponent<SpriteRenderer>().flipX = false;
                animwd.SetBool("isRunning", true);
            }
            else if (moveX < 0)
            {
                joueur.GetComponent<SpriteRenderer>().flipX = true;
                animwg.SetBool("isRunning", true);
            }
        }
        else if (moveY != 0)
        {
            if (moveY > 0)
            {
                animwd.SetBool("isRunning", true);
            }
            else if (moveY < 0)
            {
                animwg.SetBool("isRunning", true);
            }
        }
        else
        {
            animwd.SetBool("isRunning", false);
            animwg.SetBool("isRunning", false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (joueur.GetComponent<SpriteRenderer>().flipX == true){
                animag.SetBool("isAttacking", true);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.sonepee);}
            else{
                animag.SetBool("isAttacking", true);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.sonepee);}
        }
        else
        {
            animad.SetBool("isAttacking", false);
            animag.SetBool("isAttacking", false);
        }
    }
}
