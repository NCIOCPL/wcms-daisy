using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Munger
{
    [global::System.Serializable]
    public class DataAcessLayerException : Exception
    {
        public DataAcessLayerException() { }
        public DataAcessLayerException(string message) : base(message) { }
        public DataAcessLayerException(string message, Exception inner) : base(message, inner) { }
        protected DataAcessLayerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [global::System.Serializable]
    class ImageMungingException : Exception
    {
        public ImageMungingException() { }
        public ImageMungingException(string message) : base(message) { }
        public ImageMungingException(string message, Exception inner) : base(message, inner) { }
        protected ImageMungingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [global::System.Serializable]
    class ImagePathException : ImageMungingException
    {
        public ImagePathException() { }
        public ImagePathException(string message) : base(message) { }
        public ImagePathException(string message, Exception inner) : base(message, inner) { }
        protected ImagePathException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [global::System.Serializable]
    public class LinkMungingException : Exception
    {
        public LinkMungingException() { }
        public LinkMungingException(string message) : base(message) { }
        public LinkMungingException(string message, Exception inner) : base(message, inner) { }
        protected LinkMungingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [global::System.Serializable]
    class LinkPathException : LinkMungingException
    {
        public LinkPathException() { }
        public LinkPathException(string message) : base(message) { }
        public LinkPathException(string message, Exception inner) : base(message, inner) { }
        protected LinkPathException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [global::System.Serializable]
    public class LinkResolutionException : LinkMungingException
    {
        public LinkResolutionException() { }
        public LinkResolutionException(string message) : base(message) { }
        public LinkResolutionException(string message, Exception inner) : base(message, inner) { }
        protected LinkResolutionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [global::System.Serializable]
    public class ProgrammaticLinkException : LinkMungingException
    {
        public ProgrammaticLinkException() { }
        public ProgrammaticLinkException(string message) : base(message) { }
        public ProgrammaticLinkException(string message, Exception inner) : base(message, inner) { }
        protected ProgrammaticLinkException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [global::System.Serializable]
    public class StupidContentLocationException : LinkMungingException
    {
        public StupidContentLocationException() { }
        public StupidContentLocationException(string message) : base(message) { }
        public StupidContentLocationException(string message, Exception inner) : base(message, inner) { }
        protected StupidContentLocationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [global::System.Serializable]
    public class CmsSearchException : LinkMungingException
    {
        public CmsSearchException() { }
        public CmsSearchException(string message) : base(message) { }
        public CmsSearchException(string message, Exception inner) : base(message, inner) { }
        protected CmsSearchException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [global::System.Serializable]
    public class NoLinkSpecifiedException : LinkMungingException
    {
        public NoLinkSpecifiedException() { }
        public NoLinkSpecifiedException(string message) : base(message) { }
        public NoLinkSpecifiedException(string message, Exception inner) : base(message, inner) { }
        protected NoLinkSpecifiedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
