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
            //first we'll need to figure out what kind of piece was passed in 
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
            // (If we need more speed, move these methods up to GetAllMoves to avoid repeating it too much--
            // king location is needed to check for check)
            ChessLocation king = FindKing(board, myColor);
            ChessLocation otherKing = FindKing(board, OtherColor(myColor));
            // Eliminate any moves that put us into check
            //for (int i = moves.Count - 1; i >= 0; --i)
            //{
            //    if (IsCheck(board, moves[i], king, myColor))
            //    {
            //        moves.RemoveAt(i);
            //    }
            //}
            //// Flag any moves that put our opponent in check
            //foreach (ChessMove move in moves)
            //{
            //    if (IsCheck(board, move, otherKing, OtherColor(myColor)))
            //    {
            //        move.Flag = ChessFlag.Check;
            //    }
            //}
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

        // IsCheck function answers the question:
        // Does this move, performed on this board, put this color in check?
        bool IsCheck(ChessBoard b, ChessMove move, ChessLocation king, ChessColor color)
        {
            // Create a cloned board with the prospective move
            ChessBoard board = b.Clone();
            board.MakeMove(move);
            ChessPiece kingPiece;
            if (color == ChessColor.White)
            {
                kingPiece = ChessPiece.WhiteKing;
            }
            else
            {
                kingPiece = ChessPiece.BlackKing;
            }
            // Verify the king is where we think it is
            if (board[king] != kingPiece)
            {
                // If it's not there, it should be in the "to" location of the move
                if (board[move.To] == kingPiece)
                {
                    king = move.To;
                }
                else
                {
                    king = FindKing(board, color);
                }
            }
            Console.WriteLine("InCheck method, color: " + color);
            // Checks for knights within range
            if (InCheckFromKnight(board, king, color))
            {
                return true;
            }
            // Checks the tiles contiguous with the king
            if (InCheckFromPawnOrKing(board, king, color))
            {
                return true;
            }
            // Checks the diagonals for threats
            if (InCheckFromBishopOrQueen(board, king, color))
            {
                return true;
            }
            // Checks the column and row for threats
            if (InCheckFromRookOrQueen(board, king, color))
            {
                return true;
            }
            return false;
        }

        private bool InCheckFromKnight(ChessBoard board, ChessLocation king, ChessColor color)
        {
            int x = king.X, y = king.Y;
            ChessPiece knight;
            if (color == ChessColor.White)
            {
                knight = ChessPiece.BlackKnight;
            }
            else // black
            {
                knight = ChessPiece.WhiteKnight;
            }
            List<ChessLocation> locs = new List<ChessLocation>();
            locs.Add(new ChessLocation(x + 2, y + 1));
            locs.Add(new ChessLocation(x + 2, y - 1));
            locs.Add(new ChessLocation(x - 2, y + 1));
            locs.Add(new ChessLocation(x - 2, y - 1));
            locs.Add(new ChessLocation(x + 1, y + 2));
            locs.Add(new ChessLocation(x + 1, y - 2));
            locs.Add(new ChessLocation(x - 1, y + 2));
            locs.Add(new ChessLocation(x - 1, y - 2));
            foreach (ChessLocation loc in locs)
            {
                if (IsValid(loc)) // Bounds checking
                {
                    // Is there a knight there?
                    if (board[loc] == knight)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool InCheckFromPawnOrKing(ChessBoard board, ChessLocation king, ChessColor color)
        {
            int x = king.X, y = king.Y;
            List<ChessLocation> locs = new List<ChessLocation>();
            ChessPiece pawn, kingPiece;
            if (color == ChessColor.White)
            {
                pawn = ChessPiece.BlackPawn;
                kingPiece = ChessPiece.BlackKing;
            }
            else // black
            {
                pawn = ChessPiece.WhitePawn;
                kingPiece = ChessPiece.WhiteKing;
            }
            // Check for pawns:
            if (color == ChessColor.White)
            {
                locs.Add(new ChessLocation(x + 1, y - 1));
                locs.Add(new ChessLocation(x - 1, y - 1));
            }
            else // black
            {
                locs.Add(new ChessLocation(x + 1, y + 1));
                locs.Add(new ChessLocation(x - 1, y + 1));
            }
            foreach (ChessLocation loc in locs)
            {
                if (IsValid(loc))
                {
                    if (board[loc] == pawn)
                    {
                        return true;
                    }
                }
            }
            locs.Clear();
            // Check for kings all around the king
            locs.Add(new ChessLocation(x - 1, y - 1));
            locs.Add(new ChessLocation(x, y - 1));
            locs.Add(new ChessLocation(x + 1, y - 1));
            locs.Add(new ChessLocation(x + 1, y));
            locs.Add(new ChessLocation(x + 1, y + 1));
            locs.Add(new ChessLocation(x, y + 1));
            locs.Add(new ChessLocation(x - 1, y + 1));
            locs.Add(new ChessLocation(x - 1, y));
            foreach (ChessLocation loc in locs)
            {
                if (IsValid(loc))
                {
                    if (board[loc] == kingPiece)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool InCheckFromBishopOrQueen(ChessBoard board, ChessLocation king, ChessColor color)
        {
            int x = king.X, y = king.Y;
            ChessLocation nw = null, ne = null, se = null, sw = null;
            ChessPiece bishop, queen; 
            if (color == ChessColor.White)
            {
                bishop = ChessPiece.BlackBishop;
                queen = ChessPiece.BlackQueen;
            }
            else // black
            {
                bishop = ChessPiece.WhiteBishop;
                queen = ChessPiece.WhiteQueen;
            }
            // Northwest diagonal:
            // Find nearest empty piece on diagonal and check if it's a bishop or queen
            if (IsValid(x - 1, y - 1))
            {
                nw = new ChessLocation(x - 1, y - 1);
                while (board[nw] == ChessPiece.Empty)
                {
                    if (IsValid(x - 1, y - 1))
                    {
                        nw = new ChessLocation(x - 1, y - 1);
                    }
                }
                if ((board[nw] == bishop) || (board[nw] == queen))
                {
                    return true;
                }
            }
            // Northeast diagonal
            if (IsValid(x + 1, y - 1))
            {
                ne = new ChessLocation(x + 1, y - 1);
                while (board[ne] == ChessPiece.Empty)
                {
                    if (IsValid(x + 1, y - 1))
                    {
                        ne = new ChessLocation(x + 1, y - 1);
                    }
                }
                if ((board[ne] == bishop) || (board[ne] == queen))
                {
                    return true;
                }
            }
            // Southeast
            if (IsValid(x + 1, y + 1))
            {
                se = new ChessLocation(x + 1, y + 1);
                while (board[se] == ChessPiece.Empty)
                {
                    if (IsValid(x + 1, y + 1))
                    {
                        se = new ChessLocation(x + 1, y + 1);
                    }
                }
                if ((board[se] == bishop) || (board[se] == queen))
                {
                    return true;
                }
            }
            // Southwest
            if (IsValid(x - 1, y + 1))
            {
                sw = new ChessLocation(x - 1, y + 1);
                while (board[sw] == ChessPiece.Empty)
                {
                    if (IsValid(x - 1, y + 1))
                    {
                        sw = new ChessLocation(x - 1, y + 1);
                    }
                }
                if ((board[sw] == bishop) || (board[sw] == queen))
                {
                    return true;
                }
            }
            return false;
        }

        private bool InCheckFromRookOrQueen(ChessBoard board, ChessLocation king, ChessColor color)
        {
            int x = king.X, y = king.Y;
            ChessLocation nn = null, ee = null, ss = null, ww = null;
            ChessPiece rook, queen;
            if (color == ChessColor.White)
            {
                rook = ChessPiece.BlackRook;
                queen = ChessPiece.BlackQueen;
            }
            else // black
            {
                rook = ChessPiece.WhiteRook;
                queen = ChessPiece.WhiteQueen;
            }
            // North (nn)
            if (IsValid(x, y - 1))
            {
                nn = new ChessLocation(x, y - 1);
                while (board[nn] == ChessPiece.Empty)
                {
                    if (IsValid(x, y - 1))
                    {
                        nn = new ChessLocation(x, y - 1);
                    }
                }
                if ((board[nn] == rook) || (board[nn] == queen))
                {
                    return true;
                }
            }
            // East (ee)
            if (IsValid(x + 1, y))
            {
                ee = new ChessLocation(x + 1, y);
                while (board[ee] == ChessPiece.Empty)
                {
                    if (IsValid(x + 1, y))
                    {
                        ee = new ChessLocation(x + 1, y);
                    }
                }
                if ((board[ee] == rook) || (board[ee] == queen))
                {
                    return true;
                }
            }
            // South (ss)
            if (IsValid(x, y + 1))
            {
                ss = new ChessLocation(x, y + 1);
                while (board[ss] == ChessPiece.Empty)
                {
                    if (IsValid(x, y + 1))
                    {
                        ss = new ChessLocation(x, y + 1);
                    }
                }
                if ((board[ss] == rook) || (board[ss] == queen))
                {
                    return true;
                }
            }
            // West (ww)
            if (IsValid(x - 1, y))
            {
                ww = new ChessLocation(x - 1, y);
                while (board[ww] == ChessPiece.Empty)
                {
                    if (IsValid(x - 1, y))
                    {
                        ww = new ChessLocation(x - 1, y);
                    }
                }
                if ((board[ww] == rook) || (board[ww] == queen))
                {
                    return true;
                }
            }
            return false;
        }

        bool IsValid(ChessLocation loc)
        {
            if ((loc.X >= 0) && (loc.X < 8) && (loc.Y >= 0) && (loc.Y < 8))
            {
                return true;
            }
            return false;
        }

        bool IsValid(int x, int y)
        {
            if ((x >= 0) && (x < 8) && (y >= 0) && (y < 8))
            {
                return true;
            }
            return false;
        }

        ChessColor OtherColor(ChessColor color)
        {
            if (color == ChessColor.White)
            {
                return ChessColor.Black;
            }
            else return ChessColor.White;
        }

        private ChessLocation FindKing(ChessBoard board, ChessColor myColor)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ChessPiece current = board[i, j];
                    if (myColor == ChessColor.White)
                    {
                        if (current == ChessPiece.WhiteKing)
                        {
                            return new ChessLocation(i, j);
                        }
                    }
                    else
                    {
                        if (current == ChessPiece.BlackKing)
                        {
                            return new ChessLocation(i, j);
                        }
                    }
                }
            }
            return null;
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
