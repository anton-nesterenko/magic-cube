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

            foreach(Cube c in this.Children){
                possibleMoves = c.possibleMoves;
                possibleMoves.Remove((Move)f);
                if(possibleMoves.Contains(move.Key)){
                    foreach (Move m in possibleMoves) {
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

                    //c.rotations.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, -1), 90 * Convert.ToInt32(move.Value)), new Point3D(0, 0, 0)));
                    c.rotations.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(axis, 90 * Convert.ToInt32(move.Value)), new Point3D(0, 0, 0)));
                }
            }
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
