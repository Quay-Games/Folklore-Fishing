using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(BaitShopStock))]
public class BaitShopStockDrawer : PropertyDrawer
{
    private const string CatalogPath = "Assets/Resources/Player/BaitDatas.asset";

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var list = property.FindPropertyRelative(nameof(BaitShopStock.ShopStock));
        var field = new PropertyField(list, property.displayName);
        var targets = property.serializedObject.targetObjects;
        var path = property.propertyPath;

        field.schedule.Execute(() =>
        {
            if (Application.isPlaying)
                return;

            var catalog = AssetDatabase.LoadAssetAtPath<BaitDatas>(CatalogPath);
            if (catalog == null || catalog.Datas == null)
                return;

            foreach (var target in targets)
            {
                if (target == null)
                    continue;

                using var serialized = new SerializedObject(target);
                var stock = serialized.FindProperty(path);
                var entries = stock?.FindPropertyRelative(nameof(BaitShopStock.ShopStock));
                if (entries == null || entries.arraySize > 0)
                    continue;

                var defaults = new BaitShopStock();
                foreach (var bait in catalog.Datas)
                {
                    if (bait != null)
                        defaults.ShopStock.Add(new OwnedItemTypeData(0, bait));
                }

                stock.boxedValue = defaults;
                serialized.ApplyModifiedProperties();
            }
        });

        return field;
    }
}