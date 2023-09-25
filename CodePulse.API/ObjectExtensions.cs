namespace CodePulse.API
{
    using System;
    using System.Reflection;

    public static class ObjectExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// List&lt;BlogPostDto&gt; blogPostDtos = blogPosts.MapPropertiesTo&lt;BlogPost, BlogPostDto&gt;((post, dto) =&gt;
        /// {
        /// dto.Categories = post.Categories.MapPropertiesTo&lt;Category, CategoryDto&gt;();
        /// });
        /// </example>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="actions"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static List<TDestination> MapPropertiesTo<TSource, TDestination>(this IEnumerable<TSource> source, params Action<TSource, TDestination>[] actions)
            where TSource : class, new()
            where TDestination : class, new()
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            List<TDestination> destination = new List<TDestination>();

            foreach (TSource item in source)
            {
                TDestination mappedItem = item.MapPropertiesTo<TSource, TDestination>();

                foreach (Action<TSource, TDestination> action in actions)
                    action(item, mappedItem);

                destination.Add(mappedItem);
            }

            return destination;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// BlogPostDto blogPostDto = blogPost.MapPropertiesTo&lt;BlogPost, BlogPostDto&gt;((post, dto) =&gt;
        /// {
        ///   dto.Categories = post.Categories.MapPropertiesTo&lt;Category, CategoryDto&gt;();
        /// });
        /// </example>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="actions"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TDestination MapPropertiesTo<TSource, TDestination>(this TSource source, params Action<TSource, TDestination>[] actions) where TSource : class, new() 
            where TDestination : class, new()
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Type sourceType = source.GetType();
            TDestination destination = new TDestination();
            Type destinationType = typeof(TDestination);

            foreach (PropertyInfo sourceProperty in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!sourceProperty.CanRead) continue; // Ensure that the source property is readable

                PropertyInfo destinationProperty = destinationType.GetProperty(sourceProperty.Name, BindingFlags.Public | BindingFlags.Instance);

                if (destinationProperty != null && destinationProperty.CanWrite) // Ensure that the destination property exists and is writable
                {
                    if (destinationProperty.PropertyType == sourceProperty.PropertyType) // Check for type compatibility
                    {
                        object value = sourceProperty.GetValue(source, null);
                        destinationProperty.SetValue(destination, value, null);
                    }
                }
            }

            foreach (Action<TSource, TDestination> action in actions)
                action(source, destination);

            return destination;
        }
    }


}
