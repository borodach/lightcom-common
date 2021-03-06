///////////////////////////////////////////////////////////////////////////////
//
//  File:           FileMapBuilder.cs
//
//  Facility:       ????????? ????
//
//
//  Abstract:       ?????????? ???????????? ?????? BaseMapBuilder, ???????????
//                  ??????? ????? ?? ??????.
//
//  Environment:    VC# 8.0
//
//  Author:         ?????? ?.?.
//
//  Creation Date:  11-18-2006
//
//  Copyright (C) OOO "???????", 2005-2006. ??? ????? ????????.
//
///////////////////////////////////////////////////////////////////////////////

/*
 * $History: FileMapBuilder.cs $
 * 
 * *****************  Version 5  *****************
 * User: Sergey       Date: 10.07.07   Time: 8:02
 * Updated in $/LightCom/.NET/Common
 * ??????????? ????????? Windows Mobile GPS API
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 4.03.07    Time: 12:39
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 19.11.06   Time: 23:40
 * Updated in $/gps/Lightcom.Common
 * ???? ?????? ??????
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 19.11.06   Time: 0:17
 * Updated in $/gps/Lightcom.Common
 * ?????????? ????????? ?????
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 19.11.06   Time: 0:15
 * Created in $/gps/Lightcom.Common
 * ????????? ???????
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LightCom.Common
{
    /// <summary>
    /// ?????????? ???????????? ?????? BaseMapBuilder, ???????????
    /// ??????? ????? ?? ??????.
    /// </summary>
    public class FileMapBuilder: BaseMapBuilder
    {
        /// <summary>
        /// ???????????
        /// </summary>
        public FileMapBuilder (int cacheSize)
        {
            varCache = new LruCache<string, Image> (cacheSize, true);
            this.FileNamePattern = Utils.ExeDirectory + "\\jpg\\lux{0}y{1}z0.jpg";
        }

        /// <summary>
        /// ??????????
        /// </summary>
        ~FileMapBuilder ()
        {
            varCache.Clear ();
        }
               

        /// <summary>
        /// ?????????? ???????? ??????? ?????
        /// </summary>
        /// <param name="piecePos">?????????? ???????</param>
        /// <param name="gaphicsRect">??????? ??? ?????????, ?????????? ? 
        /// ????? Draw</param>
        /// <param name="mapRect">??????? ??????????????? ? 
        /// ????? Draw</param>
        /// <param name="canDispose">??????? ????, ??? ???????? ????? dispose
        /// </param>
        /// <returns>???????? ???????</returns>
        protected override Image GetImagePiece (Point piecePos,
                                                Rectangle gaphicsRect,
                                                Rectangle mapRect,
                                                out bool canDispose)
        {
            canDispose = false;
            try
            {
                string fileName = string.Format (varFileNamePattern,
                                                 piecePos.X,
                                                 piecePos.Y);

                Image result = varCache.GetValue (fileName);
                if (null != result)
                {
                    return result;
                }

                Bitmap bmp = new Bitmap (fileName);
                varCache.Add (fileName, bmp);

                return bmp;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// ?????? ????? ?????
        /// </summary>
        public string FileNamePattern
        {
            get {return varFileNamePattern;}
            set 
            {
                varFileNamePattern = value;                
                Point pt = new Point (0, 0);
                Rectangle rect = new Rectangle ();
                bool canDispose;
                Image img = GetImagePiece (pt, rect, rect, out canDispose);
                if (null != img)
                {
                    varPieceSize = img.Size;
                    if (canDispose)
                    {
                        img.Dispose ();
                    }
                }
            }
        }
        protected string varFileNamePattern;
                
        /// <summary>
        /// ???
        /// </summary>
        public LruCache<string, Image> Cache
        {
            get
            {
                return varCache;
            }
        }
        protected LruCache<string, Image> varCache;        
    }
}
