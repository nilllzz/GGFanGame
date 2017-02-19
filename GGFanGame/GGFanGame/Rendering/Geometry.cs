using System;
using System.Collections.Generic;
using System.Linq;

namespace GGFanGame.Rendering
{
    internal sealed class Geometry<VertexType> : IDisposable where VertexType : struct
    {
        private Dictionary<VertexType, int> _vertexIndexMatch = new Dictionary<VertexType, int>();
        private List<int> _indices = new List<int>();

        internal VertexType[] Vertices => _vertexIndexMatch.Keys.ToArray();
        internal int[] Indices => _indices.ToArray();
        internal bool IsDisposed { get; private set; }

        private void CheckDisposed()
        {
            if (IsDisposed) throw new ObjectDisposedException("Geometry");
        }

        public void AddVertices(VertexType[] vertices)
        {
            CheckDisposed();

            foreach (var vertex in vertices)
            {
                var index = 0;
                if (!_vertexIndexMatch.TryGetValue(vertex, out index))
                {
                    index = _vertexIndexMatch.Count;
                    _vertexIndexMatch.Add(vertex, index);
                }

                _indices.Add(index);
            }
        }

        public void AddIndexedVertices(IEnumerable<VertexType> vertices)
        {
            CheckDisposed();

            foreach (var vertex in vertices)
            {
                if (!_vertexIndexMatch.ContainsKey(vertex))
                    _vertexIndexMatch.Add(vertex, _vertexIndexMatch.Count);
            }
        }

        public void AddIndices(IEnumerable<int> indices)
        {
            CheckDisposed();

            _indices.AddRange(indices);
        }

        public void Merge(Geometry<VertexType> mesh)
        {
            CheckDisposed();

            AddVertices(mesh.Vertices);
        }

        public static Geometry<VertexType> Merge(params Geometry<VertexType>[] meshes)
        {
            if (meshes == null || meshes.Length == 0)
                return new Geometry<VertexType>();
            if (meshes.Length == 1)
                return meshes[0];

            var baseMesh = meshes[0];

            for (int i = 1; i < meshes.Length; i++)
                baseMesh.Merge(meshes[i]);

            return baseMesh;
        }

        public void Clear()
        {
            CheckDisposed();

            _indices.Clear();
            _vertexIndexMatch.Clear();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Geometry()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                _vertexIndexMatch = null;
                _indices = null;

                IsDisposed = true;
            }
        }
    }
}
