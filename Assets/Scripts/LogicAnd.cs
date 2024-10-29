using UnityEngine;
using UnityEngine.Events;

public class LogicAnd : MonoBehaviour
{
    [SerializeField] UnityEvent outputTrue;
    [SerializeField] UnityEvent outputFalse;
    bool a = false;
    bool b = false;
    bool result = false;

    public void InputA(bool input)
    {
        a = input;
        UpdateState();
    }

    public void InputB(bool input)
    {
        b = input;
        UpdateState();
    }

    void UpdateState()
    {
        if ((a && b) != result)
        {
            result = a && b;
            if (result) outputTrue.Invoke();
            else outputFalse.Invoke();
        }
    }
}
