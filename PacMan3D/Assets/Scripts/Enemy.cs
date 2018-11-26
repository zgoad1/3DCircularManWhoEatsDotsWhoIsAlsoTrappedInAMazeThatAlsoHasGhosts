using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;

/// <Main Problem>
/// 
/// My final path is only returnning suck values. It shoudl be returning 
/// many different directions. I have determined that the suck value 
/// tentativeGscore is nearly always the same or less than the other options
/// 
/// I had a similar if not the exact same problem with the program for the 
/// vaccume ai. i think the solution was that i was only pointing to the original 
/// world and not creating new copies. I cant say this is the problem because
/// i am going value by value assigning everything to the new copy. 
/// 
/// (dont worry about suck values in the final path i have built a way to 
/// filter them out)  
/// 
/// </summary>


public class Enemy : Character {
    public Transform player;
    protected direction where = direction.UP;

    #region tileMap
    _tile[,] tileMap ={{new _tile(0, 0, false), new _tile(0, 1, false), new _tile(0, 2, false), new _tile(0, 3, false), new _tile(0, 4, false), new _tile(0, 5, false), new _tile(0, 6, false), new _tile(0, 7, false), new _tile(0, 8, false), new _tile(0, 9, false), new _tile(0, 10, false), new _tile(0, 11, false), new _tile(0, 12, false), new _tile(0, 13, false), new _tile(0, 14, false), new _tile(0, 15, false), new _tile(0, 16, false), new _tile(0, 17, false), new _tile(0, 18, false), new _tile(0, 19, false), new _tile(0, 20, false), new _tile(0, 21, false), new _tile(0, 22, false), new _tile(0, 23, false), new _tile(0, 24, false), new _tile(0, 25, false), new _tile(0, 26, false), new _tile(0, 27, false)},
{new _tile(1, 0, false), new _tile(1, 1, false), new _tile(1, 2, false), new _tile(1, 3, false), new _tile(1, 4, false), new _tile(1, 5, false), new _tile(1, 6, false), new _tile(1, 7, false), new _tile(1, 8, false), new _tile(1, 9, false), new _tile(1, 10, false), new _tile(1, 11, false), new _tile(1, 12, false), new _tile(1, 13, false), new _tile(1, 14, false), new _tile(1, 15, false), new _tile(1, 16, false), new _tile(1, 17, false), new _tile(1, 18, false), new _tile(1, 19, false), new _tile(1, 20, false), new _tile(1, 21, false), new _tile(1, 22, false), new _tile(1, 23, false), new _tile(1, 24, false), new _tile(1, 25, false), new _tile(1, 26, false), new _tile(1, 27, false)},
{new _tile(2, 0, false), new _tile(2, 1, false), new _tile(2, 2, false), new _tile(2, 3, false), new _tile(2, 4, false), new _tile(2, 5, false), new _tile(2, 6, false), new _tile(2, 7, false), new _tile(2, 8, false), new _tile(2, 9, false), new _tile(2, 10, false), new _tile(2, 11, false), new _tile(2, 12, false), new _tile(2, 13, false), new _tile(2, 14, false), new _tile(2, 15, false), new _tile(2, 16, false), new _tile(2, 17, false), new _tile(2, 18, false), new _tile(2, 19, false), new _tile(2, 20, false), new _tile(2, 21, false), new _tile(2, 22, false), new _tile(2, 23, false), new _tile(2, 24, false), new _tile(2, 25, false), new _tile(2, 26, false), new _tile(2, 27, false)},
{new _tile(3, 0, false), new _tile(3, 1, false), new _tile(3, 2, false), new _tile(3, 3, false), new _tile(3, 4, false), new _tile(3, 5, false), new _tile(3, 6, false), new _tile(3, 7, false), new _tile(3, 8, false), new _tile(3, 9, false), new _tile(3, 10, false), new _tile(3, 11, false), new _tile(3, 12, false), new _tile(3, 13, false), new _tile(3, 14, false), new _tile(3, 15, false), new _tile(3, 16, false), new _tile(3, 17, false), new _tile(3, 18, false), new _tile(3, 19, false), new _tile(3, 20, false), new _tile(3, 21, false), new _tile(3, 22, false), new _tile(3, 23, false), new _tile(3, 24, false), new _tile(3, 25, false), new _tile(3, 26, false), new _tile(3, 27, false)},
{new _tile(4, 0, false), new _tile(4, 1, true), new _tile(4, 2, true), new _tile(4, 3, true), new _tile(4, 4, true), new _tile(4, 5, true), new _tile(4, 6, true), new _tile(4, 7, true), new _tile(4, 8, true), new _tile(4, 9, true), new _tile(4, 10, true), new _tile(4, 11, true), new _tile(4, 12, true), new _tile(4, 13, false), new _tile(4, 14, false), new _tile(4, 15, true), new _tile(4, 16, true), new _tile(4, 17, true), new _tile(4, 18, true), new _tile(4, 19, true), new _tile(4, 20, true), new _tile(4, 21, true), new _tile(4, 22, true), new _tile(4, 23, true), new _tile(4, 24, true), new _tile(4, 25, true), new _tile(4, 26, true), new _tile(4, 27, false)},
{new _tile(5, 0, false), new _tile(5, 1, true), new _tile(5, 2, false), new _tile(5, 3, false), new _tile(5, 4, false), new _tile(5, 5, false), new _tile(5, 6, true), new _tile(5, 7, false), new _tile(5, 8, false), new _tile(5, 9, false), new _tile(5, 10, false), new _tile(5, 11, false), new _tile(5, 12, true), new _tile(5, 13, false), new _tile(5, 14, false), new _tile(5, 15, true), new _tile(5, 16, false), new _tile(5, 17, false), new _tile(5, 18, false), new _tile(5, 19, false), new _tile(5, 20, false), new _tile(5, 21, true), new _tile(5, 22, false), new _tile(5, 23, false), new _tile(5, 24, false), new _tile(5, 25, false), new _tile(5, 26, true), new _tile(5, 27, false)},
{new _tile(6, 0, false), new _tile(6, 1, true), new _tile(6, 2, false), new _tile(6, 3, false), new _tile(6, 4, false), new _tile(6, 5, false), new _tile(6, 6, true), new _tile(6, 7, false), new _tile(6, 8, false), new _tile(6, 9, false), new _tile(6, 10, false), new _tile(6, 11, false), new _tile(6, 12, true), new _tile(6, 13, false), new _tile(6, 14, false), new _tile(6, 15, true), new _tile(6, 16, false), new _tile(6, 17, false), new _tile(6, 18, false), new _tile(6, 19, false), new _tile(6, 20, false), new _tile(6, 21, true), new _tile(6, 22, false), new _tile(6, 23, false), new _tile(6, 24, false), new _tile(6, 25, false), new _tile(6, 26, true), new _tile(6, 27, false)},
{new _tile(7, 0, false), new _tile(7, 1, true), new _tile(7, 2, false), new _tile(7, 3, false), new _tile(7, 4, false), new _tile(7, 5, false), new _tile(7, 6, true), new _tile(7, 7, false), new _tile(7, 8, false), new _tile(7, 9, false), new _tile(7, 10, false), new _tile(7, 11, false), new _tile(7, 12, true), new _tile(7, 13, false), new _tile(7, 14, false), new _tile(7, 15, true), new _tile(7, 16, false), new _tile(7, 17, false), new _tile(7, 18, false), new _tile(7, 19, false), new _tile(7, 20, false), new _tile(7, 21, true), new _tile(7, 22, false), new _tile(7, 23, false), new _tile(7, 24, false), new _tile(7, 25, false), new _tile(7, 26, true), new _tile(7, 27, false)},
{new _tile(8, 0, false), new _tile(8, 1, true), new _tile(8, 2, true), new _tile(8, 3, true), new _tile(8, 4, true), new _tile(8, 5, true), new _tile(8, 6, true), new _tile(8, 7, true), new _tile(8, 8, true), new _tile(8, 9, true), new _tile(8, 10, true), new _tile(8, 11, true), new _tile(8, 12, true), new _tile(8, 13, true), new _tile(8, 14, true), new _tile(8, 15, true), new _tile(8, 16, true), new _tile(8, 17, true), new _tile(8, 18, true), new _tile(8, 19, true), new _tile(8, 20, true), new _tile(8, 21, true), new _tile(8, 22, true), new _tile(8, 23, true), new _tile(8, 24, true), new _tile(8, 25, true), new _tile(8, 26, true), new _tile(8, 27, false)},
{new _tile(9, 0, false), new _tile(9, 1, true), new _tile(9, 2, false), new _tile(9, 3, false), new _tile(9, 4, false), new _tile(9, 5, false), new _tile(9, 6, true), new _tile(9, 7, false), new _tile(9, 8, false), new _tile(9, 9, true), new _tile(9, 10, false), new _tile(9, 11, false), new _tile(9, 12, false), new _tile(9, 13, false), new _tile(9, 14, false), new _tile(9, 15, false), new _tile(9, 16, false), new _tile(9, 17, false), new _tile(9, 18, true), new _tile(9, 19, false), new _tile(9, 20, false), new _tile(9, 21, true), new _tile(9, 22, false), new _tile(9, 23, false), new _tile(9, 24, false), new _tile(9, 25, false), new _tile(9, 26, true), new _tile(9, 27, false)},
{new _tile(10, 0, false), new _tile(10, 1, true), new _tile(10, 2, false), new _tile(10, 3, false), new _tile(10, 4, false), new _tile(10, 5, false), new _tile(10, 6, true), new _tile(10, 7, false), new _tile(10, 8, false), new _tile(10, 9, true), new _tile(10, 10, false), new _tile(10, 11, false), new _tile(10, 12, false), new _tile(10, 13, false), new _tile(10, 14, false), new _tile(10, 15, false), new _tile(10, 16, false), new _tile(10, 17, false), new _tile(10, 18, true), new _tile(10, 19, false), new _tile(10, 20, false), new _tile(10, 21, true), new _tile(10, 22, false), new _tile(10, 23, false), new _tile(10, 24, false), new _tile(10, 25, false), new _tile(10, 26, true), new _tile(10, 27, false)},
{new _tile(11, 0, false), new _tile(11, 1, true), new _tile(11, 2, true), new _tile(11, 3, true), new _tile(11, 4, true), new _tile(11, 5, true), new _tile(11, 6, true), new _tile(11, 7, false), new _tile(11, 8, false), new _tile(11, 9, true), new _tile(11, 10, true), new _tile(11, 11, true), new _tile(11, 12, true), new _tile(11, 13, false), new _tile(11, 14, false), new _tile(11, 15, true), new _tile(11, 16, true), new _tile(11, 17, true), new _tile(11, 18, true), new _tile(11, 19, false), new _tile(11, 20, false), new _tile(11, 21, true), new _tile(11, 22, true), new _tile(11, 23, true), new _tile(11, 24, true), new _tile(11, 25, true), new _tile(11, 26, true), new _tile(11, 27, false)},
{new _tile(12, 0, false), new _tile(12, 1, false), new _tile(12, 2, false), new _tile(12, 3, false), new _tile(12, 4, false), new _tile(12, 5, false), new _tile(12, 6, true), new _tile(12, 7, false), new _tile(12, 8, false), new _tile(12, 9, false), new _tile(12, 10, false), new _tile(12, 11, false), new _tile(12, 12, true), new _tile(12, 13, false), new _tile(12, 14, false), new _tile(12, 15, true), new _tile(12, 16, false), new _tile(12, 17, false), new _tile(12, 18, false), new _tile(12, 19, false), new _tile(12, 20, false), new _tile(12, 21, true), new _tile(12, 22, false), new _tile(12, 23, false), new _tile(12, 24, false), new _tile(12, 25, false), new _tile(12, 26, false), new _tile(12, 27, false)},
{new _tile(13, 0, false), new _tile(13, 1, false), new _tile(13, 2, false), new _tile(13, 3, false), new _tile(13, 4, false), new _tile(13, 5, false), new _tile(13, 6, true), new _tile(13, 7, false), new _tile(13, 8, false), new _tile(13, 9, false), new _tile(13, 10, false), new _tile(13, 11, false), new _tile(13, 12, true), new _tile(13, 13, false), new _tile(13, 14, false), new _tile(13, 15, true), new _tile(13, 16, false), new _tile(13, 17, false), new _tile(13, 18, false), new _tile(13, 19, false), new _tile(13, 20, false), new _tile(13, 21, true), new _tile(13, 22, false), new _tile(13, 23, false), new _tile(13, 24, false), new _tile(13, 25, false), new _tile(13, 26, false), new _tile(13, 27, false)},
{new _tile(14, 0, false), new _tile(14, 1, false), new _tile(14, 2, false), new _tile(14, 3, false), new _tile(14, 4, false), new _tile(14, 5, false), new _tile(14, 6, true), new _tile(14, 7, false), new _tile(14, 8, false), new _tile(14, 9, true), new _tile(14, 10, true), new _tile(14, 11, true), new _tile(14, 12, true), new _tile(14, 13, true), new _tile(14, 14, true), new _tile(14, 15, true), new _tile(14, 16, true), new _tile(14, 17, true), new _tile(14, 18, true), new _tile(14, 19, false), new _tile(14, 20, false), new _tile(14, 21, true), new _tile(14, 22, false), new _tile(14, 23, false), new _tile(14, 24, false), new _tile(14, 25, false), new _tile(14, 26, false), new _tile(14, 27, false)},
{new _tile(15, 0, false), new _tile(15, 1, false), new _tile(15, 2, false), new _tile(15, 3, false), new _tile(15, 4, false), new _tile(15, 5, false), new _tile(15, 6, true), new _tile(15, 7, false), new _tile(15, 8, false), new _tile(15, 9, true), new _tile(15, 10, false), new _tile(15, 11, false), new _tile(15, 12, false), new _tile(15, 13, false), new _tile(15, 14, false), new _tile(15, 15, false), new _tile(15, 16, false), new _tile(15, 17, false), new _tile(15, 18, true), new _tile(15, 19, false), new _tile(15, 20, false), new _tile(15, 21, true), new _tile(15, 22, false), new _tile(15, 23, false), new _tile(15, 24, false), new _tile(15, 25, false), new _tile(15, 26, false), new _tile(15, 27, false)},
{new _tile(16, 0, false), new _tile(16, 1, false), new _tile(16, 2, false), new _tile(16, 3, false), new _tile(16, 4, false), new _tile(16, 5, false), new _tile(16, 6, true), new _tile(16, 7, false), new _tile(16, 8, false), new _tile(16, 9, true), new _tile(16, 10, false), new _tile(16, 11, false), new _tile(16, 12, false), new _tile(16, 13, false), new _tile(16, 14, false), new _tile(16, 15, false), new _tile(16, 16, false), new _tile(16, 17, false), new _tile(16, 18, true), new _tile(16, 19, false), new _tile(16, 20, false), new _tile(16, 21, true), new _tile(16, 22, false), new _tile(16, 23, false), new _tile(16, 24, false), new _tile(16, 25, false), new _tile(16, 26, false), new _tile(16, 27, false)},
{new _tile(17, 0, true), new _tile(17, 1, true), new _tile(17, 2, true), new _tile(17, 3, true), new _tile(17, 4, true), new _tile(17, 5, true), new _tile(17, 6, true), new _tile(17, 7, true), new _tile(17, 8, true), new _tile(17, 9, true), new _tile(17, 10, false), new _tile(17, 11, false), new _tile(17, 12, false), new _tile(17, 13, false), new _tile(17, 14, false), new _tile(17, 15, false), new _tile(17, 16, false), new _tile(17, 17, false), new _tile(17, 18, true), new _tile(17, 19, true), new _tile(17, 20, true), new _tile(17, 21, true), new _tile(17, 22, true), new _tile(17, 23, true), new _tile(17, 24, true), new _tile(17, 25, true), new _tile(17, 26, true), new _tile(17, 27, true)},
{new _tile(18, 0, false), new _tile(18, 1, false), new _tile(18, 2, false), new _tile(18, 3, false), new _tile(18, 4, false), new _tile(18, 5, false), new _tile(18, 6, true), new _tile(18, 7, false), new _tile(18, 8, false), new _tile(18, 9, true), new _tile(18, 10, false), new _tile(18, 11, false), new _tile(18, 12, false), new _tile(18, 13, false), new _tile(18, 14, false), new _tile(18, 15, false), new _tile(18, 16, false), new _tile(18, 17, false), new _tile(18, 18, true), new _tile(18, 19, false), new _tile(18, 20, false), new _tile(18, 21, true), new _tile(18, 22, false), new _tile(18, 23, false), new _tile(18, 24, false), new _tile(18, 25, false), new _tile(18, 26, false), new _tile(18, 27, false)},
{new _tile(19, 0, false), new _tile(19, 1, false), new _tile(19, 2, false), new _tile(19, 3, false), new _tile(19, 4, false), new _tile(19, 5, false), new _tile(19, 6, true), new _tile(19, 7, false), new _tile(19, 8, false), new _tile(19, 9, true), new _tile(19, 10, false), new _tile(19, 11, false), new _tile(19, 12, false), new _tile(19, 13, false), new _tile(19, 14, false), new _tile(19, 15, false), new _tile(19, 16, false), new _tile(19, 17, false), new _tile(19, 18, true), new _tile(19, 19, false), new _tile(19, 20, false), new _tile(19, 21, true), new _tile(19, 22, false), new _tile(19, 23, false), new _tile(19, 24, false), new _tile(19, 25, false), new _tile(19, 26, false), new _tile(19, 27, false)},
{new _tile(20, 0, false), new _tile(20, 1, false), new _tile(20, 2, false), new _tile(20, 3, false), new _tile(20, 4, false), new _tile(20, 5, false), new _tile(20, 6, true), new _tile(20, 7, false), new _tile(20, 8, false), new _tile(20, 9, true), new _tile(20, 10, true), new _tile(20, 11, true), new _tile(20, 12, true), new _tile(20, 13, true), new _tile(20, 14, true), new _tile(20, 15, true), new _tile(20, 16, true), new _tile(20, 17, true), new _tile(20, 18, true), new _tile(20, 19, false), new _tile(20, 20, false), new _tile(20, 21, true), new _tile(20, 22, false), new _tile(20, 23, false), new _tile(20, 24, false), new _tile(20, 25, false), new _tile(20, 26, false), new _tile(20, 27, false)},
{new _tile(21, 0, false), new _tile(21, 1, false), new _tile(21, 2, false), new _tile(21, 3, false), new _tile(21, 4, false), new _tile(21, 5, false), new _tile(21, 6, true), new _tile(21, 7, false), new _tile(21, 8, false), new _tile(21, 9, true), new _tile(21, 10, false), new _tile(21, 11, false), new _tile(21, 12, false), new _tile(21, 13, false), new _tile(21, 14, false), new _tile(21, 15, false), new _tile(21, 16, false), new _tile(21, 17, false), new _tile(21, 18, true), new _tile(21, 19, false), new _tile(21, 20, false), new _tile(21, 21, true), new _tile(21, 22, false), new _tile(21, 23, false), new _tile(21, 24, false), new _tile(21, 25, false), new _tile(21, 26, false), new _tile(21, 27, false)},
{new _tile(22, 0, false), new _tile(22, 1, false), new _tile(22, 2, false), new _tile(22, 3, false), new _tile(22, 4, false), new _tile(22, 5, false), new _tile(22, 6, true), new _tile(22, 7, false), new _tile(22, 8, false), new _tile(22, 9, true), new _tile(22, 10, false), new _tile(22, 11, false), new _tile(22, 12, false), new _tile(22, 13, false), new _tile(22, 14, false), new _tile(22, 15, false), new _tile(22, 16, false), new _tile(22, 17, false), new _tile(22, 18, true), new _tile(22, 19, false), new _tile(22, 20, false), new _tile(22, 21, true), new _tile(22, 22, false), new _tile(22, 23, false), new _tile(22, 24, false), new _tile(22, 25, false), new _tile(22, 26, false), new _tile(22, 27, false)},
{new _tile(23, 0, false), new _tile(23, 1, true), new _tile(23, 2, true), new _tile(23, 3, true), new _tile(23, 4, true), new _tile(23, 5, true), new _tile(23, 6, true), new _tile(23, 7, true), new _tile(23, 8, true), new _tile(23, 9, true), new _tile(23, 10, true), new _tile(23, 11, true), new _tile(23, 12, true), new _tile(23, 13, false), new _tile(23, 14, false), new _tile(23, 15, true), new _tile(23, 16, true), new _tile(23, 17, true), new _tile(23, 18, true), new _tile(23, 19, true), new _tile(23, 20, true), new _tile(23, 21, true), new _tile(23, 22, true), new _tile(23, 23, true), new _tile(23, 24, true), new _tile(23, 25, true), new _tile(23, 26, true), new _tile(23, 27, false)},
{new _tile(24, 0, false), new _tile(24, 1, true), new _tile(24, 2, false), new _tile(24, 3, false), new _tile(24, 4, false), new _tile(24, 5, false), new _tile(24, 6, true), new _tile(24, 7, false), new _tile(24, 8, false), new _tile(24, 9, false), new _tile(24, 10, false), new _tile(24, 11, false), new _tile(24, 12, true), new _tile(24, 13, false), new _tile(24, 14, false), new _tile(24, 15, true), new _tile(24, 16, false), new _tile(24, 17, false), new _tile(24, 18, false), new _tile(24, 19, false), new _tile(24, 20, false), new _tile(24, 21, true), new _tile(24, 22, false), new _tile(24, 23, false), new _tile(24, 24, false), new _tile(24, 25, false), new _tile(24, 26, true), new _tile(24, 27, false)},
{new _tile(25, 0, false), new _tile(25, 1, true), new _tile(25, 2, false), new _tile(25, 3, false), new _tile(25, 4, false), new _tile(25, 5, false), new _tile(25, 6, true), new _tile(25, 7, false), new _tile(25, 8, false), new _tile(25, 9, false), new _tile(25, 10, false), new _tile(25, 11, false), new _tile(25, 12, true), new _tile(25, 13, false), new _tile(25, 14, false), new _tile(25, 15, true), new _tile(25, 16, false), new _tile(25, 17, false), new _tile(25, 18, false), new _tile(25, 19, false), new _tile(25, 20, false), new _tile(25, 21, true), new _tile(25, 22, false), new _tile(25, 23, false), new _tile(25, 24, false), new _tile(25, 25, false), new _tile(25, 26, true), new _tile(25, 27, false)},
{new _tile(26, 0, false), new _tile(26, 1, true), new _tile(26, 2, true), new _tile(26, 3, true), new _tile(26, 4, false), new _tile(26, 5, false), new _tile(26, 6, true), new _tile(26, 7, true), new _tile(26, 8, true), new _tile(26, 9, true), new _tile(26, 10, true), new _tile(26, 11, true), new _tile(26, 12, true), new _tile(26, 13, true), new _tile(26, 14, true), new _tile(26, 15, true), new _tile(26, 16, true), new _tile(26, 17, true), new _tile(26, 18, true), new _tile(26, 19, true), new _tile(26, 20, true), new _tile(26, 21, true), new _tile(26, 22, false), new _tile(26, 23, false), new _tile(26, 24, true), new _tile(26, 25, true), new _tile(26, 26, true), new _tile(26, 27, false)},
{new _tile(27, 0, false), new _tile(27, 1, false), new _tile(27, 2, false), new _tile(27, 3, true), new _tile(27, 4, false), new _tile(27, 5, false), new _tile(27, 6, true), new _tile(27, 7, false), new _tile(27, 8, false), new _tile(27, 9, true), new _tile(27, 10, false), new _tile(27, 11, false), new _tile(27, 12, false), new _tile(27, 13, false), new _tile(27, 14, false), new _tile(27, 15, false), new _tile(27, 16, false), new _tile(27, 17, false), new _tile(27, 18, true), new _tile(27, 19, false), new _tile(27, 20, false), new _tile(27, 21, true), new _tile(27, 22, false), new _tile(27, 23, false), new _tile(27, 24, true), new _tile(27, 25, false), new _tile(27, 26, false), new _tile(27, 27, false)},
{new _tile(28, 0, false), new _tile(28, 1, false), new _tile(28, 2, false), new _tile(28, 3, true), new _tile(28, 4, false), new _tile(28, 5, false), new _tile(28, 6, true), new _tile(28, 7, false), new _tile(28, 8, false), new _tile(28, 9, true), new _tile(28, 10, false), new _tile(28, 11, false), new _tile(28, 12, false), new _tile(28, 13, false), new _tile(28, 14, false), new _tile(28, 15, false), new _tile(28, 16, false), new _tile(28, 17, false), new _tile(28, 18, true), new _tile(28, 19, false), new _tile(28, 20, false), new _tile(28, 21, true), new _tile(28, 22, false), new _tile(28, 23, false), new _tile(28, 24, true), new _tile(28, 25, false), new _tile(28, 26, false), new _tile(28, 27, false)},
{new _tile(29, 0, false), new _tile(29, 1, true), new _tile(29, 2, true), new _tile(29, 3, true), new _tile(29, 4, true), new _tile(29, 5, true), new _tile(29, 6, true), new _tile(29, 7, false), new _tile(29, 8, false), new _tile(29, 9, true), new _tile(29, 10, true), new _tile(29, 11, true), new _tile(29, 12, true), new _tile(29, 13, false), new _tile(29, 14, false), new _tile(29, 15, true), new _tile(29, 16, true), new _tile(29, 17, true), new _tile(29, 18, true), new _tile(29, 19, false), new _tile(29, 20, false), new _tile(29, 21, true), new _tile(29, 22, true), new _tile(29, 23, true), new _tile(29, 24, true), new _tile(29, 25, true), new _tile(29, 26, true), new _tile(29, 27, false)},
{new _tile(30, 0, false), new _tile(30, 1, true), new _tile(30, 2, false), new _tile(30, 3, false), new _tile(30, 4, false), new _tile(30, 5, false), new _tile(30, 6, false), new _tile(30, 7, false), new _tile(30, 8, false), new _tile(30, 9, false), new _tile(30, 10, false), new _tile(30, 11, false), new _tile(30, 12, true), new _tile(30, 13, false), new _tile(30, 14, false), new _tile(30, 15, true), new _tile(30, 16, false), new _tile(30, 17, false), new _tile(30, 18, false), new _tile(30, 19, false), new _tile(30, 20, false), new _tile(30, 21, false), new _tile(30, 22, false), new _tile(30, 23, false), new _tile(30, 24, false), new _tile(30, 25, false), new _tile(30, 26, true), new _tile(30, 27, false)},
{new _tile(31, 0, false), new _tile(31, 1, true), new _tile(31, 2, false), new _tile(31, 3, false), new _tile(31, 4, false), new _tile(31, 5, false), new _tile(31, 6, false), new _tile(31, 7, false), new _tile(31, 8, false), new _tile(31, 9, false), new _tile(31, 10, false), new _tile(31, 11, false), new _tile(31, 12, true), new _tile(31, 13, false), new _tile(31, 14, false), new _tile(31, 15, true), new _tile(31, 16, false), new _tile(31, 17, false), new _tile(31, 18, false), new _tile(31, 19, false), new _tile(31, 20, false), new _tile(31, 21, false), new _tile(31, 22, false), new _tile(31, 23, false), new _tile(31, 24, false), new _tile(31, 25, false), new _tile(31, 26, true), new _tile(31, 27, false)},
{new _tile(32, 0, false), new _tile(32, 1, true), new _tile(32, 2, true), new _tile(32, 3, true), new _tile(32, 4, true), new _tile(32, 5, true), new _tile(32, 6, true), new _tile(32, 7, true), new _tile(32, 8, true), new _tile(32, 9, true), new _tile(32, 10, true), new _tile(32, 11, true), new _tile(32, 12, true), new _tile(32, 13, true), new _tile(32, 14, true), new _tile(32, 15, true), new _tile(32, 16, true), new _tile(32, 17, true), new _tile(32, 18, true), new _tile(32, 19, true), new _tile(32, 20, true), new _tile(32, 21, true), new _tile(32, 22, true), new _tile(32, 23, true), new _tile(32, 24, true), new _tile(32, 25, true), new _tile(32, 26, true), new _tile(32, 27, false)},
{new _tile(33, 0, false), new _tile(33, 1, false), new _tile(33, 2, false), new _tile(33, 3, false), new _tile(33, 4, false), new _tile(33, 5, false), new _tile(33, 6, false), new _tile(33, 7, false), new _tile(33, 8, false), new _tile(33, 9, false), new _tile(33, 10, false), new _tile(33, 11, false), new _tile(33, 12, false), new _tile(33, 13, false), new _tile(33, 14, false), new _tile(33, 15, false), new _tile(33, 16, false), new _tile(33, 17, false), new _tile(33, 18, false), new _tile(33, 19, false), new _tile(33, 20, false), new _tile(33, 21, false), new _tile(33, 22, false), new _tile(33, 23, false), new _tile(33, 24, false), new _tile(33, 25, false), new _tile(33, 26, false), new _tile(33, 27, false)},
{new _tile(34, 0, false), new _tile(34, 1, false), new _tile(34, 2, false), new _tile(34, 3, false), new _tile(34, 4, false), new _tile(34, 5, false), new _tile(34, 6, false), new _tile(34, 7, false), new _tile(34, 8, false), new _tile(34, 9, false), new _tile(34, 10, false), new _tile(34, 11, false), new _tile(34, 12, false), new _tile(34, 13, false), new _tile(34, 14, false), new _tile(34, 15, false), new _tile(34, 16, false), new _tile(34, 17, false), new _tile(34, 18, false), new _tile(34, 19, false), new _tile(34, 20, false), new _tile(34, 21, false), new _tile(34, 22, false), new _tile(34, 23, false), new _tile(34, 24, false), new _tile(34, 25, false), new _tile(34, 26, false), new _tile(34, 27, false)},
{new _tile(35, 0, false), new _tile(35, 1, false), new _tile(35, 2, false), new _tile(35, 3, false), new _tile(35, 4, false), new _tile(35, 5, false), new _tile(35, 6, false), new _tile(35, 7, false), new _tile(35, 8, false), new _tile(35, 9, false), new _tile(35, 10, false), new _tile(35, 11, false), new _tile(35, 12, false), new _tile(35, 13, false), new _tile(35, 14, false), new _tile(35, 15, false), new _tile(35, 16, false), new _tile(35, 17, false), new _tile(35, 18, false), new _tile(35, 19, false), new _tile(35, 20, false), new _tile(35, 21, false), new _tile(35, 22, false), new _tile(35, 23, false), new _tile(35, 24, false), new _tile(35, 25, false), new _tile(35, 26, false), new _tile(35, 27, false) }
};
    #endregion
    public int dirtyTiles = 0;
    List<Vector2> passableTiles = new List<Vector2>();
    protected override void Start() {
        base.Start();
        
        for (int i = 0; i < tileMap.GetLength(0); i++) {
            for (int j = 0; j < tileMap.GetLength(1); j++) {
                if (tileMap[i, j].passable) {
                    passableTiles.Add(new Vector2(i, j));
                }
            }
        }
    }

