using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BarCode39Reader
{
    public class Pattern
    {
        private bool[] nw = new bool[9];

        public static Pattern Parse( string s )
        {
            s = s.Replace( " ", "" ).ToLower();

            Pattern p = new Pattern();

            int i = 0;
            foreach ( char c in s )
                p.nw[i++] = c == 'w';

            return p;
        }

        public int GetWidth( Code39Settings settings )
        {
            int width = 0;

            for ( int i = 0; i < 9; i++ )
                width += ( nw[i] ? settings.WideWidth : settings.NarrowWidth );

            return width;
        }

        public int Paint( Code39Settings settings, Graphics g, int left )
        {
            int x = left;

            int w = 0;
            for ( int i = 0; i < 9; i++ )
            {
                int width = ( nw[i] ? settings.WideWidth : settings.NarrowWidth );

                if ( i % 2 == 0 )
                {
                    Rectangle r = new Rectangle( x, settings.TopMargin, width, settings.BarCodeHeight );
                    g.FillRectangle( Brushes.Black, r );
                }

                x += width;
                w += width;
            }

            return w;
        }
    }
}
