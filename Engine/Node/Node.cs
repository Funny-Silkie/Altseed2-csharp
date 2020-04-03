﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Altseed
{
    /// <summary>
    /// ゲームシーンを構成するノードを表します。
    /// </summary>
    [Serializable]
    public class Node : Registerable<Node>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Node()
        {
            _Children = new RegisterableCollection<Node, Node>(this);
            Children = _Children.AsReadOnly();
        }

        /// <summary>
        /// 更新
        /// </summary>
        internal virtual void Update()
        {
            OnUpdate();

            _Children.Update();
            foreach (var c in Children)
            {
                c.Update();
            }
        }

        #region Registerable (子として)

        /// <summary>
        /// 親ノードを取得または設定します。
        /// </summary>
        public Node Parent { get; private set; }

        /// <summary>
        /// <paramref name="owner"/> に登録された際の処理
        /// </summary>
        /// <param name="owner"></param>
        internal override void Added(Node owner)
        {
            Parent = owner;
            OnAdded();

            for (var n = Parent; ; n = n.Parent)
            {
                if (n == null) return;
                if (n is RootNode) break;
            }
            Registered();
        }

        /// <summary>
        /// 親要素から削除されたときの処理
        /// </summary>
        internal override void Removed()
        {
            Parent = null;
            OnRemoved();
            Unregistered();
        }

        /// <summary>
        /// エンジンに登録され、木を辿って<see cref="RootNode"/> にたどり着けるようになったとき実行
        /// </summary>
        protected internal virtual void Registered()
        {
            foreach (var c in Children)
            {
                c.Registered();
            }
            OnRegistered();
        }

        /// <summary>
        /// エンジンから削除され、木を辿って<see cref="RootNode"/> にたどり着けなくなったとき実行
        /// </summary>
        protected internal virtual void Unregistered()
        {
            foreach (var c in Children)
            {
                c.Unregistered();
            }

            OnUnregistered();
        }

        #endregion

        #region Registerable (親として)

        private RegisterableCollection<Node, Node> _Children;

        /// <summary>
        /// 子要素のコレクションを取得します。
        /// </summary>
        public ReadOnlyCollection<Node> Children { get; }

        /// <summary>
        /// 子要素を追加します。
        /// </summary>
        /// <param name="node">追加する要素</param>
        public void AddChildNode(Node node)
        {
            _Children.Add(node);
        }

        /// <summary>
        /// 子要素をすべて削除します
        /// </summary>
        public void ClearNodes()
        {
            foreach (var c in Children)
                if (c.Status == RegisterStatus.Registered)
                    RemoveChildNode(c);
        }

        /// <summary>
        /// 子要素を削除します。
        /// </summary>
        /// <param name="node">削除する要素</param>
        public void RemoveChildNode(Node node)
        {
            _Children.Remove(node);
        }

        #endregion

        #region 仮想メソッド

        protected virtual void OnAdded() { }

        protected virtual void OnRemoved() { }

        protected virtual void OnRegistered() { }

        protected virtual void OnUnregistered() { }

        protected virtual void OnUpdate() { }

        #endregion

        /// <summary>
        /// 先祖ノードを列挙します。
        /// </summary>
        public IEnumerable<Node> EnumerateAncestors()
        {
            var current = Parent;
            for (var n = Parent; current != null && !(current is RootNode); current = current.Parent)
                yield return current;

            yield break;
        }

        /// <summary>
        /// 子孫ノードを列挙します。
        /// </summary>
        public IEnumerable<Node> EnumerateDescendants()
        {
            foreach (var c in Children)
            {
                yield return c;
                foreach (var g in c.EnumerateDescendants())
                    yield return g;
            }
        }
    }
}
