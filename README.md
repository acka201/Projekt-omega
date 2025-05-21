# Logic Quest Game

**Logic Quest** is a logic-based puzzle game written in JavaScript. It challenges players to navigate a path from the start to the goal using logical reasoning and grid-based clues.

## Motivation

This game was developed as a semester project to demonstrate JavaScript DOM manipulation, dynamic UI rendering, and logical game mechanics. The goal was to create an engaging logic game that works entirely in the browser without external dependencies.

## How to Run

You can try the game live here:  
👉 [**Play Logic Quest Game**](https://your-github-username.github.io/logic-quest-game/)

Or, if you'd like to run it locally:

```bash
git clone https://github.com/your-github-username/logic-quest-game.git
cd logic-quest-game
open index.html
```

## Screenshots

![Start screen](screenshots/start_screen.png)  
*Start screen with difficulty options and sound toggle.*

![Game in progress](screenshots/game_play.png)  
*In-game view showing the puzzle grid and logic clues.*

## Project Structure

- `index.html` - Main HTML file  
- `style.css` - Game styling  
- `app.js` - Core game logic  
- `assets/` - Contains sound files and images  
- `screenshots/` - Contains images for README  

## Deployment

This is a client-only web application. The project is auto-deployed to GitHub Pages using a GitHub Action.  
To deploy your own version:

1. Fork or clone the repository.
2. Set up GitHub Pages on the `main` branch.
3. Add the `gh-pages.yml` GitHub Action to `.github/workflows/`.

See [`IntelliLogo`'s `gh-pages.yml`](https://github.com/your-example/intellilogo/blob/main/.github/workflows/gh-pages.yml) as a reference.
