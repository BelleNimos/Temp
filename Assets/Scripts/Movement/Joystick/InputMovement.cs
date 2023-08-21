using UnityEngine;

public class InputMovement : MonoBehaviour
{
    [SerializeField] private VariableJoystick _variableJoystick;
    [SerializeField] private PlayerMovement _playerMovement;

    private float _horizontal;
    private float _vertical;

    private void Update()
    {
        if (_variableJoystick.Vertical != 0 || _variableJoystick.Horizontal != 0)
        {
            _playerMovement.Move(new Vector3(_variableJoystick.Horizontal, 0, _variableJoystick.Vertical));
            _playerMovement.Rotate(new Vector3(_variableJoystick.Horizontal, 0, _variableJoystick.Vertical));
        }
        else
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");

            _playerMovement.Move(new Vector3(_horizontal, 0, _vertical));
            _playerMovement.Rotate(new Vector3(_horizontal, 0, _vertical));
        }
    }
}
