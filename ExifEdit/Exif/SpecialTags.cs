using ExifReader.Exif;
using log4net;
using log4net.Util;
using System;
using System.Threading.Tasks;


namespace ExifEdit.Exif
{
    //Exif Tag interpretation for Special Values of special tags
    class SpecialTags
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(SpecialTags));

        //   https://exiftool.org/TagNames/EXIF.html

        private static ushort GetAsUshort(Object o)
        {
            Type typeObj = o.GetType();
            ushort u = 0;
            // switch (o.GetType())
            switch (o)
            {
                case byte by: u = by; break;
                case UInt16 ui: u = ui; break;
                case UInt32 ui32: u = (ushort)ui32; break;
                case UInt64 ui64: u = (ushort)ui64; break;
            }

            return u;
        }

        /// <summary>
        /// Gibt null zurück wenn keine Interpretation vorhanden, sonst den interpretierten Wert
        /// </summary>
        /// <param name="tagType"></param>
        /// <param name="obj"></param>
        /// <param name="ifdtyp"></param>
        /// <returns></returns>
        public static String GetValueByTag(TagType tagType, object[] obj, IfdTyp ifdtyp)
        {
            String s = null;
            /* Hilfsvariable */
            Tuple<uint, uint> tuint, tuint1, tuint2;  // Tupel unit,uint
            Tuple<int, int> tint;  // Tupel unit,uint
            uint zu, nu;  // zaehler nenner unsigned 
            int zi, ni; // zaehler neenr int
            int i;
            byte[] b;
            double doub; //hilfswaviable

            if (obj == null || obj.Length == 0) {
                log.Debug("obj )== null for " + tagType + " " + ifdtyp);
                return null; }


            try
            {
                if (ifdtyp == IfdTyp.GPSIFD)
                {
                    TagTypeGPS tagTypeGPS = (TagTypeGPS)tagType;
                    switch (tagTypeGPS)
                    {

                        ////GPS
                        ///** GPS tag version GPSVersionID 0 0 BYTE 4 */
                        //TAG_VERSION_ID = 0x0000,
                        ///** North or South Latitude GPSLatitudeRef 1 1 ASCII 2 */
                        case TagTypeGPS.TAG_GPSLatitudeRef:
                        case TagTypeGPS.TAG_GPSLongitudeRef:
                            switch ((char)obj[0])
                            {
                                case 'n': case 'N': s = "North"; break;
                                case 'e': case 'E': s = "East"; break;
                                case 's': case 'S': s = "South"; break;
                                case 'w': case 'W': s = "West"; break;
                            }
                            break;
                        ///** Latitude GPSLatitude 2 2 RATIONAL 3 */
                        // case TagType.TAG_LATITUDE:
                        //tuint = (Tuple<uint, uint>)obj[0];
                        //tuint1 = (Tuple<uint, uint>)obj[1];
                        //tuint2 = (Tuple<uint, uint>)obj[2];
                        //s = $"  {tuint.Item1}/{tuint.Item2} , {tuint1.Item1}/{tuint1.Item2}, {tuint2.Item1}/{tuint2.Item2}";
                        //break;
                        ///** East or West Longitude GPSLongitudeRef 3 3 ASCII 2 */

                        ///** Longitude GPSLongitude 4 4 RATIONAL 3 */
                        case TagTypeGPS.TAG_GPSLatitude:
                        case TagTypeGPS.TAG_GPSLongitude:
                            tuint = (Tuple<uint, uint>)obj[0];
                            tuint1 = (Tuple<uint, uint>)obj[1];
                            tuint2 = (Tuple<uint, uint>)obj[2];

                            double grad = (0.0d + tuint.Item1) / tuint.Item2;
                            double minute = (0.0d + tuint1.Item1) / tuint1.Item2;
                            double sekunde = (0.0d + tuint2.Item1) / tuint2.Item2;

                            double dezimalgrad = grad + minute / 60d + sekunde / 3600d;
                            //s = $"{(0.0 + tuint.Item1) / tuint.Item2}  , {(0.0 + tuint1.Item1) / tuint1.Item2}  ,{(0.0 + tuint2.Item1) / tuint2.Item2} ";
                            // s += $" * {tuint.Item1}/{tuint.Item2} * {tuint1.Item1}/{tuint1.Item2} * {tuint2.Item1}/{tuint2.Item2} *";
                            s = $"{dezimalgrad:n6}";
                            break;
                        ///** Altitude reference GPSAltitudeRef 5 5 BYTE 1 */
                        case TagTypeGPS.TAG_GPSAltitudeRef:
                            i = (byte)obj[0];
                            s = i <= 0 ? "above sea level" : "below sea level";
                            break;
                        /////** Altitude GPSAltitude 6 6 RATIONAL 1 */
                        case TagTypeGPS.TAG_GPSAltitude:
                            tuint = (Tuple<uint, uint>)obj[0];
                            s = $"  {(0.0 + tuint.Item1) / tuint.Item2:n2}  ";

                            break;
                        ///** GPS time (atomic clock) GPSTimeStamp 7 7 RATIONAL 3 */
                        ///
                        default:
                            s = null;
                            break;
                    }
                }
                else
                {

                    if (obj != null && obj.Length > 0 && obj[0] != null)

                    {
                        switch (tagType)
                        {
                            case TagType.TAG_XResolution:
                            case TagType.TAG_YResolution:
                                if (obj[0] is Tuple<uint, uint>)
                                {
                                    tuint = (Tuple<uint, uint>)obj[0];
                                    zu = tuint.Item1;
                                    nu = tuint.Item2;
                                    s = "" + (zu / nu);
                                }
                                if (obj[0] is Tuple<int, int>)
                                {
                                    tint = (Tuple<int, int>)obj[0];
                                    zi = tint.Item1;
                                    ni = tint.Item2;
                                    s = "" + (zi / ni);
                                }

                                break;
                            case TagType.TAG_ResolutionUnit:
                                switch (GetAsUshort(obj[0]))
                                {
                                    case 1: s = "None"; break;
                                    case 2: s = "inches"; break;
                                    case 3: s = "cm"; break;
                                    default: s = null; break;
                                }
                                break;

                            /**
                             * When image format is no compression, this value shows the number of bits
                             * per component for each pixel. Usually this value is '8,8,8'.
                             */
                            case TagType.TAG_BitsPerSample:
                                s = null;
                                break;


                            case TagType.TAG_ColorSpace:
                                switch (GetAsUshort(obj[0]))
                                {
                                    case 0x1: s = "sRGB"; break;
                                    case 0x2: s = "Adobe RGB"; break;
                                    case 0xfffd: s = "Wide Gamut RGB"; break;
                                    case 0xfffe: s = "ICC Profile"; break;
                                    case 0xffff: s = "Uncalibrated"; break;
                                }
                                break;


                            /**
                               * Shows the color space of the image data components.
                               * 0 = WhiteIsZero
                               * 1 = BlackIsZero
                               * 2 = RGB
                               * 3 = RGB Palette
                               * 4 = Transparency Mask
                               * 5 = CMYK
                               * 6 = YCbCr
                               * 8 = CIELab
                               * 9 = ICCLab
                               * 10 = ITULab
                               * 32803 = Color Filter Array
                               * 32844 = Pixar LogL
                               * 32845 = Pixar LogLuv
                               * 34892 = Linear Raw
                               * 51177 = Depth Map
                               * 52527 = Semantic Mask
                               */
                            case TagType.TAG_PhotometricInterpretation:
                                switch (GetAsUshort(obj[0]))
                                {
                                    case 0: s = "WhiteIsZero"; break;
                                    case 1: s = "BlackIsZero"; break;
                                    case 2: s = "RGB"; break;
                                    case 3: s = "RGB Palette"; break;
                                    case 4: s = "Transparency Mask"; break;
                                    case 5: s = "CMYK"; break;
                                    case 6: s = "YCbCr"; break;
                                    case 8: s = "CIELab"; break;
                                    case 9: s = "ICCLab"; break;
                                    case 10: s = "ITULab"; break;
                                    case 32803: s = "Color Filter Array"; break;
                                    case 32844: s = "Pixar LogL"; break;
                                    case 32845: s = "Pixar LogLuv"; break;
                                    case 34892: s = "Linear Raw"; break;
                                    case 51177: s = "Depth Map"; break;
                                    case 52527: s = "Semantic Mask"; break;
                                }
                                break;

                            /**
                             * 1 = No dithering or halftoning
                             * 2 = Ordered dither or halftone
                             * 3 = Randomized dither
                             */
                            case TagType.TAG_Thresholding:
                                switch (GetAsUshort(obj[0]))
                                {
                                    case 1: s = "No dithering or halftoning"; break;
                                    case 2: s = "Ordered dither or halftone"; break;
                                    case 3: s = "Randomized dither"; break;

                                }
                                break;


                            /**
                             * 1 = Normal
                             * 2 = Reversed
                             */
                            case TagType.TAG_FillOrder:
                                switch (GetAsUshort(obj[0]))
                                {
                                    case 1: s = "Normal"; break;
                                    case 2: s = "Reversed"; break;
                                }
                                break;


                            case TagType.TAG_PlanarConfiguration:
                                switch (GetAsUshort(obj[0]))
                                {
                                    case 1: s = "Chunky"; break;
                                    case 2: s = "Planar"; break;
                                }
                                break;


                            /**
                             * When image format is no compression YCbCr, this value shows byte aligns of
                             * YCbCr data. If value is '1', Y/Cb/Cr value is chunky format, contiguous for
                             * each subsampling pixel. If value is '2', Y/Cb/Cr value is separated and
                             * stored to Y plane/Cb plane/Cr plane format. //??
                             * 
                             * int16u[2]! 	IFD0 	
                             * '1 1' = YCbCr4:4:4 (1 1)
                             * '1 2' = YCbCr4:4:0 (1 2)
                             * '1 4' = YCbCr4:4:1 (1 4)
                             * '2 1' = YCbCr4:2:2 (2 1)	   	
                             * '2 2' = YCbCr4:2:0 (2 2)
                             * '2 4' = YCbCr4:2:1 (2 4)
                             * '4 1' = YCbCr4:1:1 (4 1)
                             * '4 2' = YCbCr4:1:0 (4 2)
                             */
                            case TagType.TAG_YCbCrSubSampling:
                                switch ("" + GetAsUshort(obj[0]) + " " + GetAsUshort(obj[1]))
                                {
                                    case "1 1": s = "YCbCr 4:4:4"; break;
                                    case "1 2": s = "YCbCr 4:4:0"; break;
                                    case "1 4": s = "YCbCr 4:4:1"; break;

                                    case "2 1": s = "YCbCr 4:2:2"; break;
                                    case "2 2": s = "YCbCr 4:2:0"; break;
                                    case "2 4": s = "YCbCr 4:2:1"; break;

                                    case "4 1": s = "YCbCr4:1:1"; break;
                                    case "4 2": s = "YCbCr4:1:0"; break;

                                }
                                break;

                            /**
                             * The new subfile type tag.
                             * 0 = Full-resolution Image
                             * 1 = Reduced-resolution image
                             * 2 = Single page of multi-page image
                             * 3 = Single page of multi-page reduced-resolution image
                             * 4 = Transparency mask
                             * 5 = Transparency mask of reduced-resolution image
                             * 6 = Transparency mask of multi-page image
                             * 7 = Transparency mask of reduced-resolution multi-page image
                             */
                            case TagType.TAG_SubfileType:
                                s = null;
                                break;
                            /**
                             * The old subfile type tag.
                             * 1 = Full-resolution image (Main image)
                             * 2 = Reduced-resolution image (Thumbnail)
                             * 3 = Single page of multi-page image
                             */
                            case TagType.TAG_OldSubfileType:
                                s = null;
                                break;

                            /* The actual aperture value of lens when the image was taken. Unit is APEX.
                            * To convert this value to ordinary F-number (F-stop), calculate this value's
                            * power of root 2 (=1.4142). For example, if the ApertureValue is '5',
                            * F-number is 1.4142^5 = F5.6.
                            */
                            case TagType.TAG_ApertureValue:
                                tuint = (Tuple<uint, uint>)obj[0];
                                zu = tuint.Item1;
                                nu = tuint.Item2;
                                if (nu != 0) doub = (0.0 + zu) / nu; else doub = (0.0 + zu);
                                s = " F " + Math.Round(Math.Pow(Math.Sqrt(2.0), doub), 2);
                                break;

                            /*
                             *   0 = -
                             *  1 = Y
                             *  2 = Cb
                             *  3 = Cr 
                             *  4 = R
                             *  5 = G
                             *  6 = B
                             *  */
                            case TagType.TAG_ComponentsConfiguration:

                                s = null;
                                break;

                            /**
                            * Shutter speed by APEX value. To convert this value to ordinary 'Shutter Speed',
                            * calculate this value's power of 2, then reciprocal. For example, if the
                            * ShutterSpeedValue is '4', shutter speed is 1/(24)=1/16 second.
                            */
                            case TagType.TAG_ShutterSpeedValue:
                                tint = (Tuple<int, int>)obj[0];
                                zi = tint.Item1;
                                ni = tint.Item2;
                                doub = (0.0 + zi) / ni;
                                s = " 1/" + Math.Round(Math.Pow(2.0, doub), 1) + " seconds";
                                break;
                            /**
                            * Exposure time (reciprocal of shutter speed). Unit is second.
                            */
                            case TagType.TAG_ExposureTime:
                                tuint = (Tuple<uint, uint>)obj[0];
                                zu = tuint.Item1;
                                nu = tuint.Item2;
                                s = "" + zu + "/" + nu + " seconds";
                                break;
                            /**
                             * The actual F-number(F-stop) of lens when the image was taken.
                             */
                            case TagType.TAG_FNumber:
                                tuint = (Tuple<uint, uint>)obj[0];
                                zu = tuint.Item1;
                                nu = tuint.Item2;
                                s = "" + (0.0 + zu) / nu;
                                break;
                            /**
                             * Exposure program that the camera used when image was taken. '1' means
                             * manual control, '2' program normal, '3' aperture priority, '4' shutter
                             * priority, '5' program creative (slow program), '6' program action
                             * (high-speed program), '7' portrait mode, '8' landscape mode.
                             */
                            case TagType.TAG_ExposureProgram:
                                s = null;
                                switch (GetAsUshort(obj[0]))
                                {
                                    case 0: s = "Not Defined"; break;
                                    case 1: s = "Manual"; break;
                                    case 2: s = "Program AE"; break;
                                    case 3: s = "Aperture-priority AE"; break;
                                    case 4: s = "Shutter speed priority AE"; break;
                                    case 5: s = "Creative (Slow speed)"; break;
                                    case 6: s = "Action (High speed)"; break;
                                    case 7: s = "Portrait"; break;
                                    case 8: s = "Landscape"; break;
                                    case 9: s = "Bulb"; break;
                                    default: s = null; break;
                                }
                                break;


                            /**
                             * Average (rough estimate) compression level in JPEG bits per pixel.
                             * */
                            case TagType.TAG_CompressedBitsPerPixel:
                                s = null;
                                break;


                            case TagType.TAG_BrightnessValue:
                                s = null;
                                break;
                            case TagType.TAG_ExposureCompensation:
                                s = null;
                                break;
                            /**
                             * Maximum aperture value of lens. You can convert to F-number by calculating
                             * power of root 2 (same process of ApertureValue:0x9202).
                             * The actual aperture value of lens when the image was taken. To convert this
                             * value to ordinary f-number(f-stop), calculate the value's power of root 2
                             * (=1.4142). For example, if the ApertureValue is '5', f-number is 1.41425^5 = F5.6.
                             */
                            case TagType.TAG_MaxApertureValue:
                                s = null;
                                break;
                            /**
                             * Indicates the distance the autofocus camera is focused to.  Tends to be less accurate as distance increases.
                             */
                            case TagType.TAG_SubjectDistance:
                                tuint = (Tuple<uint, uint>)obj[0];
                                zu = tuint.Item1;
                                nu = tuint.Item2;
                                if (zu == uint.MaxValue) { s = "∞"; }
                                else
                                { s = "" + (0.0 + zu) / nu + " m"; }
                                break;


                            /* Exposure metering method. '0' means unknown, '1' average, '2' center
                    * weighted average, '3' spot, '4' multi-spot, '5' multi-segment, '6' partial,
                    * '255' other.
                    */
                            case TagType.TAG_MeteringMode:

                                switch (GetAsUshort(obj[0]))
                                {
                                    case 0: s = "unkown"; break;
                                    case 1: s = "Average"; break;
                                    case 2: s = "Center-weighted average"; break;
                                    case 3: s = "Spot"; break;
                                    case 4: s = "Multispot"; break;
                                    case 5: s = "Multisegment"; break;
                                    case 6: s = "Partial"; break;
                                    case 255: s = "Other"; break;
                                    default: s = null; break;
                                }
                                break;


                            /*
                             * 0 = Unknown 	
                             * 1	= Daylight 	
                             * 2	= Fluorescent 	
                             * 3	= Tungsten (Incandescent) 	
                             * 4	= Flash 	
                             * 9	= Fine Weather 	
                             * 10	= Cloudy 		 	 
                             * 11	= Shade 	
                             * 12	= Daylight Fluorescent
                             * 13	= Day White Fluorescent 	
                             * 14	= Cool White Fluorescent 	
                             * 15	= White Fluorescent 	
                             * 16	= Warm White Fluorescent 	
                             * 17	= Standard Light A 	
                             * 18	= Standard Light B 	 	 
                             * 19	= Standard Light C 	
                             * 20	= D55
                             * 21	= D65
                             * 22	= D75
                             * 23	= D50
                             * 24	= ISO Studio Tungsten
                             * 255	= Other      
                             */
                            case TagType.TAG_LightSource:
                                switch (GetAsUshort(obj[0]))
                                {
                                    case 0: s = "Unknown"; break;
                                    case 1: s = "Daylight "; break;
                                    case 2: s = "Fluorescent"; break; //leuchtsoffrühre
                                    case 3: s = "Tungsten (Incandescent)"; break; //kunstlicht  (glühlampe)
                                    case 4: s = "Flash "; break; //blitz
                                    case 9: s = "Fine Weather"; break; //sonne
                                    case 10: s = "Cloudy"; break; // Wolkig
                                    case 11: s = "Shade"; break; //Schatten
                                    case 12: s = "Daylight Fluorescent"; break;
                                    case 13: s = "Day White Fluorescent"; break;
                                    case 14: s = "Cool White Fluorescent"; break;
                                    case 15: s = "White Fluorescent"; break;
                                    case 16: s = "Warm White Fluorescent"; break;
                                    case 17: s = "Standard Light A"; break;
                                    case 18: s = "Standard Light B"; break;
                                    case 19: s = "Standard Light C"; break;
                                    case 20: s = "D55"; break;
                                    case 21: s = "D65"; break;
                                    case 22: s = "D75"; break;
                                    case 23: s = "D50"; break;
                                    case 24: s = "ISO Studio Tungsten"; break;
                                    case 255: s = "Other"; break;
                                    default: s = null; break;
                                }
                                break;

                            /**
                              0 = Auto
                              1 = Manual
                             */
                            case TagType.TAG_WhiteBalance:
                                switch (GetAsUshort(obj[0]))
                                {
                                    case 0: s = "Auto"; break;
                                    case 1: s = "Manual "; break;
                                    default: s = null; break;
                                }
                                break;


                            /**
                             * 0x0  = 0000000 = No Flash
                             * 0x1  = 0000001 = Fired
                             * 0x5  = 0000101 = Fired, Return not detected
                             * 0x7  = 0000111 = Fired, Return detected
                             * 0x9  = 0001001 = On
                             * 0xd  = 0001101 = On, Return not detected
                             * 0xf  = 0001111 = On, Return detected
                             * 0x10 = 0010000 = Off
                             * 0x18 = 0011000 = Auto, Did not fire
                             * 0x19 = 0011001 = Auto, Fired
                             * 0x1d = 0011101 = Auto, Fired, Return not detected
                             * 0x1f = 0011111 = Auto, Fired, Return detected
                             * 0x20 = 0100000 = No flash function
                             * 0x41 = 1000001 = Fired, Red-eye reduction
                             * 0x45 = 1000101 = Fired, Red-eye reduction, Return not detected
                             * 0x47 = 1000111 = Fired, Red-eye reduction, Return detected
                             * 0x49 = 1001001 = On, Red-eye reduction
                             * 0x4d = 1001101 = On, Red-eye reduction, Return not detected
                             * 0x4f = 1001111 = On, Red-eye reduction, Return detected
                             * 0x59 = 1011001 = Auto, Fired, Red-eye reduction
                             * 0x5d = 1011101 = Auto, Fired, Red-eye reduction, Return not detected
                             * 0x5f = 1011111 = Auto, Fired, Red-eye reduction, Return detected
                             *        6543210 (positions)
                             *
                             * This is a bitmask.
                             * 0 = flash fired
                             * 1 = return detected
                             * 2 = return able to be detected
                             * 3 = unknown
                             * 4 = auto used
                             * 5 = unknown
                             * 6 = red eye reduction used
                             */
                            case TagType.TAG_Flash:
                                s = null;
                                break;
                            /**
                             * Focal length of lens used to take image.  Unit is millimeter.
                             * Nice digital cameras actually save the focal length as a function of how far they are zoomed in.
                             */
                            case TagType.TAG_FocalLength:
                                tuint = (Tuple<uint, uint>)obj[0];
                                zu = tuint.Item1;
                                nu = tuint.Item2;
                                s = "" + Math.Round((0.0 + zu) / nu, 0) + " mm";
                                break;
                            // TAG_IMG_DIRECTION = 0x0011,
                            //case TagType.TAG_ImageDIRECTION:
                            //    tuint = (Tuple<uint, uint>)obj[0];
                            //    zu = tuint.Item1;
                            //    nu = tuint.Item2;
                            //    s = "" + Math.Round((0.0 + zu) / nu, 0) + " °";
                            //    break;

                            /**
                             * This tag holds the Exif Makernote. Makernotes are free to be in any format, though they are often IFDs.
                             * To determine the format, we consider the starting bytes of the makernote itself and sometimes the
                             * camera model and make.
                             * <p>
                             * The component count for this tag includes all of the bytes needed for the makernote.
                             */
                            case TagType.TAG_MakerNote:
                                s = null;
                                break;

                            case TagType.TAG_UserComment:
                                b = (byte[])obj[0];
                                s = "";
                                i = 0;
                                while (i < b.Length && b[i] != 0)
                                {
                                    s = s + (Char)b[i];
                                    i++;
                                }
                                break;
                            case TagType.TAG_FocalPlaneXResolution:
                            case TagType.TAG_FocalPlaneYResolution:
                            case TagType.TAG_FocalPlaneXResolution_:
                            case TagType.TAG_FocalPlaneYResolution_:
                                tuint = (Tuple<uint, uint>)obj[0];
                                zu = tuint.Item1;
                                nu = tuint.Item2;
                                s = $" {((0.0 + zu) / nu):n2}";
                                break;

                            /**
                             * Unit of FocalPlaneXResolution/FocalPlaneYResolution. '1' means no-unit,
                             * '2' inch, '3' centimeter.
                             *
                             * Note: Some of Fujifilm's digicam(e.g.FX2700,FX2900,Finepix4700Z/40i etc)
                             * uses value '3' so it must be 'centimeter', but it seems that they use a
                             * '8.3mm?'(1/3in.?) to their ResolutionUnit. Fuji's BUG? Finepix4900Z has
                             * been changed to use value '2' but it doesn't match to actual value also.
                             */
                            case TagType.TAG_FocalPlaneResolutionUnit:
                            case TagType.TAG_FocalPlaneResolutionUnit_:
                                switch (GetAsUshort(obj[0]))
                                {
                                    case 1: s = "none"; break;
                                    case 2: s = "Inches"; break;
                                    case 3: s = "cm"; break;
                                    case 4: s = "mm"; break;
                                    case 5: s = "µm"; break;
                                }

                                break;


                            // these tags new with Exif 2.2 (?) [A401 - A4
                            /**
                             * This tag indicates the use of special processing on image data, such as rendering
                             * geared to output. When special processing is performed, the reader is expected to
                             * disable or minimize any further processing.
                             * Tag = 41985 (A401.H)
                             * Type = SHORT
                             * Count = 1
                             * Default = 0
                             *   0 = Normal process
                             *   1 = Custom process
                             *   Other = reserved
                             */
                            case TagType.TAG_CustomRendered:
                                s = null;
                                break;

                            /**
                             * This tag indicates the exposure mode set when the image was shot. In auto-bracketing
                             * mode, the camera shoots a series of frames of the same scene at different exposure settings.
                             * Tag = 41986 (A402.H)
                             * Type = SHORT
                             * Count = 1
                             * Default = none
                             *   0 = Auto exposure
                             *   1 = Manual exposure
                             *   2 = Auto bracket
                             *   Other = reserved
                             */
                            case TagType.TAG_ExposureMode:
                                s = null;
                                break;

                            /**
                             * This tag indicates the white balance mode set when the image was shot.
                             * Tag = 41987 (A403.H)
                             * Type = SHORT
                             * Count = 1
                             * Default = none
                             *   0 = Auto white balance
                             *   1 = Manual white balance
                             *   Other = reserved
                             */
                            case TagType.TAG_WhiteBalance_:
                                s = null;
                                break;

                            /**
                             * This tag indicates the digital zoom ratio when the image was shot. If the
                             * numerator of the recorded value is 0, this indicates that digital zoom was
                             * not used.
                             * Tag = 41988 (A404.H)
                             * Type = RATIONAL
                             * Count = 1
                             * Default = none
                             */
                            case TagType.TAG_DigitalZoomRatio:
                                s = null;
                                break;

                            /**
                             * This tag indicates the equivalent focal length assuming a 35mm film camera,
                             * in mm. A value of 0 means the focal length is unknown. Note that this tag
                             * differs from the FocalLength tag.
                             * Tag = 41989 (A405.H)
                             * Type = SHORT
                             * Count = 1
                             * Default = none
                             */
                            case TagType.TAG_FocalLengthIn35mmFormat:
                                s = null;
                                break;

                            /**
                             * This tag indicates the type of scene that was shot. It can also be used to
                             * record the mode in which the image was shot. Note that this differs from
                             * the scene type (SceneType) tag.
                             * Tag = 41990 (A406.H)
                             * Type = SHORT
                             * Count = 1
                             * Default = 0
                             *   0 = Standard
                             *   1 = Landscape
                             *   2 = Portrait
                             *   3 = Night scene
                             *   Other = reserved
                             */
                            case TagType.TAG_SceneCaptureType:
                                s = null;
                                break;

                            /**
                             * This tag indicates the degree of overall image gain adjustment.
                             * Tag = 41991 (A407.H)
                             * Type = SHORT
                             * Count = 1
                             * Default = none
                             *   0 = None
                             *   1 = Low gain up
                             *   2 = High gain up
                             *   3 = Low gain down
                             *   4 = High gain down
                             *   Other = reserved
                             */
                            case TagType.TAG_GainControl:
                                s = null;
                                break;

                            /**
                             * This tag indicates the direction of contrast processing applied by the camera
                             * when the image was shot.
                             * Tag = 41992 (A408.H)
                             * Type = SHORT
                             * Count = 1
                             * Default = 0
                             *   0 = Normal
                             *   1 = Soft
                             *   2 = Hard
                             *   Other = reserved
                             */
                            case TagType.TAG_Contrast:
                                s = null;
                                break;

                            /**
                             * This tag indicates the direction of saturation processing applied by the camera
                             * when the image was shot.
                             * Tag = 41993 (A409.H)
                             * Type = SHORT
                             * Count = 1
                             * Default = 0
                             *   0 = Normal
                             *   1 = Low saturation
                             *   2 = High saturation
                             *   Other = reserved
                             */
                            case TagType.TAG_Saturation:
                                s = null;
                                break;

                            /**
                             * This tag indicates the direction of sharpness processing applied by the camera
                             * when the image was shot.
                             * Tag = 41994 (A40A.H)
                             * Type = SHORT
                             * Count = 1
                             * Default = 0
                             *   0 = Normal
                             *   1 = Soft
                             *   2 = Hard
                             *   Other = reserved
                             */
                            case TagType.TAG_Sharpness:
                                s = null;
                                break;



                            /**
                             * This tag indicates the distance to the subject.
                             * Tag = 41996 (A40C.H)
                             * Type = SHORT
                             * Count = 1
                             * Default = none
                             *   0 = unknown
                             *   1 = Macro
                             *   2 = Close view
                             *   3 = Distant view
                             *   Other = reserved
                             */
                            case TagType.TAG_SubjectDistanceRange:
                                s = null;
                                break;





                            /**
                             * When image format is no compression, this value shows the number of bits
                             * per component for each pixel. Usually this value is '8,8,8'.
                             */
                            //  TAG_BITS_PER_SAMPLE = 0x0102,

                            /**
                             * Shows compression method for Thumbnail.
                             * 1 = Uncompressed
                             * 2 = CCITT 1D
                             * 3 = T4/Group 3 Fax
                             * 4 = T6/Group 4 Fax
                             * 5 = LZW
                             * 6 = JPEG (old-style)
                             * 7 = JPEG
                             * 8 = Adobe Deflate
                             * 9 = JBIG B&amp,W
                             * 10 = JBIG Color
                             * 32766 = Next
                             * 32771 = CCIRLEW
                             * 32773 = PackBits
                             * 32809 = Thunderscan
                             * 32895 = IT8CTPAD
                             * 32896 = IT8LW
                             * 32897 = IT8MP
                             * 32898 = IT8BL
                             * 32908 = PixarFilm
                             * 32909 = PixarLog
                             * 32946 = Deflate
                             * 32947 = DCS
                             * 34661 = JBIG
                             * 34676 = SGILog
                             * 34677 = SGILog24
                             * 34712 = JPEG 2000
                             * 34713 = Nikon NEF Compressed
                             */
                            case TagType.TAG_Compression:

                                switch (GetAsUshort(obj[0]))
                                {
                                    case 1: s = "Uncompressed"; break;
                                    case 2: s = "CCITT 1D"; break;
                                    case 3: s = "T4/Group 3 Fax"; break;
                                    case 4: s = "T6/Group 4 Fax"; break;
                                    case 5: s = "LZW"; break;
                                    case 6: s = "JPEG (old-style)"; break;
                                    case 7: s = "JPEG"; break;
                                    case 8: s = "Adobe Deflate"; break;
                                    case 9: s = "JBIG B&W"; break;
                                    case 10: s = "JBIG Color"; break;
                                    case 99: s = "JPEG"; break;
                                    case 262: s = "Kodak 262"; break;
                                    case 32766: s = "Next"; break;
                                    case 32767: s = "Sony ARW Compressed"; break;
                                    case 32769: s = "Packed RAW"; break;
                                    case 32770: s = "Samsung SRW Compressed"; break;
                                    case 32771: s = "CCIRLEW"; break;
                                    case 32772: s = "Samsung SRW Compressed 2"; break;
                                    case 32773: s = "PackBits"; break;
                                    case 32809: s = "Thunderscan"; break;
                                    case 32867: s = "Kodak KDC Compressed"; break;
                                    case 32895: s = "IT8CTPAD"; break;
                                    case 32896: s = "IT8LW"; break;
                                    case 32897: s = "IT8MP"; break;
                                    case 32898: s = "IT8BL"; break;
                                    case 32908: s = "PixarFilm"; break;
                                    case 32909: s = "PixarLog"; break;
                                    case 32946: s = "Deflate"; break;
                                    case 32947: s = "DCS"; break;
                                    case 33003: s = "Aperio JPEG 2000 YCbCr"; break;
                                    case 33005: s = "Aperio JPEG 2000 RGB"; break;
                                    case 34661: s = "JBIG"; break;
                                    case 34676: s = "SGILog"; break;
                                    case 34677: s = "SGILog24"; break;
                                    case 34712: s = "JPEG 2000"; break;
                                    case 34713: s = "Nikon NEF Compressed"; break;
                                    case 34715: s = "JBIG2 TIFF FX"; break;
                                    case 34718: s = "Microsoft Document Imaging (MDI) Binary Level Codec"; break;
                                    case 34719: s = "Microsoft Document Imaging (MDI) Progressive Transform Codec"; break;
                                    case 34720: s = "Microsoft Document Imaging (MDI) Vector"; break;
                                    case 34887: s = "ESRI Lerc"; break;
                                    case 34892: s = "Lossy JPEG"; break;
                                    case 34925: s = "LZMA2"; break;
                                    case 34926: s = "Zstd"; break;
                                    case 34927: s = "WebP"; break;
                                    case 34933: s = "PNG"; break;
                                    case 34934: s = "JPEG XR"; break;
                                    case 65000: s = "Kodak DCR Compressed"; break;
                                    case 65535: s = "Pentax PEF Compressed "; break;
                                    default: s = null; break;
                                }
                                break;


                            case TagType.TAG_ExifVersion:
                                b = (byte[])obj[0];
                                s = "";
                                for (i = 0; i < b.Length; i++)
                                { s += Char.ConvertFromUtf32(b[i]); }
                                break;

                            case TagType.TAG_Orientation:
                                switch (GetAsUshort(obj[0]))
                                {
                                    case 1: s = "Horizontal(normal)"; break;
                                    case 2: s = "Mirror horizontal"; break;
                                    case 3: s = "Rotate 180"; break;
                                    case 4: s = "Mirror vertical"; break;
                                    case 5: s = "Mirror horizontal and rotate 270 CW"; break;
                                    case 6: s = "Rotate 90 CW"; break;
                                    case 7: s = "Mirror horizontal and rotate 90 CW"; break;
                                    case 8: s = "Rotate 270 CW"; break;
                                    default: s = null; break;
                                }
                                break;

                            case TagType.TAG_SensitivityType:
                                switch (GetAsUshort(obj[0]))
                                {
                                    case 0: s = "Unknown"; break;
                                    case 1: s = "Standard Output Sensitivity"; break;
                                    case 2: s = "Recommended Exposure Index"; break;
                                    case 3: s = "ISO Speed"; break;
                                    case 4: s = "Standard Output Sensitivity and Recommended Exposure Index"; break;
                                    case 5: s = "Standard Output Sensitivity and ISO Speed"; break;
                                    case 6: s = "Recommended Exposure Index and ISO Speed"; break;
                                    case 7: s = "Standard Output Sensitivity, Recommended Exposure Index and ISO Speed"; break;
                                    default: s = null; break;
                                }
                                break;
                                /**
                                 * Shows the color space of the image data components.
                                 * 0 = WhiteIsZero
                                 * 1 = BlackIsZero
                                 * 2 = RGB
                                 * 3 = RGB Palette
                                 * 4 = Transparency Mask
                                 * 5 = CMYK
                                 * 6 = YCbCr
                                 * 8 = CIELab
                                 * 9 = ICCLab
                                 * 10 = ITULab
                                 * 32803 = Color Filter Array
                                 * 32844 = Pixar LogL
                                 * 32845 = Pixar LogLuv
                                 * 34892 = Linear Raw
                                 */
                                // TAG_PHOTOMETRIC_INTERPRETATION = 0x0106,

                                /**
                                 * The position in the file of raster data.
                                 */
                                //TAG_STRIP_OFFSETS = 0x0111,


                                /**
                                 * Each pixel is composed of this many samples.
                                 */
                                //TAG_SAMPLES_PER_PIXEL = 0x0115,
                                /**
                                 * The raster is codified by a single block of data holding this many rows.
                                 */
                                //TAG_ROWS_PER_STRIP = 0x116,
                                /**
                                 * The size of the raster data in bytes.
                                 */
                                //TAG_STRIP_BYTE_COUNTS = 0x0117,




                        }
                    }


                }
            


            return s; }
               catch (Exception ex)
            {
                log.Error(ex.Message + "  " + tagType + " " + ifdtyp);
                return null; }
        }
        
    }

    }
