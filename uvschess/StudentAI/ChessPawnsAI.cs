using System;
using System.Collections.Generic;
using System.Text;
using UvsChess;

namespace ChessPawnsAI
{
    public class ChessPawnsAI : IChessAI
    {
        #region IChessAI Members that are implemented by the Student

        /// <summary>
        /// The name of your AI
        /// </summary>
        public string Name
        {
#if DEBUG
            get { return "ChessPawnsAI (Debug)"; }
#else
            get { return "ChessPawnsAI"; }
#endif
        }

        /// <summary>
        /// Evaluates the chess board and decided which move to make. This is the main method of the AI.
        /// The framework will call this method when it's your turn.
        /// </summary>
        /// <param name="board">Current chess board</param>
        /// <param name="yourColor">Your color</param>
        /// <returns> Returns the best chess move the player has for the given chess board</returns>
        public ChessMove GetNextMove(ChessBoard board, ChessColor myColor)
        {
            ChessMove move = GetRndMove(board, myColor);
            return move;
        }

        /// <summary>
        /// Calls GetAllMoves and returns a random move from the array.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="myColor"></param>
        /// <returns> Returns a random move </returns>
        public ChessMove GetRndMove(ChessBoard board, ChessColor myColor)
        {
            List<ChessMove> moves = GetAllMoves(board, myColor);
            Random random = new Random();
            int randInt = random.Next(moves.Count);
            return moves[randInt];
        }

        /// <summary>
        /// Calls GetMove on each piece in the board and adds it to a list
        /// </summary>
        /// <param name="board"></param>
        /// <param name="myColor"></param>
        /// <returns>list of possible Moves</returns>
        public List<ChessMove> GetAllMoves(ChessBoard board, ChessColor myColor)
        {
            List<ChessMove> moves = new List<ChessMove>(); // a list to hold our moves
            for (int i = 0; i < 8; i++ )
            {
                for (int j = 0; j < 8; j++)
                {
                    ChessLocation location = new ChessLocation(i, j);
                    moves.AddRange(GetMove(board, location, myColor)); // for each location add the valid moves
                }
            }
            return moves;
        }


        public List<ChessMove> GetMove(ChessBoard board, ChessLocation location, ChessColor myColor)
        {
            ChessPiece myPiece = board[location];
            //init a new location so we can have somethign to write to. 

            List<ChessMove> moves = new List<ChessMove>();
            //first well need to figure out what kind of piece was passed in 
            if (myColor == ChessColor.White)
            {
                switch (myPiece)
                {
                    case ChessPiece.WhitePawn:
                        {

                            moves.AddRange(GetWhitePawnMoves(board, location));
                            break;
                        }
                    case ChessPiece.WhiteBishop:
                        {
                            moves.AddRange(GetBishopMoves(board, location, myColor));
                            break;
                        }
                    case ChessPiece.WhiteKnight:
                        {
                            moves.AddRange(GetKnightMoves(board, location, myColor));
                            break;
                        }
                    case ChessPiece.WhiteRook:
                        {
                            moves.AddRange(GetRookeMoves(board, location, myColor));
                            break;
                        }
                    case ChessPiece.WhiteQueen:
                        {
                            moves.AddRange(GetQueenMoves(board, location, myColor));
                            break;
                        }
                    case ChessPiece.WhiteKing:
                        {
                            moves.AddRange(GetKingMoves(board, location, myColor));
                            break;
                        }
                }
            }

            else //my color is Black
            {
                switch (myPiece)
                {
                    case ChessPiece.BlackPawn:
                        {
                            moves.AddRange(GetBlackPawnMoves(board, location));
                            break; 
                        }
                    case ChessPiece.BlackBishop:
                        {
                            moves.AddRange(GetBishopMoves(board, location, myColor));
                            break;
                        }
                    case ChessPiece.BlackKnight:
                        {
                            moves.AddRange(GetKnightMoves(board, location, myColor));
                            break;
                        }
                    case ChessPiece.BlackRook:
                        {
                            moves.AddRange(GetRookeMoves(board, location, myColor));
                            break;
                        }
                    case ChessPiece.BlackQueen:
                        {
                            moves.AddRange(GetQueenMoves(board, location, myColor));
                            break;
                        }
                    case ChessPiece.BlackKing:
                        {
                            moves.AddRange(GetKingMoves(board, location, myColor));
                            break;
                        }
                }
            }
            //do the rest of the moves here. 

            return moves;
        }

        /// <summary>
        /// Validates a move. The framework uses this to validate the opponents move.
        /// </summary>
        /// <param name="boardBeforeMove">The board as it currently is _before_ the move.</param>
        /// <param name="moveToCheck">This is the move that needs to be checked to see if it's valid.</param>
        /// <param name="colorOfPlayerMoving">This is the color of the player who's making the move.</param>
        /// <returns>Returns true if the move was valid</returns>
        public bool IsValidMove(ChessBoard boardBeforeMove, ChessMove moveToCheck, ChessColor colorOfPlayerMoving)
        {
            List<ChessMove> validMoves = GetAllMoves(boardBeforeMove, colorOfPlayerMoving);
            return validMoves.Contains(moveToCheck);
        }

