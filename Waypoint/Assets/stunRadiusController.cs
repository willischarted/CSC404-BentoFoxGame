using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stunRadiusController : MonoBehaviour {
    float timer;

    public float maxBound;
    public float minBound;
    public float growthFactor;
    public float waitTime;

    MeshRenderer mRenderer;
    InteractionControllerCopy iController;
    // Use this for initialization
    void Start () {

        iController = GetComponentInParent<InteractionControllerCopy>();

        if (iController == null)
        {
            Debug.Log("Could not find the iController script in parent");
        }
        mRenderer = gameObject.GetComponent<MeshRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("Square"))
        {
            StartCoroutine("scaleStunRadius");
            /*
            timer += Time.deltaTime;
            if (timer >= 0.1f)
            {
                timer = 0f;
                transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x + 1f, 0, maxBound), transform.localScale.y, Mathf.Clamp(transform.localScale.z + 1f, 0, maxBound));


            }
            */


        }

        if (Input.GetMouseButtonUp(1) || Input.GetButtonUp("Square"))
        {

            StopAllCoroutines();

            transform.localScale = new Vector3(minBound, transform.localScale.y, minBound);

            mRenderer.enabled = false;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other is CapsuleCollider)
        {
            if (other.tag == "Monster")
            {
                EnemyMovement monScript = other.gameObject.GetComponent<EnemyMovement>();
                if (!monScript.getIsStunned())
                {

                    if (!iController.monstersInRange.Contains(other.gameObject))
                    {
                        other.GetComponent<EnemyMovement>().setLitOutline();
                        iController.monstersInRange.Add(other.gameObject);
                    }
                }
            }
            else
            {
                iController.getMonsters().Remove(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "Monster" && other is CapsuleCollider)
        {
            Debug.Log("exit");
            other.GetComponent<EnemyMovement>().resetLitOutline();
            iController.getMonsters().Remove(other.gameObject);
        }
    }


    IEnumerator scaleStunRadius()
    {

        mRenderer.enabled = true;
        float timer = 0;

        while (true) // this could also be a condition indicating "alive or dead"
        {
            // we scale all axis, so they will have the same value, 
            // so we can work with a float instead of comparing vectors
            while (maxBound > transform.localScale.x)
            {
                timer += Time.deltaTime;
                transform.localScale += new Vector3(1, 0, 1) * Time.deltaTime * growthFactor;
                yield return null;
            }
            // reset the timer

            yield return new WaitForSeconds(waitTime);
            /*
            timer = 0;
            while (1 < transform.localScale.x)
            {
                timer += Time.deltaTime;
                transform.localScale -= new Vector3(1, 0, 1) * Time.deltaTime * growthFactor;
                yield return null;
            }

            timer = 0;
            yield return new WaitForSeconds(waitTime);
            */
        }
    }
}
