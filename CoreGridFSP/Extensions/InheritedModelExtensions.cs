using System.Linq;
using System.Reflection;

namespace CoreGridFSP.Extensions
{
    public static class InheritedModelExtensions
    {
        /// <summary>
        /// Copies all common properties from one model to another. Useful when creating view models that inherit from data models. 
        /// </summary>
        /// <typeparam name="C">Type of model to copy to</typeparam>
        /// <typeparam name="F">Type of model to copy from</typeparam>
        /// <param name="newObject">The model being copied to</param>
        /// <param name="copyFrom">The model being copied from</param>
        /// <example>
        /// using CoreGridFSP.Extensions;
        /// ...
        /// var newViewModel = new ModelForView();
        /// newViewModel.ShallowCopy(existingDataModel);
        /// 
        /// </example>
        public static void ShallowCopy<C, F>(this C newObject, F copyFrom)
        {
            PropertyInfo[] fromProperties = typeof(F).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] toProperties = typeof(C).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var fpi in fromProperties)
            {
                PropertyInfo tpi = toProperties.FirstOrDefault(p => p.Name == fpi.Name);
                if (tpi != null && tpi.CanWrite)
                {
                    tpi.SetValue(newObject, fpi.GetValue(copyFrom, null), null);
                }
            }
        }
    }
}
