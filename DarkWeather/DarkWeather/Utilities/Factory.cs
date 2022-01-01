using DarkWeather.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Darkweather
{
    public class Factory
    {
        private static ILog Log = DependencyService.Get<ILog>();
        private static string logTag = typeof(Factory).FullName;

        /// <summary> Create child instance from parent instance </summary>
        /// <typeparam name="TChild">Child type</typeparam>
        /// <typeparam name="TParent">Parent type</typeparam>
        /// <param name="parent">Src instance</param>
        /// <returns>Child instance or default(Tt) - null for a ref type - if TChild is not a TParent</returns>
        public static TChild CreateChildFromParent<TChild, TParent>(TParent parent)
        {
            TChild child = default(TChild);
            try
            {
                TypeInfo parentTypeInfo = parent.GetType().GetTypeInfo();
                var parentPropInfos = parentTypeInfo.DeclaredProperties;

                Type childType = typeof(TChild);
                TypeInfo childTypeInfo = childType.GetTypeInfo();
                IEnumerable<ConstructorInfo> childConstructorInfos = childTypeInfo.DeclaredConstructors;
                ConstructorInfo childCtor = childConstructorInfos.FirstOrDefault();

                child = (TChild)childCtor.Invoke(new Object[0]);
                if (!(child is TParent))
                {
                    return default(TChild);
                }
                foreach (var parentProperty in parentPropInfos)
                {
                    PropertyInfo childProperty = childType.GetRuntimeProperty(parentProperty.Name);
                    childProperty.SetValue(child, parentProperty.GetValue(parent));
                }
            }
            catch (Exception ex)
            {
                Log.Error(logTag, "CreateChildFromParent() failed - " + ex.Message);
                throw ex;
            }
            return child;
        }


    }
}
