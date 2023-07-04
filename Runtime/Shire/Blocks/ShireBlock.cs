using System;
using UnityEngine;

/// <summary>
/// A block of a shire that represents a value with modifiers and temporary adjustments.
/// </summary>
[Serializable]
public class ShireBlock : IComparable<ShireBlock>, ISerializationCallbackReceiver
{
    [SerializeField] private float m_Value = 0;
    private float m_Temp;
    [SerializeField] private int m_Mod = 1;
    private float m_ModTemp = 1;

    /// <summary>
    /// The base value of the shire block.
    /// </summary>
    public float Value { get { return m_Value; } set { m_Value = value; } }

    /// <summary>
    /// The temporary adjustment applied to the shire block.
    /// </summary>
    public float Temp { get { return m_Temp; } set { m_Temp = value; } }

    /// <summary>
    /// The modifier value applied to the shire block.
    /// </summary>
    public int Mod { get { return m_Mod; } set { m_Mod = value; } }

    /// <summary>
    /// The temporary adjustment applied to the modifier value.
    /// </summary>
    public float ModTemp { get { return m_ModTemp; } set { m_ModTemp = value; } }

    /// <summary>
    /// The value of the shire block after applying temporary adjustments.
    /// </summary>
    public virtual float TempValue { get { return Value + Temp; } }

    /// <summary>
    /// The modifier value of the shire block after applying temporary adjustments.
    /// </summary>
    public virtual float TempMod { get { return Mod + ModTemp; } }

    /// <summary>
    /// The final value of the shire block after applying both value and modifier adjustments.
    /// </summary>
    public virtual float TrueValue { get { return TempValue * TempMod; } }

    /// <summary>
    /// Creates a copy of the shire block by copying its values.
    /// </summary>
    /// <param name="block">The shire block to copy from.</param>
    public void Copy(ShireBlock block)
    {
        m_Value = block.m_Value;
        m_Temp = block.m_Temp;
        m_Mod = block.m_Mod;
        m_ModTemp = block.m_ModTemp;
    }

    /// <summary>
    /// Compares this shire block to another shire block based on their true values.
    /// </summary>
    /// <param name="other">The other shire block to compare.</param>
    /// <returns>The comparison result.</returns>
    public virtual int CompareTo(ShireBlock other)
    {
        return TrueValue.CompareTo(other.TrueValue);
    }

    /// <summary>
    /// Computes the modifier value based on the base value.
    /// </summary>
    public virtual void ComputeMod()
    {
        m_Mod = (int)m_Value / 2 - 5;
    }

    /// <summary>
    /// Computes the modifier value based on the specified value.
    /// </summary>
    /// <param name="value">The value to compute the modifier for.</param>
    /// <returns>The computed modifier value.</returns>
    public static int ComputeMod(float value)
    {
        var tempBlock = new ShireBlock(value: value);
        tempBlock.ComputeMod();
        return tempBlock.m_Mod;
    }

    /// <summary>
    /// Constructs a new instance of the <see cref="ShireBlock"/> class with default values.
    /// </summary>
    public ShireBlock()
    {
        m_Value = 0;
        m_Temp = 0;
        m_Mod = 1;
        m_ModTemp = 1;
    }

    /// <summary>
    /// Constructs a new instance of the <see cref="ShireBlock"/> class with the specified values.
    /// </summary>
    /// <param name="value">The base value of the shire block.</param>
    /// <param name="temp">The temporary adjustment applied to the shire block.</param>
    /// <param name="mod">The modifier value applied to the shire block.</param>
    /// <param name="modTemp">The temporary adjustment applied to the modifier value.</param>
    public ShireBlock(float value = 0, float temp = 0, int mod = 1, float modTemp = 1)
    {
        m_Value = value;
        m_Temp = temp;
        m_Mod = mod;
        m_ModTemp = modTemp;
    }

    /// <summary>
    /// Computes the modifier value before serialization.
    /// </summary>
    public void OnBeforeSerialize()
    {
        ComputeMod();
    }

    /// <summary>
    /// Computes the modifier value after deserialization.
    /// </summary>
    public void OnAfterDeserialize()
    {
        ComputeMod();
    }
}
