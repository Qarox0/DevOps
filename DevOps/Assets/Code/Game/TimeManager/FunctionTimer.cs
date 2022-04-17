using System;
using System.Timers;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FunctionTimer
{
       private Action _action;
       private GameObject _gameObject;
       private float _timer;
       private float _timerStart;
       private bool _oneTimeShoot;
       private bool _isDestroyed = false;
       private bool _isPaused = false;
       
       public static FunctionTimer Create(Action action, float timer, bool oneTimeShot = true)
       {
              GameObject gameObject = new GameObject("FunctionTimer", typeof(MonoHook));
              var functionTimer = new FunctionTimer(action, timer, oneTimeShot, gameObject);

              gameObject.GetComponent<MonoHook>().onUpdate = functionTimer.Update;

              return functionTimer;
       }
       
       private class MonoHook : MonoBehaviour
       {
              public Action onUpdate;
              
              private void Update()
              {
                     if (onUpdate != null) onUpdate();
              }
       }

       private FunctionTimer(Action action, float timer, bool oneTimeShot, GameObject gameObject)
       {
              _action = action;
              _timerStart = _timer = timer;
              _oneTimeShoot = oneTimeShot;
              _gameObject = gameObject;
       }

       public void Stop()
       {
              Destroy();
       }

       public void Pause(bool pauseState)
       {
              _isPaused = pauseState;
       }

       private void Update()
       {
              if (!_isDestroyed && !_isPaused)
              {
                     _timer -= Time.deltaTime;
                     if (_timer < 0)
                     {
                            _action();
                            if (_oneTimeShoot)
                            {
                                   Destroy();
                            }
                            else
                            {
                                   _timer += _timerStart;
                            }
                     }
              }
       }

       private void Destroy()
       {
              _isDestroyed = true;
              if(_gameObject != null)
                     UnityEngine.Object.Destroy(_gameObject);
       }
}