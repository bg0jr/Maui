using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Maui.Entities
{
    [Serializable]
    public class BusinessYear
    {
        private int myBeginDay = -1;
        private int myBeginMonth = -1;
        private int myEndDay = -1;
        private int myEndMonth = -1;

        public BusinessYear()
        {
            IsModified = true;
        }

        public int BeginDay
        {
            get { return myBeginDay; }
            set
            {
                if ( myBeginDay == value )
                {
                    return;
                }

                myBeginDay = value;
                IsModified = true;
            }
        }

        public int BeginMonth
        {
            get { return myBeginMonth; }
            set
            {
                if ( myBeginMonth == value )
                {
                    return;
                }

                myBeginMonth = value;
                IsModified = true;
            }
        }

        public int EndDay
        {
            get { return myEndDay; }
            set
            {
                if ( myEndDay == value )
                {
                    return;
                }

                myEndDay = value;
                IsModified = true;
            }
        }

        public int EndMonth
        {
            get { return myEndMonth; }
            set
            {
                if ( myEndMonth == value )
                {
                    return;
                }

                myEndMonth = value;
                IsModified = true;
            }
        }

        public bool IsValid
        {
            get { return ( myBeginDay != -1 && myBeginMonth != -1 && myEndDay != -1 && myEndMonth != -1 ); }
        }

        public bool IsModified { get; set; }

        public static BusinessYear FromString( string begin, string end )
        {
            if ( begin == null )
            {
                throw new ArgumentNullException( "begin" );
            }

            if ( end == null )
            {
                throw new ArgumentNullException( "end" );
            }


            string[] tokens = begin.Split( '-', '.' );

            BusinessYear by = new BusinessYear();
            by.myBeginDay = Convert.ToInt32( tokens[ 0 ], CultureInfo.InvariantCulture );
            by.myBeginMonth = Convert.ToInt32( tokens[ 1 ], CultureInfo.InvariantCulture );

            tokens = end.Split( '-', '.' );
            by.myEndDay = Convert.ToInt32( tokens[ 0 ], CultureInfo.InvariantCulture );
            by.myEndMonth = Convert.ToInt32( tokens[ 1 ], CultureInfo.InvariantCulture );

            return by;
        }

        public override bool Equals( object obj )
        {
            if ( obj == null )
            {
                return false;
            }

            BusinessYear otherBY = obj as BusinessYear;
            if ( otherBY == null )
            {
                return false;
            }

            if ( myBeginDay != otherBY.BeginDay || myBeginMonth != otherBY.myBeginMonth ||
                myEndDay != otherBY.myEndDay || myEndMonth != otherBY.myEndMonth )
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            Int32 preHash = myBeginDay * 1000000 + myBeginMonth * 10000 + myEndDay * 100 + myEndMonth;
            return preHash.GetHashCode();
        }

        public static bool operator ==( BusinessYear left, BusinessYear right )
        {
            if ( object.ReferenceEquals( left, null ) && object.ReferenceEquals( right, null ) )
            {
                return true;
            }

            if ( object.ReferenceEquals( left, null ) || object.ReferenceEquals( right, null ) )
            {
                return false;
            }

            return ( left.myBeginDay == right.BeginDay && left.myBeginMonth == right.myBeginMonth &&
                left.myEndDay == right.myEndDay && left.myEndMonth == right.myEndMonth );
        }

        public static bool operator !=( BusinessYear left, BusinessYear right )
        {
            return !( left == right );
        }

        public static BusinessYear FromString( string value )
        {
            if ( value == null )
            {
                throw new ArgumentNullException( "value" );
            }

            string[] tokens = value.Split( ' ', '-' );

            BusinessYear by = new BusinessYear();
            by.myBeginDay = Convert.ToInt32( tokens[ 0 ], CultureInfo.InvariantCulture );
            by.myBeginMonth = Convert.ToInt32( tokens[ 1 ], CultureInfo.InvariantCulture );
            by.myEndDay = Convert.ToInt32( tokens[ 2 ], CultureInfo.InvariantCulture );
            by.myEndMonth = Convert.ToInt32( tokens[ 3 ], CultureInfo.InvariantCulture );

            return by;
        }

        public override string ToString()
        {
            if ( !IsValid )
            {
                return null;
            }

            return myBeginDay + "-" + myBeginMonth + " " + myEndDay + "-" + myEndMonth;
        }
    }
}
