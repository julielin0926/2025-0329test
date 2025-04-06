using UnityEngine;

public class CapsuleController : MonoBehaviour
{
    public TimeWindController timeWindController;
    public MicInput micInput;
    public float moveSpeed = 2f;

    void Update()
    {
        if (micInput.IsBlowing)
        {
            Vector3 direction = Vector3.zero;

            switch (timeWindController.currentWind)
            {
                case TimeWindController.WindDirection.East:
                    direction = Vector3.right; // X+
                    break;
                case TimeWindController.WindDirection.West:
                    direction = Vector3.left; // X-
                    break;
                case TimeWindController.WindDirection.None:
                    direction = Vector3.zero;
                    break;
            }

            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}
