using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class CircuitSystem : MonoBehaviour {
    public UnityEvent onLight; //events fired when circuit is fully powered
    public UnityEvent onDarken; //events fired when circuit loses power

    private int numJunctions;
    private int litJunctions = 0;
    public bool isLit = false;
    private int litFrame = 0;
    private List<CircuitJunction> junctions;
    private List<CircuitWire> wires;
    
	void Start () {
        junctions = new List<CircuitJunction>(GetComponentsInChildren<CircuitJunction>());
        wires = new List<CircuitWire>(GetComponentsInChildren<CircuitWire>());
        foreach(CircuitJunction junction in junctions) {
            junction.system = this;
        }
        numJunctions = junctions.Count;
    }

    //controls frame changes for every circuit wire in the system
    void Update() {
        if (isLit && litFrame == 0) SetFrame(1);
        else if (isLit && litFrame == 1) SetFrame(2); //lit
        else if (!isLit && litFrame == 2) SetFrame(1); //half lit
        else if (!isLit && litFrame == 1) SetFrame(0); //not lit
    }

    //called when one of the junctions is filled by a light plant
    public void ConnectJunction() {
        litJunctions++;
        if (litJunctions == numJunctions) {
            isLit = true;
            foreach (CircuitWire wire in wires)
                wire.Electrify();
            onLight.Invoke();
        }
    }

    //called when one of the junctions loses a light plant
    public void DisconnectJunction() {
        if (litJunctions == numJunctions) {
            isLit = false;
            foreach (CircuitWire wire in wires)
                wire.Unelectrify();
            onDarken.Invoke();
        }
        litJunctions--;
    }

    //helper method to change sprites for all circuit wires in the system
    private void SetFrame(int frame) {
        foreach (CircuitWire wire in wires) wire.SetFrame(frame);
        litFrame = frame;
    }
}
