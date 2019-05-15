using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public enum JointType
    {
        HingeJoint,
        CharacterJoint,
        FixedJoint,
        ConfigurableJoint,
        SpringJoint
    };

    public JointType jointType = JointType.CharacterJoint;

    private void Start()
    {
        switch (jointType)
        {
            case JointType.HingeJoint:
                gameObject.GetComponent<HingeJoint>().connectedBody = gameObject.transform.parent.GetComponent<Rigidbody>();
                break;

            case JointType.CharacterJoint:
                gameObject.GetComponent<CharacterJoint>().connectedBody = gameObject.transform.parent.GetComponent<Rigidbody>();
                break;

            case JointType.FixedJoint:
                gameObject.GetComponent<FixedJoint>().connectedBody = gameObject.transform.parent.GetComponent<Rigidbody>();
                break;

            case JointType.ConfigurableJoint:
                gameObject.GetComponent<ConfigurableJoint>().connectedBody = gameObject.transform.parent.GetComponent<Rigidbody>();
                break;

            case JointType.SpringJoint:
                gameObject.GetComponent<SpringJoint>().connectedBody = gameObject.transform.parent.GetComponent<Rigidbody>();
                break;

            default:
                Debug.Log("???");
                break;
        }
    }
}