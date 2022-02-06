///////////////////////////////////////////////////////////////////////////////
//
//  File:           LruCache.cs
//
//  Facility:       Кэширование
//
//
//  Abstract:       Класс кэша, реализующего алгоритм LRU
//
//  Environment:    VC# 8.0
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  11-19-2006
//
//  Copyright (C) OOO "ЛайтКом", 2005-2006. Все права защищены.
//
///////////////////////////////////////////////////////////////////////////////

/*
 * $History: LruCache.cs $
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 4.03.07    Time: 12:29
 * Created in $/LightCom/.NET/Common
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 22.11.06   Time: 20:50
 * Created in $/gps/Lightcom.Common
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace LightCom.Common
{
    /// <summary>
    /// Класс кэша, реализующего алгоритм LRU
    /// </summary>
    /// <typeparam name="TKey">Ключ для поиска в кэше</typeparam>
    /// <typeparam name="TValue">Значение, хранимое в кэше</typeparam>
    public class LruCache <TKey, TValue>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="cacheSize">Размер кэша</param>
        /// <param name="disposeObjects">Удалять ли хранимые в кэше объекты
        /// </param>
        public LruCache (int cacheSize, bool disposeObjects)
        {
            size = cacheSize;
            dispose = disposeObjects;
            values = new SortedList<TKey, TValue> (size);
            times = new SortedList<TKey, DateTime> (size);
        }

        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="other">Копируемый объект</param>
        public LruCache (LruCache<TKey, TValue> other)
        {
            dispose = other.dispose;
            values = new SortedList<TKey, TValue> (other.values);
            times = new SortedList<TKey, DateTime> (other.times);
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        ~LruCache ()
        {
            Clear ();
        }

        /// <summary>
        /// Очистка кэша
        /// </summary>
        public void Clear ()
        {
            if (dispose) 
            {
                foreach (KeyValuePair<TKey,TValue> pair in values)
                {
                    IDisposable iDisp = pair.Value as IDisposable;
                    if (null != iDisp)
                    {
                        iDisp.Dispose ();
                    }
                }
            }

           values.Clear ();
           times.Clear ();
        }

        /// <summary>
        /// Добавление элемента в кэш
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        public void Add (TKey key, TValue value)
        {
            int idx = values.IndexOfKey (key);
            if (idx < 0)
            {
                if (values.Count >= size)
                {
                    int idxToDelete = FindOldest ();
                    if (idxToDelete >= 0)
                    {
                        if (dispose)
                        {
                            IDisposable iDisp = values.Values [idxToDelete] as IDisposable;
                            if (null != iDisp)
                            {
                                iDisp.Dispose ();
                            }
                        }
                        values.RemoveAt (idxToDelete);
                        times.RemoveAt (idxToDelete);
                    }
                }

                values.Add (key, value);
                times.Add (key, DateTime.Now);
            }
            else
            {
                TValue oldValue = values.Values [idx];
                if (! object.ReferenceEquals (oldValue, value))
                {
                    values.Values [idx] = value;
                    if (dispose)
                    {
                        IDisposable iDisp = oldValue as IDisposable;
                        if (iDisp != null)
                        {
                            iDisp.Dispose ();
                        }
                    }
                }
                times [times.Keys [idx]] = DateTime.Now;
            }
        }

        /// <summary>
        /// Удаление элемента кэша
        /// </summary>
        /// <param name="key">Ключ</param>
        public void Remove (TKey key)
        {
            try
            {
                int idx = values.IndexOfKey (key);
                if (idx < 0)
                {
                    return;
                }

                values.RemoveAt (idx);
                times.RemoveAt (idx);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Проверяет, есть ли в кэше значение с заданным ключем
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>true, если кэш содержит значение с заданным ключем
        /// </returns>
        public bool Contains (TKey key)
        {
            return values.ContainsKey (key);
        }

        /// <summary>
        /// Возвращает значение с заданным ключем
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Значение</returns>
        public TValue GetValue (TKey key)
        {
            try
            {
                int idx = values.IndexOfKey (key);
                if (idx < 0)
                    return default (TValue);
                TValue value = values.Values [idx];
                times [times.Keys [idx]] = DateTime.Now;

                return value;
            }
            catch (Exception)
            {
                return default (TValue);
            }
        }

        /// <summary>
        /// Индекс кандидата на удаление
        /// </summary>
        /// <returns>Индекc самого старого элемента</returns>
        protected int FindOldest ()
        {
            int count = times.Count;
            int result = -1;
            DateTime time = DateTime.MaxValue;
            for (int idx = 0; idx < count; ++idx)
            {
                DateTime currentValue = times.Values [idx];
                if (time > currentValue)
                {
                    result = idx;
                    time = currentValue;
                }
            }

            return result;
        }

        /// <summary>
        /// Оператор доступа к значениям по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Значение, хранимое в кэше</returns>
        TValue this [TKey key]
        {
            get
            {
                return GetValue (key);
            }
            set
            {
                Add (key, value);
            }
        }

        /// <summary>
        /// Признак того, что элементы кэша нужно dispose
        /// </summary>
        public bool DisposeEntries
        {
            get {return dispose;}
        }
        protected bool dispose;

        /// <summary>
        /// Размер кэша
        /// </summary>
        public int CacheSize
        {
            get {return size;}
        }
        protected int size;

        /// <summary>
        /// Элементы, хранимые в кэше
        /// </summary>
        protected SortedList<TKey, TValue> values;

        /// <summary>
        /// Время последнего обращения к элементам кэша
        /// </summary>
        protected SortedList<TKey, DateTime> times;
    }
}