    // confirming the tiles are depleting.
    int PhysicalDirtyTiles()
    {
        int dirty = 0;
        for (int i = 0; i < tileMap.GetLength(0); i++) {
            for (int j = 0; j < tileMap.GetLength(1); j++) {
                if (tileMap[i, j].dirty) {
                    dirty++;
                }
            }
        }
        return dirty;
    }
    // using this heuristic instead of just dirty tiles way to inefficient 
    static int heuristic(int d, int w) {
        // d dist to closest dirty node 
        // w number of dirty nodes
        return d * (w * 2 + 1) + w * w - 1;
    }

    // is not being called nor is the SetNextTile function atleast to my knowledge
    // this is because that check is failing. It only fails with this script not with 
    // character by itself. 
    protected override direction GetDefaultDirection(direction d) {
       
        switch (where) {
            case direction.DOWN:
                nextTile = tile.down;
                break;
            case direction.LEFT:
                nextTile = tile.left;
                break;
            case direction.RIGHT:
                nextTile = tile.right;
                break;
            default:
                nextTile = tile.up;
                break;
        }
        tileMap[nextTile.i, nextTile.j].dirty = false;
        return where;
    }
    protected override void ReachTile()
    {
        AstarSearch(new node(100000000, 0, tileMap, nextTile.i, nextTile.j));
        base.ReachTile();
    }


