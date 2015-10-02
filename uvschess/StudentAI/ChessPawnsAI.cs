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
            get { return "ChessPawnsAI (Greedy Evaluate Board)"; }
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
            ChessMove move;// = GetRndMove(board, myColor);
            //return new ChessMove(new ChessLocation(0, 0), new ChessLocation(0, 0), ChessFlag.);
            //move = GetStrategicMove(board, myColor);
            move = GetLookAheadMove(board, myColor);
            return move;
        }

        // Like GetMoveValue, EvaluateBoard sets the ValueOfMove property of the ChessMove object
        // But it does so by evaluating the whole board.
        // A higher (more positive) integer means a better move.
        public void EvaluateBoard(ChessMove move, ChessBoard b, ChessColor color)
        {
            ChessBoard board = b.Clone();
            board.MakeMove(move);
            ChessColor other = OtherColor(color);
            // Values based on https://en.wikipedia.org/wiki/Chess_piece_relative_value#Hans_Berliner.27s_system
            int pawn = 100;
            int knight = 320;
            int bishop = 333;
            int rook = 510;
            int queen = 880;
            int king = 10000;
            int val = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    ChessPiece piece = board[x, y];
                    // Add the values of my pieces
                    if (color == Color(piece))
                    {
                        if ((piece == ChessPiece.WhitePawn) || (piece == ChessPiece.BlackPawn))
                        {
                            val += pawn;
                        }
                        else if ((piece == ChessPiece.WhiteKnight) || (piece == ChessPiece.BlackKnight))
                        {
                            val += knight;
                        }
                        else if ((piece == ChessPiece.WhiteBishop) || (piece == ChessPiece.BlackBishop))
                        {
                            val += bishop;
                        }
                        else if ((piece == ChessPiece.WhiteRook) || (piece == ChessPiece.BlackRook))
                        {
                            val += rook;
                        }
                        else if ((piece == ChessPiece.WhiteQueen) || (piece == ChessPiece.BlackQueen))
                        {
                            val += queen;
                        }
                        else if ((piece == ChessPiece.WhiteKing) || (piece == ChessPiece.BlackKing))
                        {
                            val += king;
                        }
                        // or it's empty; then don't add anything
                    }
                    else // subtract the value of the opponent's pieces
                    {
                        if ((piece == ChessPiece.WhitePawn) || (piece == ChessPiece.BlackPawn))
                        {
                            val -= pawn;
                        }
                        else if ((piece == ChessPiece.WhiteKnight) || (piece == ChessPiece.BlackKnight))
                        {
                            val -= knight;
                        }
                        else if ((piece == ChessPiece.WhiteBishop) || (piece == ChessPiece.BlackBishop))
                        {
                            val -= bishop;
                        }
                        else if ((piece == ChessPiece.WhiteRook) || (piece == ChessPiece.BlackRook))
                        {
                            val -= rook;
                        }
                        else if ((piece == ChessPiece.WhiteQueen) || (piece == ChessPiece.BlackQueen))
                        {
                            val -= queen;
                        }
                        else if ((piece == ChessPiece.WhiteKing) || (piece == ChessPiece.BlackKing))
                        {
                            val -= king;
                        }
                    }
                }
            }
            move.ValueOfMove = val;
        }

        public void GetMoveValue(ChessMove move, ChessBoard board)
        {
            // Taking a piece? Value lowers according to what piece you're taking (Queen is more important than a Pawn)
            int baseMoveValue = 1000;
            int capturePawn = 20;
            int captureKnight = 50;
            int captureBishop = 100;
            int captureRook = 110;
            int captureQueen = 350;
            int check = 0;
            int checkMate = baseMoveValue;

            ChessLocation destination = move.To;
            ChessPiece whatsThere = board[destination];

            move.ValueOfMove = baseMoveValue;

            if (move.Flag == ChessFlag.Check)
                move.ValueOfMove -= check;
            if (move.Flag == ChessFlag.Checkmate)
                move.ValueOfMove -= checkMate;
            if (whatsThere == ChessPiece.WhitePawn || whatsThere == ChessPiece.BlackPawn)
                move.ValueOfMove -= capturePawn;
            else if (whatsThere == ChessPiece.WhiteKnight || whatsThere == ChessPiece.BlackKnight)
                move.ValueOfMove -= captureKnight;
            else if (whatsThere == ChessPiece.WhiteBishop || whatsThere == ChessPiece.BlackBishop)
                move.ValueOfMove -= captureBishop;
            else if (whatsThere == ChessPiece.WhiteRook || whatsThere == ChessPiece.BlackRook)
                move.ValueOfMove -= captureRook;
            else if (whatsThere == ChessPiece.WhiteQueen || whatsThere == ChessPiece.BlackQueen)
                move.ValueOfMove -= captureQueen;

        }
        

        public ChessMove GetStrategicMove(ChessBoard board, ChessColor myColor)
        {
            List<ChessMove> moves = GetAllMoves(board, myColor);
            if (moves.Count < 1) // if we are in stalemate
            {
                return new ChessMove(new ChessLocation(0, 0), new ChessLocation(0, 0), ChessFlag.Stalemate);
            }
            
            foreach (ChessMove move in moves)
            {
                EvaluateBoard(move, board, myColor);
 //               GetMoveValue(move, board);
            }
            
            List<ChessMove> bestMoves = getBestMoves(moves);
                        
            //Log("There are " + moves.Count + " possible moves, with " + bestMoves.Count + " moves that seem decent");
            
            Random random = new Random();
            int randInt = random.Next(bestMoves.Count);
            return bestMoves[randInt];
        }

        public ChessMove GetLookAheadMove(ChessBoard board, ChessColor myColor)
        {
            List<ChessMove> moves = GetAllMoves(board, myColor);
            if (moves.Count < 1) // if we are in stalemate
            {
                return new ChessMove(new ChessLocation(0, 0), new ChessLocation(0, 0), ChessFlag.Stalemate);
            }

            foreach (ChessMove move in moves)
            {
                EvaluateBoard(move, board, myColor);
//                GetMoveValue(move, board);
            }

            foreach (ChessMove move in moves)
            {
                ChessBoard tBoard = board.Clone();
                tBoard.MakeMove(move);
                move.ValueOfMove -= (GetStrategicMove(tBoard, OtherColor(myColor)).ValueOfMove);
            }

            List<ChessMove> bestMoves = getBestMoves(moves);

            Log("There are " + moves.Count + " possible moves, with " + bestMoves.Count + " moves that seem decent.");

            Random random = new Random();
            int randInt = random.Next(bestMoves.Count);
            return bestMoves[randInt];
        }

        public List<ChessMove> getBestMoves(List<ChessMove> allMoves)
        {
            List<ChessMove> bestMoves = new List<ChessMove>();
            allMoves.Sort();
            // If Sort works the way it should, the last item should be the highest value move
            // Add the last item
            bestMoves.Add(allMoves[allMoves.Count - 1]);
            int bestVal = (allMoves[allMoves.Count - 1]).ValueOfMove;
            // Start from 2nd to last item and iterate backwards
            for (int i = allMoves.Count - 2; i >= 0; i--)
            {
                if (allMoves[i].ValueOfMove == bestVal)
                {
                    bestMoves.Add(allMoves[i]);
                }
                if (allMoves[i].ValueOfMove > bestVal)
                {
                    // Then Sort didn't work
                    Log("Sort didn't work; revamp GetBestMoves method");
                }
                // TODO: if sort works, then break out of this loop when we get a value less than bestVal
            }

            //// Iterate through moveValues and add the move with the lowest value, 
            //// clearing the list and re-making it if you find something lower.
            //int lowestValue = 10000;
            //int numMoves = 1;
            //for (int i = 0; i < allMoves.Count; ++i)
            //{
            //    if (allMoves[i].ValueOfMove < lowestValue)
            //    {
            //        numMoves = 1;
            //        bestMoves.Add(allMoves[i]);
            //        lowestValue = allMoves[i].ValueOfMove;
            //    }
            //    else if (allMoves[i].ValueOfMove == lowestValue)
            //    {
            //        bestMoves.Add(allMoves[i]);
            //        ++numMoves;
            //    }
            //}
            //bestMoves.Reverse();
            //return bestMoves.GetRange(0, numMoves);
            return bestMoves;
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
            List<ChessMove> myMoves = new List<ChessMove>();
            for (int i = 0; i < moves.Count; ++i)
            {
                if (!IsCheck(board, moves[i], king, myColor))
                {
                    myMoves.Add(moves[i]);
                }
                
            }

            
            // Flag any moves that put our opponent in check
            foreach (ChessMove move in myMoves)
            {
                if (IsCheck(board, move, otherKing, OtherColor(myColor)))
                {
                    move.Flag = ChessFlag.Check;
                }
            }

            // Flag any moves that put our opponent in checkmate
            GetCheckMate(myMoves, board, myColor);

            return myMoves;
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

        public void GetCheckMate(List<ChessMove> moves, ChessBoard board, ChessColor myColor)
        {

            foreach (ChessMove move in moves)
            {
                if (move.Flag == ChessFlag.Check)
                {
                    ChessBoard testBoard = board.Clone();
                    testBoard.MakeMove(move);
                    List<ChessMove> checkMoves = GetAllMoves(testBoard, OtherColor(myColor));
                    if (checkMoves.Count < 1)
                    {
                        move.Flag = ChessFlag.Checkmate;
                    }
                }
            }
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
            int x, y;
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
            x = king.X - 1;
            y = king.Y - 1;
            while (IsValid(x, y))
            {
                if ((board[x, y] == bishop) || (board[x, y] == queen))
                {
                    return true;
                }
                if (board[x, y] != ChessPiece.Empty)
                {
                    break;
                }
                x -= 1;
                y -= 1;
            }

            // Northeast diagonal
            x = king.X + 1; 
            y = king.Y - 1;
            while (IsValid(x, y))
            {
                if ((board[x, y] == bishop) || (board[x, y] == queen))
                {
                    return true;
                }
                if (board[x, y] != ChessPiece.Empty)
                {
                    break;
                }
                x += 1;
                y -= 1;
            }

            // Southeast
            x = king.X + 1;
            y = king.Y + 1;
            while (IsValid(x, y))
            {
                if ((board[x, y] == bishop) || (board[x, y] == queen))
                {
                    return true;
                }
                if (board[x, y] != ChessPiece.Empty)
                {
                    break;
                }
                x += 1;
                y += 1;
            }

            // Southwest
            x = king.X - 1;
            y = king.Y + 1;
            while (IsValid(x, y))
            {
                if ((board[x, y] == bishop) || (board[x, y] == queen))
                {
                    return true;
                }
                if (board[x, y] != ChessPiece.Empty)
                {
                    break;
                }
                x -= 1;
                y += 1;
            }
            return false;
        }

        private bool InCheckFromRookOrQueen(ChessBoard board, ChessLocation king, ChessColor color)
        {
            int x, y;
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

            // North:
            // Find nearest empty piece and check if it's a rooke or queen
            x = king.X;
            y = king.Y - 1;
            while (IsValid(x, y))
            {
                if ((board[x, y] == rook) || (board[x, y] == queen))
                {
                    return true;
                }
                if (board[x, y] != ChessPiece.Empty)
                {
                    break;
                }
                y -= 1;
            }

            // East (ee)
            x = king.X + 1;
            y = king.Y;
            while (IsValid(x, y))
            {
                if ((board[x, y] == rook) || (board[x, y] == queen))
                {
                    return true;
                }
                if (board[x, y] != ChessPiece.Empty)
                {
                    break;
                }
                x += 1;
            }

            // South (ss)
            x = king.X;
            y = king.Y + 1;
            while (IsValid(x, y))
            {
                if ((board[x, y] == rook) || (board[x, y] == queen))
                {
                    return true;
                }
                if (board[x, y] != ChessPiece.Empty)
                {
                    break;
                }
                y += 1;
            }

            // West (ww)
            x = king.X - 1;
            y = king.Y;
            while (IsValid(x, y))
            {
                if ((board[x, y] == rook) || (board[x, y] == queen))
                {
                    return true;
                }
                if (board[x, y] != ChessPiece.Empty)
                {
                    break;
                }
                x -= 1;
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
            ChessPiece king;
            if (myColor == ChessColor.White)
            {
                king = ChessPiece.WhiteKing;
            }
            else // black
            {
                king = ChessPiece.BlackKing;
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ChessPiece current = board[i, j];
                    if (current == king)
                    {
                        return new ChessLocation(i, j);
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
