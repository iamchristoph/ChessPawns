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
            ChessLocation testLocation;
            List<ChessMove> moves = new List<ChessMove>();
            //first well need to figure out what kind of piece was passed in 
            if (myColor == ChessColor.White)
            {
                switch (myPiece)
                {
                    case ChessPiece.WhitePawn:
                        {
<<<<<<< HEAD
                            moves.AddRange(GetWhitePawnMoves(board, location));
=======
                            //set locationweretesting to some valid move for white pawn
                            //is this the first move for this pawn?
                            if (location.Y == 6)
                            {
                                ChessMove move = new ChessMove(location, new ChessLocation(location.X, 4));
                                moves.Add(move);
                            }
                            //is there a piece in front of you?
                            testLocation = new ChessLocation(location.X, location.Y + 1);
                            if(board[testLocation] == ChessPiece.Empty)
                            {
                                //no theres not. 
                                ChessMove move = new ChessMove(location, testLocation);
                                moves.Add(move);
                            }

                            //is there a piece to either of your diagonals?
                            testLocation = new ChessLocation(location.X - 1, location.Y + 1);
                            if(board[testLocation] < ChessPiece.Empty)
                            {
                                ChessMove move = new ChessMove(location, testLocation);
                                moves.Add(move);
                            }
                            testLocation = new ChessLocation(location.X + 1, location.Y + 1);
                            if(board[testLocation] < ChessPiece.Empty)
                            {
                                ChessMove move = new ChessMove(location, testLocation);
                                moves.Add(move);
                            }
                            //    ChessMove move = new ChessMove(location, );
                            //moves.Add(move);
>>>>>>> origin/master
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
            testLocation = new ChessLocation(location.X, location.Y + 1);
            if (board[testLocation] == ChessPiece.Empty)
            {
                //no theres not. 
                ChessMove move = new ChessMove(location, testLocation);
                moves.Add(move);
            }

            //is there a piece to either of your diagonals?
            testLocation = new ChessLocation(location.X - 1, location.Y + 1);
            if (board[testLocation] < ChessPiece.Empty)
            {
                ChessMove move = new ChessMove(location, testLocation);
                moves.Add(move);
            }
            testLocation = new ChessLocation(location.X + 1, location.Y + 1);
            if (board[testLocation] < ChessPiece.Empty)
            {
                ChessMove move = new ChessMove(location, testLocation);
                moves.Add(move);
            }
            //    ChessMove move = new ChessMove(location, );
            //moves.Add(move);
            return moves;
        }
        List<ChessMove> GetBlackPawnMoves(ChessBoard board, ChessLocation location) { return new List<ChessMove>(); }
        List<ChessMove> GetBishopMoves(ChessBoard board, ChessLocation location, ChessColor myColor) { return new List<ChessMove>(); }
        List<ChessMove> GetKnightMoves(ChessBoard board, ChessLocation location, ChessColor myColor) { return new List<ChessMove>(); }
        List<ChessMove> GetRookeMoves(ChessBoard board, ChessLocation location, ChessColor myColor) { return new List<ChessMove>(); }
        List<ChessMove> GetQueenMoves(ChessBoard board, ChessLocation location, ChessColor myColor) { return new List<ChessMove>(); }
        List<ChessMove> GetKingMoves(ChessBoard board, ChessLocation location, ChessColor myColor) { return new List<ChessMove>(); }

        #endregion














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
