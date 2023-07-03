using Codice.CM.Common;
using System;
using UnityEngine;

[Serializable]
public class ShireBlock : IComparable<ShireBlock>, ISerializationCallbackReceiver
{
    [SerializeField] protected float m_Value = 0;
    protected float m_Temp;
    [SerializeField] protected int m_Mod = 1;
    protected float m_ModTemp = 1;

    public float Value { get { return m_Value; } set { m_Value = value; } }
    public float Temp { get { return m_Temp; } set { m_Temp = value; } }
    public int Mod { get { return m_Mod; } set { m_Mod = value; } }
    public float ModTemp { get { return m_ModTemp; } set { m_ModTemp = value; } }
    public virtual float TempValue { get { return Value + Temp; } }
    public virtual float TempMod { get { return Mod + ModTemp; } }
    public virtual float TrueValue { get { return (TempValue) * (TempMod); } }

    public void Copy(ShireBlock block)
    {
        m_Value = block.m_Value;
        m_Temp = block.m_Temp;
        m_Mod = block.m_Mod;
        m_ModTemp = block.m_ModTemp;
    }

    public virtual int CompareTo(ShireBlock other)
    {
        return TrueValue.CompareTo(other.TrueValue);
    }

    public virtual void ComputeMod()
    {
        m_Mod = (int)m_Value / 2 - 5;
    }

    public static int ComputeMod(float value)
    {
        var tempBlock = new ShireBlock(value: value);
        tempBlock.ComputeMod();
        return tempBlock.m_Mod;
    }

    public override string ToString()
    {
        return string.Format("{0} {1} {2} {3}", m_Value, m_Temp, m_Mod, m_ModTemp);
    }

    public ShireBlock()
    {
        m_Value = 0;
        m_Temp = 0;
        m_Mod = 1;
        m_ModTemp = 1;
    }

    public ShireBlock(float value=0, float temp = 0, int mod = 1, float modTemp = 1)
    {
        m_Value = value;
        m_Temp = temp;
        m_Mod = mod;
        m_ModTemp = modTemp;
    }

    public void OnBeforeSerialize()
    {
        ComputeMod();
    }

    public void OnAfterDeserialize()
    {
        ComputeMod();
    }
}
