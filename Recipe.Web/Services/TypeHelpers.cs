using System;
using System.Linq;

namespace Recipe.Web
{
    /// <summary>
    /// Type helper methods
    /// </summary>
    public static class TypeHelpers
    {
        /// <summary>
        /// Recursively walks the inheritance tree, to see if a given Type derives from a specified base type.
        /// Thus given an object where C derives from B and B derives from A, this would return true where
        /// C is the generic type and A or B were the toCheck type. However, if you check 
        /// IsSubclassOfRawGeneric(typeof(B), typeof(C) this would return false.
        /// 
        /// In the case of generic objects, this recursively walks the generic instance type. For example in
        /// the case of a type defined as IEnumerable(of Action(of C)), this would return true if the toCheck value was
        /// typeof(A) because C derives from A.
        /// </summary>
        /// <param name="generic">Object to check.</param>
        /// <param name="toCheck">Desired type to be compared against.</param>
        /// <returns>True if the given type derives from the specified base type or a Generic type contains
        /// instances of the specified target type.</returns>
        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {


            while (toCheck != typeof(object))
            {
                if (toCheck == null)
                {
                    return false;
                }
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                if (generic.IsAssignableFrom(toCheck))
                {
                    return true;
                }

                if (toCheck.GetGenericArguments().Any())
                {
                    return IsSubclassOfRawGeneric(generic, toCheck.GetGenericArguments()[0]);
                }

                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}