    // copied strait from another pathfinding program i have
    // tryig to get this to work with the current situation 
    // it is unfinished and untested. 
    protected void AstarSearch(node start) {
        dirtyTiles = PhysicalDirtyTiles();

        string[] actions = new string[] { "Right", "Up", "Left", "Down", "Suck" };
        int countPath = 0;
        List<node> openSet = new List<node>();
        List<node> closedSet = new List<node>();
        openSet.Add(start);

        openSet[0].FScore = heuristic(start.distToDirty(), start.getDirty().Count);
        openSet[0].GScore = 0;
        node curNode = openSet[0];

        while (openSet.Count != 0) {
            curNode = openSet[0];
            if (countPath > 50) {
                break;
            }
            if (curNode.getDirty().Count == 0) { 
                break;
            }

            openSet.Remove(curNode);
            closedSet.Add(curNode);

            foreach (string a in actions) {
                curNode.neighbors.Add(curNode.affect(a));
            }
            
            foreach (node neighbor in curNode.neighbors) {
                countPath++;
                // skips over all nodes that are not passable so only left with passible nodes
                if (!tileMap[neighbor.agentPos[0], neighbor.agentPos[1]].passable) {
                    continue;
                }
                
                if (closedSet.Contains(neighbor))
                    continue;

                // Right here the the scores are being assigned to each node Im not sure whats 
                // screwing with the tenative_gScore but i am fairly annoyed. It could be something 
                // entirely indirectly releated and out of left field. 
                int tenative_gScore = curNode.GScore + curNode.getActionCost(neighbor.direction);

                Debug.Log("agent at [" + neighbor.agentPos[0] + ", " + neighbor.agentPos[1]
                                       + "]: tenGs =" + tenative_gScore + ", nGs = " + neighbor.GScore
                                       + ", dir = " + neighbor.direction);

                if (!openSet.Contains(neighbor)) {
                    openSet.Add(neighbor);
                }
                else if (tenative_gScore >= neighbor.GScore) {
                    // for some reason this code is never called 
                    continue;
                }

                neighbor.cameFrom = curNode;
                neighbor.GScore = tenative_gScore;
                neighbor.FScore = neighbor.GScore + heuristic(start.distToDirty(), start.getDirty().Count); 
            } 
            openSet = openSet.OrderBy(o => o.FScore).ToList();
        } 
        
        reconstructPath(curNode);
    }


