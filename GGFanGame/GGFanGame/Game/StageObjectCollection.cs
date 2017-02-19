using System;
using System.Collections;
using System.Collections.Generic;

namespace GGFanGame.Game
{
    internal sealed class StageObjectCollection : IEnumerable<StageObject>
    {
        private readonly List<StageObject> _opaqueObjects, _transparentObjects;

        internal IEnumerable<StageObject> OpaqueObjects => _opaqueObjects;
        internal IEnumerable<StageObject> TransparentObjects => _transparentObjects;

        public StageObjectCollection(StageObject[] objects)
        {
            _opaqueObjects = new List<StageObject>();
            _transparentObjects = new List<StageObject>();
            AddRange(objects);
        }

        internal void AddRange(params StageObject[] objs)
        {
            foreach (var obj in objs)
                Add(obj);
        }

        internal void Add(StageObject obj)
        {
            if (obj.IsOpaque)
                _opaqueObjects.Add(obj);
            else
                _transparentObjects.Add(obj);
        }

        internal void ForEach(Action<StageObject> method)
        {
            _opaqueObjects.ForEach(method);
            _transparentObjects.ForEach(method);
        }

        private StageObject GetItem(int index)
        {
            if (index < _opaqueObjects.Count)
                return _opaqueObjects[index];
            else
                return _transparentObjects[index - _opaqueObjects.Count];
        }

        private void SetItem(int index, StageObject obj)
        {
            if (index < _opaqueObjects.Count)
                _opaqueObjects[index] = obj;
            else
                _transparentObjects[index - _opaqueObjects.Count] = obj;
        }

        internal void RemoveAt(int index)
        {
            if (index < _opaqueObjects.Count)
                _opaqueObjects.RemoveAt(index);
            else
                _transparentObjects.RemoveAt(index - _opaqueObjects.Count);
        }

        internal int Count => _opaqueObjects.Count + _transparentObjects.Count;

        internal void Sort()
        {
            _transparentObjects.Sort();
        }

        public IEnumerator<StageObject> GetEnumerator()
        {
            var index = 0;
            while (index < _opaqueObjects.Count + _transparentObjects.Count)
            {
                yield return GetItem(index);
                index++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal StageObject this[int index]
        {
            get
            {
                return GetItem(index);
            }
            set
            {
                SetItem(index, value);
            }
        }
    }
}
