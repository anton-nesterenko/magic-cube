﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace magic_cube {
    class Cube2D {
        private int size;
        private CubeFace[,] projection;
        
        public Cube2D(int size) {
            this.size = size;
            this.projection = new CubeFace[size*4,size*3];
            createCube();
        }

        private void createCube(){
            for(int i=0; i<size*4; i++){
                for(int j=0; j<size*3; j++){
                    if (i < size && j >= size && j < size * 2) {
                        projection[i, j] = CubeFace.B;
                    }
                    else if (i >= size && i < size * 2) {
                        if (j < size) {
                            projection[i, j] = CubeFace.L;
                        }
                        else if (j >= size && j < size * 2) {
                            projection[i, j] = CubeFace.D;
                        }
                        else {
                            projection[i, j] = CubeFace.R;
                        }
                    }
                    else if (i >= size * 2 && i < size * 3 && j >= size && j < size * 2) {
                        projection[i, j] = CubeFace.F;
                    }
                    else if (i >= size * 3 && j >= size && j < size * 2) {
                        projection[i, j] = CubeFace.U;
                    }
                    else {
                        projection[i, j] = CubeFace.None;
                    }
                }
            }
        }

        public void rotate(KeyValuePair<Move, RotationDirection> move) {
            switch (move.Key) {
                case Move.F:
                case Move.S:
                    rotateFS(move.Key, move.Value);
                    break;
                case Move.B:
                    rotateB(move.Value);
                    break;
                case Move.R:
                    rotateR(move.Value);
                    break;
                case Move.L:
                case Move.M:
                    rotateLM(move.Key, move.Value);
                    break;
                case Move.U:
                    break;
                case Move.D:
                    break;
                case Move.E:
                    break;
            }
        }

        private void rotateR(RotationDirection d) {
            CubeFace t;
            int j = 5;

            List<List<int>> substitutions = new List<List<int>> {
                new List<int>{3, 0, 5, 0},
                new List<int>{5, 0, 5, 2},
                new List<int>{5, 2, 3, 2},
                new List<int>{3, 2, 3, 0},
                new List<int>{5, 1, 4, 2},
                new List<int>{4, 2, 3, 1},
                new List<int>{3, 1, 4, 0},
                new List<int>{4, 0, 5, 1},
            };

            if (d == RotationDirection.ClockWise) {
                t = projection[0, j];
                projection[0, j] = projection[9, j];
                projection[9, j] = projection[6, j];
                projection[6, j] = projection[3, j];
                projection[3, j] = t;

                t = projection[1, j];
                projection[1, j] = projection[10, j];
                projection[10, j] = projection[7, j];
                projection[7, j] = projection[4, j];
                projection[4, j] = t;

                t = projection[2, j];
                projection[2, j] = projection[11, j];
                projection[11, j] = projection[8, j];
                projection[8, j] = projection[5, j];
                projection[5, j] = t;
            }
            else {
                t = projection[9, j];
                projection[9, j] = projection[0, j];
                projection[0, j] = projection[3, j];
                projection[3, j] = projection[6, j];
                projection[6, j] = t;

                t = projection[4, j];
                projection[4, j] = projection[7, j];
                projection[7, j] = projection[10, j];
                projection[10, j] = projection[1, j];
                projection[1, j] = t;

                t = projection[11, j];
                projection[11, j] = projection[2, j];
                projection[2, j] = projection[5, j];
                projection[5, j] = projection[8, j];
                projection[8, j] = t;
            }

            rotateFace(substitutions, d);
        }

        private void rotateLM(Move m, RotationDirection d) {
            CubeFace t;
            int j;

            List<List<int>> substitutions = new List<List<int>> {
                new List<int>{5, 0, 3, 0},
                new List<int>{5, 2, 5, 0},
                new List<int>{3, 2, 5, 2},
                new List<int>{3, 0, 3, 2},
                new List<int>{4, 2, 5, 1},
                new List<int>{3, 1, 4, 2},
                new List<int>{4, 0, 3, 1},
                new List<int>{5, 1, 4, 0},
            };

            if (m == Move.L) {
                j = 3;
            }
            else {
                j = 4;
            }

            if(d == RotationDirection.ClockWise){
                t = projection[9, j];
                projection[9, j] = projection[0, j];
                projection[0, j] = projection[3, j];
                projection[3, j] = projection[6, j];
                projection[6, j] = t;

                t = projection[4, j];
                projection[4, j] = projection[7, j];
                projection[7, j] = projection[10, j];
                projection[10, j] = projection[1, j];
                projection[1, j] = t;

                t = projection[11, j];
                projection[11, j] = projection[2, j];
                projection[2, j] = projection[5, j];
                projection[5, j] = projection[8, j];
                projection[8, j] = t;

            }
            else{
                t = projection[0, j];
                projection[0, j] = projection[9, j];
                projection[9, j] = projection[6, j];
                projection[6, j] = projection[3, j];
                projection[3, j] = t;

                t = projection[1, j];
                projection[1, j] = projection[10, j];
                projection[10, j] = projection[7, j];
                projection[7, j] = projection[4, j];
                projection[4, j] = t;

                t = projection[2, j];
                projection[2, j] = projection[11, j];
                projection[11, j] = projection[8, j];
                projection[8, j] = projection[5, j];
                projection[5, j] = t;
            }

            if (m == Move.L) {
                rotateFace(substitutions, d);
            }
        }

        private void rotateFS(Move m, RotationDirection d) {
            CubeFace t;
            int i1 = 5, i2 = 9;

            List<List<int>> substitutions = new List<List<int>> {
                new List<int>{6, 3, 6, 5},
                new List<int>{6, 5, 8, 5},
                new List<int>{8, 5, 8, 3},
                new List<int>{8, 3, 6, 3},
                new List<int>{6, 4, 7, 5},
                new List<int>{7, 5, 8, 4},
                new List<int>{8, 4, 7, 3},
                new List<int>{7, 3, 6, 4},
            };

            if (m == Move.S) {
                i1 = 4;
                i2 = 10;
            }

            if (d == RotationDirection.ClockWise) {
                t = projection[i1, 5];
                projection[i1, 5] = projection[i1, 8];
                projection[i1, 8] = projection[i2, 3];
                projection[i2, 3] = projection[i1, 2];
                projection[i1, 2] = t;

                t = projection[i1, 4];
                projection[i1, 4] = projection[i1, 7];
                projection[i1, 7] = projection[i2, 4];
                projection[i2, 4] = projection[i1, 1];
                projection[i1, 1] = t;

                t = projection[i1, 3];
                projection[i1, 3] = projection[i1, 6];
                projection[i1, 6] = projection[i2, 5];
                projection[i2, 5] = projection[i1, 0];
                projection[i1, 0] = t;
            }
            else {
                t = projection[i1, 8];
                projection[i1, 8] = projection[i1, 5];
                projection[i1, 5] = projection[i1, 2];
                projection[i1, 2] = projection[i2, 3];
                projection[i2, 3] = t;

                t = projection[i1, 7];
                projection[i1, 7] = projection[i1, 4];
                projection[i1, 4] = projection[i1, 1];
                projection[i1, 1] = projection[i2, 4];
                projection[i2, 4] = t;

                t = projection[i1, 6];
                projection[i1, 6] = projection[i1, 3];
                projection[i1, 3] = projection[i1, 0];
                projection[i1, 0] = projection[i2, 5];
                projection[i2, 5] = t;
            }

            if(m == Move.F){
                rotateFace(substitutions, d);
            }
        }

        private void rotateB(RotationDirection d) {
            CubeFace t;

            List<List<int>> substitutions = new List<List<int>> {
                new List<int>{0, 3, 0, 5},
                new List<int>{0, 5, 2, 5},
                new List<int>{2, 5, 2, 3},
                new List<int>{2, 3, 0, 3},
                new List<int>{0, 4, 1, 5},
                new List<int>{1, 5, 2, 4},
                new List<int>{2, 4, 1, 3},
                new List<int>{1, 3, 0, 4},
            };

            if (d == RotationDirection.ClockWise) {
                t = projection[3, 3];
                projection[3, 3] = projection[3, 0];
                projection[3, 0] = projection[11, 5];
                projection[11, 5] = projection[3, 6];
                projection[3, 6] = t;

                t = projection[11, 3];
                projection[11, 3] = projection[3, 8];
                projection[3, 8] = projection[3, 5];
                projection[3, 5] = projection[3, 2];
                projection[3, 2] = t;

                t = projection[3, 4];
                projection[3, 4] = projection[3, 1];
                projection[3, 1] = projection[11, 4];
                projection[11, 4] = projection[3, 7];
                projection[3, 7] = t;
            }
            else {
                t = projection[3, 0];
                projection[3, 0] = projection[3, 3];
                projection[3, 3] = projection[3, 6];
                projection[3, 6] = projection[11, 5];
                projection[11, 5] = t;

                t = projection[3, 8];
                projection[3, 8] = projection[11, 3];
                projection[11, 3] = projection[3, 2];
                projection[3, 2] = projection[3, 5];
                projection[3, 5] = t;

                t = projection[3, 1];
                projection[3, 1] = projection[3, 4];
                projection[3, 4] = projection[3, 7];
                projection[3, 7] = projection[11, 4];
                projection[11, 4] = t;
            }

            rotateFace(substitutions, d);
        }

        private void rotateFace(List<List<int>> substitutions, RotationDirection d) {
            CubeFace t;
            int first_lhs = 0, second_lhs = 1, first_rhs = 2, second_rhs = 3;

            if (d == RotationDirection.CounterClockWise) {
                first_lhs = 2;
                second_lhs = 3;

                first_rhs = 0;
                second_rhs = 1;
            }

            t = projection[substitutions[0][first_lhs], substitutions[0][second_lhs]];

            for (int i = 0; i < substitutions.Count - 1; i++) {
                projection[substitutions[i][first_lhs], substitutions[i][second_lhs]] = projection[substitutions[i][first_rhs], substitutions[i][second_rhs]];
            }

            projection[substitutions[substitutions.Count - 1][first_lhs], substitutions[substitutions.Count - 1][second_lhs]] =
                projection[substitutions[substitutions.Count - 1][first_rhs], substitutions[substitutions.Count - 1][second_rhs]];
        }

        public void dbg(){
            for (int i = 0; i < size * 4; i++) {
                for (int j = 0; j < size * 3; j++) {
                    Debug.Write(projection[i, j].ToString().PadLeft(5, ' '));
                }
                Debug.WriteLine("");
            }
        }
    }
}
