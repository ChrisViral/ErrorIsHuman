using System;
using System.Collections.Generic;
using System.Text;

//Warning for inheriting from obsolete class
#pragma warning disable 618

namespace ErrorIsHuman.Utils
{
    /// <summary>
    /// Unused. Anything in here should ONLY ever be called through EnumUtils.
    /// </summary>
    /// <typeparam name="TEnum">Type should be Enum. Really, it's gonna throw if you don't.</typeparam>
    [Obsolete("This class is only a helper class, do not use this, use EnumUtils instead")]
    public abstract class EnumConstraint<TEnum> where TEnum : class
    {
        /// <summary>
        /// Generic enum conversion utility class
        /// </summary>
        private sealed class EnumConverter
        {
            #region Fields
            /// <summary>
            /// Stores the string -> enum conversion
            /// </summary>
            private readonly Dictionary<string, TEnum> values = new Dictionary<string, TEnum>();

            /// <summary>
            /// Stores the enum -> string conversion
            /// </summary>
            private readonly Dictionary<TEnum, string> names = new Dictionary<TEnum, string>();

            /// <summary>
            /// The name of the enum values correctly ordered for index search
            /// </summary>
            public readonly string[] orderedNames;

            /// <summary>
            /// The values of the Enum correctly ordered for index search
            /// </summary>
            public readonly TEnum[] orderedValues;
            #endregion

            #region Constructor
            /// <summary>
            /// Creates a new EnumConvertor from the given type
            /// </summary>
            /// <param name="enumType">Type of converter. Must be an enum type.</param>
            public EnumConverter(Type enumType)
            {
                if (enumType == null) { throw new ArgumentNullException(nameof(enumType), "Enum conversion type cannot be null"); }
                Array v = Enum.GetValues(enumType);
                
                List<string> tempNames = new List<string>(v.Length);
                List<TEnum> tempValues = new List<TEnum>(v.Length);
                for (int i = 0; i < v.Length; i++)
                {
                    TEnum value = (TEnum)v.GetValue(i);
                    string name = Enum.GetName(enumType, value);
                    if (!string.IsNullOrEmpty(name) && !this.names.ContainsKey(value) && !this.values.ContainsKey(name))
                    {
                        tempNames.Add(name);
                        tempValues.Add(value);
                        this.names.Add(value, name);
                        this.values.Add(name, value);
                    }
                }

                this.orderedNames = tempNames.ToArray();
                this.orderedValues = tempValues.ToArray();
            }
            #endregion

            #region Methods
            /// <summary>
            /// Gets the stored enum value for this name
            /// </summary>
            /// <param name="name">Name of the value to get</param>
            public T GetEnumValue<T>(string name) where T : struct, TEnum => (T)this.values[name];

            /// <summary>
            /// Gets the stored name for the given enum value
            /// </summary>
            /// <param name="value">Enum value to get the name for</param>
            public string GetEnumName<T>(T value) where T : struct, TEnum => this.names[value];
            #endregion
        }

        #region Fields
        /// <summary>
        /// Holds all the known enum converters
        /// </summary>
        private static readonly Dictionary<Type, EnumConverter> converters = new Dictionary<Type, EnumConverter>();

        /// <summary>
        /// Dummy array containing only the underscore char
        /// </summary>
        private static readonly char[] underscore = { '_' };
        #endregion

        #region Methods
        /// <summary>
        /// Returns the converter of the given type or creates one if there are none
        /// </summary>
        /// <typeparam name="T">Type of the enum conversion</typeparam>
        private static EnumConverter GetConverter<T>() where T : struct, TEnum
        {
            Type enumType = typeof(T);
            if (!converters.TryGetValue(enumType, out EnumConverter converter))
            {
                converter = new EnumConverter(enumType);
                converters.Add(enumType, converter);
            }
            return converter;
        }

        /// <summary>
        /// Returns the string value of an Enum
        /// </summary>
        /// <param name="value">Enum value to convert to string</param>
        public static string GetName<T>(T value) where T : struct, TEnum => GetConverter<T>().GetEnumName(value);