    // Basic function:      returns the first node on the path...
    // this is not the final encarnation to this reconstruct function. the final will 
    // go through the string "dirs" adding each nodes action the only action that is to 
    // be returned is the final action which will be the first character in the string 
    protected direction reconstructPath(node curNode) {
        string dirs = "";
        if (curNode.cameFrom != null) { 
            while (curNode.cameFrom.cameFrom != null) {
                curNode = curNode.cameFrom;

                // this filters out the suck nodes while adding 
                // up the nodes to the final path
                if (curNode.direction != 's')
                    dirs += curNode.direction;
            }
        }
        // showing the full path of directions. 
        Debug.Log("dirs = " + dirs);

        // is how you apply the actual direction 
        switch (curNode.direction){
            case 'u':
                Debug.Log("Up");
                where = direction.UP;
                break;
            case 'd':
                Debug.Log("Down");
                where = direction.DOWN;
                break;
            case 'l':
                Debug.Log("Left");
                where = direction.LEFT;
                break;
            case 'r':
                Debug.Log("Right");
                where = direction.RIGHT;
                break;
            case 's':
                Debug.LogWarning("It tried to suck Shouldn't do that");
                break;
        }
        return where;
    }
}

// Creates an image of the tiles in imaginary space this will only count the passible tiles 
// otherwise we would have to create these tiles over and over and would be taxing if they where 
// all monobehavior this also helps to simplify and make it more like the program for homework
[System.Serializable]
public class _tile {
    public bool dirty = false;
    public int[] location = new int[] { 0, 0 };
    public bool passable = false;

