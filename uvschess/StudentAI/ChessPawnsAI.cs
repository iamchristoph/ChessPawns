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
            this.Log(myColor.ToString() + " (" + this.Name + ") just moved.");

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
                    moves.AddRange(GetMove(board, new ChessLocation(i, j), myColor)); // for each location add the valid moves
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
                            List<ChessMove> check = new List<ChessMove>(GetKingMoves(board, location, myColor));
                            foreach (ChessMove move in check)
                            {
                                if (!isCheck(board, location, myColor))
                                {
                                    moves.Add(move);
                                }
                            }
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

            //set locationweretesting to some valid move for white pawn
            int x = location.X, y = location.Y;

            if (y == 6) //pawn hasn't moved
            {
                if (board[x, y - 1] == ChessPiece.Empty)
                {
                    moves.Add(new ChessMove(location, new ChessLocation(x, y - 1)));
                    if (board[x, y - 2] == ChessPiece.Empty)
                        moves.Add(new ChessMove(location, new ChessLocation(x, y - 2)));
                }
            }
            // get forward move if available
            if (y - 1 >= 0)
            {
                if (board[x, y - 1] == ChessPiece.Empty)
                {
                    //no theres not. 
                    ChessMove move = new ChessMove(location, new ChessLocation(x, y - 1));
                    moves.Add(move);
                }

                //is there a piece to either of your diagonals?
                if (x - 1 >= 0 && board[x - 1, y - 1] < ChessPiece.Empty)
                {
                    moves.Add(new ChessMove(location, new ChessLocation(x - 1, y - 1)));
                }
                if (x + 1 < 8 && board[x + 1, y - 1] < ChessPiece.Empty)
                {
                    moves.Add(new ChessMove(location, new ChessLocation(x + 1, y - 1)));
                }
            }
            return moves;
        }
        List<ChessMove> GetBlackPawnMoves(ChessBoard board, ChessLocation location)
        {
            List<ChessMove> moves = new List<ChessMove>();

            int x = location.X, y = location.Y;

            if (y == 1) //pawn hasn't moved
            {
                if (board[x, y + 1] == ChessPiece.Empty)
                {
                    moves.Add(new ChessMove(location, new ChessLocation(x, y + 1)));
                    if (board[x, y + 2] == ChessPiece.Empty)
                        moves.Add(new ChessMove(location, new ChessLocation(x, y + 2)));
                }
            }
            // get forward move if available
            if (y + 1 < 8)
            {
                if (board[x, y + 1] == ChessPiece.Empty)
                {
                    //no theres not. 
                    ChessMove move = new ChessMove(location, new ChessLocation(x, y + 1));
                    moves.Add(move);
                }

                //is there a piece to either of your diagonals?
                if (x - 1 >= 0 && board[x - 1, y + 1] > ChessPiece.Empty)
                {
                    moves.Add(new ChessMove(location, new ChessLocation(x - 1, y + 1)));
                }
                if (x + 1 < 8 && board[x + 1, y + 1] > ChessPiece.Empty)
                {
                    moves.Add(new ChessMove(location, new ChessLocation(x + 1, y + 1)));
                }
            }
            return moves;
        }
        List<ChessMove> GetBishopMoves(ChessBoard board, ChessLocation location, ChessColor myColor)
        {
            List<ChessMove> moves = new List<ChessMove>();
            int x = location.X, y = location.Y;
            for (int i = 1; i < 8; i++)  
            {
                if (x + i < 8 && y + i < 8) // down right 
                {
                    if (board[x+i, y+i] == ChessPiece.Empty)  // check if spot is empty
                        moves.Add(new ChessMove(location, new ChessLocation(x + i, y + i)));
                    else if (Color(board[x + i, y + i]) == myColor)
                        break;  // if we get to a filled space, we can quit going down this path
                    else // is opponent piece take it.
                    {
                        moves.Add(new ChessMove(location, new ChessLocation(x + i, y + i)));
                        break;  // take the opponents piece and end searching this path
                    }
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (x - i >= 0 && y + i < 8) // down left 
                {
                    if (board[x - i, y + i] == ChessPiece.Empty)  // check if spot is empty
                        moves.Add(new ChessMove(location, new ChessLocation(x - i, y + i)));
                    else if (Color(board[x - i, y + i]) == myColor)
                        break;  // if we get to a filled space, we can quit going down this path
                    else // is opponent piece take it.
                    {
                        moves.Add(new ChessMove(location, new ChessLocation(x - i, y + i)));
                        break;  // take the opponents piece and end searching this path
                    }
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (x + i < 8 && y - i >= 0) // up right 
                {
                    if (board[x + i, y - i] == ChessPiece.Empty)  // check if spot is empty
                        moves.Add(new ChessMove(location, new ChessLocation(x + i, y - i)));
                    else if (Color(board[x + i, y - i]) == myColor)
                        break;  // if we get to a filled space, we can quit going down this path
                    else // is opponent piece take it.
                    {
                        moves.Add(new ChessMove(location, new ChessLocation(x + i, y - i)));
                        break;  // take the opponents piece and end searching this path
                    }
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (x - i >= 0 && y - i >= 0) // up left 
                {
                    if (board[x - i, y - i] == ChessPiece.Empty)  // check if spot is empty
                        moves.Add(new ChessMove(location, new ChessLocation(x - i, y - i)));
                    else if (Color(board[x - i, y - i]) == myColor)
                        break;  // if we get to a filled space, we can quit going down this path
                    else // is opponent piece take it.
                    {
                        moves.Add(new ChessMove(location, new ChessLocation(x - i, y - i)));
                        break;  // take the opponents piece and end searching this path
                    }
                }
            }
            return moves;
        }
        List<ChessMove> GetKnightMoves(ChessBoard board, ChessLocation location, ChessColor myColor) 
        { 

            List<ChessMove> moves = new List<ChessMove>();
            int x = location.X, y = location.Y; 

            // get right 2, up, down 1
            if (x + 2 < 8)
            {//can go right 2 and stay on board
                if (y + 1 < 8) // can go down 1 and stay on board
                {
                    ChessPiece piece = board[x+2, y+1];
                    if (piece == ChessPiece.Empty || Color(piece) != myColor) //space is empty or opponent I can go there
                        moves.Add(new ChessMove(location, new ChessLocation(x + 2, y + 1)));
                }
                if (y - 1 >= 0) // can go up 1 and stay on board
                {
                    ChessPiece piece = board[x + 2, y - 1];
                    if (piece == ChessPiece.Empty || Color(piece) != myColor) //space is empty or opponent I can go there
                        moves.Add(new ChessMove(location, new ChessLocation(x + 2, y - 1)));
                }
            }

            // get right 1, up, down 2
            if (x + 1 < 8)
            {//can go right 1 and stay on board
                if (y + 2 < 8) // can go up 2 and stay on board
                {
                    ChessPiece piece = board[x + 1, y + 2];
                    if (piece == ChessPiece.Empty || Color(piece) != myColor) //space is empty or opponent I can go there
                        moves.Add(new ChessMove(location, new ChessLocation(x + 1, y + 2)));
                }
                if (y - 2 >= 0) // can go up 1 and stay on board
                {
                    ChessPiece piece = board[x + 1, y - 2];
                    if (piece == ChessPiece.Empty || Color(piece) != myColor) //space is empty or opponent I can go there
                        moves.Add(new ChessMove(location, new ChessLocation(x + 1, y - 2)));
                }
            }

            // get left 2, up, down 1
            if (x - 2 >= 0)
            {//can go left 2 and stay on board
                if (y + 1 < 8) // can go down 1 and stay on board
                {
                    ChessPiece piece = board[x - 2, y + 1];
                    if (piece == ChessPiece.Empty || Color(piece) != myColor) //space is empty or opponent I can go there
                        moves.Add(new ChessMove(location, new ChessLocation(x - 2, y + 1)));
                }
                if (y - 1 >= 0) // can go up 1 and stay on board
                {
                    ChessPiece piece = board[x - 2, y - 1];
                    if (piece == ChessPiece.Empty || Color(piece) != myColor) //space is empty or opponent I can go there
                        moves.Add(new ChessMove(location, new ChessLocation(x - 2, y - 1)));
                }
            }

            // get left 1, up, down 2
            if (x - 1 >= 0)
            {//can go left 1 and stay on board
                if (y + 2 < 8) // can go down 1 and stay on board
                {
                    ChessPiece piece = board[x - 1, y + 2];
                    if (piece == ChessPiece.Empty || Color(piece) != myColor) //space is empty or opponent I can go there
                        moves.Add(new ChessMove(location, new ChessLocation(x - 1, y + 2)));
                }
                if (y - 2 >= 0) // can go up 1 and stay on board
                {
                    ChessPiece piece = board[x - 1, y - 2];
                    if (piece == ChessPiece.Empty || Color(piece) != myColor) //space is empty or opponent I can go there
                        moves.Add(new ChessMove(location, new ChessLocation(x - 1, y - 2)));
                }
            }

            return moves;
        }

        List<ChessMove> GetRookeMoves(ChessBoard board, ChessLocation location, ChessColor myColor) 
        {
            List<ChessMove> moves = new List<ChessMove>();

            for (int dY = location.Y - 1; dY >= 0; dY-- )  // Up
            {
                if (board[location.X, dY] == ChessPiece.Empty)  // check if spot is empty
                    moves.Add(new ChessMove(location, new ChessLocation(location.X, dY)));
                else if (Color(board[location.X, dY]) == myColor)
                    break;  // if we get to a filled space, we can quit going down this path
                else // it is opponent peice take it.
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
                else // is opponent piece take it.
                {
                    moves.Add(new ChessMove(location, new ChessLocation(location.X, dY)));
                    break;  // take the opponents piece and end searching this path
                }
            }

            for (int dX = location.X - 1; dX >= 0; dX--)  // Left
            {
                if (board[dX, location.Y] == ChessPiece.Empty)  // check if spot is empty
                    moves.Add(new ChessMove(location, new ChessLocation(dX, location.Y)));
                else if (Color(board[dX, location.Y]) == myColor)
                    break;  // if we get to a filled space, we can quit going down this path
                else // is opponent piece take it.
                {
                    moves.Add(new ChessMove(location, new ChessLocation(dX, location.Y)));
                    break;  // take the opponents piece and end searching this path
                }
            }

            for (int dX = location.X + 1; dX < 8; dX++)  // Right
            {
                if (board[dX, location.Y] == ChessPiece.Empty)  // check if spot is empty
                    moves.Add(new ChessMove(location, new ChessLocation(dX, location.Y)));
                else if (Color(board[dX, location.Y]) == myColor)
                    break;  // if we get to a filled space, we can quit going down this path
                else // is opponent piece take it.
                {
                    moves.Add(new ChessMove(location, new ChessLocation(dX, location.Y)));
                    break;  // take the opponents piece and end searching this path
                }
            }


            return moves;
        }

        List<ChessMove> GetQueenMoves(ChessBoard board, ChessLocation location, ChessColor myColor) 
        {
            // queen is combination of Bishop and Rooke moves
            List<ChessMove> moves = GetBishopMoves(board, location, myColor);
            moves.AddRange(GetRookeMoves(board, location, myColor));
            return moves; 
        }

        List<ChessMove> GetKingMoves(ChessBoard board, ChessLocation location, ChessColor myColor) 
        { 
            List<ChessMove> moves = new List<ChessMove>();
            int x = location.X, y = location.Y;

            // right
            if (x + 1 < 8 ) // still on the board
            {
                if (board[x + 1, y] == ChessPiece.Empty)  // check if spot is empty
                    moves.Add(new ChessMove(location, new ChessLocation(x + 1, y)));
                else if (Color(board[x + 1, y]) != myColor) // is opponent piece take it.
                {
                    moves.Add(new ChessMove(location, new ChessLocation(x + 1, y)));
                    // take the opponents piece
                }
                // down right
                if  (y + 1 < 8) // still on board
                {
                    if (board[x + 1, y + 1] == ChessPiece.Empty)  // check if spot is empty
                        moves.Add(new ChessMove(location, new ChessLocation(x + 1, y + 1)));
                    else if (Color(board[x + 1, y + 1]) != myColor) // is opponent piece take it.
                    {
                        moves.Add(new ChessMove(location, new ChessLocation(x + 1, y + 1)));
                        // take the opponents piece
                    }
                }
                // up right
                if (y - 1 >= 0) // still on board
                {
                    if (board[x + 1, y - 1] == ChessPiece.Empty)  // check if spot is empty
                        moves.Add(new ChessMove(location, new ChessLocation(x + 1, y - 1)));
                    else if (Color(board[x + 1, y - 1]) != myColor) // is opponent piece take it.
                    {
                        moves.Add(new ChessMove(location, new ChessLocation(x + 1, y - 1)));
                        // take the opponents piece
                    }
                }
            }

            // left
            if (x - 1 >= 0) // still on the board
            {
                if (board[x - 1, y] == ChessPiece.Empty)  // check if spot is empty
                    moves.Add(new ChessMove(location, new ChessLocation(x - 1, y)));
                else if (Color(board[x - 1, y]) != myColor) // is opponent piece take it.
                {
                    moves.Add(new ChessMove(location, new ChessLocation(x - 1, y)));
                    // take the opponents piece
                }
                // up left
                if (y - 1 >= 0) // still on board
                {
                    if (board[x - 1, y - 1] == ChessPiece.Empty)  // check if spot is empty
                        moves.Add(new ChessMove(location, new ChessLocation(x - 1, y - 1)));
                    else if (Color(board[x - 1, y - 1]) != myColor) // is opponent piece take it.
                    {
                        moves.Add(new ChessMove(location, new ChessLocation(x - 1, y - 1)));
                        // take the opponents piece
                    }
                }
                // down left
                if (y + 1 < 8) // still on board
                {
                    if (board[x - 1, y + 1] == ChessPiece.Empty)  // check if spot is empty
                        moves.Add(new ChessMove(location, new ChessLocation(x - 1, y + 1)));
                    else if (Color(board[x - 1, y + 1]) != myColor) // is opponent piece take it.
                    {
                        moves.Add(new ChessMove(location, new ChessLocation(x - 1, y + 1)));
                        // take the opponents piece
                    }
                }
            }

            // up
            if (y - 1 >= 0) // still on the board
            {
                if (board[x, y - 1] == ChessPiece.Empty)  // check if spot is empty
                    moves.Add(new ChessMove(location, new ChessLocation(x, y - 1)));
                else if (Color(board[x, y - 1]) != myColor) // is opponent piece take it.
                {
                    moves.Add(new ChessMove(location, new ChessLocation(x, y - 1)));
                    // take the opponents piece
                }
            }

            // down
            if (y + 1 < 8) // still on the board
            {
                if (board[x, y + 1] == ChessPiece.Empty)  // check if spot is empty
                    moves.Add(new ChessMove(location, new ChessLocation(x, y + 1)));
                else if (Color(board[x, y + 1]) != myColor) // is opponent piece take it.
                {
                    moves.Add(new ChessMove(location, new ChessLocation(x, y + 1)));
                    // take the opponents piece
                }
            }

            return moves;
        }




        #endregion

        public static ChessColor Color(ChessPiece piece)
        {
            return piece > ChessPiece.Empty ? ChessColor.White : ChessColor.Black;
        }

        bool isCheck(ChessBoard board, ChessLocation king, ChessColor color)
        {
            //int x = king.X, y = king.Y;
            
            // check for pawns
            //if () {}

            // check for knights

            // check for queens bishops rookes or kings

            return false;
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
