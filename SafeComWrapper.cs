///////////////////////////////////////////////////////////////////////////////
//
//  File:           SafeComWrapper.cs
//
//  Facility:       Работа с COM объектами.
//
//
//  Abstract:       Обертка для COM объектов реализующая интерфейс IDisposable
//
//  Environment:    VC# 8.0
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  03-11-2007
//
//  Copyright (C) OOO "ЛайтКом", 2007. Все права защищены.
//
///////////////////////////////////////////////////////////////////////////////

/*
* $History: SafeComWrapper.cs $
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 11.03.07   Time: 11:46
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 11.03.07   Time: 9:24
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 11.03.07   Time: 9:17
 * Created in $/LightCom/.NET/Common
* 
*/
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LightCom.Common
{
    public class SafeComWrapper<ComObjectType>: IDisposable where ComObjectType : class
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public SafeComWrapper () 

        {
            comObject = null;
        }

        /// <summary>
        /// Конструктор копиования
        /// </summary>
        /// <param name="copy"></param>
        public SafeComWrapper (ComObjectType copy)
        {
            comObject = copy;
        }

        #region "IDisposable implementation"
        ///
        /// Track whether Dispose has been called.
        /// 
        private bool disposed = false;

        /// <summary>
        /// Implement IDisposable.
        /// Do not make this method virtual.
        /// A derived class should not be able to override this method.
        /// </summary>
        public void Dispose ()
        {
            Dispose (true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize (this);
        }

        
        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// Managed and unmanaged resources can be disposed.
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference 
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing">If disposing equals true, the method has 
        /// been called directly or indirectly by a user's code. 
        /// </param>
        private void Dispose (bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.                    
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here.
                // If disposing is false, 
                // only the following code is executed.

                COMObject = null;
            }
            disposed = true;
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        ~SafeComWrapper ()
        {
            Dispose (false);
        }

        /// <summary>
        /// Оборачиваемый COM object
        /// </summary>
        private ComObjectType comObject;
        public ComObjectType COMObject
        {
            get
            {
                return comObject;
            }
            set
            {
                if (null != comObject)
                {
                    while (Marshal.ReleaseComObject (comObject) > 0);
                }
                comObject = value;
            }
        }
        #endregion
    }
}
