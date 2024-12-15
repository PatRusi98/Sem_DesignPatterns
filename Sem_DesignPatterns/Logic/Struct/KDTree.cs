using Sem_DesignPatterns.Logic.Struct.Interfaces;
using Sem_DesignPatterns.Logic.Utils;

namespace Sem_DesignPatterns.Logic.Struct
{
    public class KDTree<T> : ITree<T> where T : IStorable
    {
        private KDTreeNode<T>? Root = null;
        private int Dimensions = 0;

        public KDTree() { }

        public bool Insert(T value)              // pozn.cvicenie: doplnit si vlastny komparator!!! Key1 neposielat, posielat objekt s komparatorom (T value)
        {
            if (Root == null)                                                                   // ak neexistuje localRoot, vkladany prvok sa nim stane a dimenzia jeho kluca urci dimenziu stromu
            {
                Root = new(value, 0);
                Dimensions = value.GetKeys().Length;
                return true;
            }

            if (value.GetKeys().Length != Dimensions)                                           // kontrola dimenzii kluca
                return false;

            KDTreeNode<T>? currNode = Root;                                                     // nastavim aktualny workingNode na localRoot, kedze prehladavam od korena
            KDTreeNode<T>? parentNode = null;
            bool isLeft = true;
            int depth = 0;

            while (currNode != null)                                                            // pokial sa nedostaneme do listu (nema syna)
            {
                parentNode = currNode;
                int axis = depth % Dimensions;                                                  // podla ktoreho kluca sa rozhodujem

                if (value.GetKeys()[axis].CompareKeys(currNode.Value!.GetKeys()[axis]) == 0)
                {
                    var isDuplicate = true;
                    for (int i = 0; i < Dimensions; i++)
                    {
                        if (value.GetKeys()[i].CompareKeys(currNode.Value.GetKeys()[i]) != 0)
                        {
                            isDuplicate = false;
                            break;
                        }
                    }

                    if (isDuplicate)
                    {
                        currNode.Duplicates.Add(new(value, depth, currNode.Parent));
                        return true;
                    }
                }

                if (value.GetKeys()[axis].CompareKeys(currNode.Value.GetKeys()[axis]) <= 0)      // porovnanie klucov v danej dimenzii
                {
                    if (currNode.LeftSon != null)                                               // kontrola ci nam nevidi node s prazdnym value
                        if (currNode.LeftSon!.Value == null)
                            currNode.LeftSon = null;

                    currNode = currNode.LeftSon;                                                // ak je kluc mensi, ideme cestou dolava
                    isLeft = true;
                }
                else
                {
                    if (currNode.RightSon != null)                                               // kontrola ci nam nevidi node s prazdnym value
                        if (currNode.RightSon!.Value == null)
                            currNode.RightSon = null;

                    currNode = currNode.RightSon;                                               // ak je kluc vacsi, ideme cestou doprava
                    isLeft = false;
                }

                depth++;
            }

            if (isLeft && parentNode != null)
                parentNode.LeftSon = new(value, depth, parentNode);
            else if (parentNode != null)
                parentNode.RightSon = new(value, depth, parentNode);
            else
                return false;

            return true;
        }

        public List<T>? Search(T value)                                                         // bodove vyhladavanie na operacie s konkretymi prvkami
        {
            if (Root == null)                                                                   // ak neexistuje localRoot, strom je prazdny
                return new();

            if (value.GetKeys().Length != Dimensions)                                           // kontrola dimenzii kluca
                throw new ArgumentException("Wrong key dimension!");

            List<T>? found = new();

            KDTreeNode<T>? currNode = Root;                                                     // nastavim aktualny workingNode na localRoot, kedze prehladavam od korena
            int depth = 0;

            while (currNode != null)                                                            // pokial sa nedostaneme do listu (nema syna)
            {
                if (currNode.Value!.GetKeys().SequenceEqual(value.GetKeys()))                    // ak sa kluc zhoduje s hladanym klucom, vloz prvok do listu
                {
                    found.Add(currNode.Value);

                    if (currNode.Duplicates.Count > 0)                                          // ak obsahuje duplikaty
                    {
                        foreach (var item in currNode.Duplicates)
                        {
                            found.Add(item.Value!);                                              // pridaj duplikaty do zoznamu najdenych
                        }
                    }

                    return found;
                }

                int axis = depth % Dimensions;

                if (value.GetKeys()[axis].CompareKeys(currNode.Value.GetKeys()[axis]) <= 0)     // porovnanie klucov v danej dimenzii
                {
                    if (currNode.LeftSon != null)                                               // kontrola ci nam nevidi node s prazdnym value
                        if (currNode.LeftSon!.Value == null)
                            currNode.LeftSon = null;

                    currNode = currNode.LeftSon;                                                // ak je kluc mensi, ideme dolava
                }
                else
                {
                    if (currNode.RightSon != null)
                        if (currNode.RightSon!.Value == null)
                            currNode.RightSon = null;

                    currNode = currNode.RightSon;                                               // ak je kluc vacsi, ideme doprava
                }

                depth++;
            }

            return found;
        }