        /// <summary>
        /// Returns the name of the given enum member in Title Case
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Value of the enum member</param>
        /// <returns>Title Case name</returns>
        public static string GetNameTitleCase<T>(T value) where T : struct, TEnum => ToCamelCase(GetName(value));

        /// <summary>
        /// Parses the given string to the given Enum type 
        /// </summary>
        /// <typeparam name="T">Type of the enum</typeparam>
        /// <param name="name">String to parse</param>
        public static T GetValue<T>(string name) where T : struct, TEnum => GetConverter<T>().GetEnumValue<T>(name);

        /// <summary>
        /// Gets the enum value at the given index
        /// </summary>
        /// <typeparam name="T">Type of the enum</typeparam>
        /// <param name="index">Index of the element to get</param>
        public static T GetValueAt<T>(int index) where T : struct, TEnum
        {
            EnumConverter converter = GetConverter<T>();
            if (index < 0 || index >= converter.orderedNames.Length) { return default; }
            return converter.GetEnumValue<T>(converter.orderedNames[index]);
        }

        /// <summary>
        /// Finds the string name of the enum value at the given index
        /// </summary>
        /// <typeparam name="T">Type of the enum</typeparam>
        /// <param name="index">Index of the name to find</param>
        public static string GetNameAt<T>(int index) where T : struct, TEnum
        {
            EnumConverter converter = GetConverter<T>();
            return index < 0 || index >= converter.orderedNames.Length ? null : converter.orderedNames[index];
        }

        /// <summary>
        /// Returns the string representation of each enum member in order
        /// </summary>
        /// <typeparam name="T">Type of the enum</typeparam>
        public static string[] GetNames<T>() where T : struct, TEnum => GetConverter<T>().orderedNames;

        /// <summary>
        /// Gets an array of all the values of the Enum
        /// </summary>
        /// <typeparam name="T">Type of the Enum</typeparam>
        public static T[] GetValues<T>() where T : struct, TEnum => Array.ConvertAll(GetConverter<T>().orderedValues, v => (T)v);

        /// <summary>
        /// Returns the index of the Enum value of the given name
        /// </summary>
        /// <typeparam name="T">Type of the Enum</typeparam>
        /// <param name="name">Name of the element to find</param>
        public static int IndexOf<T>(string name) where T : struct, TEnum => Array.IndexOf(GetNames<T>(), name);

        /// <summary>
        /// Returns the index of the Enum member of the given value
        /// </summary>
        /// <typeparam name="T">Type of the Enum</typeparam>
        /// <param name="value">Value to find the index of</param>
        public static int IndexOf<T>(T value) where T : struct, TEnum => Array.IndexOf(GetValues<T>(), value);

        /// <summary>
        /// Transforms an ALL_CAPS string to a CamelCase string
        /// </summary>
        /// <param name="s">String to transform</param>
        /// <returns>The CamelCase version of the string</returns>
        public static string ToCamelCase(string s)
        {
            //Setup Stringbuilder
            bool upper = true;
            StringBuilder sb = new StringBuilder(s.Length);

            //Loop through reference string
            foreach (char c in s)
            {
                //Add characters correctly
                if (c == '_') { upper = true; }
                else if (upper)
                {
                    sb.Append(c);
                    upper = false;
                }
                else { sb.Append(char.ToLower(c)); }
            }
            //Return the final string
            return sb.ToString();
        }
        #endregion
    }

    /// <summary>
    /// Enum utility class, containing various enum parsing/tostring methods.
    /// </summary>
    public sealed class EnumUtils : EnumConstraint<Enum>
    {
        /* Nothing to see here, this is mostly just a dummy class to force T to be an Enum.
         * The actual implementation is in EnumConstraint */

        #region Constructor
        /// <summary>
        /// Prevents object instantiation, this should act as a static class
        /// </summary>
        private EnumUtils() { }
        #endregion
    }
}