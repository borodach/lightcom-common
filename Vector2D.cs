////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           Vector2D.cs
//
//  Facility:       Двумерные векторные вычисления.
//
//
//  Abstract:       Класс двумерного вектора.
//
//  Environment:    VC# 8.0
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  28/09/2005
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: Vector2D.cs $
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:37
 * Updated in $/gps/Lightcom.Common
 * 
 */

using System;
using System.Drawing;

namespace LightCom.Common
{
    /// <summary>
    /// Класс двумерного вектора
    /// </summary>
    public class Vector2D
    {

        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        public Vector2D ()
        {
            Reset ();
        }

        /// <summary>
        /// Конструктор коприования
        /// </summary>
        /// <param name="Vector2D">Копируемый вектор</param>
        public Vector2D (Vector2D other)
        {
            Init (other);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pt0">Начало вектора</param>
        /// <param name="pt1">Конец вектора</param>
        public Vector2D (Point pt0, Point pt1)
        {
            Init (pt0, pt1);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pt">Координаты конца вектора</param>
        public Vector2D (Point pt)
        {
            Init (pt);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="x0">Координата X начала вектора</param>
        /// <param name="y0">Координата Y начала вектора</param>
        /// <param name="x1">Координата X конца вектора</param>
        /// <param name="y1">Координата Y конца вектора</param>
        public Vector2D (double x0, double y0, double x1, double y1)
        {
            Init (x0, y0, x1, y1);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="x">Координата X конца вектора</param>
        /// <param name="y">Координата X конца вектора</param>
        public Vector2D (double x, double y)
        {
            Init (x, y);
        }

        /// <summary>
        /// Сброс вектора в нулевой
        /// </summary>
        public void Reset ()
        {
            X = 0;
            Y = 0;
        }

        /// <summary>
        /// Инициализация вектора
        /// </summary>
        /// <param name="pt0">Начало вектора</param>
        /// <param name="pt1">Конец вектора</param>
        public void Init (Vector2D other)
        {
            X = other.X;
            Y = other.Y;
        }

        /// <summary>
        /// Инициализация вектора
        /// </summary>
        /// <param name="pt0">Начало вектора</param>
        /// <param name="pt1">Конец вектора</param>
        public void Init (Point pt0, Point pt1)
        {
            X = pt1.X - pt0.X;
            Y = pt1.Y - pt0.Y;
        }

        /// <summary>
        /// Инициализация вектора
        /// </summary>
        /// <param name="pt">Координаты конца вектора</param>
        public void Init (Point pt)
        {
            X = pt.X;
            Y = pt.Y;
        }
                
        /// <summary>
        /// Инициализация вектора
        /// </summary>
        /// <param name="x0">Координата X начала вектора</param>
        /// <param name="y0">Координата Y начала вектора</param>
        /// <param name="x1">Координата X конца вектора</param>
        /// <param name="y1">Координата Y конца вектора</param>
        public void Init (double x0, double y0, double x1, double y1)
        {
            X = x1 - x0;
            Y = y1 - y0;
        }

        /// <summary>
        /// Инициализация вектора
        /// </summary>
        /// <param name="x">Координата X конца вектора</param>
        /// <param name="y">Координата X конца вектора</param>
        public void Init (double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Сравнение объектов
        /// </summary>
        /// <param name="other">Сравниваемый вектор</param>
        /// <returns>true, если векторы равны</returns>
        public override bool Equals (object other)
        {
            Vector2D vector = other as Vector2D;
            if (null == vector)
            {
                return false;
            }

            return this == vector;
        }

        /// <summary>
        /// Возвращает хэш объекта
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode ()
        {
            return (int)(X + Y * 0x00010000);
        }

        /// <summary>
        /// Скалярное произведение векторов
        /// </summary>
        /// <param name="one">Множитель</param>
        /// <param name="other">Множитель</param>
        /// <returns>Скалярное произведение векторов</returns>
        public static double operator * (Vector2D one, Vector2D other)
        {
            double result = one.X * other.X + one.Y * other.Y;
            return result;
        }

        /// <summary>
        /// Сложение векторов
        /// </summary>
        /// <param name="one">Множитель</param>
        /// <param name="other">Множитель</param>
        /// <returns>Сумма векторов</returns>
        public static Vector2D operator + (Vector2D one, Vector2D other)
        {
            Vector2D result = new Vector2D (one.X + other.X,
                one.Y + other.Y);
            return result;
        }

        /// <summary>
        /// Сравнение векторов
        /// </summary>
        /// <param name="one">Первый вектор</param>
        /// <param name="other">Второй вектор</param>
        /// <returns>Сумма векторов</returns>
        public static bool operator == (Vector2D one, Vector2D other)
        {
            if (Object.ReferenceEquals (one, null) && 
                Object.ReferenceEquals (other, null))
            {
                return true;
            }

            if (Object.ReferenceEquals (one, null) ||
                Object.ReferenceEquals (other, null))
            {
                return false;
            }

            bool result = one.X == other.X && one.Y == other.Y;

            return result;
        }

        /// <summary>
        /// Сравнение векторов
        /// </summary>
        /// <param name="one">Первый вектор</param>
        /// <param name="other">Второй вектор</param>
        /// <returns>Сумма векторов</returns>
        public static bool operator != (Vector2D one, Vector2D other)
        {
            bool result = !(one == other);

            return result;
        }

        /// <summary>
        /// Вычитание векторов
        /// </summary>
        /// <param name="one">Уменьшаемое</param>
        /// <param name="other">Вычитаемое</param>
        /// <returns>Разность векторов</returns>
        public static Vector2D operator - (Vector2D one, Vector2D other)
        {
            Vector2D result = new Vector2D (one.X - other.X,
                one.Y - other.Y);
            return result;
        }

        /// <summary>
        /// Умножение вектора на число
        /// </summary>
        /// <param name="k">Множитель</param>
        /// <returns>Масштабированный вектор</returns>
        public static Vector2D operator * (Vector2D vector, double k)
        {
            Vector2D result = new Vector2D (vector.X * k, vector.Y * k);
            return result;
        }

        /// <summary>
        /// Умножение числа на вектор
        /// </summary>
        /// <param name="k">Множитель</param>
        /// <returns>Масштабированный вектор</returns>
        public static Vector2D operator * (double k, Vector2D vector)
        {
            return vector * k;
        }

        /// <summary>
        /// Синус угла между векторами
        /// </summary>
        /// <param name="other">Вектор</param>
        /// <returns>Косинус угла между векторами</returns>
        public double GetAngle (Vector2D other)
        {
            double len0 = Length ();
            if (0 == len0) return 1;
            double len1 = other.Length ();
            if (0 == len1) return double.NaN;

            double mul = this * other;

            try
            {
                return mul / (len0 * len1);
            }
            catch (Exception)
            {
                return Double.NaN;
            }
        }

        /// <summary>
        /// Нормализация вектора
        /// </summary>
        /// <returns>true, если нормализовать удалось.</returns>
        public bool Normalize ()
        {
            double fLength = Length ();
            if (0 == fLength) return false;
            Init (this * (1 / fLength));

            return true;
        }

        /// <summary>
        /// Возвращает вектор, пертпендикулярный this (поворот на PI/2). 
        /// Длина вектора такая же, как this
        /// </summary>
        /// <returns>Возвращает вектор, перпендикулярный this</returns>
        public Vector2D GetNormale ()
        {
            Vector2D result = new Vector2D (Y, -X);
            return result;
        }

        /// <summary>
        /// Поворачивает вектор на angle радиан.
        /// </summary>
        /// <param name="angle"></param>
        public void Rotate (double angle)
        {
            double s = Math.Sin (angle);
            double c = Math.Cos (angle);
            Init (X * s - Y * c,
                  X * c + Y * s);
        }

        /// <summary>
        /// Возвращает длину вектора
        /// </summary>
        /// <returns>Длина вектора</returns>
        public double Length ()
        {
            return Math.Sqrt (X * X + Y * Y);
        }

        /// <summary>
        /// Возвращает длину вектора
        /// </summary>
        /// <returns>Длина вектора</returns>
        public int IntLength ()
        {
            return (int) Math.Round (Length ());
        }

        /// <summary>
        /// Координата X
        /// </summary>
        public double X;
        public int IntX
        {
            get
            {
                return (int) Math.Round (X);
            }
        }

        /// <summary>
        /// Координата Y
        /// </summary>
        public double Y;
        public int IntY
        {
            get
            {
                return (int) Math.Round (Y);
            }
        }
    }
}
