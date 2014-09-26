using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace SomeCompany.Pathfinder.GraphObjects
{
    public class PathCollection : IPathCollection
    {
        private List<IPath> Paths { get; set; }

        [DebuggerStepThrough]
        public PathCollection() {
            Paths = new List<IPath>();
        }

        [DebuggerStepThrough]
        public IEnumerator<IPath> GetEnumerator() {
            return Paths.GetEnumerator();
        }

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        [DebuggerStepThrough]
        public void Add(IPath item) {
            Paths.Add(item);
        }

        [DebuggerStepThrough]
        public void Clear() {
            Paths.Clear();
        }

        [DebuggerStepThrough]
        public bool Contains(IPath item) {
            return Paths.Contains(item);
        }

        [DebuggerStepThrough]
        public void CopyTo(IPath[] array, int arrayIndex) {
            Paths.CopyTo(array,arrayIndex);
        }

        [DebuggerStepThrough]
        public bool Remove(IPath item) {
            return Paths.Remove(item);
        }

        public int Count { get { return Paths.Count; } }
        public bool IsReadOnly { get { return false; } }

        [DebuggerStepThrough]
        public int IndexOf(IPath item) {
            return Paths.IndexOf(item);
        }

        [DebuggerStepThrough]
        public void Insert(int index, IPath item) {
            Paths.Insert(index, item);
        }

        [DebuggerStepThrough]
        public void RemoveAt(int index) {
            Paths.RemoveAt(index);
        }

        public IPath this[int index] { get { return Paths[index]; } set { Paths[index] = value; } }

        public string DescribeAllPaths() {
            var pathDescriptions = "";
            foreach (var path in Paths)
            {
                pathDescriptions += path.PathDescription + "\n\n----------------------------------\n";
            }
            return pathDescriptions;
        }
    }
}
