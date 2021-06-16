using System;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public static class ExtensionMethods
{
    public static void DestroyAllChildren(this Transform a_transform)
    {
        foreach (Transform child in a_transform)
            GameObject.Destroy(child.gameObject);
        a_transform.DetachChildren();
    }

    public static object GetNewBaseObject(this Type a_type)
    {
		if (a_type == typeof(string))
			return "";
		else if (a_type == typeof(int) || a_type == typeof(int?))
			return 0;
		else if (a_type == typeof(long) || a_type == typeof(long?))
			return 0L;
		else if (a_type == typeof(float) || a_type == typeof(float?))
			return 0f;
		else if (a_type == typeof(double) || a_type == typeof(double?))
			return 0.0;
		else
			return Activator.CreateInstance(a_type);
    }

    public static Vector2 GetSnapToPositionToBringChildIntoView(this ScrollRect instance, RectTransform child)
    {
        Canvas.ForceUpdateCanvases();
        Vector2 viewportLocalPosition = instance.viewport.localPosition;
        Vector2 childLocalPosition = child.localPosition;
        Vector2 result = new Vector2(
            0 - (viewportLocalPosition.x + childLocalPosition.x),
            0 - (viewportLocalPosition.y + childLocalPosition.y)
        );
        return result;
    }

    public static bool IsNullable<U>(this U obj)
    {
        if (obj == null) return true;
        Type type = obj.GetType();
        if (!type.IsValueType) return true; // ref-type
        if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
        return false; // value-type
    }
}

