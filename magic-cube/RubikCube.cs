﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Diagnostics;

namespace magic_cube {
    public class RubikCube : Cube {
        /// <summary>
        /// The cube will be size x size x size
        /// </summary>
        private int size;

        private Point3D origin;

        /// <summary>
        /// Length of the cube edge
        /// </summary>
        private double edge_len;

        /// <summary>
        /// Space between the cubes forming the bigger cube
        /// </summary>
        private double space;
        
        public RubikCube(int size, Point3D o, double len = 1, double space = 0.1) {
            this.size = size;
            this.origin = o;
            this.edge_len = len;
            this.space = space;

            //TODO: create the cube out of faces?

            createCube();
        }

        //TODO: roteste si geometria, nu doar modelul
        //TODO: la fiecare Cube, in functie de pozitie, ii atribui un grup de rotatii

        //TODO: marcheaza fetele cubului in loc de dreptunghiuri invizibile si roteste-le pe alea
        protected override void createCube() {
            Cube c;
            Dictionary<CubeFace, Material> colors;

            double x_offset, y_offset, z_offset;

            for (int y = 0; y < size; y++) {
                for (int z = 0; z < size; z++) {
                    for (int x = 0; x < size; x++) {
                         if (y == 1 && x == 1 && z == 1) {
                             continue;
                        }

                        x_offset = (edge_len + space) * x;
                        y_offset = (edge_len + space) * y;
                        z_offset = (edge_len + space) * z;

                        Point3D p = new Point3D(origin.X + x_offset, origin.Y + y_offset, origin.Z + z_offset);

                        colors = setFaceColors(x, y, z);

                        c = new Cube(p, edge_len, colors, getPossibleMoves(x, y, z));
                        this.Children.Add(c);
                    }
                }
            }
        }

        private HashSet<Move> getPossibleMoves(int x, int y, int z){
            HashSet<Move> moves =  new HashSet<Move>();

            if (y == 0) {
                moves.Add(Move.D);
            }
            else if (y == size - 1) {
                moves.Add(Move.U);
            }
            else {
                moves.Add(Move.E);
            }

            if (x == 0) {
                moves.Add(Move.L);
            }
            else if (x == size - 1) {
                moves.Add(Move.R);
            }
            else {
                moves.Add(Move.M);
            }

            if (z == 0) {
                moves.Add(Move.B);
            }
            else if (z == size - 1) {
                moves.Add(Move.F);
            }
            else {
                moves.Add(Move.S);
            }

            return moves;
        }

        public void rotate(KeyValuePair<Move, RotationDirection> move, CubeFace f) {
            HashSet<Move> possibleMoves = new HashSet<Move>();
            Vector3D axis = new Vector3D();

            int ct = 0;
            
            foreach(Cube c in this.Children){
                possibleMoves = new HashSet<Move>(c.possibleMoves);
                possibleMoves.Remove((Move)f);
                if(possibleMoves.Contains(move.Key)){
                    ct++;
                    foreach (Move m in c.possibleMoves) {
                        Debug.Write(m.ToString()+",");   
                    }
                    Debug.WriteLine("");

                    switch (move.Key) {
                        case Move.F:
                        case Move.S:
                            axis.X = 0;
                            axis.Y = 0;
                            axis.Z = -1;
                            break;
                        case Move.R:                            
                            axis.X = -1;
                            axis.Y = 0;
                            axis.Z = 0;
                            break;
                        case Move.B:
                            axis.X = 0;
                            axis.Y = 0;
                            axis.Z = 1;
                            break;
                        case Move.L:
                        case Move.M:
                            axis.X = 1;
                            axis.Y = 0;
                            axis.Z = 0;
                            break;
                        case Move.U:
                            axis.X = 0;
                            axis.Y = -1;
                            axis.Z = 0;
                            break;
                        case Move.D:
                        case Move.E:
                            axis.X = 0;
                            axis.Y = 1;
                            axis.Z = 0;
                            break;
                    }

                    c.rotations.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(axis, 90 * Convert.ToInt32(move.Value)), new Point3D(0, 0, 0)));

                    c.possibleMoves = getNextPossibleMoves(c.possibleMoves, move.Key, move.Value);
                }
            }

