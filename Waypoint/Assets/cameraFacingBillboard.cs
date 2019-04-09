using UnityEngine;

using System.Collections;

public class cameraFacingBillboard : MonoBehaviour
{
    public Camera m_Camera;
    private RotateCamera camScript;
    public GameObject textObject; 

    public GameObject nextTutorialText;

    public GameObject player;
    playerControllerCopy pScript;

    public bool isWorldSpace;
    public bool isLookTutorial; //make them look at position
    public bool isMoveTutorial;
    public bool isLightTutorial;
    public bool isInfoTutorial;
    public bool isLookTargetTutorial; //force the camera at position
    public bool isIntermmediate;
    public bool isIntermmediateUnLock;

    public LayerMask myMask;

    public GameObject[] targetObjects;
    
    public void Start()
    {
        camScript = m_Camera.GetComponent<RotateCamera>();
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

       
        if (isInfoTutorial)
        {
            Invoke("setTextActive", 0.2f);
            Invoke("setTargetActive", 0.2f);
            setInfoTutorial();
            StartCoroutine("infoPhase");
        }
        if (isLookTargetTutorial)
        {
            setLookTargetTutorial();
            Invoke("lookAtTarget", 0.2f);
            StartCoroutine("lookTargetPhase");
        }





    }

    public void Update()
    {

    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        if (isWorldSpace)
        {
            transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
              m_Camera.transform.rotation * Vector3.up);
            this.gameObject.transform.position = player.transform.position;

        }
    }

    public void setNextTutorial()
    {
        pScript.TutorialText = nextTutorialText;
        if (nextTutorialText != null)
        {
            nextTutorialText.SetActive(true);// = true;
        }
        Destroy(this.gameObject);

        /*
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
        */
    }

    public void setLightTutorial()
    {
        //pScript.setRestrictMovement(false);
        pScript.setInTutorial(false);

    }
    public void setLookTargetTutorial()
    {
        camScript.inTutorial = true;
        Debug.Log("Setting camera to tutorial");
        pScript.setInTutorial(true);
    }

    public void setInfoTutorial()
    {
        //pScript.setRestrictMovement(false);
        pScript.setInTutorial(true);

    }

    public void restrictionReset(int degree)
    {
        Debug.Log("reseting all restrictions");
        if (degree == 0)
        {
            pScript.setInTutorial(false);
        }
        else if (degree == 1)
        {
            camScript.inTutorial = false;
            pScript.setInTutorial(false);
        }
    }
    void setTextActive()
    {
        textObject.SetActive(true);
       
    }

    void setTargetActive()
    {
        if (targetObjects.Length != 0)
        {
            foreach (GameObject g in targetObjects)
                g.SetActive(true);
        }

    }
    /*
    void lookAtTarget()
    {
        m_Camera.transform.LookAt(targetObject.transform);
    }
    */
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
                        if (targetObjects.Length != 0)
                        {
                            foreach (GameObject g in targetObjects)
                              Destroy(g);
                        }
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
                if (targetObjects.Length != 0)
                {
                    foreach (GameObject g in targetObjects)
                        Destroy(g);
                }
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

    public IEnumerator infoPhase()
    {
        float timer = 0f;

        while (timer < 0.2f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        while (true)
        {
            if ((Input.GetButtonDown("X") || Input.GetMouseButtonDown(0)))
            {
                restrictionReset(0);

                Destroy(textObject);
                if (targetObjects.Length != 0)
                {
                    foreach (GameObject g in targetObjects)
                        Destroy(g);
                }

                setNextTutorial();
            }
           
            yield return null;
        }
        


    }

    public IEnumerator lookTargetPhase()
    {
        

        while (true)
        {
            if ((Input.GetButtonDown("X") || Input.GetMouseButtonDown(0)))
            {
                Destroy(textObject);
                if (targetObjects.Length != 0)
                {
                    foreach (GameObject g in targetObjects)
                        Destroy(g);
                }
                restrictionReset(1);
                setNextTutorial();
            }
            //lookAtTarget();
            yield return null;
        }



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