using System;

namespace Recipe.Web.Areas.HelpPage
{
    /// <summary>
    /// This represents an image sample on the help page. There's a display template named ImageSample associated with this class.
    /// </summary>
    public class ImageSample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSample"/> class.
        /// </summary>
        /// <param name="src">The URL of an image.</param>
        public ImageSample(string src)
        {
            Src = src ?? throw new ArgumentNullException("src");
        }

        public string Src { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is ImageSample other && Src == other.Src;
        }

        public override int GetHashCode()
        {
            return Src.GetHashCode();
        }

        public override string ToString()
        {
            return Src;
        }
    }
}