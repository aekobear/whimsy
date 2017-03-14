using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class QuadList
{
    private int ratio;
    private int[] bounds;
    private QuadList parent;
    private int quadrant;
    private List<IPoint> items;
    private int max;
    private QuadList[] children;


    public QuadList() : this(null, -1)
    {
        ratio = 2;
        bounds = new int[2] { 0, 0 };
    }

    protected QuadList(QuadList[] children, int[] bounds, int ratio)
    {
        for (int i = 0; i < 4; i++)
        {
            if (children[i] == null)
            {
                children[i] = new QuadList(this, i);
            }
        }
        this.children = children;
        this.bounds = bounds;
        this.ratio = ratio;
        max = 5;
        items = new List<IPoint>();
        this.parent = null;
        this.quadrant = -1;
    }

    protected QuadList(QuadList parent, int quadrant)
    {
        this.parent = parent;
        this.quadrant = quadrant;
        items = new List<IPoint>();
        max = 5;
        children = new QuadList[4];
    }

    public bool Insert(IPoint item, bool overlap = false)
    {
        if (parent == null)
        {
            return Add(item, overlap);
        }
        else
        {
            return parent.Insert(item, overlap);
        }
    }

    public bool Add(IPoint item, bool overlap = false)
    {
        if (!IsInBounds(item))
        {
            return parent == null && Expand(item);
        }

        if (children[0] == null)
        {
            if (!overlap)
            {
                foreach (IPoint i in items)
                {
                    if (i.X == item.X && i.Y == item.Y)
                    {
                        return false;
                    }
                }
            }
            items.Add(item);
            if (items.Count > max)
            {
                Split();
            }
            return true;
        }

        Stow(item, overlap);
        return true;
    }

    public QuadList Head()
    {
        if(parent == null)
        {
            return this;
        }
        return parent.Head();
    }

    public IPoint Find(int x, int y)
    {
        if(!IsInBounds(x, y))
        {
            return null;
        }

        if(items.Count > 0)
        {
            foreach (IPoint item in items)
            {
                if(item.X == x && item.Y == y)
                {
                    return item;
                }
            }
        }
        else if(children[0] != null)
        {
            foreach(var child in children)
            {
                var found = child.Find(x, y);
                if(found != null)
                {
                    return found;
                }
            }
        }

        return null;
    }

    public IPoint Pop(int x, int y)
    {
        if (!IsInBounds(x, y))
        {
            return null;
        }

        if (items.Count > 0)
        {
            IPoint found = null;
            foreach (IPoint item in items)
            {
                if (item.X == x && item.Y == y)
                {
                    found = item;
                    break;
                }
            }
            items.Remove(found);
            return found;
        }
        else if (children[0] != null)
        {
            foreach (var child in children)
            {
                var found = child.Pop(x, y);
                if (found != null)
                {
                    Consolidate();
                    return found;
                }
            }
        }

        return null;

    }

    private bool Expand(IPoint item)
    {
        if (IsInBounds((item)))
            throw new System.ArgumentOutOfRangeException("item cannot be within bounds of QuadFlexTree");

        int r = GetRatio();
        int[] b = GetBounds();
        this.quadrant = 0;
        if (item.X > b[2] && item.Y > b[3])
        {
            quadrant = 1;
        }
        if (item.X >= b[0] && item.Y <= b[3])
        {
            quadrant = 2;
        }
        if (item.X < b[0] && item.Y < b[2])
        {
            quadrant = 3;

        }
        QuadList[] c = new QuadList[4];
        c[quadrant] = this;

        if (quadrant == 0)
        {
            b[0] -= r;
        }
        else if (quadrant == 1)
        {
            // stay right where you are!
        }
        else if (quadrant == 2)
        {
            b[1] -= r;
        }
        else if (quadrant == 3)
        {
            b[0] -= r;
            b[1] -= r;
        }
        int[] newBounds = new int[2] { b[0], b[1] };
        this.parent = new QuadList(c, newBounds, r * 2);
        return this.parent.Add(item);
    }

    private bool IsInBounds(IPoint item)
    {
        return IsInBounds(item.X, item.Y);
    }

    private bool IsInBounds(int x, int y)
    {
        int[] b = GetBounds();
        return (x >= b[0] && y >= b[1] && x <= b[2] && y <= b[3]);
    }

    public int[] GetBounds()
    {
        if (parent == null)
        {
            return new int[4] { bounds[0], bounds[1], bounds[0] + ratio - 1, bounds[1] + ratio - 1 };
        }

        int[] pbounds = parent.GetBounds();
        int[] cbounds = new int[4];

        if (quadrant == 0)
        {
            pbounds[0] += GetRatio();
            pbounds[3] -= GetRatio();
        }
        if (quadrant == 1)
        {
            pbounds[2] -= GetRatio();
            pbounds[3] -= GetRatio();
        }
        if (quadrant == 2)
        {
            pbounds[1] += GetRatio();
            pbounds[2] -= GetRatio();
        }
        if (quadrant == 3)
        {
            pbounds[0] += GetRatio();
            pbounds[1] += GetRatio();
        }

        return pbounds;

    }

    public void Consolidate()
    {
        if(children[0] == null)
        {
            return;
        }
        if (IsEmpty())
        {
            children[0] = null;
            children[1] = null;
            children[2] = null;
            children[3] = null;
        }
        else
        {
            foreach(var child in children)
            {
                child.Consolidate();
            }
        }
    }

    public bool IsEmpty()
    {
        bool empty = true;
        empty = empty && (items.Count == 0);
        if(children[0] == null)
        {
            return empty;
        }
        else
        {
            foreach(var child in children)
            {
                empty = empty && child.IsEmpty();
            }
            return empty;
        }

    }

    private void Split()
    {
        if (children[0] == null)
        {
            children[0] = new QuadList(this, 0);
            children[1] = new QuadList(this, 1);
            children[2] = new QuadList(this, 2);
            children[3] = new QuadList(this, 3);
        }

        foreach (IPoint item in items)
        {
            Stow(item, true);
        }

        items = new List<IPoint>();

    }

    private void Stow(IPoint item, bool overlap)
    {
        foreach (QuadList child in children)
        {
            if (child.Add(item, overlap))
            {
                break;
            }
        }
    }

    public void SetRatio(int ratio)
    {
        if (parent != null)
        {
            parent.SetRatio(ratio);
            return;
        }
        this.ratio = ratio;
    }

    public int GetRatio()
    {
        if (parent != null)
        {
            return parent.GetRatio() / 2;
        }
        return this.ratio;
    }

    public string State(string indent = "")
    {
        string state = indent + "(";
        if(parent == null)
        {
            state += "p ";
        }
        if(children[0] == null)
        {
            foreach(var item in items)
            {
                state += item + ", ";
            }
            state += ")";
            return state;
        }
        else
        {
            for(int i = 0; i < 4; i++){
                state += i + ": " + children[i].State(indent + " ") + "\n";
            }
            state += "p)";
            return state;
        }
    }

}