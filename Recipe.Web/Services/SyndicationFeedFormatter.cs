using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Recipe.Web
{

    /// <summary>
    /// Formats objects that support the ISyndicationItemSerializable to serialize them
    /// as Rss or atom output types based on the supplied "accept" header. 
    /// Adapted from http://www.strathweb.com/2012/04/rss-atom-mediatypeformatter-for-asp-net-webapi
    /// </summary>
    public class SyndicationFeedFormatter : MediaTypeFormatter
    {
        const string atom = "application/atom+xml";
        const string rss = "application/rss+xml";

        /// <summary>
        /// Constructor. Adds Atom and RSS as supported media types
        /// </summary>
        public SyndicationFeedFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(atom));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(rss));
        }

        /// <summary>
        /// Indicates if a given type can be written. Checks to see if the type
        /// derives from <see cref="ISyndicationItemSerializable"/> or in the case
        /// of an enumerable, if the collection contains items that derive from that interface.
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True if the item or generic collection's items derives from 
        /// <see cref="ISyndicationItemSerializable"/>. Otherwise returns false.</returns>
        public override bool CanWriteType(Type type)
        {
            if ((TypeHelpers.IsSubclassOfRawGeneric(typeof(ISyndicationItemSerializable), type) || type.IsSubclassOf(typeof(IEnumerable<ISyndicationItemSerializable>))))
            {
                return true;
            }
            else if (type.IsSubclassOf(typeof(XNode)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// The current implementation only supports writing. It does not read syndication feeds.
        /// </summary>
        /// <param name="type">Ignored</param>
        /// <returns>False</returns>
        public override bool CanReadType(Type type)
        {
            return false;
        }

        /// <summary>
        /// Asynchronously writes the supplied values to the output stream.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="stream"></param>
        /// <param name="content"></param>
        /// <param name="transportContext"></param>
        /// <returns></returns>
        public override Task WriteToStreamAsync(Type type, object value, System.IO.Stream stream, HttpContent content, System.Net.TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
                {
                    BuildSyndicationFeed(value, stream, content.Headers.ContentType.MediaType);
                });
        }

        /// <summary>
        /// Builds the syndication feed.
        /// </summary>
        /// <param name="models">Object or list of objects to be serialized.</param>
        /// <param name="stream">Output stream</param>
        /// <param name="contentType">Determines if the output should be formatted as Atom or RSS.</param>
        public void BuildSyndicationFeed(object models, Stream stream, string contentType)
        {
            if (null == models)
            {
                Trace.WriteLine("No Models supplied. Feed is empty.");
                return;
            }
            if (null == stream)
            {
                throw new ArgumentNullException("stream");
            }

            List<SyndicationItem> items = new List<SyndicationItem>();
            var feed = new SyndicationFeed
            {
                Title = new TextSyndicationContent("Reports")
            };
            if (models is IEnumerable<ISyndicationItemSerializable>)
            {
                var enumerator = ((IEnumerable<ISyndicationItemSerializable>)models).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    items.Add(enumerator.Current.BuildSyndicationItem());
                }
            }
            else if (models is ISyndicationItemSerializable)
            {
                items.Add(((ISyndicationItemSerializable)models).BuildSyndicationItem());
            }
            var xModels = models as XElement;
            if (models != null)
            {
                foreach (var child in xModels.Elements())
                {
                    var item = new SyndicationItem();
                    foreach (var node in child.Elements())
                    {
                        item.ElementExtensions.Add(node);
                    }
                    items.Add(item);
                    //items.Add(new SyndicationItem { Content = new TextSyndicationContent( child.ToString()), Title = new TextSyndicationContent( child.Elements().First().Value)});
                }
            }
            feed.Items = items;

            using (XmlWriter writer = XmlWriter.Create(stream))
            {
                if (string.Equals(contentType, atom))
                {
                    Atom10FeedFormatter atomFormatter = new Atom10FeedFormatter(feed);
                    atomFormatter.WriteTo(writer);
                }
                else
                {
                    Rss20FeedFormatter rssFormatter = new Rss20FeedFormatter(feed);
                    rssFormatter.WriteTo(writer);
                }
            }
        }
    }

    /// <summary>
    /// Core interface to indicate that a type supports syndication Serialization.
    /// </summary>
    interface ISyndicationItemSerializable
    {
        /// <summary>
        /// Implement this method to indicate how a specific type should be 
        /// syndicated (Rss/Atom)
        /// </summary>
        /// <returns>Formatted SyndicationItem for a given type</returns>
        SyndicationItem BuildSyndicationItem();
    }
}