        public List<T>? SearchAll()
        {
            return GetAllItems();
        }

        public List<T>? GetAllItems(KDTreeNode<T>? root = null, bool includeDuplicates = true)
        {
            List<T> items = new();
            Stack<KDTreeNode<T>> stack = new();
            KDTreeNode<T>? currNode = null;

            if (root == null)
                currNode = Root;
            else
                currNode = root;

            while (currNode != null || stack.Count > 0)
            {
                while (currNode != null)                                                        // dostaneme sa co najviac dolava
                {
                    stack.Push(currNode);
                    currNode = currNode.LeftSon;
                }

                currNode = stack.Pop();



                if (currNode.LeftSon != null)                                                   // kontrola ci nam nevisi prazdna noda
                    if (currNode.LeftSon!.Value == null)
                        currNode.LeftSon = null;

                if (currNode.RightSon != null)
                    if (currNode.RightSon!.Value == null)
                        currNode.RightSon = null;

                items.Add(currNode.Value!);

                if (includeDuplicates)                                                          // pridame duplikaty
                {
                    foreach (var duplicate in currNode.Duplicates)
                    {
                        items.Add(duplicate.Value!);
                    }
                }

                currNode = currNode.RightSon;
            }

            return items;
        }

        public bool Delete(T value)
        {
            if (Root == null)                                                                   // ak je strom prazdny
                return false;

            if (value.GetKeys().Length != Dimensions)                                           // kontrola dimenzii kluca
                return false;

            KDTreeNode<T>? currNode = Root;
            KDTreeNode<T>? parentNode = null;
            bool isLeft = true;
            int axis = 0;
            int depth = 0;

            while (currNode != null)
            {
                axis = depth % Dimensions;
                if (value.GetKeys()[axis].CompareKeys(currNode.Value!.GetKeys()[axis]) == 0)         // ak sa kluce v danej dimenzii rovnaju
                {
                    var isDuplicateKey = true;
                    for (int i = 0; i < Dimensions; i++)
                    {
                        if (value.GetKeys()[i].CompareKeys(currNode.Value.GetKeys()[i]) != 0)
                        {
                            isDuplicateKey = false;
                            break;
                        }
                    }

                    if (isDuplicateKey)
                    {
                        var (deleted, breaked) = HandleDuplicateDelete(currNode, value, isLeft);

                        if (deleted) return true;
                        if (breaked) break;
                        if (!breaked && !deleted) return false;
                    }
                }

                parentNode = currNode;

                if (value.GetKeys()[axis].CompareKeys(currNode.Value.GetKeys()[axis]) <= 0)     // porovnanie klucov v danej dimenzii
                {
                    if (currNode.LeftSon != null)                                               // kontrola ci nam nevidi node s prazdnym value
                        if (currNode.LeftSon!.Value == null)
                            currNode.LeftSon = null;

                    currNode = currNode.LeftSon;
                    isLeft = true;
                }
                else
                {
                    if (currNode.RightSon != null)                                               // kontrola ci nam nevidi node s prazdnym value
                        if (currNode.RightSon!.Value == null)
                            currNode.RightSon = null;

                    currNode = currNode.RightSon;
                    isLeft = false;
                }

                depth++;
            }

            if (currNode == null)
                return false;

            if (currNode?.LeftSon == null && currNode?.RightSon == null)                          // ak je listom
            {
                if (parentNode == null)
                {
                    Root = null;
                    return true;
                }
                else if (parentNode == Root)
                {
                    if (isLeft)
                        Root.LeftSon = null;
                    else
                        Root.RightSon = null;

                    return true;
                }

                if (isLeft)
                    parentNode!.LeftSon = null;
                else
                    parentNode!.RightSon = null;

                currNode.Value = default;

                return true;
            }

            List<KDTreeNode<T>> nodesToRemove = new();
            List<KDTreeNode<T>> alreadyServed = new();
            nodesToRemove.Add(currNode);

            while (nodesToRemove.Count > 0)
            {
                var nodeToHandle = nodesToRemove.Last();

                if (nodeToHandle.LeftSon == null && nodeToHandle.RightSon == null)
                {
                    if (nodesToRemove.Count == 2)
                    {
                        HandleReplacing(nodesToRemove.First(), nodeToHandle);
                        nodesToRemove.Remove(nodesToRemove.First());
                        continue;
                    }

                    if (nodeToHandle.Parent != null)
                    {
                        if (nodeToHandle.Parent.LeftSon == nodeToHandle)
                        {
                            if (Root?.LeftSon == nodeToHandle)
                                Root.LeftSon = null;
                            else
                                nodeToHandle.Parent.LeftSon = null;
                        }
                        else
                        {
                            if (Root?.RightSon == nodeToHandle)
                                Root.RightSon = null;
                            else
                                nodeToHandle.Parent.RightSon = null;
                        }

                        nodeToHandle.Parent = null;
                    }
                    else
                    {
                        Root = null;
                    }

                    alreadyServed.Add(nodeToHandle);
                    foreach (var item in nodeToHandle.Duplicates)
                    {
                        alreadyServed.Add(item);
                    }

                    nodesToRemove.Remove(nodeToHandle);
                    continue;
                }

                var duplicates = GetReplacements(nodeToHandle);
                foreach (var item in duplicates)
                {
                    if (!alreadyServed.Contains(item))
                        nodesToRemove.Add(item);
                }

                if (duplicates.Count == 1)
                {
                    HandleReplacing(nodeToHandle, duplicates[0]);
                    nodesToRemove.Remove(nodeToHandle);
                }
            }

            foreach (var item in alreadyServed)
            {
                if (item.Value.Equals(value))
                {
                    continue;
                }

                Insert(item.Value!);
            }

            var dlt = alreadyServed.Where(x => x.Value.Equals(value));
            dlt.First().Value = default;

            return true;
        }