        #endregion

        #region Get moves functions by piece
        List<ChessMove> GetWhitePawnMoves(ChessBoard board, ChessLocation location)
        {
            List<ChessMove> moves = new List<ChessMove>();

            ChessLocation testLocation;
            //set locationweretesting to some valid move for white pawn
            //is this the first move for this pawn?
            if (location.Y == 6)
            {
                ChessMove move = new ChessMove(location, new ChessLocation(location.X, 4));
                moves.Add(move);
            }
            //is there a piece in front of you?
            testLocation = new ChessLocation(location.X, location.Y - 1);
            if (board[testLocation] == ChessPiece.Empty)
            {
                //no theres not. 
                ChessMove move = new ChessMove(location, testLocation);
                moves.Add(move);
            }

            //is there a piece to either of your diagonals?
            testLocation = new ChessLocation(location.X - 1, location.Y - 1);
            if (board[testLocation] < ChessPiece.Empty)
            {
                ChessMove move = new ChessMove(location, testLocation);
                moves.Add(move);
            }
            testLocation = new ChessLocation(location.X + 1, location.Y - 1);
            if (board[testLocation] < ChessPiece.Empty)
            {
                ChessMove move = new ChessMove(location, testLocation);
                moves.Add(move);
            }
            //    ChessMove move = new ChessMove(location, );
            //moves.Add(move);
            return moves;
        }
        List<ChessMove> GetBlackPawnMoves(ChessBoard board, ChessLocation location)
        {
            List<ChessMove> moves = new List<ChessMove>();

            ChessLocation testLocation;
            if (location.Y == 1)
            {
                ChessMove move = new ChessMove(location, new ChessLocation(location.X, 3));
                moves.Add(move);
            }
            //is there a piece in front of you?
            testLocation = new ChessLocation(location.X, location.Y + 1);
            if (board[testLocation] == ChessPiece.Empty)
            {
                //no theres not. 
                ChessMove move = new ChessMove(location, testLocation);
                moves.Add(move);
            }

            //is there a piece to either of your diagonals?
            testLocation = new ChessLocation(location.X - 1, location.Y + 1);
            //> here because all white pieces are > empty
            if (board[testLocation] > ChessPiece.Empty)
            {
                ChessMove move = new ChessMove(location, testLocation);
                moves.Add(move);
            }
            testLocation = new ChessLocation(location.X + 1, location.Y + 1);
            if (board[testLocation] > ChessPiece.Empty)
            {
                ChessMove move = new ChessMove(location, testLocation);
                moves.Add(move);
            }
            //    ChessMove move = new ChessMove(location, );
            //moves.Add(move);
            return moves;
        }
        List<ChessMove> GetBishopMoves(ChessBoard board, ChessLocation location, ChessColor myColor)
        {
            List<ChessMove> moves = new List<ChessMove>();
            ChessLocation testLocation = new ChessLocation(0, 0);
            for(int i = 0; i < 8; i++)
            {
                testLocation = new ChessLocation(location.X - i, location.Y - i);
                moves.Add(new ChessMove(location, testLocation));
                testLocation = new ChessLocation(location.X - i, location.Y + i);
                moves.Add(new ChessMove(location, testLocation));
                testLocation = new ChessLocation(location.X + i, location.Y - i);
                moves.Add(new ChessMove(location, testLocation));
                testLocation = new ChessLocation(location.X + i, location.Y + i);
                moves.Add(new ChessMove(location, testLocation));
            }
            //now validate the moves
            foreach (ChessMove m in moves)
            {
                if (myColor == ChessColor.White)
                {
                    //if either coordinate is invalid OR if theres a white piece there
                    //im not sure if we want to do validation here, but it seems like this would be pretty quick. 
                    if ((m.To.X > 7 || m.To.X < 0 || m.To.Y > 7 || m.To.Y < 0) || (board[m.To] > ChessPiece.Empty))
                    {
                        moves.Remove(m);
                    }
                }
                else //your color isnt white
                {
                    //are any cooridantes invalid or is there a black piece already there?
                    if ((m.To.X > 7 || m.To.X < 0 || m.To.Y > 7 || m.To.Y < 0) || (board[m.To] < ChessPiece.Empty))
                    {
                        moves.Remove(m);
                    }
                }
            }
            return moves;
        }
        List<ChessMove> GetKnightMoves(ChessBoard board, ChessLocation location, ChessColor myColor) { return new List<ChessMove>(); }

