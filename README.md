# Simple Online Multiplayer Tank Game

This project showcases a simple Host-Client online multiplayer game made using Unity Game Engine and [Photon Fusion 1.1.8](https://doc.photonengine.com/fusion/v1/getting-started/sdk-download). You can play the project in your browser and/or download the game here: [Simple Online Multiplayer Tank Game by BACON BEAST (itch.io)](https://bacon-beast.itch.io/simple-online-multiplayer-tank-game) 

The game is a simple competitive tank game where players shoot each other for points within a time limit, and the player with the most points when the game ends wins.

## Acknowledgements
This project was inspired by [Asteroids Simple | Photon Engine](https://doc.photonengine.com/fusion/v1/game-samples/fusion-asteroids) and [Tanknarok | Photon Engine](https://doc.photonengine.com/fusion/v1/game-samples/fusion-tanknarok) examples.

## Getting Started
- Download ZIP or Clone Project
- Open Project in Unity (2020.3 or above)
- Setup with Photon - Create and use your own App ID
	- Go to [Photon Dashboard](https://dashboard.photonengine.com/)
	- Create New Application: Set Photon SDK as Fusion and Version as Fusion 1; Create
	- Copy & Paste App ID into 'App Id Fusion' in Unity -> Fusion > Realtime Settings > App Id Fusion
	- Note that using your own ID means you won't connect with other players unless they use your version of the game which uses your ID.

## Highlights
- Hosted mode support (Host-Client)
- Lobby scene with "*READY UP*" functionality and ability for Host to *kick* players
- Movement prediction for players and bullets with physics
- Lag Compensated Raycasts
- Complete Game Loop - *Menu -> Lobby -> Game -> Game End -> Lobby or Menu*

## Screenshots
![Lobby Scene](https://github.com/BACONBEAST9000/Simple-Online-Multiplayer-Tank-Game/blob/main/Screenshots/LobbyScene.png)

![Game Scene](https://github.com/BACONBEAST9000/Simple-Online-Multiplayer-Tank-Game/blob/main/Screenshots/GameScene.png)

![Results Screen](https://github.com/BACONBEAST9000/Simple-Online-Multiplayer-Tank-Game/blob/main/Screenshots/ResultsScreen.png)

## Future Features for Future Me
- [ ] **Object Pooling** - Pooling and reusing objects, such as tank bullets.
- [ ] **Predictive Spawning** - Clients predicting whether networked object has spawned on the network, creates a placeholder until confirmed spawned.
- [ ] **Host Migration** - When Host leaves, promote another player to be the Host instead of booting everybody from the game session.
- [ ] **Lobby Session Browser** - Have an option to view available lobbies and have players be able to join any available lobby.
- [ ] **Public/Private lobbies** - Allow host to set whether the lobby is public or private. When private, outsiders cannot join without the Room Name or some access code. Right now, any player can join any lobby, as long as it's available (On lobby scene), with the Room Name being used as a way to connect to a specific lobby.
- [ ] Upgrade to Fusion 2
