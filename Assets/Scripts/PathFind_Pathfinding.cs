/**
* Provide simple path-finding algorithm with support in penalties.
* Heavily based on code from this tutorial: https://www.youtube.com/watch?v=mZfyt03LDH4
* This is just a Unity port of the code from the tutorial + option to set penalty + nicer API.
*
* Original Code author: Sebastian Lague.
* Modifications & API by: Ronen Ness.
* Since: 2016.
*/
using UnityEngine;
using System.Collections.Generic;

namespace PathFind
{
    /**
    * Main class to find the best path from A to B.
    * Use like this:
    * Grid grid = new Grid(width, height, tiles_costs);
    * List<Point> path = Pathfinding.FindPath(grid, from, to);
    */

    public class Pathfinding
    {
        // The API you should use to get path
        // grid: grid to search in.
        // startPos: starting position.
        // targetPos: ending position.
        public static List<Point> FindPath(Grid grid, Point startPos, Point targetPos)
        {
            // find path
            List<Node> nodes_path = _ImpFindPath(grid, startPos, targetPos);

            // convert to a list of points and return
            List<Point> ret = new List<Point>();
            if (nodes_path != null)
            {
                foreach (Node node in nodes_path)
                {
                    ret.Add(new Point(node.gridX, node.gridY));
                }
            }
            return ret;
        }

        // internal function to find path, don't use this one from outside
        private static List<Node> _ImpFindPath(Grid grid, Point startPos, Point targetPos)
        {
            Node startNode = grid.nodes[startPos.x, startPos.y];
            Node targetNode = grid.nodes[targetPos.x, targetPos.y];

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return RetracePath(grid, startNode, targetNode);
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) * (int)(10.0f * neighbour.penalty);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }

            return null;
        }

        private static List<Node> RetracePath(Grid grid, Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            return path;
        }

        private static int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }

	public class Grid
    {
        public Node[,] nodes;
        int gridSizeX, gridSizeY;

        /**
        * Create a new grid with tile prices.
        * width: grid width.
        * height: grid height.
        * tiles_costs: 2d array of floats, representing the cost of every tile.
        *               0.0f = unwalkable tile.
        *               1.0f = normal tile.
        */
        public Grid(int width, int height, float[,] tiles_costs)
        {
            gridSizeX = width;
            gridSizeY = height;
            nodes = new Node[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    nodes[x, y] = new Node(tiles_costs[x, y], x, y);

                }
            }
        }

        /**
        * Create a new grid of just walkable / unwalkable.
        * width: grid width.
        * height: grid height.
        * walkable_tiles: the tilemap. true for walkable, false for blocking.
        */
        public Grid(int width, int height, bool[,] walkable_tiles)
        {
            gridSizeX = width;
            gridSizeY = height;
            nodes = new Node[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    nodes[x, y] = new Node(walkable_tiles[x, y] ? 1.0f : 0.0f, x, y);
                }
            }
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(nodes[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }
    }

	public class Node
    {
        // node starting params
        public bool walkable;
        public int gridX;
        public int gridY;
        public float penalty;

        // calculated values while finding path
        public int gCost;
        public int hCost;
        public Node parent;

        // create the node
        // _price - how much does it cost to pass this tile. less is better, but 0.0f is for non-walkable.
        // _gridX, _gridY - tile location in grid.
        public Node(float _price, int _gridX, int _gridY)
        {
            walkable = _price != 0.0f;
            penalty = _price;
            gridX = _gridX;
            gridY = _gridY;
        }

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
    }

	public class Point
    {
        public int x;
        public int y;

        public Point()
        {
            x = 0;
            y = 0;
        }
        public Point(int iX, int iY)
        {
            this.x = iX;
            this.y = iY;
        }

        public Point(Point b)
        {
            x = b.x;
            y = b.y;
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }

        public override bool Equals(System.Object obj)
        {
            // Unlikely to compare incorrect type so removed for performance
            // if (!(obj.GetType() == typeof(PathFind.Point)))
            //     return false;
            Point p = (Point)obj;

            if (ReferenceEquals(null, p))
            {
                return false;
            }

            // Return true if the fields match:
            return (x == p.x) && (y == p.y);
        }

        public bool Equals(Point p)
        {
            if (ReferenceEquals(null, p))
            {
                return false;
            }
            // Return true if the fields match:
            return (x == p.x) && (y == p.y);
        }

        public static bool operator ==(Point a, Point b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (ReferenceEquals(null, a))
            {
                return false;
            }
            if (ReferenceEquals(null, b))
            {
                return false;
            }
            // Return true if the fields match:
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }

        public Point Set(int iX, int iY)
        {
            this.x = iX;
            this.y = iY;
            return this;
        }
    }
}
