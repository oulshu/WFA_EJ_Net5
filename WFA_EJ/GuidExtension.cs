using CSharpVitamins;

namespace WFA_EJ
{
    public static class GuidExtension
    {
        #region Методы

        public static string GetNewGuid() { return ShortGuid.NewGuid().Value; }

        #endregion
    }
}