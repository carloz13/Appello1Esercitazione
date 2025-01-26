namespace ExtensionMethod
{
    public static class ExtensionMethod
    {
        public static IEnumerable<bool?> IntersectOn<T1, T2>(this IEnumerable<Func<T1, T2>> source, IEnumerable<Func<T1, T2>> other, T1 p)
        {
            using var eSource = source.GetEnumerator();
            using var eOther = other.GetEnumerator();

            while (eSource.MoveNext() && eOther.MoveNext())
            {
                bool? result = null;

                try
                {
                    result = eSource.Current(p).Equals(eOther.Current(p));
                }
                catch (Exception)
                {

                }

                yield return result;

            }

            if (eSource.MoveNext() || eOther.MoveNext()) throw new ArgumentException();


        }



    }
}
