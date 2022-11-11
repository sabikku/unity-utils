using System;
using System.Reflection;
using System.Linq;
using UnityEngine;

/**
 * Copying components from one GameObject to another through the use of reflection.
 */

public static class CopyComponents {
	public static void CopyProperties<T, Y>(T copyFrom, Y copyTo) {
		PropertyInfo[] copyFromProperties = copyFrom.GetType().GetProperties();
		PropertyInfo[] copyToProperties = copyTo.GetType().GetProperties();

		foreach (PropertyInfo property in copyToProperties) {
			PropertyInfo copiedProperty = copyFromProperties.FirstOrDefault(p => p.Name.Equals(property.Name));

			if (copiedProperty != null) {
				try {
					property.SetValue(copyTo, copiedProperty.GetValue(copyFrom, null), null);
				} catch {
				}
			}
		}
	}

	public static void CopyFields<T, Y>(T copyFrom, Y copyTo) {
		FieldInfo[] copyFromFields = copyFrom.GetType().GetFields();
		FieldInfo[] copyToFields = copyTo.GetType().GetFields();

		foreach (FieldInfo field in copyToFields) {
			FieldInfo copiedField = copyFromFields.FirstOrDefault(p => p.Name.Equals(field.Name));

			if (copiedField != null) {
				try {
					field.SetValue(copyTo, copiedField.GetValue(copyFrom));
				} catch {
				}
			}
		}
	}

	public static void CopyGameObjectComponents(GameObject copyFrom, GameObject copyTo) {
		Component[] copyFromComponents = copyFrom.GetComponents<Component>();

		foreach (Component copyFromComponent in copyFromComponents) {
			Component copyToComponent = copyTo.GetComponent(copyFromComponent.GetType());
			if (copyToComponent == null) {
				copyToComponent = copyTo.AddComponent(copyFromComponent.GetType());
			}

			CopyProperties(copyFromComponent, copyToComponent);
			CopyFields(copyFromComponent, copyToComponent);
		}
	}

	public static T GetCopyOf<T>(this Component comp, T other) where T : Component {
		Type type = comp.GetType();
		if (type != other.GetType()) return null; // type mis-match
		BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
		PropertyInfo[] pinfos = type.GetProperties(flags);
		foreach (var pinfo in pinfos) {
			if (pinfo.CanWrite) {
				try {
					pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
				} catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
			}
		}
		FieldInfo[] finfos = type.GetFields(flags);
		foreach (var finfo in finfos) {
			finfo.SetValue(comp, finfo.GetValue(other));
		}
		return comp as T;
	}

	public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component {
		return go.AddComponent<T>().GetCopyOf(toAdd) as T;
	}
}