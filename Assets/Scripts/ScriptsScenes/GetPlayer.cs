using Unity.Cinemachine;
using UnityEngine;

public class GetPlayer : MonoBehaviour
{
    public CinemachineCamera camerascene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camerascene = gameObject.GetComponent<CinemachineCamera>();
        camerascene.Follow = GameObject.FindGameObjectWithTag("Joueur").GetComponent<Transform>();
    }
}
