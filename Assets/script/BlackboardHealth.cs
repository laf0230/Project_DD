using BlackboardSystem;
using ServiceLocator;
using UnityEngine;

public class BlackboardHealth : Health
{
    [Header("업데이트할 데이터")]
    [SerializeField] BlackboardEntryData data;

    public override void Start()
    {
        base.Start();

        data.value = new AnyValue() { type = AnyValue.ValueType.Float, floatValue = currentHealth / maxHealth};

        OnDamaged.AddListener((damage) => {
            data.value = new AnyValue() { type = AnyValue.ValueType.Float, floatValue = currentHealth / maxHealth};
        });
    }
}
