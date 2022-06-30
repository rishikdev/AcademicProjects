using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    private const float SELECT_TIME = 1;
    private const float PROGRESS_BAR_WIDTH = 200;
    private const float PROGRESS_BAR_HEIGHT = 10;
    private const float HOVER_TIME = 2f;
    private const string GAMEOBJECT_TAG = "Object";

    public Transform rightIndexTip;
    public Transform leftIndexTip;
    public GameObject interactionMenu;
    public Toggle toggleGravity;
    public GameObject progressBarContext;
    public Button buttonRed;
    public Button buttonGreen;
    public Button buttonBlue;
    public Button buttonExit;
    public GameObject axes;
    public GameObject contextMenu;

    private Client client;
    private Camera cam;
    private GameObject previousButton;
    private GameObject currentButton;
    private RectTransform progressContextForeground;
    private Rigidbody objectRigidbody;
    private GameObject[] gameObjects;

    private float selectTimer;
    private int xOffset = 200;
    private int yOffset = 50;
    private float hoverTimer;
    private bool callContextMenu;

    // Start is called before the first frame update
    void Start()
    {
        client = FindObjectOfType<Client>();
        cam = Camera.main;
        interactionMenu.SetActive(false);
        axes.SetActive(false);
        contextMenu.SetActive(false);

        progressContextForeground = progressBarContext.transform.Find("Progress").GetComponent<RectTransform>();

        objectRigidbody = GetComponent<Rigidbody>();

        if (gameObjects == null)
            gameObjects = GameObject.FindGameObjectsWithTag(GAMEOBJECT_TAG);
    }

    // Update is called once per frame
    void Update()
    {
        if (callContextMenu)
        {
            ContextButtonInteraction();
            toggleGravity.isOn = objectRigidbody.useGravity;
        }
    }

    private void ContextButtonInteraction()
    {
        disableContextMenu();

        Vector2 leftIndexTipPixel = cam.WorldToScreenPoint(leftIndexTip.position);
        Vector2 rightIndexTipPixel = cam.WorldToScreenPoint(rightIndexTip.position);

        interactionMenu.SetActive(false);
        axes.SetActive(false);

        if ((rightIndexTipPixel.x >= toggleGravity.transform.position.x - xOffset && rightIndexTipPixel.x <= toggleGravity.transform.position.x + xOffset
            && rightIndexTipPixel.y >= toggleGravity.transform.position.y - yOffset && rightIndexTipPixel.y <= toggleGravity.transform.position.y + yOffset)
            || (leftIndexTipPixel.x >= toggleGravity.transform.position.x - xOffset && leftIndexTipPixel.x <= toggleGravity.transform.position.x + xOffset
            && leftIndexTipPixel.y >= toggleGravity.transform.position.y - yOffset && leftIndexTipPixel.y <= toggleGravity.transform.position.y + yOffset))
        {
            previousButton = EventSystem.current.currentSelectedGameObject;
            currentButton = toggleGravity.gameObject;
            toggleGravity.Select();
        }

        else if ((rightIndexTipPixel.x >= buttonRed.transform.position.x - xOffset && rightIndexTipPixel.x <= buttonRed.transform.position.x + xOffset
            && rightIndexTipPixel.y >= buttonRed.transform.position.y - yOffset && rightIndexTipPixel.y <= buttonRed.transform.position.y + yOffset)
            || (leftIndexTipPixel.x >= buttonRed.transform.position.x - xOffset && leftIndexTipPixel.x <= buttonRed.transform.position.x + xOffset
            && leftIndexTipPixel.y >= buttonRed.transform.position.y - yOffset && leftIndexTipPixel.y <= buttonRed.transform.position.y + yOffset))
        {
            previousButton = EventSystem.current.currentSelectedGameObject;
            currentButton = buttonRed.gameObject;
            buttonRed.Select();
        }

        else if ((rightIndexTipPixel.x >= buttonGreen.transform.position.x - xOffset && rightIndexTipPixel.x <= buttonGreen.transform.position.x + xOffset
            && rightIndexTipPixel.y >= buttonGreen.transform.position.y - yOffset && rightIndexTipPixel.y <= buttonGreen.transform.position.y + yOffset)
            || (leftIndexTipPixel.x >= buttonGreen.transform.position.x - xOffset && leftIndexTipPixel.x <= buttonGreen.transform.position.x + xOffset
            && leftIndexTipPixel.y >= buttonGreen.transform.position.y - yOffset && leftIndexTipPixel.y <= buttonGreen.transform.position.y + yOffset))
        {
            previousButton = EventSystem.current.currentSelectedGameObject;
            currentButton = buttonGreen.gameObject;
            buttonGreen.Select();
        }

        else if ((rightIndexTipPixel.x >= buttonBlue.transform.position.x - xOffset && rightIndexTipPixel.x <= buttonBlue.transform.position.x + xOffset
            && rightIndexTipPixel.y >= buttonBlue.transform.position.y - yOffset && rightIndexTipPixel.y <= buttonBlue.transform.position.y + yOffset)
            || (leftIndexTipPixel.x >= buttonBlue.transform.position.x - xOffset && leftIndexTipPixel.x <= buttonBlue.transform.position.x + xOffset
            && leftIndexTipPixel.y >= buttonBlue.transform.position.y - yOffset && leftIndexTipPixel.y <= buttonBlue.transform.position.y + yOffset))
        {
            previousButton = EventSystem.current.currentSelectedGameObject;
            currentButton = buttonBlue.gameObject;
            buttonBlue.Select();
        }

        else if ((rightIndexTipPixel.x >= buttonExit.transform.position.x - xOffset && rightIndexTipPixel.x <= buttonExit.transform.position.x + xOffset
            && rightIndexTipPixel.y >= buttonExit.transform.position.y - yOffset && rightIndexTipPixel.y <= buttonExit.transform.position.y + yOffset)
            || (leftIndexTipPixel.x >= buttonExit.transform.position.x - xOffset && leftIndexTipPixel.x <= buttonExit.transform.position.x + xOffset
            && leftIndexTipPixel.y >= buttonExit.transform.position.y - yOffset && leftIndexTipPixel.y <= buttonExit.transform.position.y + yOffset))
        {
            previousButton = EventSystem.current.currentSelectedGameObject;
            currentButton = buttonExit.gameObject;
            buttonExit.Select();
        }

        else
        {
            previousButton = EventSystem.current.currentSelectedGameObject;
            currentButton = null;
            EventSystem.current.SetSelectedGameObject(null);
        }

        if (previousButton != currentButton || currentButton == null)
        {
            selectTimer = 0;
            progressContextForeground.sizeDelta = new Vector2(0, PROGRESS_BAR_HEIGHT);
        }

        else
        {
            selectTimer = (selectTimer >= SELECT_TIME) ? SELECT_TIME : selectTimer + Time.deltaTime;
            float percent = selectTimer / SELECT_TIME;
            progressContextForeground.sizeDelta = new Vector2(PROGRESS_BAR_WIDTH * percent, PROGRESS_BAR_HEIGHT);

            if (selectTimer >= SELECT_TIME)
            {
                selectTimer = 0;
                progressContextForeground.sizeDelta = new Vector2(0, PROGRESS_BAR_HEIGHT);

                if (currentButton == toggleGravity.gameObject)
                {
                    objectRigidbody.useGravity = !objectRigidbody.useGravity;

                    toggleGravity.isOn = !toggleGravity.isOn;

                    interactionMenu.SetActive(false);
                    axes.SetActive(false);
                    contextMenu.SetActive(false);
                    callContextMenu = false;
                }

                else if(currentButton == buttonRed.gameObject)
                {
                    gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

                    interactionMenu.SetActive(false);
                    axes.SetActive(false);
                    contextMenu.SetActive(false);
                    callContextMenu = false;
                }

                else if (currentButton == buttonGreen.gameObject)
                {
                    gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);

                    interactionMenu.SetActive(false);
                    axes.SetActive(false);
                    contextMenu.SetActive(false);
                    callContextMenu = false;
                }

                else if (currentButton == buttonBlue.gameObject)
                {
                    gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);

                    interactionMenu.SetActive(false);
                    axes.SetActive(false);
                    contextMenu.SetActive(false);
                    callContextMenu = false;
                }

                else if (currentButton == buttonExit.gameObject)
                {
                    contextMenu.SetActive(false);

                    interactionMenu.SetActive(false);
                    axes.SetActive(false);
                    contextMenu.SetActive(false);
                    callContextMenu = false;
                }

                enableContextMenu();
            }
        }
    }

    private void disableContextMenu()
    {
        foreach(GameObject otherGameObject in gameObjects)
        {
            otherGameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;

            Collider[] colliders = otherGameObject.GetComponents<Collider>();

            foreach (Collider collider in colliders)
                collider.enabled = false;
        }
    }

    private void enableContextMenu()
    {
        foreach (GameObject otherGameObject in gameObjects)
        {
            otherGameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            Collider[] colliders = otherGameObject.GetComponents<Collider>();

            foreach (Collider collider in colliders)
                collider.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player")
            return;
        
        if ((other.name == "Right Hand" && client.rightHandPose == Client.HandPose.Point)
            || (other.name == "Left Hand" && client.leftHandPose == Client.HandPose.Point))
        {
            if (hoverTimer >= HOVER_TIME)
            {
                if (!contextMenu.activeInHierarchy)
                {
                    contextMenu.SetActive(true);
                    hoverTimer = 0;
                    callContextMenu = true;
                }
            }

            else
            {
                hoverTimer = (hoverTimer >= HOVER_TIME) ? HOVER_TIME : hoverTimer + Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        hoverTimer = 0;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        hoverTimer = 0;
    }
}
