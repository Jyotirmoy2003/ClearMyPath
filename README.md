# 💖 Clear My Path
[![Play on itch.io](https://img.shields.io/badge/Play-Itch.io-FA5C5C?style=for-the-badge&logo=itch.io)](https://yourname.itch.io/clearmypath)

A 2-Player Cooperative WebGL Game built with Unity + Photon PUN.

One runs.  
One protects.  
Together, you survive.

---

## 🎮 About The Game

**Clear My Path** is a real-time multiplayer co-op game where teamwork is everything.

- 🏃 One player navigates a platform filled with swinging obstacles.
- 🎯 The other player supports from above, clearing obstacles using bombs.
- 💣 Bomb pouches can be collected to strengthen the team.
- 🏁 Reach the red flag together to win.

This project was built as a fast-paced multiplayer challenge and evolved into a full WebGL deployment with backend logging and a leaderboard system.

---

## 🚀 Features

### 🔹 Multiplayer (Photon PUN)
- Master-authoritative obstacle system
- Deterministic obstacle syncing
- Scene synchronization
- Restart & level progression handling

### 🔹 Cooperative Gameplay
- Role-based player logic
- Bomb support mechanic
- Stun system for obstacles
- Shared victory condition

### 🔹 Firebase Integration
- Session logging
- Anonymous authentication
- Publishable leaderboard
- Time-based ranking system

### 🔹 WebGL Optimized
- Mobile + Desktop support
- Landscape enforcement overlay
- Brotli compression
- JS ↔ Unity communication bridge

---

## 🏆 Leaderboard System

After completing the level, players can choose to publish their score.

- Only published scores appear on leaderboard.
- Fastest duos rank at the top.

---

## 🛠 Tech Stack

- **Unity (C#)**
- **Photon PUN 2**
- **Firebase Realtime Database**
- **Firebase Anonymous Auth**
- **WebGL**
- **DOTween**

---

## 🌐 Build Target

Primary Target:  
✔ WebGL (itch.io deployment)

---

## 🧠 Architecture Highlights

- Master-side obstacle simulation
- RPC-based state syncing
- Custom Player Properties via Hashtable
- JS ↔ Unity communication bridge
- Secure Firebase rule configuration

---

## 💡 Lessons Learned

- Multiplayer scene synchronization in Photon
- WebGL compression handling
- Firebase security rule design
- Async JS authentication flow
- Preventing race conditions in scene reloads

---

## Developer

Developed by **Jyotirmoy Mondal**

Built as a creative multiplayer experiment and polished into a full web experience.

---



> Some paths are easier when cleared together.
