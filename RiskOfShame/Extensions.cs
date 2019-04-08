using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RiskOfShame
{
    public static class Extensions
    {
        public static BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                           BindingFlags.Static | BindingFlags.FlattenHierarchy;

        public static Object GetField(this Object obj, String fieldName, String fieldType, Int32 level)
        {
            if (obj == null)
                return null;
            Type objType = obj.GetType();
            for (Int32 i = 0; i < level; i++)
                objType = objType.BaseType;
            FieldInfo field;
            if (fieldType == "")
                field = objType.GetField(fieldName, flags);
            else
                field = objType.GetFields(flags).FirstOrDefault(f => f.Name == fieldName && f.FieldType.ToString().Contains(fieldType));
            if (field == null)
            {
                var property = objType.GetProperty(fieldName, flags);
                return property.GetValue(obj, null);
            }
            if (field != null)
                return field.GetValue(obj);
            else
                return null;
        }
        public static Object GetField(this Object obj, String fieldName, String fieldType)
        {
            return GetField(obj, fieldName, fieldType, 0);
        }
        public static Object GetField(this Object obj, String fieldName, Int32 level)
        {
            return GetField(obj, fieldName, "", level);
        }
        public static Object GetField(this Object obj, String fieldName)
        {
            return GetField(obj, fieldName, 0);
        }
        public static T GetField<T>(this Object obj, String fieldName)
        {
            return (T)GetField(obj, fieldName, typeof(T).Name);
        }
        public static T GetStaticField<T>(this Type obj, String fieldName)
        {
            var field = obj.GetField(fieldName, flags);
            return (T)field.GetValue(null);
        }
        public static void SetStaticField<T>(this Type obj, String fieldName, T val)
        {
            var field = obj.GetField(fieldName, flags);
            field.SetValue(null, val);
        }

        public static void SetField<T>(this Object obj, String fieldName, String fieldType, Int32 level, T val)
        {
            Type objType = obj.GetType();
            for (var i = 0; i < level; i++)
                objType = objType.BaseType;
            FieldInfo field;
            if (fieldType == "")
                field = objType.GetField(fieldName, flags);
            else
                field = objType.GetFields(flags).FirstOrDefault(f => f.Name == fieldName && f.FieldType.ToString().Contains(fieldType));
            if (field != null)
                field.SetValue(obj, val);
        }
        public static void SetField<T>(this Object obj, String fieldName, String fieldType, T val)
        {
            SetField(obj, fieldName, fieldType, 0, val);
        }
        public static void SetField<T>(this Object obj, String fieldName, Int32 level, T val)
        {
            SetField(obj, fieldName, "", level, val);
        }
        public static void SetField<T>(this Object obj, String fieldName, T val)
        {
            SetField(obj, fieldName, "", 0, val);
        }

        public static List<Object> GetList(this Object obj)
        {
            var methods = obj.GetType().GetMethods(flags);
            var obj_get_Item = methods.First(m => m.Name == "get_Item");
            var obj_Count = obj.GetType().GetProperty("Count");
            var count = (Int32)obj_Count.GetValue(obj, new Object[0]);
            var elements = new List<Object>();
            for (Int32 i = 0; i < count; i++)
                elements.Add(obj_get_Item.Invoke(obj, new Object[] { i }));
            return elements;
        }
        public static void Invoke(this Type obj, String methodName, params Object[] paramArray)
        {
            var method = obj.GetMethod(methodName, flags);
            method.Invoke(null, paramArray);
        }
        public static void Invoke(this Object obj, String methodName, params Object[] paramArray)
        {
            var type = obj.GetType();
            var method = type.GetMethod(methodName, flags);
            method.Invoke(obj, paramArray);
        }
        public static T CreateInstance<T>(params Object[] paramArray)
        {
            return (T)Activator.CreateInstance(typeof(T), args: paramArray);
        }
    }
}