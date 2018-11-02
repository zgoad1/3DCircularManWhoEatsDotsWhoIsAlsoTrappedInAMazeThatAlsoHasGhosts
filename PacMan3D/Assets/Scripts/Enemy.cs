using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : Character {
    public Transform player;
    protected override void Start()
    {
        base.Start();
        node uno = new node(new Vector3(0, 0, 0), null);
        node dos = new node(new Vector3(0, 1, 0), uno);
        node tres = new node(new Vector3(0, 2, 0), dos);
        // test for reconstruct path is successful
        // in the final version it will only need to 
        // return the first node. 
        reconstructPath(tres);
    }

    // is not being called nor is the SetNextTile function atleast to my knowledge
    // this is because that check is failing. It only fails with this script not with 
    // character by itself. 
    protected override direction GetDefaultDirection(direction d) {
        
        List<node> myList =  new List<node>();

      
        return direction.DOWN;
    }
    // copied strait from another pathfinding program i have
    // tryig to get this to work with the current situation 
    // it is unfinished and untested. 
    protected direction AstarSearch()
    {
        List<node> openSet = new List<node>();
        List<node> closedSet = new List<node>();
        ///   List<node> CameFrom = new List<node>();
        int countPath = 0;

        //openSet.Add(new node(enemyShip.transform.position));
        openSet[0].FScore = Mathf.Infinity;
        openSet[0].GScore = 0;

        node curNode = openSet[0];
        //buildGrid(curNode);

        while (openSet.Count != 0)
        {
            curNode = openSet[0];

            // this ends the search 
      //      if (Vector3.Distance(curNode.pos, End) < RadiusToStop)
           //     break;

            // perhaps a way to check if this has gone on for too long
            openSet.Remove(curNode);
            closedSet.Add(curNode);

            foreach (node neighbor in curNode.neighbors)
            {
                countPath++;
                // NEED TO FIX FUNCTION IN THE AI TO WORK WITH THESE PAREAMETERS!!!
                // AI decides if we can use this path 
                //if (!enemyShip.GetComponent<Enemy>().CanUseNode(neighbor.pos))
                //    continue; point2

              //  if (Vector3.Distance(player.position, neighbor.pos) < playerRad || Vector3.Distance(point2.position, neighbor.pos) < playerRad)
               //     continue;

                if (closedSet.Contains(neighbor))
                    continue;

                float tenative_gScore = curNode.GScore + Vector3.Distance(curNode.pos, neighbor.pos);

                if (!openSet.Contains(neighbor))
                    openSet.Add(neighbor);
                else if (tenative_gScore >= neighbor.GScore)
                    continue;


                neighbor.cameFrom = curNode;
                neighbor.GScore = tenative_gScore;
                neighbor.FScore = neighbor.GScore + heuristic();
            }
            openSet = openSet.OrderBy(o => o.FScore).ToList();
        }
        
        return reconstructPath(curNode);//, CameFrom);
    }
    // will get the amount of tiles with dots at that node 
    // right now i just have it set to zero while i figure 
    // everything out.
    static int heuristic ()
    {
        return 0;
    }
    
    // Recreates the path 
    protected direction reconstructPath(node curNode) {

        while (curNode.cameFrom != null) {
            curNode = curNode.cameFrom;
        }
        Debug.Log(curNode.pos);
        direction DIR = direction.UP; 
        switch (curNode.direction) {
            case 'u':
                DIR = direction.UP;
                break;
            case 'd':
                DIR = direction.DOWN;
                break;
            case 'l':
                DIR = direction.LEFT;
                break;
            case 'r':
                DIR = direction.RIGHT;
                break;
        }
        return DIR;
    }
}

// still determining the thints i need in this class 
// may take some time 
public class node {
    public List<node> neighbors = new List<node>();
    public Vector3 pos;
    public float GScore, FScore;
    public node cameFrom;
    public char direction; // u d l r
    public int numOfDirtyTiles;

    public node(Vector3 pos, node cameFrom) {
        this.pos = pos;
        this.cameFrom = cameFrom;
    }
}