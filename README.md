# Logic Quest Game

## Description

Logic Quest is a bilingual (Hungarian and English) console adventure game developed in F#. In this game, players create a profile and then embark on an interactive journey filled with puzzles, combat encounters, and narrative choices.  
  
Gameplay elements include:  
- **Math Puzzles:** Solve quick arithmetic challenges to proceed.  
- **Combat Encounters:** Engage in battles with enemies using various attack and defense strategies.  
- **Story Narratives:** Choose your path during branching story events, with narratives loaded from external text files specific to each level.  
- **Level Progression:** Earn XP and level up (up to level 10) with each successful encounter. On leveling up, new quests and items are loaded from language-specific text files (e.g., `level3_hu.txt` or `level3_en.txt`).  
- **Profile Management:** Save and load your profile as a JSON file so you can continue your adventure later.

The game also features simple graphical enhancements for a better console experience—using ASCII art, colored text output, and brief animation sequences.

## Motivation

The goal of Logic Quest is to offer an engaging and challenging interactive experience that not only entertains but also hones vocabulary, quick thinking, and problem-solving skills. Its bilingual nature and dynamic level system ensure that players of different backgrounds and skill levels can enjoy the game.

## Development Environment

- **Language:** F#
- **Target Framework:** .NET (6 or newer)
- **Libraries Used:**
  - `System.IO` for file handling
  - `System.Text.Json` for JSON serialization (saving/loading profiles)
  - `System.Threading` for implementing pause-based animations
- **IDE Options:** Visual Studio, Visual Studio Code, or any F# compatible development environment

## Features

- **Bilingual Support:** Choose between Hungarian and English from the start; all game messages, quests, and UI elements adapt accordingly.
- **Dynamic Level System:** Leveling up is tied to experience (XP). Each new level (maximum of 10) loads external text files containing new quests and items.
- **Interactive Gameplay:** Players must solve math puzzles, engage in combat with diverse enemies (complete with ASCII art visuals and color-coded output), and make narrative choices during story events.
- **Profile Management:** Create a new game profile, save it as a JSON file, and reload it later so you can resume your adventure.
- **Enhanced Console Presentation:** The game uses simple ASCII graphics, colored text, and short animations (such as a trailing dot effect) to enrich the console experience.


## Running the Project

1. Clone the repository to your local machine:
    ```bash
    git clone https://github.com/acka201/Project-omega.git
    ```

2. Open the project in an F# supporting IDE (e.g., Visual Studio).
   
3. Ensure that all required data files (*.txt in the txt.zip) are available in the project directory.

4. To start the game, run the `Logic Quest Game` project.

### Prerequisites

Before running the project, ensure that:  
- You have .NET 6 (or later) installed on your machine.
- The necessary level mission files are in the same directory as the executable (txt.zip):
  - **Hungarian:** `level1_hu.txt`, `level2_hu.txt`, …, `level10_hu.txt`
  - **English:** `level1_en.txt`, `level2_en.txt`, …, `level10_en.txt`

### Screenshot of the Game

See pictures.  
[https://github.com/acka201/Project-first/blob/main/1.jpg](https://github.com/acka201/Projekt-omega/blob/main/1.jpg)
[https://github.com/acka201/Project-first/blob/main/2.jpg](https://github.com/acka201/Projekt-omega/blob/main/2.jpg)
[https://github.com/acka201/Project-first/blob/main/3.jpg](https://github.com/acka201/Projekt-omega/blob/main/3.jpg)
[https://github.com/acka201/Project-first/blob/main/4.jpg](https://github.com/acka201/Projekt-omega/blob/main/4.jpg)
[https://github.com/acka201/Project-first/blob/main/5.jpg](https://github.com/acka201/Projekt-omega/blob/main/5.jpg)

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/acka201/Project-first.git

## Development Plan

In the future, we plan to add the following features:
- 🌐 Web-based version (e.g., using GitHub Pages)
- 🌍 Additional language support
- 📈 Enhanced statistical analysis (best times, performance history)
- 🎨 Improved UI/UX


## License

This project is licensed under the **MIT License**. See the `LICENSE` file for more details.

How to Use
To run the project, ensure that the necessary files, such as level1_hu.txt, level10_en.txt, etc., are available when you run the program.

If you encounter any issues, double-check that the files are correctly formatted and located in the appropriate directory.

Screenshots
You can replace the "See pictures" placeholder with actual screenshots once you have them available. If no screenshot is available, feel free to leave it as is and add it later.
