using UnityEngine;

using System.Collections;

public class cameraFacingBillboard : MonoBehaviour
{
    public Camera m_Camera;

    public GameObject textObject; 

    public GameObject nextTutorialText;

    public GameObject player;
    playerControllerCopy pScript;

    public bool isLookTutorial;
    public bool isMoveTutorial;
    public bool isLightTutorial;

    public bool isIntermmediate;
    public bool isIntermmediateUnLock;

    public LayerMask myMask;

    public GameObject targetObject;
    public void Start()
    {
        pScript = player.gameObject.GetComponent<playerControllerCopy>();

        if (pScript == null)
        {
            Debug.Log("could not find");
        }


        if (isLookTutorial)
        {
            Invoke("setTextActive", 0.2f);
            setLookTutorial();
            StartCoroutine("lookTutorial");
           
            //OnDrawGizmosSelected();
        }
        if (isMoveTutorial)
        {
            Invoke("setTextActive", 0.2f);
            setMoveTutorial();
            StartCoroutine("moveTutorial");
            

        }
        if (isLightTutorial)
        {
            Invoke("setTextActive", 0.2f);
            setLightTutorial();
            StartCoroutine("lightTutorial");

        }
        if (isIntermmediate)
        {
            Invoke("setTextActive", 0.2f);
            setIntermediatePhase();
            StartCoroutine("intermeddiatePhase");
            
        }
        if (isIntermmediateUnLock)
        {
            Invoke("setTextActive", 0.2f);
            setIntermediatePhaseUnlock();
            StartCoroutine("intermeddiatePhase");

        }



    }

    public void Update()
    {

    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
       
        //transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
         //   m_Camera.transform.rotation * Vector3.up);
    }

    public void setNextTutorial()
    {
        if (nextTutorialText == null)
        {
            pScript.setInTutorial(false);
        }

        else
        {
            
           
            pScript.TutorialText = nextTutorialText;
            nextTutorialText.SetActive(true);// = true;
            Destroy(this.gameObject);

           
        }
    } 

    public void setLightTutorial()
    {
        //pScript.setRestrictMovement(false);
        pScript.setInTutorial(false);

    }

    void setTextActive()
    {
        textObject.SetActive(true);
       
    }

    void setIntermediatePhase()
    {
        pScript.setInTutorial(true);
    }
    void setIntermediatePhaseUnlock()
    {
        pScript.setInTutorial(false);
    }
    void setLookTutorial()
    {
        pScript.setInTutorial(true);
        //pScript.setRestrictMovement(true);
    }


    public IEnumerator lookTutorial() {
        while (true)
        {
            RaycastHit hit;
            Ray ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out hit, 100f, myMask))
            {
                if (hit.collider != null)
                {
                    //ebug.DrawRay(transform.position, transform.forward, Color.green);
                    //rint("Hit");

                    if (hit.collider.gameObject.tag == "tutorial")
                    {
                        //Debug.Log(hit.collider.gameObject);

                        Destroy(textObject);
                        if (targetObject != null)
                            Destroy(targetObject);
                        //yield return new WaitForSeconds(1f);
                        setNextTutorial();
                    }
                }
            }
            yield return null;
        }
    }

    public void setMoveTutorial()
    {
        //pScript.setRestrictMovement(true);
        pScript.setInTutorial(true);
    }

    public IEnumerator moveTutorial()
    {   while (true)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            if (moveHorizontal != 0 || moveVertical != 0)
            {
                pScript.setRestrictMovement(false);

                Destroy(textObject);
                if (targetObject != null)
                    Destroy(targetObject);
                //yield return new WaitForSeconds(1f);
                setNextTutorial();
            }
            yield return null;
        }
        
    }

    public IEnumerator intermeddiatePhase()
    {
        float timer = 0f;
       
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(textObject);
        setNextTutorial();

    
    }

    public IEnumerator lightTutorial()
    {
        float timer = 0f;

        while (timer < 5.5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(textObject);
        setNextTutorial();


    }
    /*
    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
        Gizmos.DrawRay(transform.position, direction);
    }
    */
}