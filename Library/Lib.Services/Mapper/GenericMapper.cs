using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Lib.Services
{

    public interface IGenericMapper : IDisposable
    {
        /// <summary>
        /// Map Entites to Dto class and return collection
        /// </summary>
        /// <typeparam name="TResult">ViewModel</typeparam>
        /// <typeparam name="TSource">Db Entity</typeparam>
        /// <param name="entites">Db Entites Data</param>
        /// <param name="action">Action<TResult></param>
        /// <returns>IEnumerable<ViewModel></returns>
        IEnumerable<TResult> Map<TResult, TSource>(IEnumerable<TSource> entites, Action<TResult> action = null) where TResult : new();

        /// <summary>
        /// Map Entity to Dto class and return
        /// </summary>
        /// <typeparam name="TResult">ViewModel</typeparam>
        /// <typeparam name="TSource">Db Entity</typeparam>
        /// <param name="entity">Db Entity Data</param>
        /// <param name="action">Action<TResult></param>
        /// <returns>ViewModel</returns>
        TResult Map<TResult, TSource>(TSource entity, Action<TResult> action = null) where TResult : new();

        /// <summary>
        /// Map Entity to Dto class
        /// </summary>
        /// <typeparam name="TResult">ViewModel</typeparam>
        /// <typeparam name="TSource">Db Entity</typeparam>
        /// <param name="entity">Db Entity Data</param>
        /// <param name="result">ViewModel</param>
        /// <param name="action">Action<TResult></param>
        /// <returns>void</returns>
        void MapBind<TResult, TSource>(TSource entity, out TResult result, Action<TResult> action = null) where TResult : new();

        /// <summary>
        /// Map Entites to Dto class
        /// </summary>
        /// <typeparam name="TResult">ViewModel</typeparam>
        /// <typeparam name="TSource">Db Entity</typeparam>
        /// <param name="entites">Db Entites Data</param>
        /// <param name="result">Colection of ViewModel </param>
        /// <param name="action">Action<TResult></param>
        /// <returns>void</returns>
        void MapBind<TResult, TSource>(IEnumerable<TSource> entites, out IEnumerable<TResult> result, Action<TResult> action = null) where TResult : new();
    }

    public class GenericMapper : IGenericMapper
    {

        #region Properties
        private readonly MapperConfig _mapperConfig;
        private bool disposedValue;
        #endregion

        #region Ctor
        public GenericMapper(MapperConfig mapperConfig = null)
        {
            if (mapperConfig == null)
                _mapperConfig = new MapperConfig();
            else
                _mapperConfig = mapperConfig;
        }
        #endregion

        #region Mapper
        public TResult Map<TResult, TSource>(TSource entity, Action<TResult> action = null) where TResult : new()
        {
            if (entity != null)
            {
                var target = (TResult)Activator.CreateInstance(typeof(TResult));
                Merge(target, entity, action, entity.GetType());

                if (action != null)
                    action.Invoke(target);

                return target;
            }
            return default;
        }

        public IEnumerable<TResult> Map<TResult, TSource>(IEnumerable<TSource> entites, Action<TResult> action = null) where TResult : new()
        {
            var sdf= entites.Select(p => Map<TResult, TSource>(p, action));
            return sdf.ToList();
        }

        public void MapBind<TResult, TSource>(TSource entity, out TResult result, Action<TResult> action = null) where TResult : new()
        {
            var target = (TResult)Activator.CreateInstance(typeof(TResult));
            Merge(target, entity, action, entity.GetType());

            if (action != null)
                action.Invoke(target);

            result = target;
        }

        public void MapBind<TResult, TSource>(IEnumerable<TSource> entites, out IEnumerable<TResult> result, Action<TResult> action = null) where TResult : new()
        {
            result = entites.Select(r =>
            {
                MapBind(r, out TResult s, action);
                return s;
            });
        }

        #endregion

        #region Private
        private void SetValue<TResult, TSource>(
            TResult target, TSource source,
            PropertyInfo targetProp, PropertyInfo srcProperty, Type parentSrc = null)
        {

            if (parentSrc != null && srcProperty.PropertyType.Equals(parentSrc))
            {
                srcProperty.SetValue(source, null, null);
                return;
            }


            object getSrcValue = srcProperty.GetValue(source, null);

            Type tPropType = targetProp.PropertyType;
            Type sPropType = getSrcValue?.GetType();
            //Nullable properties have to be treated differently, since we 
            //  use their underlying property to set the value in the object

            if (tPropType.Name.ToLower() == nameof(String).ToLower())
            {
                targetProp.SetValue(target, Convert.ToString(getSrcValue), null);
                return;
            }

            if (tPropType.IsGenericType && tPropType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                //if it's null, just set the value from the reserved word null, and return
                if (getSrcValue == null)
                    targetProp.SetValue(target, null, null);
                else
                {
                    tPropType = new NullableConverter(tPropType).UnderlyingType;
                    targetProp.SetValue(target, Convert.ChangeType(getSrcValue, tPropType), null);
                }
            }
            else
            {
                if (getSrcValue == null)
                    targetProp.SetValue(target, null, null);
                else
                {
                    /// check property from dto 
                    if (tPropType.IsEnum)
                        targetProp.SetValue(target, getSrcValue, null);
                    else if (tPropType.IsClass && !tPropType.Equals(sPropType))
                    {
                        /// if db.entity property is int and has enum type
                        /// 

                        if (_mapperConfig != null && _mapperConfig.Enums !=null && _mapperConfig.Enums.ContainsKey(srcProperty.Name))
                        {
                            var enumType = _mapperConfig.Enums[srcProperty.Name];
                            if (tPropType.Name.ToLower() == "string" && srcProperty.PropertyType.Name.ToLower() == "int32")
                            {
                                var val = Enum.GetName(enumType, getSrcValue);
                                targetProp.SetValue(target, Convert.ChangeType(val, tPropType), null);
                            }
                            else if (tPropType.Name.ToLower() == "int32" && srcProperty.PropertyType.Name.ToLower() == "string")
                            {
                                var val = (int)Enum.Parse(enumType, getSrcValue.ToString());
                                targetProp.SetValue(target, Convert.ChangeType(val, tPropType), null);
                            }
                        }
                        else
                        {
                            var t1 = Activator.CreateInstance(tPropType);
                            Merge(t1, getSrcValue, null, parentSrc);
                            targetProp.SetValue(target, Convert.ChangeType(t1, tPropType), null);
                        }
                    }
                    else if (tPropType != typeof(string) &&
                        typeof(IEnumerable).IsAssignableFrom(tPropType) ||
                        typeof(IList).IsAssignableFrom(tPropType) ||
                        typeof(List<>).IsAssignableFrom(tPropType))
                    {
                        var targetP = tPropType.GenericTypeArguments[0];
                        var srcProp = srcProperty.PropertyType.GenericTypeArguments[0];

                        //We can't "instantiate" something as ephemeral as an IEnumerable,
                        //so we need something more concrete like a List
                        //There might be other ways to filter - this seemed to be the easiest

                        var listType = typeof(List<>).MakeGenericType(targetP);
                        var instance = (IList)Activator.CreateInstance(listType);

                        var currentEnum = (IEnumerable)getSrcValue;
                        foreach (var item in currentEnum)
                        {
                            if (item != default) // != null would be silly for booleans and ints
                            {
                                var gent1 = tPropType.GenericTypeArguments[0];
                                var t1 = Activator.CreateInstance(gent1);
                                Merge(t1, item, null, source.GetType());
                                instance.Add(t1);
                            }
                        }

                        targetProp.SetValue(target, instance, null);
                    }
                    else
                        targetProp.SetValue(target, Convert.ChangeType(getSrcValue, tPropType), null);
                }
            }
        }

        private void Merge<TResult, TSource>(TResult target, TSource source, Action<TResult> action = null, Type parentSrc = null)
        {
            if (source != null)
            {
                if (target == null)
                    target = (TResult)Activator.CreateInstance(typeof(TResult));

                var properties = target.GetType().GetProperties().Where(prop => prop.CanRead && prop.CanWrite).ToList();
                foreach (var propertyInfo in properties)
                {
                    var srcProperty = source.GetType().GetProperties().FirstOrDefault(
                        f =>
                        f.Name.ToLower().Equals(propertyInfo.Name.ToLower()) ||
                        (f.GetCustomAttributes(typeof(JsonPropertyAttribute), true).Cast<JsonPropertyAttribute>().FirstOrDefault()?.PropertyName.ToLower().Equals(propertyInfo.Name.ToLower()) ?? false));

                    ///If map property exist in source
                    if (srcProperty != null)
                        SetValue(target, source, propertyInfo, srcProperty, parentSrc);
                }

                if (action != null)
                    action.Invoke(target);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~GenericMapper()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    public class MapperConfig
    {
        public bool DisableSelfRef { get; set; }
        public Dictionary<string, Type> Enums { get; set; }
    }
}
