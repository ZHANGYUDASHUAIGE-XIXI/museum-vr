using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("===== 旧版UI绑定区 =====")]
    public Text questionTxt;        // 题干文本
    public Button[] optionBtns;     // 4个选项按钮 顺序：0 1 2 3
    public Text tipTxt;             // 对错提示、答案解析
    public Button operateBtn;       // 合并按钮：确定/下一题/关闭
    public Text operateBtnTxt;      // 合并按钮文字
    public Button prevBtn;          // 上一题按钮
    public Text scoreTxt;           // 得分显示文本

    [Header("===== 题目配置 =====")]
    public Question[] questionList; // 题目数组，固定10道题

    // 内部状态
    private int currentQuesIndex;       // 当前题号 0=第1题  9=第10题
    private int selectIndex;            // 用户选中选项 -1=未选择
    private bool[] questionAnswered;    // 【修复】数组：单独记录每一题是否已作答
    private int totalScore;             // 总得分
    private const int singleScore = 10; // 每题固定10分

    void Start()
    {
        // 绑定按钮事件
        operateBtn.onClick.AddListener(OperateBtnClick);
        prevBtn.onClick.AddListener(PrevQuestion);

        // 绑定选项按钮
        for (int i = 0; i < optionBtns.Length; i++)
        {
            int tempIndex = i;
            optionBtns[i].onClick.AddListener(() => SelectOption(tempIndex));
        }

        gameObject.SetActive(false);
        totalScore = 0;
        RefreshScoreText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            CloseQuiz();
        }
    }

    /// <summary>
    /// 打开答题面板，从头开始答题（外部调用）
    /// </summary>
    public void OpenQuiz()
    {
        gameObject.SetActive(true);
        currentQuesIndex = 0;
        totalScore = 0;
        RefreshScoreText();

        // 初始化作答状态数组：题目有多少道，数组就有多少元素，默认全部未作答
        if (questionList != null)
        {
            questionAnswered = new bool[questionList.Length];
        }

        LoadQuestion(currentQuesIndex);
    }

    /// <summary>
    /// 关闭答题面板，重置所有状态
    /// </summary>
    public void CloseQuiz()
    {
        gameObject.SetActive(false);
        ResetAllState();
    }

    /// <summary>
    /// 加载指定题目（核心修复：区分每题独立状态）
    /// </summary>
    private void LoadQuestion(int index)
    {
        if (index < 0 || index >= questionList.Length) return;

        Question nowQues = questionList[index];
        // 加载题干
        questionTxt.text = $"第{index + 1}题：{nowQues.questionText}";

        // 加载选项文本
        for (int i = 0; i < optionBtns.Length; i++)
        {
            optionBtns[i].GetComponentInChildren<Text>().text = nowQues.optionTexts[i];
        }

        // 获取当前题的作答状态（独立状态，不共用全局变量）
        bool currentIsAnswered = questionAnswered[currentQuesIndex];

        if (!currentIsAnswered)
        {
            // ========== 全新题目：重置所有状态、解锁选项 ==========
            selectIndex = -1;
            tipTxt.text = "";
            operateBtn.interactable = true;

            foreach (var btn in optionBtns)
            {
                btn.interactable = true;
                btn.GetComponent<Image>().color = Color.white;
            }
        }
        else
        {
            // ========== 回看已答题目：锁定选项，禁止修改 ==========
            foreach (var btn in optionBtns)
            {
                btn.interactable = false;
            }
        }

        // 【修复Bug1】第一题直接隐藏「上一题」按钮，其余题目显示
        if (currentQuesIndex == 0)
        {
            prevBtn.gameObject.SetActive(false);
        }
        else
        {
            prevBtn.gameObject.SetActive(true);
        }

        // 刷新合并按钮文字
        UpdateOperateBtnText();
    }

    /// <summary>
    /// 选择选项
    /// </summary>
    private void SelectOption(int index)
    {
        // 已作答的题目禁止重选
        if (questionAnswered[currentQuesIndex]) return;

        foreach (var btn in optionBtns)
        {
            btn.GetComponent<Image>().color = Color.white;
        }
        selectIndex = index;
        optionBtns[index].GetComponent<Image>().color = new Color(0.3f, 0.7f, 1f);
    }

    /// <summary>
    /// 合并按钮点击：确定 / 下一题 / 关闭
    /// </summary>
    private void OperateBtnClick()
    {
        bool currentIsAnswered = questionAnswered[currentQuesIndex];
        if (!currentIsAnswered)
        {
            // 未答题 → 执行判题
            CheckAnswer();
        }
        else
        {
            // 已答题 → 切换下一题 / 关闭
            NextQuestion();
        }
    }

    /// <summary>
    /// 判题逻辑
    /// </summary>
    private void CheckAnswer()
    {
        if (selectIndex == -1)
        {
            tipTxt.text = "请先选择一个选项！";
            return;
        }

        Question nowQues = questionList[currentQuesIndex];
        bool isRight = (selectIndex == nowQues.correctIndex);

        // 答对加分
        if (isRight)
        {
            totalScore += singleScore;
            RefreshScoreText();
        }

        // 标记本题为【已作答】（核心：写入独立数组，不影响其他题目）
        questionAnswered[currentQuesIndex] = true;

        // 拼接提示文本
        string rightOption = nowQues.optionTexts[nowQues.correctIndex];
        if (isRight)
        {
            tipTxt.text = "回答正确！\n" + nowQues.explain;
        }
        else
        {
            tipTxt.text = $"回答错误！\n正确答案：{rightOption}\n{nowQues.explain}";
        }

        // 选项标色 + 锁定
        for (int i = 0; i < optionBtns.Length; i++)
        {
            Image img = optionBtns[i].GetComponent<Image>();
            if (i == nowQues.correctIndex)
                img.color = Color.green;
            else if (i == selectIndex && !isRight)
                img.color = Color.red;

            optionBtns[i].interactable = false;
        }

        // 刷新按钮文字
        UpdateOperateBtnText();
    }

    /// <summary>
    /// 下一题 / 关闭面板
    /// </summary>
    private void NextQuestion()
    {
        // 最后一题 → 关闭面板
        if (currentQuesIndex == questionList.Length - 1)
        {
            CloseQuiz();
            return;
        }
        // 切换下一题
        currentQuesIndex++;
        LoadQuestion(currentQuesIndex);
    }

    /// <summary>
    /// 上一题（仅查看，不可重做）
    /// </summary>
    private void PrevQuestion()
    {
        if (currentQuesIndex <= 0) return;
        currentQuesIndex--;
        LoadQuestion(currentQuesIndex);
    }

    /// <summary>
    /// 动态更新合并按钮文字
    /// </summary>
    private void UpdateOperateBtnText()
    {
        bool currentIsAnswered = questionAnswered[currentQuesIndex];
        if (!currentIsAnswered)
        {
            operateBtnTxt.text = "确定";
        }
        else
        {
            if (currentQuesIndex == questionList.Length - 1)
            {
                operateBtnTxt.text = "关闭";
            }
            else
            {
                operateBtnTxt.text = "下一题";
            }
        }
    }

    /// <summary>
    /// 刷新得分显示
    /// </summary>
    private void RefreshScoreText()
    {
        scoreTxt.text = $"当前得分：{totalScore} / 100";
    }

    /// <summary>
    /// 全局重置
    /// </summary>
    private void ResetAllState()
    {
        currentQuesIndex = 0;
        selectIndex = -1;
        tipTxt.text = "";
        totalScore = 0;
        RefreshScoreText();
        questionAnswered = null;
    }
}