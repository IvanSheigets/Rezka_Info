using System;
using System.Collections.Generic;
using System.Text;

namespace BarCode39Reader
{
    /// <summary>
    /// This class represents a Bar in a BarCode
    /// </summary>
    public class Bar
    {
        #region Private Fields
        private int x1;
        private int x2;
        private bool isBlack;
        private Width width;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x1">the leftmost location</param>
        /// <param name="x2">the rightmost location</param>
        /// <param name="isBlack">true if the bar is black, false if the bar is white</param>
        public Bar( int x1, int x2, bool isBlack )
        {
            this.x1 = x1;
            this.x2 = x2;
            this.isBlack = isBlack;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the leftmost location
        /// </summary>
        public int X1
        {
            get { return this.x1; }
        }

        /// <summary>
        /// Gets and sets the rightmost location
        /// </summary>
        public int X2
        {
            get { return this.x2; }
            set { this.x2 = value; }
        }

        /// <summary>
        /// Gets the color of the bar (true if the bar is black, false if the bar is white)
        /// </summary>
        public bool IsBlack
        {
            get { return this.isBlack; }
        }

        /// <summary>
        /// Gets and sets the Width of the bar
        /// </summary>
        public Width Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

        /// <summary>
        /// Returns the Code39 format of the bar
        /// </summary>
        public char Code
        {
            get
            {
                if ( this.isBlack )
                {
                    if ( this.width == Width.Wide )
                    {
                        return 'W';
                    }
                    else
                    {
                        return 'N';
                    }
                }
                else
                {
                    if ( this.width == Width.Wide )
                    {
                        return 'w';
                    }
                    else
                    {
                        return 'n';
                    }
                }
            }
        }
        #endregion
    }
}
