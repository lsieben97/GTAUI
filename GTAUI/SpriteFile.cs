using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI
{
    /// <summary>
    /// Represents a sprite file located on disk.
    /// </summary>
    public sealed class SpriteFile
    {
        /// <summary>
        /// The path to the sprite file.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// The with of the sprite.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// The height of the sprite.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// The size of the sprite. Created from the <see cref="Width"/> and <see cref="Height"/>.
        /// </summary>
        public SizeF Size { get => new SizeF(Width, Height); }

        /// <summary>
        /// Create a new sprite file.
        /// </summary>
        /// <param name="path">The path to the sprite file.</param>
        /// <param name="width">The with of the sprite.</param>
        /// <param name="height">The height of the sprite.</param>
        public SpriteFile(string path, int width, int height)
        {
            Path = path;
            Width = width;
            Height = height;
        }
    }
}