        #region private
        private List<KDTreeNode<T>> GetMaxInorder(KDTreeNode<T> localRoot, int parentDepth)
        {
            Stack<KDTreeNode<T>> stack = new();
            List<KDTreeNode<T>> maxDuplicates = new();
            KDTreeNode<T> currNode = localRoot;
            var keyPosition = parentDepth % Dimensions;
            object? maxKeyValue = null;
            var depth = localRoot.Depth;
            var keyAxis = parentDepth % Dimensions;
            var axis = depth % Dimensions;

            while (currNode != null || stack.Count > 0)
            {
                while (currNode != null)
                {
                    axis = currNode.Depth % Dimensions;
                    if (maxDuplicates.Count == 0)
                    {
                        maxDuplicates.Add(currNode);
                        maxKeyValue = currNode.Value!.GetKeys()[keyPosition];
                    }
                    else if (currNode.Value!.GetKeys()[keyPosition].CompareKeys(maxKeyValue!) == 0)
                    {
                        maxDuplicates.Add(currNode);
                    }
                    else if (currNode.Value.GetKeys()[keyPosition].CompareKeys(maxKeyValue!) > 0)
                    {
                        maxKeyValue = currNode.Value.GetKeys()[keyPosition];
                        maxDuplicates.Clear();
                        maxDuplicates.Add(currNode);
                    }

                    if (keyAxis == axis)
                    {
                        stack.Push(currNode);
                        currNode = currNode.RightSon!;
                    }
                    else
                    {
                        currNode = currNode.LeftSon!;
                    }
                }

                currNode = stack.Pop();
                currNode = currNode.RightSon!;
            }

            return maxDuplicates;
        }