        List<ChessMove> GetRookeMoves(ChessBoard board, ChessLocation location, ChessColor myColor) 
        {
            List<ChessMove> moves = new List<ChessMove>();

            for (int dY = location.Y - 1; dY >= 0; dY-- )  // Up
            {
                if (board[location.X, dY] == ChessPiece.Empty)  // check if spot is empty
                    moves.Add(new ChessMove(location, new ChessLocation(location.X, dY)));
                else if (Color(board[location.X, dY]) == myColor)
                    break;  // if we get to a filled space, we can quit going down this path

                if (board[location.X, dY] != ChessPiece.Empty && Color(board[location.X, dY]) != myColor)
                {
                    moves.Add(new ChessMove(location, new ChessLocation(location.X, dY)));
                    break;  // take the opponents piece and end searching this path
                }
            }

            for (int dY = location.Y + 1; dY < 8; dY++)  // Down
            {
                if (board[location.X, dY] == ChessPiece.Empty)  // check if spot is empty
                    moves.Add(new ChessMove(location, new ChessLocation(location.X, dY)));
                else if (Color(board[location.X, dY]) == myColor)
                    break;  // if we get to a filled space, we can quit going down this path

                if (board[location.X, dY] != ChessPiece.Empty && Color(board[location.X, dY]) != myColor)
                {
                    moves.Add(new ChessMove(location, new ChessLocation(location.X, dY)));
                    break;  // take the opponents piece and end searching this path
                }
            }

            for (int dX = location.X; dX >= 0; dX--)  // Left
            {
                if (board[dX, location.Y] == ChessPiece.Empty)
                {
                    moves.Add(new ChessMove(location, new ChessLocation(dX, location.Y)));
                    break;  // if we get to a filled space, we can quit going down this path
                }

                if (board[dX, location.Y] != ChessPiece.Empty) // not our color)
                {
                    moves.Add(new ChessMove(location, new ChessLocation(dX, location.Y)));
                    break;   // take the opponents piece and end searching this path
                }
            }

            for (int dX = location.X; dX < 8; dX++)  // Right
            {
                if (board[dX, location.Y] == ChessPiece.Empty)
                {
                    moves.Add(new ChessMove(location, new ChessLocation(dX, location.Y)));
                    break;  // if we get to a filled space, we can quit going down this path
                }

                if (board[dX, location.Y] != ChessPiece.Empty) // not our color)
                {
                    moves.Add(new ChessMove(location, new ChessLocation(dX, location.Y)));
                    break;   // take the opponents piece and end searching this path
                }
            }

            return moves;
        }

        List<ChessMove> GetQueenMoves(ChessBoard board, ChessLocation location, ChessColor myColor) 
        {
            // queen is combination of Bishop and Rooke moves
            List<ChessMove> moves = GetBishopMoves(board, location, myColor);
            List<ChessMove> rMoves = GetRookeMoves(board, location, myColor);

            foreach (var m in rMoves)
                moves.Add(m);

            return moves; 
        }

        List<ChessMove> GetKingMoves(ChessBoard board, ChessLocation location, ChessColor myColor) { return new List<ChessMove>(); }




        #endregion

        public static ChessColor Color(ChessPiece piece)
        {
            return piece > ChessPiece.Empty ? ChessColor.White : ChessColor.Black;
        }














        #region IChessAI Members that should be implemented as automatic properties and should NEVER be touched by students.
        /// <summary>
        /// This will return false when the framework starts running your AI. When the AI's time has run out,
        /// then this method will return true. Once this method returns true, your AI should return a 
        /// move immediately.
        /// 
        /// You should NEVER EVER set this property!
        /// This property should be defined as an Automatic Property.
        /// This property SHOULD NOT CONTAIN ANY CODE!!!
        /// </summary>
        public AIIsMyTurnOverCallback IsMyTurnOver { get; set; }

        /// <summary>
        /// Call this method to print out debug information. The framework subscribes to this event
        /// and will provide a log window for your debug messages.
        /// 
        /// You should NEVER EVER set this property!
        /// This property should be defined as an Automatic Property.
        /// This property SHOULD NOT CONTAIN ANY CODE!!!
        /// </summary>
        /// <param name="message"></param>
        public AILoggerCallback Log { get; set; }

        /// <summary>
        /// Call this method to catch profiling information. The framework subscribes to this event
        /// and will print out the profiling stats in your log window.
        /// 
        /// You should NEVER EVER set this property!
        /// This property should be defined as an Automatic Property.
        /// This property SHOULD NOT CONTAIN ANY CODE!!!
        /// </summary>
        /// <param name="key"></param>
        public AIProfiler Profiler { get; set; }

        /// <summary>
        /// Call this method to tell the framework what decision print out debug information. The framework subscribes to this event
        /// and will provide a debug window for your decision tree.
        /// 
        /// You should NEVER EVER set this property!
        /// This property should be defined as an Automatic Property.
        /// This property SHOULD NOT CONTAIN ANY CODE!!!
        /// </summary>
        /// <param name="message"></param>
        public AISetDecisionTreeCallback SetDecisionTree { get; set; }
        #endregion
    }
}
