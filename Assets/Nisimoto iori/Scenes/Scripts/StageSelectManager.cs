using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageSelectManager : MonoBehaviour
{
    [Header("ステージ表示")]
    public TMP_Text stageName;

    [Header("ステージ一覧")]
    public string[] stageNames =
    {
        "家具売り場",
        "家電売り場",
        "倉庫"
    };

    [Header("コントローラー")]
    public Joycon joycon;

    private int currentStageIndex = 0;
    private bool leftHeld = false;
    private bool rightHeld = false;
    public GameObject StageSelectPanel;


    void Start()
    {
        UpdateStage();
    }

    void Update()
    {
        if (!StageSelectPanel.activeSelf)
        {
            leftHeld = false;
            rightHeld = false;
            return;
        }

        if (joycon == null || joycon.Player1Pad == null)
        {
            return;
        }

        bool left = joycon.Player1Pad.leftStick.ReadValue().x < -0.5f;
        bool right = joycon.Player1Pad.leftStick.ReadValue().x > 0.5f;

        if (left && !leftHeld)
        {
            PreviousStage();
        }

        if (right && !rightHeld)
        {
            NextStage();
        }

        leftHeld = left;
        rightHeld = right;
    }

    public void NextStage()
    {
        currentStageIndex++;

        if (currentStageIndex >= stageNames.Length)
        {
            currentStageIndex = 0;
        }

        UpdateStage();
    }

    public void PreviousStage()
    {
        currentStageIndex--;

        if (currentStageIndex < 0)
        {
            currentStageIndex = stageNames.Length - 1;
        }

        UpdateStage();
    }

    void UpdateStage()
    {
        stageName.text = stageNames[currentStageIndex];
    }
}