        private List<KDTreeNode<T>> GetMinInorder(KDTreeNode<T> localRoot, int parentDepth)
        {
            Stack<KDTreeNode<T>> stack = new();
            List<KDTreeNode<T>> minDuplicates = new();
            KDTreeNode<T> currNode = localRoot;
            var keyPosition = parentDepth % Dimensions;
            object? minKeyValue = null;
            var depth = localRoot.Depth;
            var keyAxis = parentDepth % Dimensions;
            var axis = depth % Dimensions;

            while (currNode != null || stack.Count > 0)
            {
                while (currNode != null)
                {
                    axis = currNode.Depth % Dimensions;
                    if (minDuplicates.Count == 0)
                    {
                        minDuplicates.Add(currNode);
                        minKeyValue = currNode.Value!.GetKeys()[keyPosition];
                    }
                    else if (currNode.Value!.GetKeys()[keyPosition].CompareKeys(minKeyValue!) == 0)
                    {
                        minDuplicates.Add(currNode);
                    }
                    else if (currNode.Value.GetKeys()[keyPosition].CompareKeys(minKeyValue!) < 0)
                    {
                        minKeyValue = currNode.Value.GetKeys()[keyPosition];
                        minDuplicates.Clear();
                        minDuplicates.Add(currNode);
                    }

                    if (keyAxis == axis)
                    {
                        stack.Push(currNode);
                    }

                    currNode = currNode.LeftSon!;
                }

                currNode = stack.Pop();
                currNode = currNode.RightSon!;
            }

            return minDuplicates;
        }

        private List<KDTreeNode<T>> GetReplacements(KDTreeNode<T> localRoot)
        {
            if (localRoot.LeftSon != null)
                return GetMaxInorder(localRoot.LeftSon, localRoot.Depth);
            else if (localRoot.RightSon != null)
                return GetMinInorder(localRoot.RightSon, localRoot.Depth);
            else
                return new();
        }

        private (bool, bool) HandleDuplicateDelete(KDTreeNode<T> currNode, T value, bool isLeft)
        {
            if (currNode.Duplicates.Count > 0)                                              // kontrola duplikatov
            {
                if (currNode.Value!.Equals(value))                                           // ak sa kluc zhoduje s hlavnym workingNode, vyberieme prvy duplikat a nahradime ho nim
                {
                    var newHeadNode = currNode.Duplicates[0];
                    newHeadNode.LeftSon = currNode.LeftSon;
                    newHeadNode.RightSon = currNode.RightSon;
                    newHeadNode.Duplicates = currNode.Duplicates;
                    newHeadNode.Duplicates.RemoveAt(0);

                    if (currNode.Parent == null)
                        Root = newHeadNode;
                    else if (isLeft)
                        currNode.Parent.LeftSon = newHeadNode;
                    else
                        currNode.Parent.RightSon = newHeadNode;

                    currNode = null;                                                        // zmazeme povodny workingNode

                    return (true, false);
                }

                foreach (var item in currNode.Duplicates)
                {
                    if (item.Value!.Equals(value))                                       // ak v liste duplikatov najdeme hladany prvok, zmazeme ho z neho
                    {
                        currNode.Duplicates.Remove(item);
                        return (true, false);
                    }
                }

                return (false, false);
            }
            else if (currNode.Value!.Equals(value))                                      // ak nema duplikaty a rovna sa, vyskocime z while a pokracujeme
            {
                return (false, true);
            }
            else
            {
                return (false, false);
            }
        }

        private void HandleReplacing(KDTreeNode<T> oldNode, KDTreeNode<T> newNode)
        {
            var root = false;
            if (Root == oldNode)
                root = true;

            KDTreeNode<T> temp = new(oldNode.Value, 0);
            temp.Duplicates = oldNode.Duplicates;
            oldNode.Value = newNode.Value;
            oldNode.Duplicates = newNode.Duplicates;
            newNode.Value = temp.Value;
            newNode.Duplicates = temp.Duplicates;

            foreach (var item in oldNode.Duplicates)
            {
                item.Parent = oldNode.Parent;
            }

            foreach (var item in newNode.Duplicates)
            {
                item.Parent = newNode.Parent;
            }

            if (oldNode.LeftSon != null)
            {
                oldNode.LeftSon.Parent = oldNode;
                foreach (var item in oldNode.LeftSon.Duplicates)
                {
                    item.Parent = oldNode;
                }
            }

            if (oldNode.RightSon != null)
            {
                oldNode.RightSon.Parent = oldNode;
                foreach (var item in oldNode.RightSon.Duplicates)
                {
                    item.Parent = oldNode;
                }
            }

            if (newNode.LeftSon != null)
            {
                newNode.LeftSon.Parent = newNode;
                foreach (var item in newNode.LeftSon.Duplicates)
                {
                    item.Parent = newNode;
                }

            }

            if (newNode.RightSon != null)
            {
                newNode.RightSon.Parent = newNode;
                foreach (var item in newNode.RightSon.Duplicates)
                {
                    item.Parent = newNode;
                }
            }

            if (root)
                Root = oldNode;
        }
        #endregion
    }
}
