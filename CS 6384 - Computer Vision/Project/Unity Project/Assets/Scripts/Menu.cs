using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    private const float FIST_TIME = 0.5f;
    private const float MENU_FIST_COUNT = 2;
    private const float SELECT_TIME = 1;
    private const float PROGRESS_BAR_WIDTH = 200;
    private const float PROGRESS_BAR_HEIGHT = 10;
    private const string GAMEOBJECT_TAG = "Object";

    public GameObject gameObjectLeftHand;
    public GameObject gameObjectRightHand;

    public Transform rightIndexTip;
    public Transform leftIndexTip;
    public GameObject interactionMenu;
    public Button buttonMove;
    public Button buttonRotate;
    public Button buttonScale;
    public Button buttonClearAction;
    public Button buttonReset;
    public Button buttonExit;
    public Toggle toggleX;
    public Toggle toggleY;
    public Toggle toggleZ;
    public GameObject progressBarMenu;
    public GameObject progressBarAxes;
    public GameObject axes;
    public GameObject contextMenuGameObject;

    private Client client;
    private Camera cam;
    private GameObject previousButton;
    private GameObject currentButton;
    private GameObject[] gameObjects;
    private RectTransform progressMenuForeground;
    private RectTransform progressAxesForeground;
    private InteractionProperties interactionProperties;
    private List<Vector3> initialPositions;
    private List<Quaternion> initialRotations;
    private List<Vector3> initialScales;

    private float fistTimer;
    private int fistCount;
    private float selectTimer;
    private int xOffset = 200;
    private int yOffset = 50;

    // Start is called before the first frame update
    void Start()
    {
        client = FindObjectOfType<Client>();
        cam = Camera.main;
        interactionMenu.SetActive(false);
        axes.SetActive(false);
        contextMenuGameObject.SetActive(false);

        progressMenuForeground = progressBarMenu.transform.Find("Progress").GetComponent<RectTransform>();
        progressAxesForeground = progressBarAxes.transform.Find("Progress").GetComponent<RectTransform>();

        gameObjects = GameObject.FindGameObjectsWithTag(GAMEOBJECT_TAG);
        initialPositions = new List<Vector3>();
        initialRotations = new List<Quaternion>();
        initialScales = new List<Vector3>();

        foreach (GameObject thisGameObject in gameObjects)
        {
            initialPositions.Add(thisGameObject.transform.position);
            initialRotations.Add(thisGameObject.transform.rotation);
            initialScales.Add(thisGameObject.transform.localScale);
        }

        interactionProperties = FindObjectOfType<InteractionProperties>();
    }

    // Update is called once per frame
    void Update()
    {
        if (client.leftHandPose == Client.HandPose.Stop)
        {
            if (fistTimer >= FIST_TIME)
            {
                // Display menu
                fistCount = fistCount + 1;

                if (fistCount >= MENU_FIST_COUNT)
                {
                    fistCount = 0;
                    interactionMenu.SetActive(true);
                }
            }

            fistTimer = 0;
        }

        else if (client.leftHandPose == Client.HandPose.Fist)
            fistTimer = (fistTimer >= FIST_TIME) ? FIST_TIME : fistTimer + Time.deltaTime;

        if(interactionMenu.activeInHierarchy)
        {
            MenuButtonInteraction();
        }

        else if(axes.activeInHierarchy)
        {
            AxesButtonInteraction();
        }
    }

    private void MenuButtonInteraction()
    {
        Vector2 leftIndexTipPixel = cam.WorldToScreenPoint(leftIndexTip.position);
        Vector2 rightIndexTipPixel = cam.WorldToScreenPoint(rightIndexTip.position);

        contextMenuGameObject.SetActive(false);
        disableContextMenu();

        if ((rightIndexTipPixel.x >= buttonMove.transform.position.x - xOffset && rightIndexTipPixel.x <= buttonMove.transform.position.x + xOffset
            && rightIndexTipPixel.y >= buttonMove.transform.position.y - yOffset && rightIndexTipPixel.y <= buttonMove.transform.position.y + yOffset)
            || (leftIndexTipPixel.x >= buttonMove.transform.position.x - xOffset && leftIndexTipPixel.x <= buttonMove.transform.position.x + xOffset
            && leftIndexTipPixel.y >= buttonMove.transform.position.y - yOffset && leftIndexTipPixel.y <= buttonMove.transform.position.y + yOffset))
        {
            previousButton = EventSystem.current.currentSelectedGameObject;
            currentButton = buttonMove.gameObject;
            buttonMove.Select();
        }

        else if ((rightIndexTipPixel.x >= buttonRotate.transform.position.x - xOffset && rightIndexTipPixel.x <= buttonRotate.transform.position.x + xOffset
            && rightIndexTipPixel.y >= buttonRotate.transform.position.y - yOffset && rightIndexTipPixel.y <= buttonRotate.transform.position.y + yOffset)
            || (leftIndexTipPixel.x >= buttonRotate.transform.position.x - xOffset && leftIndexTipPixel.x <= buttonRotate.transform.position.x + xOffset
            && leftIndexTipPixel.y >= buttonRotate.transform.position.y - yOffset && leftIndexTipPixel.y <= buttonRotate.transform.position.y + yOffset))
        {
            previousButton = EventSystem.current.currentSelectedGameObject;
            currentButton = buttonRotate.gameObject;
            buttonRotate.Select();
        }

        else if ((rightIndexTipPixel.x >= buttonScale.transform.position.x - xOffset && rightIndexTipPixel.x <= buttonScale.transform.position.x + xOffset
            && rightIndexTipPixel.y >= buttonScale.transform.position.y - yOffset && rightIndexTipPixel.y <= buttonScale.transform.position.y + yOffset)
            || (leftIndexTipPixel.x >= buttonScale.transform.position.x - xOffset && leftIndexTipPixel.x <= buttonScale.transform.position.x + xOffset
            && leftIndexTipPixel.y >= buttonScale.transform.position.y - yOffset && leftIndexTipPixel.y <= buttonScale.transform.position.y + yOffset))
        {
            previousButton = EventSystem.current.currentSelectedGameObject;
            currentButton = buttonScale.gameObject;
            buttonScale.Select();
        }

        else if ((rightIndexTipPixel.x >= buttonClearAction.transform.position.x - xOffset && rightIndexTipPixel.x <= buttonClearAction.transform.position.x + xOffset
            && rightIndexTipPixel.y >= buttonClearAction.transform.position.y - yOffset && rightIndexTipPixel.y <= buttonClearAction.transform.position.y + yOffset)
            || (leftIndexTipPixel.x >= buttonClearAction.transform.position.x - xOffset && leftIndexTipPixel.x <= buttonClearAction.transform.position.x + xOffset
            && leftIndexTipPixel.y >= buttonClearAction.transform.position.y - yOffset && leftIndexTipPixel.y <= buttonClearAction.transform.position.y + yOffset))
        {
            previousButton = EventSystem.current.currentSelectedGameObject;
            currentButton = buttonClearAction.gameObject;
            buttonClearAction.Select();
        }

        else if ((rightIndexTipPixel.x >= buttonReset.transform.position.x - xOffset && rightIndexTipPixel.x <= buttonReset.transform.position.x + xOffset
            && rightIndexTipPixel.y >= buttonReset.transform.position.y - yOffset && rightIndexTipPixel.y <= buttonReset.transform.position.y + yOffset)
            || (leftIndexTipPixel.x >= buttonReset.transform.position.x - xOffset && leftIndexTipPixel.x <= buttonReset.transform.position.x + xOffset
            && leftIndexTipPixel.y >= buttonReset.transform.position.y - yOffset && leftIndexTipPixel.y <= buttonReset.transform.position.y + yOffset))
        {
            previousButton = EventSystem.current.currentSelectedGameObject;
            currentButton = buttonReset.gameObject;
            buttonReset.Select();
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
            progressMenuForeground.sizeDelta = new Vector2(0, PROGRESS_BAR_HEIGHT);
        }

        else
        {
            selectTimer = (selectTimer >= SELECT_TIME) ? SELECT_TIME : selectTimer + Time.deltaTime;
            float percent = selectTimer / SELECT_TIME;
            progressMenuForeground.sizeDelta = new Vector2(PROGRESS_BAR_WIDTH * percent, PROGRESS_BAR_HEIGHT);

            if (selectTimer >= SELECT_TIME)
            {
                selectTimer = 0;
                progressMenuForeground.sizeDelta = new Vector2(0, PROGRESS_BAR_HEIGHT);
                enableContextMenu();

                if (currentButton == buttonMove.gameObject)
                {
                    interactionProperties.interaction = InteractionProperties.Interaction.Move;
                    interactionMenu.SetActive(false);
                    axes.SetActive(false);
                }

                else if (currentButton == buttonRotate.gameObject)
                {
                    interactionProperties.interaction = InteractionProperties.Interaction.Rotate;
                    interactionMenu.SetActive(false);
                    axes.SetActive(true);
                }

                else if (currentButton == buttonScale.gameObject)
                {
                    interactionProperties.interaction = InteractionProperties.Interaction.Scale;
                    interactionMenu.SetActive(false);
                    axes.SetActive(true);
                }

                else if (currentButton == buttonClearAction.gameObject)
                {
                    interactionProperties.interaction = InteractionProperties.Interaction.None;
                    interactionMenu.SetActive(false);
                    axes.SetActive(false);
                }

                else if (currentButton == buttonReset.gameObject)
                {
                    interactionProperties.interaction = InteractionProperties.Interaction.None;
                    interactionMenu.SetActive(false);
                    axes.SetActive(false);
                    ResetObjects();
                }

                else if (currentButton == buttonExit.gameObject)
                {
                    interactionMenu.SetActive(false);
                }
            }
        }
    }

    private void AxesButtonInteraction()
    {
        Vector2 leftIndexTipPixel = cam.WorldToScreenPoint(leftIndexTip.position);
        Vector2 rightIndexTipPixel = cam.WorldToScreenPoint(rightIndexTip.position);

        if ((rightIndexTipPixel.x >= toggleX.transform.position.x - yOffset && rightIndexTipPixel.x <= toggleX.transform.position.x + yOffset
            && rightIndexTipPixel.y >= toggleX.transform.position.y - yOffset && rightIndexTipPixel.y <= toggleX.transform.position.y + yOffset)
            || (leftIndexTipPixel.x >= toggleX.transform.position.x - yOffset && leftIndexTipPixel.x <= toggleX.transform.position.x + yOffset
            && leftIndexTipPixel.y >= toggleX.transform.position.y - yOffset && leftIndexTipPixel.y <= toggleX.transform.position.y + yOffset))
        {
            previousButton = EventSystem.current.currentSelectedGameObject;
            currentButton = toggleX.gameObject;
            toggleX.Select();
        }

        else if ((rightIndexTipPixel.x >= toggleY.transform.position.x - yOffset && rightIndexTipPixel.x <= toggleY.transform.position.x + yOffset
            && rightIndexTipPixel.y >= toggleY.transform.position.y - yOffset && rightIndexTipPixel.y <= toggleY.transform.position.y + yOffset)
            || (leftIndexTipPixel.x >= toggleY.transform.position.x - yOffset && leftIndexTipPixel.x <= toggleY.transform.position.x + yOffset
            && leftIndexTipPixel.y >= toggleY.transform.position.y - yOffset && leftIndexTipPixel.y <= toggleY.transform.position.y + yOffset))
        {
            previousButton = EventSystem.current.currentSelectedGameObject;
            currentButton = toggleY.gameObject;
            toggleY.Select();
        }

        else if ((rightIndexTipPixel.x >= toggleZ.transform.position.x - yOffset && rightIndexTipPixel.x <= toggleZ.transform.position.x + yOffset
            && rightIndexTipPixel.y >= toggleZ.transform.position.y - yOffset && rightIndexTipPixel.y <= toggleZ.transform.position.y + yOffset)
            || (leftIndexTipPixel.x >= toggleZ.transform.position.x - yOffset && leftIndexTipPixel.x <= toggleZ.transform.position.x + yOffset
            && leftIndexTipPixel.y >= toggleZ.transform.position.y - yOffset && leftIndexTipPixel.y <= toggleZ.transform.position.y + yOffset))
        {
            previousButton = EventSystem.current.currentSelectedGameObject;
            currentButton = toggleZ.gameObject;
            toggleZ.Select();
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
            progressAxesForeground.sizeDelta = new Vector2(0, PROGRESS_BAR_HEIGHT);
        }

        else
        {
            selectTimer = (selectTimer >= SELECT_TIME) ? SELECT_TIME : selectTimer + Time.deltaTime;
            float percent = selectTimer / SELECT_TIME;
            progressAxesForeground.sizeDelta = new Vector2(PROGRESS_BAR_WIDTH * percent, PROGRESS_BAR_HEIGHT);

            if (selectTimer >= SELECT_TIME)
            {
                selectTimer = 0;
                progressAxesForeground.sizeDelta = new Vector2(0, PROGRESS_BAR_HEIGHT);

                if (currentButton == toggleX.gameObject)
                {
                    toggleX.isOn = !toggleX.isOn;
                    interactionProperties.axes.x = (toggleX.isOn) ? 1 : 0;
                }

                else if (currentButton == toggleY.gameObject)
                {
                    toggleY.isOn = !toggleY.isOn;
                    interactionProperties.axes.y = (toggleY.isOn) ? 1 : 0;
                }

                else if (currentButton == toggleZ.gameObject)
                {
                    toggleZ.isOn = !toggleZ.isOn;
                    interactionProperties.axes.z = (toggleZ.isOn) ? 1 : 0;
                }
            }
        }
    }

    private void ResetObjects()
    {
        int i = 0;

        gameObjectLeftHand.GetComponent<LeftHand>().currentInteractableObject = null;
        gameObjectRightHand.GetComponent<RightHand>().currentInteractableObject = null;

        foreach (GameObject thisGameObject in gameObjects)
        {
            thisGameObject.transform.position = initialPositions[i];
            thisGameObject.transform.rotation = initialRotations[i];
            thisGameObject.transform.localScale = initialScales[i];

            thisGameObject.GetComponent<Rigidbody>().useGravity = true;
            thisGameObject.GetComponent<Interaction>().hand = null;

            Color originalColour = new Color();
            ColorUtility.TryParseHtmlString("#93DAF5", out originalColour);
            thisGameObject.GetComponent<Renderer>().material.SetColor("_Color", originalColour);

            i = i + 1;
        }
    }

    private void disableContextMenu()
    {
        foreach (GameObject otherGameObject in gameObjects)
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
}