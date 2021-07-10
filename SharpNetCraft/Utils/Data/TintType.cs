using System.Drawing;

namespace SharpNetCraft.Utils.Data
{
    public enum TintType
    {
        Default,
        Color,
        Grass,
        Foliage,
        Water
    }

    public interface ITinted
    {
        TintType TintType { get; }
        Color TintColor { get; }
    }
}