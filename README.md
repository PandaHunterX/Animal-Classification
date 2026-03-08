# 🐾 Animal Classification App

A desktop image classification application built with **WPF** and **ML.NET** that identifies over 111 animal species from a photo — complete with a confidence score, light/dark theme, and audio feedback.

---

## ✨ Features

- 📷 **Image upload** support for `.jpg`, `.jpeg`, and `.png` files
- 🤖 **ML.NET-powered** animal classification across 111 classes
- 📊 **Prediction confidence score** displayed with each result
- 🌙 **Light & dark theme** toggle
- 🎬 **Animated prediction results**
- 🔊 **Audio feedback** for interactions
- 🔗 **GitHub profile** integration in the UI

---

## 🚀 Installation

### Option 1 — Installer (Recommended)

The app ships as a Windows installer built with **Inno Setup**.

1. Go to the [**Releases**](../../releases) page.
2. Download the latest `AnimalClassifierSetup.exe`.
3. Run the installer and follow the on-screen prompts.
4. Launch **Animal Classifier** from the Start Menu or Desktop shortcut.

> ⚠️ **Requirements:** Windows 10 or later · [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (if not bundled)

---

### Option 2 — Build from Source

**Prerequisites**

| Tool | Version |
|---|---|
| Visual Studio | 2022 (Community or higher) |
| .NET SDK | 8.0+ |
| ML.NET | via NuGet |

**Steps**

```bash
# 1. Clone the repository
git clone https://github.com/YOUR_USERNAME/animal-classifier.git
cd animal-classifier

# 2. Open the solution in Visual Studio
start AnimalClassifier.sln

# 3. Restore NuGet packages
# Visual Studio does this automatically, or run:
dotnet restore

# 4. Build and run
dotnet run
```

---

## 🖼️ How to Use

1. Launch the application.
2. Click **Upload Image** and select a `.jpg`, `.jpeg`, or `.png` file of an animal.
3. The model will analyze the image and display:
   - The **predicted animal class**
   - A **confidence score** (e.g., 94.2%)
4. Toggle between **light and dark** themes using the theme button.

---

## 🧠 ML Model

The classification model was trained using **ML.NET Model Builder** on a combined dataset of **111 animal classes** sourced from three public datasets:

| Dataset | Source |
|---|---|
| Multi-animal dataset | [SciDB](https://www.scidb.cn/en/detail?dataSetId=e2ebd46cb1304a82bab54a8873cb3004) |
| Animals10 | [Kaggle – alessiocorrado99](https://www.kaggle.com/datasets/alessiocorrado99/animals10) |
| Sea Animals | [Kaggle – vencerlanz09](https://www.kaggle.com/datasets/vencerlanz09/sea-animals-image-dataste) |

### Dataset Preparation

- Combined images from all three sources
- Organized into labeled folders per class
- Removed corrupted or invalid images
- Standardized file formats
- Structured for ML.NET Model Builder compatibility

### Supported Animal Classes (111)

<details>
<summary>Click to expand full class list</summary>

`antelope` `badger` `bat` `bear` `bee` `beetle` `bison` `boar` `butterfly` `camel` `capybara` `cat` `caterpillar` `chicken` `chimpanzee` `clams` `cockroach` `coral` `cow` `coyote` `crab` `crocodile` `crow` `deer` `dog` `dolphin` `donkey` `dragonfly` `duck` `eagle` `elephant` `fish` `flamingo` `fly` `fox` `frog` `giraffe` `goat` `goose` `gorilla` `grasshopper` `hamster` `hare` `hedgehog` `hippopotamus` `hornbill` `horse` `hummingbird` `hyena` `jellyfish` `kangaroo` `koala` `ladybug` `leopard` `lion` `lizard` `lobster` `mandrill` `mantis` `mosquito` `moth` `mouse` `nudibranch` `octopus` `orangutan` `otter` `owl` `panda` `pangolin` `parrot` `peacock` `pelican` `penguin` `pigeon` `platypus` `porcupine` `possum` `pufferfish` `raccoon` `ray` `reindeer` `rhinoceros` `sandpiper` `scallop` `scorpion` `sea urchin` `seahorse` `seal` `shark` `sheep` `shrimp` `snake` `sparrow` `spider` `squid` `squirrel` `starfish` `swan` `tiger` `turkey` `turtle` `walrus` `whale` `wombat` `woodpecker`

</details>

---

## 🛠️ Tech Stack

- **UI Framework:** WPF (.NET)
- **ML Framework:** ML.NET
- **Installer:** Inno Setup
- **Language:** C#

---
