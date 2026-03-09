# 📚 LEARN.md — Animal Classifier

Hey there! 👋 This guide will walk you through exactly how the machine learning part of this project was built — step by step, in plain language. No prior ML experience needed.

By the end of this guide you'll understand how to go from a folder of animal photos to a working ML model running inside a C# desktop app.

---

## Table of Contents

1. [What is ML.NET?](#1-what-is-mlnet)
2. [How Image Classification Works](#2-how-image-classification-works)
3. [Dataset Preparation](#3-dataset-preparation)
4. [Training the Model with ML.NET Model Builder](#4-training-the-model-with-mlnet-model-builder)
5. [Understanding the Output](#5-understanding-the-output)
6. [Tips for Better Accuracy](#6-tips-for-better-accuracy)
7. [Further Reading](#7-further-reading)

---

## 1. What is ML.NET?

**ML.NET** is a free, open-source machine learning framework made by Microsoft specifically for .NET developers. It lets you build and use machine learning models directly in C# — no Python, no separate ML server, no complicated setup.

Think of it like this: normally, machine learning is done in Python with libraries like TensorFlow or PyTorch. ML.NET brings that same power into the C# world that .NET developers already know.

For this project, ML.NET is used to look at an image of an animal and predict what species it is.

**Why ML.NET for this project?**
- You write everything in C# — no need to learn Python
- It comes with a visual tool called **Model Builder** built into Visual Studio
- Once trained, the model is saved as a single file you can ship with your app
- Works great with WPF desktop applications

---

## 2. How Image Classification Works

"Image classification" just means: *look at a picture, and decide what category it belongs to.* In our case, the categories are animal species like `cat`, `shark`, `eagle`, and 108 others.

### What is Transfer Learning?

Training a model from zero would require millions of images and days of computation. Instead, ML.NET uses a technique called **transfer learning** — it starts from a model that Microsoft already trained on millions of general images, and then teaches it *your specific categories* on top of that.

Here's a simple way to think about it:

```
Imagine hiring someone who already knows how to read.
You don't teach them the alphabet again.
You just teach them your specific subject.

ML.NET does the same thing with images.
```

In technical terms:

```
[Pre-trained model — already knows general shapes, colors, textures]
        ↓
[You provide: your labeled animal photos]
        ↓
[Model learns: "oh, THESE shapes = eagle, THOSE = jellyfish"]
        ↓
[Your custom model — ready to classify your 111 animals]
```

### What is a Confidence Score?

When the model makes a prediction, it doesn't just say "that's a cat." It gives a probability for *every single class*, like:

```
cat        → 91.3%
leopard    → 5.1%
lion       → 2.4%
others     → 1.2%
```

The highest one wins and becomes the prediction. That percentage is what you see as the **confidence score** in the app.

| Score Range | What it Means |
|---|---|
| 80% and above | High confidence — very likely correct |
| 50% – 80% | Moderate — probably right, but not certain |
| Below 50% | Low confidence — image may be unclear or unusual |

---

## 3. Dataset Preparation

Before you can train any model, you need data — lots of labeled images. This is the most important step. **Garbage in, garbage out.**

### 3.1 Where the Data Came From

This project combined three public datasets to get enough variety across 111 animal classes:

| Dataset | Link |
|---|---|
| Multi-animal dataset (SciDB) | [View Dataset](https://www.scidb.cn/en/detail?dataSetId=e2ebd46cb1304a82bab54a8873cb3004) |
| Animals10 (Kaggle) | [View Dataset](https://www.kaggle.com/datasets/alessiocorrado99/animals10) |
| Sea Animals (Kaggle) | [View Dataset](https://www.kaggle.com/datasets/vencerlanz09/sea-animals-image-dataste) |

### 3.2 Folder Structure — This is Important!

ML.NET Model Builder reads your dataset from folders. The rule is simple: **one folder = one class, and the folder name becomes the label.**

```
dataset/
├── cat/
│   ├── cat_001.jpg
│   ├── cat_002.jpg
│   └── ...
├── shark/
│   ├── shark_001.jpg
│   └── ...
├── eagle/
│   └── ...
└── (one folder per animal)
```

If your folder is named `golden_eagle`, the model will predict `golden_eagle`. Make sure folder names are clean and consistent.

### 3.3 Cleaning the Data

Raw downloaded datasets are messy. Before training, remove:

- ❌ Corrupted or broken image files
- ❌ Mislabeled images (a dog photo inside the `cat` folder)
- ❌ Images that are too tiny (under 100×100 pixels)
- ❌ Duplicate images

### 3.4 Balance Your Classes

Try to have a similar number of images per animal. If you have 2,000 cat photos but only 50 seahorse photos, the model will be much better at recognizing cats and will often misclassify seahorses.

A good rule of thumb: **aim for at least 100–200 images per class**, and try not to let any one class have more than 5× the images of another.

---

## 4. Training the Model with ML.NET Model Builder

ML.NET Model Builder is a visual tool built directly into Visual Studio. You don't need to write any training code — it walks you through the whole process with a step-by-step UI.

Here is the exact process used to build this project, following these steps: **Scenario → Environment → Data → Train → Evaluate → Consume.**

---

### Step 1 — Add a Machine Learning Model to Your Project

<img width="601" height="834" alt="image" src="https://github.com/user-attachments/assets/d6cde77c-3707-4e39-a411-fe2487ff152d" />


1. In Visual Studio, open your WPF project in **Solution Explorer**
2. **Right-click** on your project folder
3. Select **Add → Machine Learning Model**
4. Give it a name (e.g. `AnimalClassifier.mbconfig`) and click **Add**

This opens the Model Builder wizard inside Visual Studio.

---

### Step 2 — Select a Scenario

<img width="1551" height="741" alt="image" src="https://github.com/user-attachments/assets/31b8a9ee-a24a-4354-948b-ddbc27f080c8" />


The first screen asks what kind of ML problem you're solving.

➡️ Choose **Image Classification**

> **What is a scenario?** It's the type of prediction task. Image Classification means: "given an image, which category does it belong to?" Other scenarios exist for things like text analysis or number prediction — but for animal photos, Image Classification is the right pick.

---

### Step 3 — Select an Environment

<img width="1551" height="740" alt="image" src="https://github.com/user-attachments/assets/56346476-308a-4a72-b486-c77370afbbfd" />

This screen asks where the model should be trained.

➡️ Choose **Local (CPU)**

> **CPU vs GPU?** A GPU (graphics card) trains much faster because it can process thousands of calculations at once. However, CPU works perfectly fine and needs no extra setup. For 111 classes, CPU training may take several hours — you can let it run overnight.

---

### Step 4 — Load Your Data

<img width="1552" height="738" alt="image" src="https://github.com/user-attachments/assets/eae23ac0-16a0-4404-84a5-2f0656ff8438" />


Point Model Builder at your prepared dataset folder.

1. Click **Browse** and select your dataset root folder (the one containing all the animal subfolders)
2. Set the **train/test split** to **80% train / 20% test**

> **What is the 80/20 split?**
> - **80% (Train):** These images are shown to the model during training so it can learn from them.
> - **20% (Test):** These images are hidden during training and only used at the end to check how well the model performs on images it has *never seen before*.
>
> This gives you an honest accuracy score. If you tested on the same images used for training, the score would be misleadingly high.

Once your folder is selected, Model Builder will display a preview of your classes and image counts. It should look something like:

```
✅ cat           → 312 images
✅ shark         → 278 images
✅ eagle         → 245 images
... (111 classes total)
```

---

### Step 5 — Train

<img width="1543" height="737" alt="image" src="https://github.com/user-attachments/assets/fa3cd264-07a7-4050-805e-edbeee9385a4" />


Click **Start Training** and wait.

Model Builder will automatically:
- Load and preprocess all your images
- Fine-tune the pre-trained model on your 111 animal classes
- Test its performance on the held-back 20%
- Save the best version of the model

⏱️ Training time depends on your CPU speed and dataset size. For 111 classes, expect anywhere from 1–6 hours.

**This project achieved a Micro Accuracy of `0.8987`** — meaning the model correctly identified roughly 9 out of every 10 test images.

> **What is Micro Accuracy?**
> It's the overall percentage of images the model got right, across all 111 classes. `0.8987` = 89.87% correct. For a 111-class problem trained locally on CPU, this is a strong result.

---

### Step 6 — Evaluate

<img width="1548" height="739" alt="image" src="https://github.com/user-attachments/assets/874e4ebe-bd10-4f7e-9722-3b4b7897880b" />


After training, switch to the **Evaluate** tab in Model Builder. This lets you test the model with your own photo before adding it to your project.

1. Click **Browse image** and pick any animal photo from your computer
2. Model Builder displays the **predicted class** and **confidence score** in real time
3. Try a few different photos to get a feel for how accurate it is in practice

> 💡 Try uploading a photo that wasn't in your training dataset — for example, a random image from Google. This gives a more realistic idea of real-world performance.

---

### Step 7 — Add to Your Code

Once you're satisfied, click **Add to project**. Model Builder automatically generates everything you need:

| Generated File | What it Does |
|---|---|
| `AnimalClassifier.mbconfig` | The main model configuration |
| `AnimalClassifier.mlnet` | The trained model (the actual ML weights) |
| `ModelInput.cs` | C# class that holds the input image |
| `ModelOutput.cs` | C# class that holds the prediction result |
| `AnimalClassifierModel.cs` | The prediction engine — call `.Predict()` from here |

Using the model in C# is then just a few lines:

```csharp
// 1. Load the image
var sampleData = new ModelInput()
{
    ImageSource = targetPath,
};

// 2. Run the prediction
var output = AnimalClassifierModel.Predict(sampleData);

// 3. Read the results
string prediction = output.Prediction;
float confidence = output.Score.Max() * 100;

Console.WriteLine($"Prediction: {prediction}");
Console.WriteLine($"Confidence: {confidence:F1}%");
```

That's it! The model runs entirely on the user's machine — no internet connection required.

---

## 5. Understanding the Output

When `.Predict()` runs, it returns a `ModelOutput` object with two important fields:

| Field | Type | Description |
|---|---|---|
| `Prediction` | `string` | The animal class with the highest probability (e.g. `"eagle"`) |
| `Score` | `float[]` | An array of probabilities — one value per class, all adding up to 1.0 |

The `Score` array has 111 values (one per animal). The confidence percentage shown in the app is simply `Score.Max() * 100` — the highest probability converted to a percentage.

**Example output for a photo of an eagle:**

```
Prediction: eagle
Score[eagle_index]: 0.913  →  91.3% confidence
Score[hawk_index]:  0.051  →   5.1%
Score[owl_index]:   0.024  →   2.4%
... (108 more near-zero values)
```

---

## 6. Tips for Better Accuracy

| Problem | Likely Cause | What to Try |
|---|---|---|
| Overall accuracy is low | Not enough training images | Add more photos per class — aim for 200+ each |
| Certain animals always wrong | Too few images for those specific classes | Find more images for those categories specifically |
| Similar-looking animals confused | They're visually close (e.g. leopard vs cheetah) | Add varied images — different angles, lighting, backgrounds |
| Good in training, bad in real life | Training images are too clean or uniform | Mix in more varied, real-world photos |
| Training crashes mid-way | Corrupted image in the dataset | Re-run the cleaning script from Section 3.3 |

---

## 7. Further Reading

Want to go deeper? Here are some great next steps:

- [ML.NET Official Documentation](https://learn.microsoft.com/en-us/dotnet/machine-learning/)
- [ML.NET Model Builder Guide](https://learn.microsoft.com/en-us/dotnet/machine-learning/automate-training-with-model-builder)
- [Image Classification Tutorial — ML.NET Docs](https://learn.microsoft.com/en-us/dotnet/machine-learning/tutorials/image-classification)
- [Image Classification in Web Apps with ML.NET — dev.to](https://dev.to/esdanielgomez/image-classification-in-web-applications-with-mlnet-ipl) *(the tutorial this project's workflow is based on)*
- [What is Transfer Learning? — Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/machine-learning/how-does-mldotnet-work#transfer-learning)

---

*Have questions or want to contribute improvements to this guide? Feel free to open a [Discussion](../../discussions) on the repository. Happy coding! 🐾*
