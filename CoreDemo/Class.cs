using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo
{
    public class Class
    {
        // OfficeOpenXml.ExcelWorksheet
        public T GetTypedValue<T>(object v)
        {
            if (v == null)
            {
                T result = default(T);
                return result;
            }
            Type type = v.GetType();
            Type type2 = typeof(T);
            Type type3 = (type2.IsGenericType && type2.GetGenericTypeDefinition().Equals(typeof(Nullable<>))) ? Nullable.GetUnderlyingType(type2) : null;
            if (type == type2 || type == type3)
            {
                return (T)((object)v);
            }
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            if (type2 == typeof(DateTime) || type3 == typeof(DateTime))
            {
                if (type == typeof(TimeSpan))
                {
                    return (T)((object)new DateTime(((TimeSpan)v).Ticks));
                }
                if (type == typeof(string))
                {
                    DateTime dateTime;
                    if (DateTime.TryParse(v.ToString(), out dateTime))
                    {
                        return (T)((object)dateTime);
                    }
                    T result = default(T);
                    return result;
                }
                else
                {
                    if (converter.CanConvertTo(typeof(double)))
                    {
                        return (T)((object)DateTime.FromOADate((double)converter.ConvertTo(v, typeof(double))));
                    }
                    T result = default(T);
                    return result;
                }
            }
            else
            {
                if (type2 == typeof(TimeSpan) || type3 == typeof(TimeSpan))
                {
                    if (type == typeof(DateTime))
                    {
                        return (T)((object)new TimeSpan(((DateTime)v).Ticks));
                    }
                    if (type == typeof(string))
                    {
                        TimeSpan timeSpan;
                        if (TimeSpan.TryParse(v.ToString(), out timeSpan))
                        {
                            return (T)((object)timeSpan);
                        }
                        T result = default(T);
                        return result;
                    }
                    else
                    {
                        if (converter.CanConvertTo(typeof(double)))
                        {
                            return (T)((object)new TimeSpan(DateTime.FromOADate((double)converter.ConvertTo(v, typeof(double))).Ticks));
                        }
                        try
                        {
                            T result = (T)((object)Convert.ChangeType(v, typeof(T)));
                            return result;
                        }
                        catch (Exception)
                        {
                            T result = default(T);
                            return result;
                        }
                    }
                }
                if (converter.CanConvertTo(type2))
                {
                    return (T)((object)converter.ConvertTo(v, typeof(T)));
                }
                if (type3 != null)
                {
                    type2 = type3;
                    if (converter.CanConvertTo(type2))
                    {
                        return (T)((object)converter.ConvertTo(v, type2));
                    }
                }
                if (type == typeof(double) && type2 == typeof(decimal))
                {
                    return (T)((object)Convert.ToDecimal(v));
                }
                if (type == typeof(decimal) && type2 == typeof(double))
                {
                    return (T)((object)Convert.ToDouble(v));
                }
                return default(T);
            }
        }

        public object GetTypedValue(object value, Type conversionType)
        {
            if (conversionType == (Type)null)
            {
                throw new ArgumentNullException("conversionType");
            }
            if (value == null)
            {
                if (conversionType.IsValueType)
                {
                    //throw new InvalidCastException(Environment.GetResourceString("InvalidCast_CannotCastNullToValueType"));
                }
                return null;
            }
            IConvertible convertible = value as IConvertible;
            if (convertible == null)
            {
                if (value.GetType() == conversionType)
                {
                    return value;
                }
                //throw new InvalidCastException(Environment.GetResourceString("InvalidCast_IConvertible"));
            }
            Type type = value.GetType();
            Type type3 = (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>))) ? Nullable.GetUnderlyingType(conversionType) : null;
            if (type == conversionType || type == type3)
            {
                return ((object)value);
            }
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            if (conversionType == typeof(DateTime) || type3 == typeof(DateTime))
            {
                if (type == typeof(TimeSpan))
                {
                    return ((object)new DateTime(((TimeSpan)value).Ticks));
                }
                if (type == typeof(string))
                {
                    DateTime dateTime;
                    if (DateTime.TryParse(value.ToString(), out dateTime))
                    {
                        return ((object)dateTime);
                    }                    
                    //T result = default(T);
                    //return result;
                    return Activator.CreateInstance(conversionType);
                }
                else
                {
                    if (converter.CanConvertTo(typeof(double)))
                    {
                        return ((object)DateTime.FromOADate((double)converter.ConvertTo(value, typeof(double))));
                    }
                    //T result = default(T);
                    //return result;
                    return Activator.CreateInstance(conversionType);
                }
            }
            else
            {
                if (conversionType == typeof(TimeSpan) || type3 == typeof(TimeSpan))
                {
                    if (type == typeof(DateTime))
                    {
                        return ((object)new TimeSpan(((DateTime)value).Ticks));
                    }
                    if (type == typeof(string))
                    {
                        TimeSpan timeSpan;
                        if (TimeSpan.TryParse(value.ToString(), out timeSpan))
                        {
                            return ((object)timeSpan);
                        }
                        //T result = default(T);
                        //return result;
                        return Activator.CreateInstance(conversionType);
                    }
                    else
                    {
                        if (converter.CanConvertTo(typeof(double)))
                        {
                            return ((object)new TimeSpan(DateTime.FromOADate((double)converter.ConvertTo(value, typeof(double))).Ticks));
                        }
                        try
                        {
                            //T result = (T)((object)Convert.ChangeType(value, typeof(T)));
                            //return result;
                            return Convert.ChangeType(value, conversionType);
                        }
                        catch (Exception)
                        {
                            //T result = default(T);
                            //return result;
                            return Activator.CreateInstance(conversionType);
                        }
                    }
                }
                if (converter.CanConvertTo(conversionType))
                {
                    return ((object)converter.ConvertTo(value, conversionType));
                }
                if (type3 != null)
                {
                    conversionType = type3;
                    if (converter.CanConvertTo(conversionType))
                    {
                        return ((object)converter.ConvertTo(value, conversionType));
                    }
                }
                if (type == typeof(double) && conversionType == typeof(decimal))
                {
                    return ((object)Convert.ToDecimal(value));
                }
                if (type == typeof(decimal) && conversionType == typeof(double))
                {
                    return ((object)Convert.ToDouble(value));
                }
                //return default(T);
                return Activator.CreateInstance(conversionType);
            }
        }
    }
}
