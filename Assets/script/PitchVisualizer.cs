using UnityEngine;
using System.Collections.Generic;

public class PitchVisualizer : MonoBehaviour
{
    public LineRenderer lineRenderer; // 用來畫聲音曲線
    public float timeScale = 1f;      // 控制時間軸速度（目前沒用到，可未來加速/減速）

    public int maxPoints = 1000;      // 曲線最多保留幾個點
    public float xSpacing = 0.3f;     // 每個點在 X 軸的間距
    public float yScale = 0.01f;      // 將音調轉成畫面 Y 值的縮放比例
    private Queue<Vector3> points = new Queue<Vector3>(); // 儲存所有點（先進先出）


    // 更新曲線，根據目前音調新增一個點
    public void UpdatePitch(float pitch)
    {
        float newY = Mathf.Clamp(pitch * yScale, -5f, 400f); // 音調轉成 y 值（限制最大最小）
        Vector3 newPoint = new Vector3(points.Count * xSpacing, newY, 0); // 決定新點位置

        points.Enqueue(newPoint); // 加到曲線中
        if (points.Count > maxPoints)
        {
            points.Dequeue();     // 超過點數就刪掉最前面的點
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());

        Debug.Log("偵測到頻率: " + pitch);
        Debug.Log("計算後的 Y 值: " + newY);
    }
}
