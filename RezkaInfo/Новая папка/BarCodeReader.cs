using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BarCode39Reader
{
    /// <summary>
    /// This class represents a BarCodeReader
    /// </summary>
    public class BarCodeReader
    {
        #region Public Methods
        /// <summary>
        /// Decodes the Code39 barcode in a given bitmap
        /// </summary>
        /// <param name="barCodeBitmap">the bitmap</param>
        /// <returns>the code</returns>
        public static string Decode( Bitmap barCodeBitmap )
        {
            StringBuilder code = new StringBuilder();

            Bar[] bars = FindBars( barCodeBitmap );
            Dictionary<string, char> codeDictionary = GetDictionary();

            int i = 0;
            while ( i < bars.Length )
            {
                StringBuilder key = new StringBuilder();
                for ( int j = i; j < i + 9 && j < bars.Length; ++j )
                {
                    key.Append( bars[j].Code );
                }

                char character;
                if ( codeDictionary.TryGetValue( key.ToString(), out character ) )
                {
                    code.Append( character );
                }

                i += 10;
            }

            if ( code[0] == code[code.Length - 1] && code[0] == '*' )
            {
                return code.ToString().Substring(1, code.Length - 2);
            }

            throw new Exception( "Could not decode barcode" );

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets a dictionary with the characters each collection of 9 bars represents
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, char> GetDictionary()
        {
            Dictionary<string, char> dictionary = new Dictionary<string, char>();
            dictionary.Add( "NwNnWnWnN", '*' );
            dictionary.Add( "NwNnNnWnW", '-' );
            dictionary.Add( "NwNwNwNnN", '$' );
            dictionary.Add( "NnNwNwNwN", '%' );
            dictionary.Add( "NwWnNnWnN", ' ' );
            dictionary.Add( "WwNnNnWnN", '.' );
            dictionary.Add( "NwNwNnNwN", '/' );
            dictionary.Add( "NwNnNwNwN", '+' );
            dictionary.Add( "NnNwWnWnN", '0' );
            dictionary.Add( "WnNwNnNnW", '1' );
            dictionary.Add( "NnWwNnNnW", '2' );
            dictionary.Add( "WnWwNnNnN", '3' );
            dictionary.Add( "NnNwWnNnW", '4' );
            dictionary.Add( "WnNwWnNnN", '5' );
            dictionary.Add( "NnWwWnNnN", '6' );
            dictionary.Add( "NnNwNnWnW", '7' );
            dictionary.Add( "WnNwNnWnN", '8' );
            dictionary.Add( "NnWwNnWnN", '9' );
            dictionary.Add( "WnNnNwNnW", 'A' );
            dictionary.Add( "NnWnNwNnW", 'B' );
            dictionary.Add( "WnWnNwNnN", 'C' );
            dictionary.Add( "NnNnWwNnW", 'D' );
            dictionary.Add( "WnNnWwNnN", 'E' );
            dictionary.Add( "NnWnWwNnN", 'F' );
            dictionary.Add( "NnNnNwWnW", 'G' );
            dictionary.Add( "WnNnNwWnN", 'H' );
            dictionary.Add( "NnWnNwWnN", 'I' );
            dictionary.Add( "NnNnWwWnN", 'J' );
            dictionary.Add( "WnNnNnNwW", 'K' );
            dictionary.Add( "NnWnNnNwW", 'L' );
            dictionary.Add( "WnWnNnNwN", 'M' );
            dictionary.Add( "NnNnWnNwW", 'N' );
            dictionary.Add( "WnNnWnNwN", 'O' );
            dictionary.Add( "NnWnWnNwN", 'P' );
            dictionary.Add( "NnNnNnWwW", 'Q' );
            dictionary.Add( "WnNnNnWwN", 'R' );
            dictionary.Add( "NnWnNnWwN", 'S' );
            dictionary.Add( "NnNnWnWwN", 'T' );
            dictionary.Add( "WwNnNnNnW", 'U' );
            dictionary.Add( "NwWnNnNnW", 'V' );
            dictionary.Add( "WwWnNnNnN", 'W' );
            dictionary.Add( "NwNnWnNnW", 'X' );
            dictionary.Add( "WwNnWnNnN", 'Y' );
            dictionary.Add( "NwWnWnNnN", 'Z' );

            return dictionary;
        }

        /// <summary>
        /// Returns the Bars in a Bitmap
        /// </summary>
        /// <param name="barCodeBitmap">the bitmap</param>
        /// <returns>A collection of Bars found in the bitmap</returns>
        private static Bar[] FindBars( Bitmap barCodeBitmap )
        {
            List<Bar> bars = new List<Bar>();

            Bar currentBarCode = new Bar( 0, 0, false );

            int height = (int)( barCodeBitmap.Height / 2.0 );
            for ( int currentX = 0; currentX < barCodeBitmap.Width; ++currentX )
            {
                Color color = barCodeBitmap.GetPixel( currentX, height );
                bool currentlyBlack = IsBlack( color );
                if ( currentBarCode.IsBlack != currentlyBlack )
                {
                    currentBarCode.X2 = currentX;
                    currentBarCode = new Bar( currentX, currentX, currentlyBlack );
                    bars.Add( currentBarCode );
                }
            }

            int lastIndex = bars.Count - 1;
            if ( !bars[lastIndex].IsBlack )
            {
                bars.RemoveAt( lastIndex );
            }

            return SetWidths( bars.ToArray() );
        }

        /// <summary>
        /// Verifies if a color is black
        /// </summary>
        /// <param name="color">the color</param>
        /// <returns>true if the color is black, false otherwise</returns>
        private static bool IsBlack( Color color )
        {
            if ( color.A >= 128 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the width of the small width bar in a given collection of bars
        /// </summary>
        /// <param name="barCodes"></param>
        /// <returns>the width of the narrowest bar</returns>
        private static int GetSmallestBarWidth( Bar[] bars )
        {
            int smallest = Int32.MaxValue;

            foreach ( Bar bar in bars )
            {
                int width = bar.X2 - bar.X1;
                if ( width < smallest )
                {
                    smallest = width;
                }
            }

            return smallest;
        }

        /// <summary>
        /// Updates the width property of each of the bars 
        /// </summary>
        /// <param name="barCodes">the bars</param>
        /// <returns>the updated bars</returns>
        private static Bar[] SetWidths( Bar[] bars )
        {
            int narrowest = GetSmallestBarWidth( bars );
            int wideTreshold = (int)2.2 * narrowest;
            foreach ( Bar barCode in bars )
            {
                int width = barCode.X2 - barCode.X1;
                if ( width > wideTreshold )
                {
                    barCode.Width = Width.Wide;
                }
                else
                {
                    barCode.Width = Width.Narrow;
                }
            }

            return bars;
        }

        #endregion
    }
}