using UnityEngine;

public class EnvironmentSetup : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        if (!Application.isEditor)
        {
            WebGLInput.captureAllKeyboardInput = false;
        }
    }
}
