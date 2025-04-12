namespace ModbusDebugging.Model;

/// <summary>
/// 键值对
/// </summary>
/// <typeparam name="TKey">键泛型</typeparam>
/// <typeparam name="TValue">值泛型</typeparam>
public class Pair<TKey, TValue>(TKey key, TValue value)
{
    /// <summary>
    /// 键
    /// </summary>
    public TKey Key { get; } = key;

    /// <summary>
    /// 值
    /// </summary>
    public TValue Value { get; } = value;
}