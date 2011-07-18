using System;

namespace NCI.CMS.Percussion.Manager.CMS
{
    /// <summary>
    /// Wrapper class for manipulating the bit-mapped long values Percussion uses
    /// as GUIDs.  This type is immutable.
    /// </summary>
    public sealed class PercussionGuid
    {
        // Percussion object type for user-defined content items.
        public const int UserDefinedContentType = 101;

        public PercussionGuid(int id)
        {
            ID = id;
            Revision = -1;
            Type = UserDefinedContentType;
        }

        public PercussionGuid(long guid)
        {
            Guid = guid;
        }

        public long Guid { get; private set; }

        public static int GetID(long guid) { return (int)(guid & 0xFFFFFFFFL); }

        public int ID
        {
            get { return (int)(Guid & 0xFFFFFFFFL); }
            private set
            {
                unchecked
                {
                    long mask = (long)0xFFFFFFFF00000000L;
                    Guid = (mask & Guid) | (uint)value;
                }
            }
        }

        public static int GetRevision(long guid) { return (int)(guid >> 40); }

        public int Revision
        {
            get { return (int)(Guid >> 40); }
            set
            {
                ulong mask = 0x000000FFFFFFFFFFL;
                ulong temp = (mask & (ulong)Guid) | (((ulong)value) << 40);
                Guid = (long)temp;
            }
        }

        public static int GetType(long guid) { return (int)(guid >> 32) & 0xFF; }

        public int Type
        {
            get { return (int)(Guid >> 32) & 0xFF; }
            set
            {
                unchecked
                {
                    ulong mask = 0xFFFFFF00FFFFFFFFL;
                    ulong temp = (mask & (ulong)Guid) | (((ulong)value) << 32);
                    Guid = (long)temp;
                }
            }
        }

        public override String ToString()
        {
            return string.Format("{0}-{1}-{2}", Revision, Type, ID);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            PercussionGuid rhs = obj as PercussionGuid;
            if ((Object)rhs == null)
            {
                return false;
            }

            return this == rhs;
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public static bool operator ==(PercussionGuid lhs, PercussionGuid rhs)
        {
            bool result;

            if ((Object)lhs != null && (Object)rhs != null)
            {
                result = lhs.Type == rhs.Type
                    && lhs.ID == rhs.ID;
            }
            else
            {
                result = (Object)lhs == (Object)rhs;
            }

            return result;
        }

        public static bool operator !=(PercussionGuid lhs, PercussionGuid rhs)
        {
            return !(lhs == rhs);
        }

    }
}
