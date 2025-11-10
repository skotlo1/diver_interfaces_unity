using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public GameObject InfoBoard;

    private void OnTriggerEnter(Collider other)
    {
        InfoBoard.SetActive(false);
    }
    

    // Update is called once per frame
    private void OnTriggerExit(Collider other)
    {
        InfoBoard.SetActive(true);
        
    }
}