            Debug.Print("No. cubes selected: " + ct);

            if(move.Key == Move.S || move.Key == Move.M || move.Key == Move.E){
                Debug.Assert(ct == 8);
            }
            else{
                Debug.Assert(ct == 9);
            }
        }

        private HashSet<Move> getNextPossibleMoves(HashSet<Move>moves, Move m, RotationDirection direction){
             Dictionary<Move, List<List<Move>>> substitutions = new Dictionary<Move, List<List<Move>>> {
                {Move.F, new List<List<Move>>{
                    new List<Move>{Move.U, Move.L, Move.U, Move.R},
                    new List<Move>{Move.U, Move.R, Move.D, Move.R},
                    new List<Move>{Move.D, Move.R, Move.D, Move.L},
                    new List<Move>{Move.D, Move.L, Move.U, Move.L},
                    new List<Move>{Move.U, Move.M, Move.E, Move.R},
                    new List<Move>{Move.E, Move.R, Move.M, Move.D},
                    new List<Move>{Move.M, Move.D, Move.L, Move.E},
                    new List<Move>{Move.L, Move.E, Move.U, Move.M},
                }},
                {Move.B, new List<List<Move>>{
                    new List<Move>{Move.R, Move.U, Move.L, Move.U},
                    new List<Move>{Move.R, Move.D, Move.R, Move.U},
                    new List<Move>{Move.L, Move.D, Move.R, Move.D},
                    new List<Move>{Move.L, Move.U, Move.L, Move.D},
                    new List<Move>{Move.R, Move.E, Move.M, Move.U},
                    new List<Move>{Move.D, Move.M, Move.R, Move.E},
                    new List<Move>{Move.E, Move.L, Move.D, Move.M},
                    new List<Move>{Move.M, Move.U, Move.E, Move.L},
                }},
                {Move.U, new List<List<Move>>{
                    new List<Move>{Move.B, Move.L, Move.B, Move.R},
                    new List<Move>{Move.B, Move.R, Move.F, Move.R},
                    new List<Move>{Move.F, Move.R, Move.F, Move.L},
                    new List<Move>{Move.F, Move.L, Move.B, Move.L},
                    new List<Move>{Move.B, Move.M, Move.S, Move.R},
                    new List<Move>{Move.S, Move.R, Move.M, Move.F},
                    new List<Move>{Move.M, Move.F, Move.L, Move.S},
                    new List<Move>{Move.L, Move.S, Move.B, Move.M},
                }},                
                {Move.D, new List<List<Move>>{
                    new List<Move>{Move.R, Move.B, Move.L, Move.B},
                    new List<Move>{Move.R, Move.F, Move.R, Move.B},
                    new List<Move>{Move.L, Move.F, Move.R, Move.F},
                    new List<Move>{Move.L, Move.B, Move.L, Move.F},
                    new List<Move>{Move.R, Move.S, Move.M, Move.B},
                    new List<Move>{Move.F, Move.M, Move.R, Move.S},
                    new List<Move>{Move.S, Move.L, Move.F, Move.M},
                    new List<Move>{Move.M, Move.B, Move.S, Move.L},
                }},
                {Move.L, new List<List<Move>>{
                    new List<Move>{Move.B, Move.U, Move.F, Move.U},
                    new List<Move>{Move.F, Move.U, Move.F, Move.D},
                    new List<Move>{Move.F, Move.D, Move.B, Move.D},
                    new List<Move>{Move.B, Move.D, Move.B, Move.U},
                    new List<Move>{Move.S, Move.U, Move.E, Move.F},
                    new List<Move>{Move.E, Move.F, Move.D, Move.S},
                    new List<Move>{Move.D, Move.S, Move.B, Move.E},
                    new List<Move>{Move.B, Move.E, Move.S, Move.U},
                }},  
                {Move.R, new List<List<Move>>{
                    new List<Move>{Move.U, Move.F, Move.U, Move.B},
                    new List<Move>{Move.D, Move.F, Move.U, Move.F},
                    new List<Move>{Move.D, Move.B, Move.D, Move.F},
                    new List<Move>{Move.U, Move.B, Move.D, Move.B},
                    new List<Move>{Move.F, Move.E, Move.U, Move.S},
                    new List<Move>{Move.S, Move.D, Move.F, Move.E},
                    new List<Move>{Move.E, Move.B, Move.S, Move.D},
                    new List<Move>{Move.U, Move.S, Move.E, Move.B},
                }},                
                {Move.M, new List<List<Move>>{
                    new List<Move>{Move.U, Move.F, Move.D, Move.F},
                    new List<Move>{Move.D, Move.F, Move.D, Move.B},
                    new List<Move>{Move.B, Move.D, Move.B, Move.U},
                    new List<Move>{Move.B, Move.U, Move.F, Move.U},
                    new List<Move>{Move.E, Move.F, Move.D, Move.S},
                    new List<Move>{Move.D, Move.S, Move.B, Move.E},
                    new List<Move>{Move.B, Move.E, Move.U, Move.S},
                    new List<Move>{Move.U, Move.S, Move.E, Move.F},
                }},         
                {Move.E, new List<List<Move>>{
                    new List<Move>{Move.L, Move.F, Move.R, Move.F},
                    new List<Move>{Move.R, Move.F, Move.R, Move.B},
                    new List<Move>{Move.R, Move.B, Move.L, Move.B},
                    new List<Move>{Move.L, Move.B, Move.L, Move.F},
                    new List<Move>{Move.M, Move.F, Move.R, Move.S},
                    new List<Move>{Move.R, Move.S, Move.B, Move.M},
                    new List<Move>{Move.B, Move.M, Move.L, Move.S},
                    new List<Move>{Move.L, Move.S, Move.M, Move.F},
                }},         
                {Move.S, new List<List<Move>>{
                    new List<Move>{Move.U, Move.R, Move.D, Move.R},
                    new List<Move>{Move.D, Move.R, Move.D, Move.L},
                    new List<Move>{Move.D, Move.L, Move.U, Move.L},
                    new List<Move>{Move.U, Move.L, Move.U, Move.R},
                    new List<Move>{Move.M, Move.U, Move.E, Move.R},
                    new List<Move>{Move.E, Move.R, Move.M, Move.D},
                    new List<Move>{Move.M, Move.D, Move.E, Move.L},
                    new List<Move>{Move.E, Move.L, Move.M, Move.U},
                }},
            };

            foreach (List<Move> s in substitutions[m]) {
                if (direction == RotationDirection.ClockWise) {
                    if (moves.Contains(s[0]) && moves.Contains(s[1])) {
                        moves.Remove(s[0]);
                        moves.Add(s[2]);

                        moves.Remove(s[1]);
                        moves.Add(s[3]);

                        break;
                    }
                }
                else {
                    if (moves.Contains(s[2]) && moves.Contains(s[3])) {
                        moves.Remove(s[2]);
                        moves.Add(s[0]);

                        moves.Remove(s[3]);
                        moves.Add(s[1]);

                        break;
                    }
                }
            }

            return moves;
        }

        private Dictionary<CubeFace, Material> setFaceColors(int x, int y, int z){
            Dictionary<CubeFace, Material> colors = new Dictionary<CubeFace,Material>();

            if (x == 0) {
                colors.Add(CubeFace.L, new DiffuseMaterial(new SolidColorBrush(Colors.Red)));
            }

            if (y == 0) {
                colors.Add(CubeFace.D, new DiffuseMaterial(new SolidColorBrush(Colors.Yellow)));
            }

            if (z == 0) {
                colors.Add(CubeFace.B, new DiffuseMaterial(new SolidColorBrush(Colors.Green)));
            }

            if (x == size-1) {
                colors.Add(CubeFace.R, new DiffuseMaterial(new SolidColorBrush(Colors.Orange)));
            }

            if (y == size - 1) {
                colors.Add(CubeFace.U, new DiffuseMaterial(new SolidColorBrush(Colors.White)));
            }

            if (z == size - 1) {
                colors.Add(CubeFace.F, new DiffuseMaterial(new SolidColorBrush(Colors.Blue)));
            }

            return colors;
        }
    }
}
