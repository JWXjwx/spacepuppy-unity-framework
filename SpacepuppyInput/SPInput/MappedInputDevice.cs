﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace com.spacepuppy.SPInput
{

    /// <summary>
    /// InputDevice based on a mapping value instead of a hash. This mapping value should usually be an enum, you can also use an int/long/etc if you want.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MappedInputDevice<T> : IMappedInputDevice<T> where T : struct, System.IConvertible
    {

        #region Fields

        private string _id;
        private bool _active = true;
        private MappedInputSignatureCollection<T> _signatures;

        #endregion

        #region CONSTRUCTOR

        public MappedInputDevice(string id)
        {
            _id = id;
            _signatures = new MappedInputSignatureCollection<T>();
        }

        public MappedInputDevice(string id, MappedInputSignatureCollection<T> sig)
        {
            _id = id;
            _signatures = sig ?? new MappedInputSignatureCollection<T>();
        }

        public MappedInputDevice(string id, IEqualityComparer<T> comparer)
        {
            _id = id;
            _signatures = new MappedInputSignatureCollection<T>(comparer);
        }

        #endregion

        #region Properties

        public MappedInputSignatureCollection<T> InputSignatures { get { return _signatures; } }

        #endregion

        #region IInputSignature Interface

        public string Id
        {
            get { return _id; }
        }

        public int Hash
        {
            get { return _id.GetHashCode(); }
        }

        float IInputSignature.Precedence
        {
            get
            {
                return 0f;
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
        
        void IInputSignature.Update()
        {
            var e = _signatures.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.Update();
            }
        }

        void IInputSignature.FixedUpdate()
        {
            var e = _signatures.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.FixedUpdate();
            }
        }

        #endregion

        #region IInputDevice Interface

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public bool Contains(string id)
        {
            return _signatures.Contains(id);
        }

        IInputSignature IInputDevice.GetSignature(string id)
        {
            return _signatures.GetSignature(id);
        }

        public ButtonState GetButtonState(string id, bool consume = false)
        {
            if (!_active) return ButtonState.None;

            var sig = _signatures.GetSignature(id);
            //if (sig == null) return ButtonState.None;
            if (!(sig is IButtonInputSignature)) return ButtonState.None;

            var result = (sig as IButtonInputSignature).CurrentState;
            if (consume) (sig as IButtonInputSignature).Consume();
            return result;
        }
        
        public float GetAxleState(string id)
        {
            if (!_active) return 0f;

            var sig = _signatures.GetSignature(id);
            if (sig == null) return 0f;
            if (!(sig is IAxleInputSignature)) return 0f;

            return (sig as IAxleInputSignature).CurrentState;
        }

        public Vector2 GetDualAxleState(string id)
        {
            if (!_active) return Vector2.zero;

            var sig = _signatures.GetSignature(id);
            if (sig == null) return Vector2.zero;
            if (!(sig is IDualAxleInputSignature)) return Vector2.zero;

            return (sig as IDualAxleInputSignature).CurrentState;
        }
        
        public Vector2 GetCursorState(string id)
        {
            if (!_active) return Vector2.zero;

            var sig = _signatures.GetSignature(id);
            if (sig == null) return Vector2.zero;
            if (!(sig is ICursorInputSignature)) return Vector2.zero;

            return (sig as ICursorInputSignature).CurrentState;
        }
        
        #endregion

        #region IMappedInputDevice Interface

        IInputSignature IMappedInputDevice<T>.GetSignature(T mapping)
        {
            return _signatures.GetSignature(mapping);
        }

        public ButtonState GetButtonState(T mapping, bool consume = false)
        {
            if (!_active) return ButtonState.None;

            var sig = _signatures.GetSignature(mapping);
            //if (sig == null) return ButtonState.None;
            if (!(sig is IButtonInputSignature)) return ButtonState.None;

            var result = (sig as IButtonInputSignature).CurrentState;
            if (consume) (sig as IButtonInputSignature).Consume();
            return result;
        }
        
        public float GetAxleState(T mapping)
        {
            if (!_active) return 0f;

            var sig = _signatures.GetSignature(mapping);
            if (sig == null) return 0f;
            if (!(sig is IAxleInputSignature)) return 0f;

            return (sig as IAxleInputSignature).CurrentState;
        }

        public Vector2 GetDualAxleState(T mapping)
        {
            if (!_active) return Vector2.zero;

            var sig = _signatures.GetSignature(mapping);
            if (sig == null) return Vector2.zero;
            if (!(sig is IDualAxleInputSignature)) return Vector2.zero;

            return (sig as IDualAxleInputSignature).CurrentState;
        }

        public Vector2 GetCursorState(T mapping)
        {
            if (!_active) return Vector2.zero;

            var sig = _signatures.GetSignature(mapping);
            if (sig == null) return Vector2.zero;
            if (!(sig is ICursorInputSignature)) return Vector2.zero;

            return (sig as ICursorInputSignature).CurrentState;
        }

        #endregion


    }

}
