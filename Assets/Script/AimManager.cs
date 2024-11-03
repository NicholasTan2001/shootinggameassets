using UnityEngine;
using Cinemachine;

public class AimManager : MonoBehaviour
{
    public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] Transform camFollowPos;
    
    void Start()
    {
        
    }

    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);
    }

    private void LateUpdate()
    {
        float invertedYAxisValue = -yAxis.Value;

        camFollowPos.localEulerAngles = new Vector3(invertedYAxisValue, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3 (transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);

    }
}
