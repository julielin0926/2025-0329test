using UnityEngine;

public class CapsuleController : MonoBehaviour
{
    public TimeWindController timeWindController;
    public MicInput micInput;
    public float moveSpeed = 2f;

    void Update()
    {
        // 只有在吹氣時才檢查風向與移動
        if (micInput != null && micInput.IsBlowing)
        {
            Vector3 direction = Vector3.zero;

            switch (timeWindController.currentWind)
            {
                case TimeWindController.WindDirection.East:
                    direction = Vector3.right; // 向右（東風）
                    break;
                case TimeWindController.WindDirection.West:
                    direction = Vector3.left; // 向左（西風）
                    break;
                case TimeWindController.WindDirection.None:
                    // 無風，角色不動
                    direction = Vector3.zero;
                    break;
            }

            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}