    // this initializer is only used to create the initial dirty tiles in the scene
    public _tile(int x, int y, bool dirty) {
        location[0] = x;
        location[1] = y;
        this.dirty = dirty;
        passable = dirty;
    }
    // anywhere else you make a _tile you use this initilization method here
    public _tile(int x, int y, bool dirty, bool passible) {
        location[0] = x;
        location[1] = y;
        this.dirty = dirty;
        passable = passible;
    }
     
    // I think something is wrong with copying either here or 
    public _tile Copy() {
        return new _tile(location[0], location[1], dirty, passable);
    }
}

// I have more or less modeled this class off of the original code. With some additions and changes
// to fit the program... Well i tried to at least. I believe this contains the main source of the problem
public class node {
    public List<node> neighbors = new List<node>();
    public Vector3 pos;
    public int[] agentPos = new int[] { 1, 1 };
    public node cameFrom;
    public _tile[,] world = new _tile[36, 28];
    public char direction; // u d l r s
    public int mySerial = 1;
    public int curItems;
    
    public int GScore = int.MaxValue,
               FScore = int.MaxValue;

    public node(int FScore, int GScore, _tile[,] world, int i, int j) {
        this.FScore = FScore;
        this.GScore = GScore;
        this.world = world;
        agentPos[0] = i;
        agentPos[1] = j;
    }
    public string ToStr() {
        string s = "Node " + mySerial + ": (agent at " + agentPos[0] + ", " + agentPos[1] + ")\n";

        return s;
    }
    public node affect(string action)
    {
        // this is where i create a new world supposidly this is where i create new worlds.
        // and not just point to old worlds... I had problems with this in the vaccume program.
        // i dont see how this could be a problem here i have created the exact same set up 
        _tile[,] newWorld = new _tile[36, 28];
        for (int i = 0; i < newWorld.GetLength(0); i++) {
            for (int j = 0; j < newWorld.GetLength(1); j++) {
                newWorld[i, j] = world[i, j].Copy();
            }
        }
        
        int[] pos = new int[] { agentPos[0], agentPos[1] };

        // this actually assigns the values for the direction of the node. this time 
        // i used characters instead of a string to change up down left right and suck
        switch (action) {
            case ("Right"):
                pos = new int[] { agentPos[0], Math.Min(27, agentPos[1] + 1) };
                direction = 'r';
                break;
            case ("Left"):
                pos = new int[] { agentPos[0], Math.Max(0, agentPos[1] - 1) };
                direction = 'l';
                break;
            case ("Up"):
                pos = new int[] { Math.Max(0, agentPos[0] - 1), agentPos[1] };
                direction = 'u';
                break;
            case ("Down"):
                pos = new int[] { Math.Min(35, agentPos[0] + 1), agentPos[1] };
                direction = 'd';
                break;
            case ("Suck"):
                newWorld[agentPos[0], agentPos[1]].dirty = false;
                direction = 's';
                break;
        }

        // and here is where the actual node is created... bask in its probably broken glory
        var newNode = new node(int.MaxValue, int.MaxValue, newWorld, pos[0], pos[1]);
        newNode.direction = direction;

        return newNode;
    }

    // This is one of the deciding factors to the tenGscore
    public int getActionCost(char action)
    {
        if (action == '\0') return 0;
        if (action == 's' && world[agentPos[0], agentPos[1]].dirty)
        {
            return 1 + 2 * (getDirty().Count - 1);
        }
        return 1 + 2 * getDirty().Count;
        // check
    }
    public List<_tile> getDirty()
    {
        var dirtyTiles = new List<_tile>();
        for (int i = 0; i < world.GetLength(0); i++)
            for (int j = 0; j < world.GetLength(1); j++)
                if (world[i, j].dirty)
                    dirtyTiles.Add(world[i, j]);

        return dirtyTiles;
        // check
    }
    // using this again for distance to dirty tiles
    public int distToDirty()
    {
        var dirtyTiles = getDirty();
        if (dirtyTiles.Count == 0)
            return 0;
        int dist = int.MaxValue;

        foreach (var n in dirtyTiles)
        {
            int newDist = Math.Abs(world[agentPos[0], agentPos[1]].location[0] - n.location[0]) +
                          Math.Abs(world[agentPos[0], agentPos[1]].location[1] - n.location[1]);
            if (newDist < dist)
                dist = newDist;
        }
        return dist;
        // check
    }
}