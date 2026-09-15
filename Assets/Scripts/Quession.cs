using UnityEngine;

[System.Serializable]
public class Question
{
    [Header("题目内容")]
    public string questionText;          // 题干
    public string[] optionTexts;        // 4个选项文本

    [Header("答案与解析")]
    [Range(0, 3)] public int correctIndex; // 正确选项编号：0/1/2/3
    [TextArea(2, 4)] public string explain;// 答案解析
}
