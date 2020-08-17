# KM Technical Assessment

https://technical-assessment.konicaminoltamarketplace.com/

## Game Rules
* The game is played on a 4x4 grid of 16 nodes.
* Players take turns drawing octilinear lines connecting nodes.
* Each line must begin at the start or end of the existing path, so that all lines form a continuous path.
* The first line may begin on any node.
* A line may connect any number of nodes.
* Lines may not intersect.
* No node can be visited twice.
* The game ends when no valid lines can be drawn.
* The player who draws the last line is the loser.

## Endpoints

### GET /initialize

#### No Request Body

#### Expected Response

    {
        "msg": "INITIALIZE",
        "body": {
            "newLine": null,
            "heading": "Player 1",
            "message": "Awaiting Player 1's Move"
        }
    }

### POST /node-clicked

#### Request Body
The request will be a `POINT` object with the x and y indicies for the point the user clicked on.


    {
        "x": 0,
        "y": 2
    }

#### Expected Responses

    {
        "msg": "VALID_START_NODE",
        "body": {
            "newLine": null,
            "heading": "Player 2",
            "message": "Select a second node to complete the line."
        }
    }

    {
        "msg": "INVALID_START_NODE",
        "body": {
            "newLine": null,
            "heading": "Player 2",
            "message": "Not a valid starting position."
        }
    }

    {
        "msg": "VALID_END_NODE",
        "body": {
            "newLine": {
                "start": {
                    "x": 0,
                    "y": 0
                },
                "end": {
                    "x": 0,
                    "y": 2
                }
            },
            "heading": "Player 1",
            "message": null
        }
    }

    {
        "msg": "INVALID_END_NODE",
        "body": {
            "newLine": null,
            "heading": "Player 2",
            "message": "Invalid move!"
        }
    }

    {
        "msg": "GAME_OVER",
        "body": {
            "newLine": {
                "start": {
                    "x": 0,
                    "y": 0
                },
                "end": {
                    "x": 0,
                    "y": 2
                }
            },
            "heading": "Game Over",
            "message": "Player 2 Wins!"
        }
    }

### POST /error
This is a general mechanism for the Client to report errors. The request is solely intended to aid debugging, and the Client ignores any response.

#### Request Body
    {
        "error": "Invalid type for `id`: Expected INT but got a STRING"
    }
