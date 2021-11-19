using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System;

public enum MatterState {
    Solid, Liquid, Gas, Plasma, BoseEinsteinCondensate
}

[CreateAssetMenu(fileName = "New Cell")]
public class Cell : ScriptableObject {
    public string Name;
    public Color32 Color;

    public MatterState State;

    [HideInInspector]
    public bool IsSolid;
    [HideInInspector]
    public bool IsLiquid;
    [HideInInspector]
    public bool IsGas;

    public bool IsStatic;   // Immovable and non-interactable or not

    [EnableIf("IsSolid")] [HideIf("IsStatic")]
    public bool UseRigidGravity = false;        // Tower-shaped pile
    [EnableIf("IsSolid")] [HideIf("IsStatic")]
    public bool UsePowderGravity = false;       // Pyramid-shaped pile
    [EnableIf("IsLiquid")] [HideIf("IsStatic")]
    public bool UseLiquidGravity = false;       // Fall and spread available space
    [EnableIf("IsGas")] [HideIf("IsStatic")]
    public bool UseGasGravity = false;          // Rise in a pyramid-shaped pile

    // Corrosive property
    public bool Corrode = false;
    [ShowIf("Corrode")]
    public int CorrodeSlowModifier = 7;

    // Material property to decay into another cell over time
    public bool SelfDecay = false;
    [ShowIf("SelfDecay")]
    public int DecaySlowModifier;
    [ShowIf("SelfDecay")]
    public Cell DecayInto;

    // Material property to morph into another cell type on collision
    public bool MorphOnCollision;
    [ShowIf("MorphOnCollision")]
    public int NumInteractions;
    [ShowIf("MorphOnCollision")]
    public int[] MorphSlowMod;
    [ShowIf("MorphOnCollision")]
    public Cell[] MorphCollision;
    [ShowIf("MorphOnCollision")]
    public Cell[] MorphInto;

    public List<Cell> GoThrough = new List<Cell>();
    
    public bool Destructible;

    public int FloatSlowModifier = 10; // Chance to float

    void OnValidate() {
        switch(State) {
            case MatterState.Solid:
                IsSolid = true;
                IsLiquid = false;
                IsGas = false;
                break;
            case MatterState.Liquid:
                IsSolid = false;
                IsLiquid = true;
                IsGas = false;
                break;
            case MatterState.Gas:
                IsSolid = false;
                IsLiquid = false;
                IsGas = true;
                break;
        }

        Array.Resize(ref MorphSlowMod, NumInteractions);
        Array.Resize(ref MorphCollision, NumInteractions);
        Array.Resize(ref MorphInto, NumInteractions);
    }
}