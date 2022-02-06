////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           PipeLine.cs
//
//  Facility:       Организация потока обработки данных.
//
//
//  Abstract:       Конвейер с асинхронной обработкой данных. Класс является 
//                  активным -  в нем есть два потока. Один отвечает за получение 
//                  исходных данных, а второй за их  обработку. Между потоками 
//                  расположена очередь FIFO, буферизирующая запросы, 
//                  подготовленные первым потоком. Размер очереди может быть как 
//                  неограниченным, так и ограниченным заданным размером.
//
//  Environment:    VC# 7.1
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  19/09/2005
//
//  Copyright (C) OOO "ЛайтКом", 2005-2006. Все права защищены.
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: PipeLine.cs $
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 10.07.07   Time: 8:02
 * Updated in $/LightCom/.NET/Common
 * Реализована поддержка Windows Mobile GPS API
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 4.03.07    Time: 12:39
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:37
 * Updated in $/gps/Lightcom.Common
 * 
 */

using System;
using System.Collections;
using System.Threading;

namespace LightCom.Common
{

/// 
/// <summary>
/// Конвейер с асинхронной обработкой данных. Класс является активным - 
/// в нем есть два потока. Один отвечает за получение исходных данных,
/// а второй за их  обработку. Между потоками расположена очередь FIFO,
/// буферизирующая запросы, подготовленные первым потоком. Размер 
/// очереди может быть как неограниченным, так и ограниченным заданным
/// размером.
/// </summary>
/// 

public abstract class PipeLine: IDisposable
{

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Dispose
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////
    
public void Dispose ()
{
    m_Event.Close ();    
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Constructor.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

protected PipeLine ()
{
    this.m_Queue = new Queue ();
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Максимальное количество элементов конвейера. 0 - неограниченное 
/// количество.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public virtual int MaxCount
{
    get
    {
        lock (this.m_Queue.SyncRoot)
        {
            return this.m_nMaxSize;
        }
    }
    set
    {
        lock (this.m_Queue.SyncRoot)
        {
            this.m_nMaxSize = value;
            if (0 != this.m_nMaxSize)
            {
                while (this.m_Queue.Count >= this.m_nMaxSize)
                {
                    this.m_Queue.Dequeue ();
                }
            }
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Количество элементов в конвейере.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public virtual int Count
{
    get
    {
        lock (this.m_Queue)
        {
            return this.m_Queue.Count;
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Удаление всех элементов конвейера.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public virtual void Clear ()
{
    lock (this.m_Queue)
    {
        this.m_Queue.Clear ();
    }
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Добавляет элемент в конвейер.
/// </summary>
/// <param name="obj">Добавляемый элемент.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

public virtual void Put (Object obj)
{
    lock (this.m_Queue.SyncRoot )
    {
        if (this.m_nMaxSize != 0)
        {
            while (this.m_Queue.Count >= this.m_nMaxSize)
            {
                this.m_Queue.Dequeue ();
            }
        }
    
        this.m_Queue.Enqueue (obj);
        this.m_Event.Set ();
    }
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Добавляет элемент в конец конвейера.
/// </summary>
/// <param name="obj">Добавляемый элемент.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

public virtual void PutBack (Object obj)
{
    lock (this.m_Queue.SyncRoot )
    {
        if (this.m_nMaxSize != 0)
        {
            if (this.m_Queue.Count >= this.m_nMaxSize)
            {
                return;
            }
        }
        this.m_Queue.Enqueue (obj);
        this.m_Event.Set ();
    }
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Извлекает элемент из конвейера.
/// </summary>
/// <returns>Извлеченный элемент.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public virtual Object Get ()
{
    lock (this.m_Queue)
    {
        try
        {
            if (this.m_Queue.Count > 0)
                return this.m_Queue.Dequeue ();
            else
                return null;
        }
        catch (Exception)
        {
            return null;
        }
    }    
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Метод выполняется в отдельном потоке. Должен реализовывать
/// цикл получения данных.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

protected void ProvideData ()
{
    while (! m_bStopSignal)
    {
        try
        {
            object obj;
            if (GetDataItem (out obj))
            {
                //
                // Данные были получены - кладем их в конвейер и 
                // устанавливаем событие, "пробуждающее" обработчик 
                // данных
                //

                this.Put (obj);
            }
            else
            {
                continue;
            }
        }
        catch (Exception)
        {               
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Цикл обработки данных.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

protected void ProcessData ()
{
    //
    //Пока не установлен сигнал остановки вычислений, читаем данные
    //с конца конвейера и обрабатываем их.
    //

    while (! m_bStopSignal)
    {
        try
        {

        //MessageBox.Show ("hello" + this.m_nWaitTimeout.ToString () + " " +  m_Event.Handle.ToString ());
        //m_Event.WaitOne ((int) m_nWaitTimeout, false);
        //Utils.WaitForSingleObject (m_Event.Handle, (int) this.m_nWaitTimeout);        
        m_Event.WaitOne ();
        //MessageBox.Show ("wait");
        
            ProcessItem (this.Get ());
        }
        catch (Exception /*e*/)
        {
            //MessageBox.Show ("sender: " + e.ToString ());
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Запускает конвейер.
/// </summary>
/// <returns>
/// Возвращает true, если потоки запустились.
/// </returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public virtual bool Start ()
{
    if (null == this.m_ProcessingThread)
    {
        this.m_ProvidingThread = new Thread (new ThreadStart (this.ProvideData));
        this.m_ProcessingThread = new Thread (new ThreadStart (this.ProcessData));
        this.m_bStopSignal = false;
        this.m_Event.Reset ();
        this.m_ProvidingThread.Start ();
        this.m_ProcessingThread.Start ();

        return true;
    }

    if (! this.Stop ((this.m_nWaitTimeout * 3) / 2)) return false;

    this.m_ProvidingThread = new Thread (new ThreadStart (this.ProvideData));
    this.m_ProcessingThread = new Thread (new ThreadStart (this.ProcessData));

    this.m_bStopSignal = false;
    this.m_Event.Reset ();
    this.m_ProvidingThread.Start ();
    this.m_ProcessingThread.Start ();

    return true;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Завершает работу конвейера.
/// </summary>
/// <param name="nWaitTimeOut">Время одного интервала ожидания
/// завершеия потока (в мс.). Суммарное время ожидание может быть
/// до 4 x nWaitTimeOut .
/// </param>
/// <returns>Возвращает true, если потоки остановились.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public virtual bool Stop (int nWaitTimeOut)
{
    if (null == this.m_ProvidingThread &&
        null == this.m_ProcessingThread)
    {
        return true;
    }

    this.m_bStopSignal = true;
    this.m_Event.Set ();
    
    Thread.Sleep ((int) nWaitTimeOut);
    //this.m_ProvidingThread.Abort ();
    //this.m_ProcessingThread.Abort ();    
    this.m_ProvidingThread = null;
    this.m_ProcessingThread = null;

    return true;
    

    /*
    for (int i = 0; i < 2; ++ i)
    {
        //
        // Ждем завершения работы потоков.
        //

        if (this.m_ProvidingThread.IsAlive)
        {
            this.m_ProvidingThread.Join ((int) nWaitTimeOut);
        }
        if (this.m_ProvidingThread.IsAlive)
        {
            this.m_ProcessingThread.Join ((int) nWaitTimeOut);
        }

        if (! this.m_ProvidingThread.IsAlive &&
            ! this.m_ProvidingThread.IsAlive)
        {
            //
            //##Threads exited normal way.
            //

            this.m_bStopSignal = false;
            this.m_Event.Reset ();

            return true;
        }

        //
        //  Выводим потоки из состояния ожидания и даем им шанс 
        //  добровольно завершить выполнение.
        //

        
          
        if (this.m_ProvidingThread.IsAlive)
        {
            this.m_ProvidingThread.Interrupt ();
        }
        if (this.m_ProvidingThread.IsAlive)
        {
            this.m_ProcessingThread.Interrupt ();
        }
        
    }

    //
    // Потоки не завершили работу добровольно - завершаем их 
    // принудительно.
    //

    
    if (this.m_ProvidingThread.IsAlive)
    {
        this.m_ProvidingThread.Abort ();
    }
    if (this.m_ProvidingThread.IsAlive)
    {
        this.m_ProcessingThread.Abort ();
    }
    

    return ! this.m_ProvidingThread.IsAlive &&
            ! this.m_ProvidingThread.IsAlive;
     */
            
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Проверяет состояние потоков.
/// </summary>
/// <returns>Возвращает true, если потоки конвейера работают.
/// </returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool IsAlive ()
{   /*
    if (null == m_ProcessingThread)
    {
        m_ProvidingThread = new Thread (new ThreadStart (this.ProvideData));
        m_ProcessingThread = new Thread (new ThreadStart (this.ProcessData));
    }
    
    return this.m_ProvidingThread.IsAlive &&
            this.m_ProvidingThread.IsAlive;
    */
    
    return this.m_ProvidingThread != null;
}


////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Обработка элемента конвейера.
/// Метод выполняется в отдельном потоке.
/// </summary>
/// <param name="obj">Обрабатываемый элемент.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

protected abstract void ProcessItem (object obj);

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Обработка элемента конвейера.
/// Метод выполняется в отдельном потоке.
/// </summary>
/// <param name="obj">Обрабатываемый элемент.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

protected abstract bool GetDataItem (out object obj);

/// 
/// <summary>
/// Максимальное количество элементов конвейера.
/// </summary>
/// 

protected int m_nMaxSize;// = 0;

/// 
/// <summary>
/// Контейнер элементов.
/// </summary>
/// 

protected Queue m_Queue;

///
/// <summary>
/// Событие, указывающее на то, что в конвейере появились элементы.
/// </summary>
/// 

AutoResetEvent Event {get {return this.m_Event;}}
AutoResetEvent m_Event = new AutoResetEvent (false);

/// 
/// <summary>
/// Processing thread.
/// </summary>
/// 

Thread m_ProcessingThread;// = null;

/// 
/// <summary>
/// Providing thread.
/// </summary>
/// 

Thread m_ProvidingThread;// = null;
    
/// 
/// <summary>
///Сигнал потокам, что нужно остановить вычисления.
/// </summary>
/// 

protected bool m_bStopSignal;// = false;

/// 
/// <summary>
/// Интервал (в миллисекундах), в течение которого поток-обрабочик
/// ожидает поступления данных.
/// </summary>
/// 

public int WaitTimeOut {get {return this.m_nWaitTimeout;} set {this.m_nWaitTimeout = value;}}
protected int m_nWaitTimeout = 1000;

}
}